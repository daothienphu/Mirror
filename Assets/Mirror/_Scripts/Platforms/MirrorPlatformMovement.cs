using System;
using SpookyCore.Runtime.EntitySystem;
using SpookyCore.Runtime.Systems;
using SpookyCore.Runtime.Utilities;
using UnityEngine;

namespace Mirror.Logic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MirrorPlatformMovement : EntityComponent
    {
        #region Enum and Class
        
        [Serializable]
        public class DirectionConfig
        {
            public Directions Direction;
            public float Limit;
        }

        #endregion

        #region Fields

        [SerializeField] private KinematicObjectMovement _kinematicObjectMovement;
        
        [Header("Platform Movement")]
        [SerializeField] private DirectionConfig _firstDirection;
        [SerializeField] private DirectionConfig _secondDirection;
        
        private Vector2 _initialPosition;
        private float _minX, _maxX;
        private float _minY, _maxY;
        
        [SerializeField] private PlayerMirrorMovement _mirrorMovement;

        #endregion

        #region Properties

        // public Vector3 Velocity => _kinematicObjectMovement.Velocity;

        #endregion
        
        #region Life Cycle

#if UNITY_EDITOR
        private void OnValidate()
        {
            CalculateMovementBounds();
        }
#endif

        public override void OnAwake()
        {
            if (!_kinematicObjectMovement)
            {
                _kinematicObjectMovement = GetComponent<KinematicObjectMovement>();
            }
        }

        public override void OnStart()
        {
            AssignPlayerMovement();
            CalculateMovementBounds();
        }
        
        public override void OnFixedUpdate()
        {
            if (!_mirrorMovement) return;

            var firstVelocity = _mirrorMovement.GetActiveMirrorVelocity(_firstDirection.Direction);
            var secondVelocity = _mirrorMovement.GetActiveMirrorVelocity(_secondDirection.Direction);
            var deltaMovement = GetClampedDeltaMovement(firstVelocity + secondVelocity);
            _kinematicObjectMovement.Move(deltaMovement);
        }

        private Vector2 GetClampedDeltaMovement(Vector2 velocity)
        {
            var current = transform.position.V2();
            var delta = velocity * Time.fixedDeltaTime;
            
            var targetX = Mathf.Clamp(current.x + delta.x, _minX, _maxX);
            var targetY = Mathf.Clamp(current.y + delta.y, _minY, _maxY);
            return new Vector2(targetX - current.x, targetY - current.y);
        }

        #endregion

        #region Private Methods

        private void AssignPlayerMovement()
        {
            if (GameManager.Instance.PlayerObservable.HasValue)
            {
                _mirrorMovement = GameManager.Instance.PlayerObservable.Value.Get<PlayerMirrorMovement>();
            }
            else
            {
                GameManager.Instance.PlayerObservable.Subscribe(
                    entity => { _mirrorMovement = entity.Get<PlayerMirrorMovement>();}
                );
            }
        }
        
        private void CalculateMovementBounds()
        {
            _initialPosition = transform.position;
            
            _minX = _maxX = _initialPosition.x;
            _minY = _maxY = _initialPosition.y;

            ApplyBound(_firstDirection);
            ApplyBound(_secondDirection);
        }

        private void ApplyBound(DirectionConfig config)
        {
            if (config == null) return;

            switch (config.Direction)
            {
                case Directions.Left:  _minX -= config.Limit; break;
                case Directions.Right: _maxX += config.Limit; break;
                case Directions.Down:  _minY -= config.Limit; break;
                case Directions.Up:    _maxY += config.Limit; break;
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            var width = _maxX - _minX;
            var height = _maxY - _minY;
            Gizmos.DrawWireCube(new Vector2((_minX + _maxX) / 2f, (_minY + _maxY) / 2f), new Vector3(width, height));
        }
#endif
        
        #endregion
    }
}