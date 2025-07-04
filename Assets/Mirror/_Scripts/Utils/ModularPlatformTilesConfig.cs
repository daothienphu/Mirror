using UnityEngine;

namespace Mirror.Logic
{
    [CreateAssetMenu(menuName = "Mirror/Logic/Platform Generator/Tiles Config", fileName = "Platform Tiles Config")]
    public class ModularPlatformTilesConfig : ScriptableObject
    {
        public Sprite[] TileSprites;
    }
}