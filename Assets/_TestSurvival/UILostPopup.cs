using SpookyCore.Runtime.Systems;
using SpookyCore.Runtime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Test
{
    public class UILostPopupModel : BaseModel
    {
        
    }
    public class UILostPopup : BaseViewPresenter<UILostPopupModel>
    {
        [SerializeField] private TMP_Text _enemiesDefeated;
        [SerializeField] private Button _restart;
        
        public override void OnViewPresenterReady()
        {
            var defeated = GameManager.Instance.EnemyKilled;
            _enemiesDefeated.text = $"Enemies defeated: {defeated}";
            _restart.onClick.RemoveAllListeners();
            _restart.onClick.AddListener(OnRestartButtonClicked);
        }

        protected override void OnViewPresenterPreDispose()
        {
        }

        protected override void OnUpdate()
        {
            
        }

        public void OnRestartButtonClicked()
        {
            SceneFlowSystem.Instance.ReloadCurrentScene();
        }
    }
}