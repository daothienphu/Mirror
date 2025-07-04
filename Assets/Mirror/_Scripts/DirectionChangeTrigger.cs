using System.Collections.Generic;
using SpookyCore.Runtime.EntitySystem;
using UnityEngine;

namespace Mirror.Logic
{
    public class DirectionChangeTrigger : EntityTrigger
    {
        [SerializeField] private List<Directions> _possibleDirections;
        [SerializeField] private int _currentDirectionIndex = -1;
        [field: SerializeField] public Directions CurrentDirection { get; private set; }

        public override void OnStart()
        {
            ChangeDirection();
        }

        protected override void OnEntityEntered(Entity entity)
        {
            if (!entity.ID.IsPlayer()) return;

            var mirrorDirection = entity.Get<PlayerMirrorMovement>();

            ChangeDirection();
            mirrorDirection.ChangeMirrorDirection(CurrentDirection);
        }

        private void ChangeDirection()
        {
            _currentDirectionIndex = (_currentDirectionIndex + 1) % _possibleDirections.Count;
            CurrentDirection = _possibleDirections[_currentDirectionIndex];
        }
    }
}