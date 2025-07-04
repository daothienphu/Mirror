using SpookyCore.Runtime.EntitySystem;
using UnityEngine;

namespace Mirror.Logic
{
    public class DoorTrigger : EntityTrigger
    {
        protected override void OnEntityEntered(Entity entity)
        {
            if (entity.ID.IsPlayer())
            {
                Debug.Log("Level Finished");           
            }
        }
    }
}