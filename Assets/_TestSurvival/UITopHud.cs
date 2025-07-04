using SpookyCore.Runtime.Systems;
using SpookyCore.Runtime.UI;
using TMPro;
using UnityEngine;

namespace _Test
{
    public class TopHudModel : BaseModel
    {
        
    }
    
    public class UITopHud : BaseViewPresenter<TopHudModel>
    {
        [SerializeField] private TMP_Text _enemyCounter;
        private GameManager _gameManager;
        
        public override void OnViewPresenterReady()
        {
            _gameManager = GameManager.Instance;
            _enemyCounter.text = "0";
        }

        protected override void OnViewPresenterPreDispose() { }

        protected override void OnUpdate()
        {
            if (!_gameManager)
            {
                _gameManager = GameManager.Instance;
            }

            if (_gameManager)
            {
                _enemyCounter.text = $"{_gameManager.EnemyKilled}";
            }
        }
    }
}