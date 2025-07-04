using SpookyCore.Runtime.Utilities;
using UnityEngine;

namespace Mirror.Logic
{
    public class ModularPlatformGenerator : MonoBehaviour
    {
        [SerializeField] private ModularPlatformTilesConfig _tilesConfig; 
        private Sprite[] _tileSprites = new Sprite[9];
        [Min(1)] 
        [SerializeField] private int _width = 3;
        [Min(1)] 
        [SerializeField] private int _height = 1;
        [SerializeField] private int _sortingOrder = 1;

        [CreateInspectorButton("Generate Platform")]
        public void Generate()
        {
            _tileSprites = _tilesConfig.TileSprites;
            if (_tileSprites.Length != 9 || !_tileSprites[0])
            {
                Debug.LogError("Please assign all 9 sprites before generating.");
                return;
            }

            // Clear old children
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            var tileWidth = _tileSprites[4].bounds.size.x;
            var tileHeight = _tileSprites[4].bounds.size.y;

            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    var tileGO = new GameObject($"Tile_{x}_{y}");
                    tileGO.transform.parent = transform;

                    var sr = tileGO.AddComponent<SpriteRenderer>();
                    sr.sprite = GetSpriteForPosition(x, y);
                    sr.sortingOrder = _sortingOrder;
                    
                    var posX = (x - _width / 2f + 0.5f) * tileWidth;
                    var posY = (y - _height / 2f + 0.5f) * tileHeight;
                    tileGO.transform.localPosition = new Vector3(posX, posY, 0);
                }
            }
        }

        private Sprite GetSpriteForPosition(int x, int y)
        {
            var left = x == 0;
            var right = x == _width - 1;
            var top = y == _height - 1;
            var bottom = y == 0;
            
            if (top && left) return _tileSprites[0];
            if (top && right) return _tileSprites[2];
            if (bottom && left) return _tileSprites[6];
            if (bottom && right) return _tileSprites[8];
            
            if (top) return _tileSprites[1];
            if (bottom) return _tileSprites[7];
            if (left) return _tileSprites[3];
            if (right) return _tileSprites[5];
            
            return _tileSprites[4];
        }
    }
}