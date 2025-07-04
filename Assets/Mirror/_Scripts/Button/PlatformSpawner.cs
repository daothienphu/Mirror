using SpookyCore.Runtime.EntitySystem;
using SpookyCore.Runtime.Systems;
using UnityEngine;

namespace Mirror.Logic
{
    public class PlatformSpawner : EntityComponent
    {
        [SerializeField] private GameObject _platformPrefab;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private float _spawnCooldown = 3f;
        [SerializeField] private float _spawnTimer;
        private ObjectPool<Entity> _platformPool; 
        
        public override void OnStart()
        {
            _platformPool = new ObjectPool<Entity>(_platformPrefab, 5, transform);
            _spawnTimer = _spawnCooldown;
        }

        public override void OnUpdate()
        {
            if (_spawnTimer > 0)
            {
                _spawnTimer -= Time.deltaTime;
            }
        }

        public void RequestSpawnPlatform()
        {
            if (_spawnTimer > 0) return;

            var platform = _platformPool.Get();
            platform.transform.position = _spawnPosition.position;
            platform.transform.rotation = Quaternion.identity;

            _spawnTimer = _spawnCooldown;
        }
    }
}