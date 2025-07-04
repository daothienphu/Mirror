using SpookyCore.Runtime.EntitySystem;
using SpookyCore.Runtime.UI;

namespace _Test._Scripts
{
    public class PlayerHealth : EntityHealth
    {
        public override void OnBeforeDead()
        {
            base.OnBeforeDead();
            if (Entity.ID.IsPlayer())
            {
                UISystem.Instance.ShowUI<UILostPopup>(new UILostPopupModel());
            }
        }
    }
}