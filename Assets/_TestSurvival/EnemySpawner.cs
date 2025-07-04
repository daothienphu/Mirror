using SpookyCore.Runtime.EntitySystem;
using SpookyCore.Runtime.Systems;
using UnityEngine;

namespace _Test
{
    public class EnemySpawner : MonoSingleton<EnemySpawner>
    {
        private float _timer;

        protected override void OnUpdate()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                SpawnEnemy();
                _timer = Random.Range(3f, 5f);
            }
        }

        private void SpawnEnemy()
        {
            var spawnPosition = Random.insideUnitCircle * 7f;
            var instance = PoolSystem.Instance.Get<Entity>(EntityID.BasicEnemy, onGetCallback: entity => entity.transform.position = spawnPosition);
        }
    }
}

