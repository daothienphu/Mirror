using SpookyCore.Runtime.EntitySystem;
using SpookyCore.Runtime.Systems;
using UnityEngine;

namespace Mirror.Logic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class AutoPlatformMovement : EntityComponent
    {
        #region Fields

        [SerializeField] private Transform _startingPosition;
        [SerializeField] private Transform _finalPosition;
        [SerializeField] private KinematicObjectMovement _kinematicObjectMovement;
        [SerializeField] private float _moveSpeed = 1f;

        [SerializeField] private Vector3 _currentDestination;
        [SerializeField] private PlayerMovement _playerMovement;
        private AutoPlatformTrigger _trigger;

        #endregion
        
        #region Life Cycle
        
        public override void OnStart()
        {
            _currentDestination = _startingPosition.position;
            transform.position = _startingPosition.position;
            if (!_kinematicObjectMovement)
            {
                _kinematicObjectMovement = GetComponent<KinematicObjectMovement>();
            }
            _trigger = Entity.Get<AutoPlatformTrigger>();

            if (GameManager.Instance.PlayerObservable.HasValue)
            {
                _playerMovement = GameManager.Instance.PlayerObservable.Value.Get<PlayerMovement>();
            }
            else
            {
                GameManager.Instance.PlayerObservable.Subscribe(entity => _playerMovement = entity.Get<PlayerMovement>());
            }
        }

        public override void OnFixedUpdate()
        {
            if (IsAtDestination())
            {
                if (IsNearDestination(_finalPosition.position))
                {
                    if (HasPlayerOnTop())
                    {
                        return;
                    }
                    _currentDestination = _startingPosition.position;
                }
                else if (IsNearDestination(_startingPosition.position))
                {
                    return;
                }
            }
            
            var direction = _currentDestination - transform.position;
            var deltaMovement = Mathf.Min(_moveSpeed * Time.fixedDeltaTime, direction.magnitude) * direction.normalized;
            if (HasPlayerOnTop())
            {
                _playerMovement.Move(deltaMovement);
            }
            _kinematicObjectMovement.Move(deltaMovement);
        }
        
        #endregion

        #region Public Methods

        public void MoveToFinalPosition()
        {
            _currentDestination = _finalPosition.position;
        }

        public void ReturnToStartingPosition()
        {
            _currentDestination = _startingPosition.position;
        }

        #endregion

        #region Private Methods

        private bool IsAtDestination()
        {
            return Vector3.SqrMagnitude(transform.position - _currentDestination) < 0.01f;
        }

        private bool IsNearDestination(Vector3 destination)
        {
            return Vector3.SqrMagnitude(_currentDestination - destination) < 0.01f;
        }

        private bool HasPlayerOnTop()
        {
            return _playerMovement.IsStandingOnPlatform() && _trigger.IsCollidingWithPlayer();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Debug.DrawLine(_startingPosition.position, _finalPosition.position);
        }
        #endif
        
        #endregion
    }
}