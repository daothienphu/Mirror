using SpookyCore.Runtime.EntitySystem;
namespace Mirror.Logic
{
    public class SpawnerButtonTrigger : EntityTrigger
    {
        private PlatformSpawner _spawner;
        
        public override void OnStart()
        {
            _spawner = Entity.Get<PlatformSpawner>();
        }

        public override void OnUpdate()
        {
            if (IsPressed())
            {
                _spawner.RequestSpawnPlatform();
            }
        }

        private bool IsPressed()
        {
            foreach (var entity in _detectedEntities)
            {
                if (entity.ID.IsPlayer() || entity.ID.IsPlatform())
                {
                    return true;
                }
            }

            return false;
        }
    }
}