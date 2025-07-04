using SpookyCore.Runtime.EntitySystem;
using UnityEngine;

namespace Mirror.Logic
{
    public class AutoPlatformTrigger : EntityTrigger
    {
        #region Fields

        [SerializeField] private float _activationDelay;
        [SerializeField] private float _deactivationDelay;
        [SerializeField] private float _activationTimer;
        [SerializeField] private float _deactivationTimer;

        [SerializeField] private bool _isActivated;
        [SerializeField] private bool _isPlayerDetected;
        private AutoPlatformMovement _movement;

        #endregion

        #region Life Cycle

        public override void OnStart()
        {
            _activationTimer = _activationDelay;
            _movement = Entity.Get<AutoPlatformMovement>();
        }
        
        public override void OnUpdate()
        {
            if (_isPlayerDetected && !_isActivated)
            {
                _activationTimer -= Time.deltaTime;
                if (_activationTimer < 0)
                {
                    _movement.MoveToFinalPosition();
                    _isActivated = true;
                    _activationTimer = _activationDelay;
                }
            }
            
            if (!_isPlayerDetected && _isActivated)
            {
                _deactivationDelay -= Time.deltaTime;
                if (_deactivationTimer < 0)
                {
                    _movement.ReturnToStartingPosition();
                    _isActivated = false;
                    _deactivationTimer = _deactivationDelay;
                }
            }
        }

        #endregion

        #region Public Methods

        public bool IsCollidingWithPlayer()
        {
            foreach (var entity in _detectedEntities)
            {
                if (entity.ID.IsPlayer())
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
        
        #region Private Methods

        protected override void OnEntityEntered(Entity entity)
        {
            if (!entity.ID.IsPlayer()) return;

            _isPlayerDetected = true;
        }

        protected override void OnEntityExited(Entity entity)
        { 
            if (!entity.ID.IsPlayer()) return;

            _isPlayerDetected = false;
        }

        #endregion
    }
}