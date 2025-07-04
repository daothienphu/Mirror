using System;
using SpookyCore.Runtime.EntitySystem;
using UnityEngine;

namespace Mirror.Logic
{
    [Serializable] public enum Directions { Left, Right, Up, Down }
    
    public class PlayerMirrorMovement : EntityComponent
    {
        [field: SerializeField] public Directions CurrentMirrorDirection { get; protected set; }
        private PlayerMovement _movement;

        public override void OnStart()
        {
            _movement = Entity.Get<PlayerMovement>();
        }

        public void ChangeMirrorDirection(Directions direction)
        {
            CurrentMirrorDirection = direction;
        }
        
        public Vector2 GetActiveMirrorVelocity(Directions direction)
        {
            var velocity = Vector2.zero;
            
            switch (direction)
            {
                case Directions.Left:// when _movement.Velocity.x < 0:
                case Directions.Right:// when _movement.Velocity.x > 0:
                    velocity.x = _movement.Velocity.x;
                    break;
                case Directions.Up:// when _movement.Velocity.x > 0:
                case Directions.Down:// when _movement.Velocity.x < 0:
                    velocity.y = _movement.Velocity.x;
                    break;
            }

            return velocity;
        }
    }
}