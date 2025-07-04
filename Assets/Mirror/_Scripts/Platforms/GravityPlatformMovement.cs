using SpookyCore.Runtime.EntitySystem;
using UnityEngine;

namespace Mirror.Logic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityPlatformMovement: EntityComponent
    {
        [SerializeField] private float _gravity = -40f;
        [SerializeField] private float _maxFallSpeed = -20f;
        [SerializeField] private KinematicObjectMovement _kinematicObjectMovement;
        [SerializeField] private Vector2 _velocity;

        public override void OnStart()
        {
            if (!_kinematicObjectMovement)
            {
                _kinematicObjectMovement = GetComponent<KinematicObjectMovement>();
            }

            _velocity = Vector2.zero;
        }

        public override void OnFixedUpdate()
        {
            ApplyGravityVelocity(ref _velocity);
            var adjustedDeltaMovement = _kinematicObjectMovement.Move(_velocity * Time.fixedDeltaTime);
            _velocity = adjustedDeltaMovement / Time.fixedDeltaTime;
        }
        
        private void ApplyGravityVelocity(ref Vector2 velocity)
        {
            if (!_kinematicObjectMovement.Collisions.Below)
            {
                velocity.y += _gravity * Time.fixedDeltaTime;
                velocity.y = Mathf.Max(velocity.y, _maxFallSpeed);
            } 
            else
            {
                velocity.y = 0;
            }
        }
    }
}
