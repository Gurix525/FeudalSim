using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;

        private static Map _current;
        private Image _image;
        private Vector3 _lastPlayerPosition = Vector3.zero;

        public static Map Current
        {
            get
            {
                if (_current == null )
                    _current = FindObjectOfType<Map>();
                return _current;
            }
        }

        public Texture2D MapTexture { get; private set; }

        public void Initialize(Texture2D texture)
        {
            if (texture != null)
            {
                MapTexture = texture;
            }
        }

        public void ForceFixedUpdate()
        {
            FixedUpdate();
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            MapTexture = new(600, 600);
            UpdateMap();
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(_playerTransform.position, _lastPlayerPosition) > 5F)
            {
                UpdateMap();
                _lastPlayerPosition = _playerTransform.position;
            }
        }

        private void UpdateMap()
        {
            Vector3 position = _playerTransform.position;
            for (int z = (int)position.z - 10; z <= (int)position.z + 10; z++)
            {
                for (int x = (int)position.x - 10; x <= (int)position.x + 10; x++)
                {
                    if (!x.IsInRangeInclusive(-275, 275) || !z.IsInRangeInclusive(-275, 275))
                        continue;
                    if (Vector3.Distance(new(x, z), position) > 10)
                        continue;
                    float height = World.Terrain.GetHeight(new(x, z));
                    MapTexture.SetPixel(x + 300, z + 300, GetMapColor(height));
                }
            }
            MapTexture.Apply();
            _image.sprite = Sprite.Create(MapTexture, new(new(0, 0),
                new(MapTexture.width, MapTexture.height)), Vector2.one / 2F);
        }

        private Color GetMapColor(float height)
        {
            if (height < 0)
                return Color.blue;
            if (height == 0)
                return Color.yellow;
            if (height > 0)
                return Color.green;
            return Color.black;
        }
    }
}