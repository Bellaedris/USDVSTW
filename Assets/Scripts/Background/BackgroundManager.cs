using UnityEngine;
using usd.Utils;

namespace usd.Background
{
    public class BackgroundManager : MonoBehaviour
    {
        public GameObject[] backgroundObjects;
        public int maxNumberOfBackgroundObjects = 2;
        public float minScale = 0.5f;
        public float maxScale = 5f;
        public GameObject cam;
        
        private BoxCollider2D _boxCollider;
        private Vector2 _boxCenter;
        
        // Start is called before the first frame update
        void Start()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _boxCenter = new Vector2(_boxCollider.bounds.center.x, _boxCollider.bounds.center.y);
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.childCount < maxNumberOfBackgroundObjects)
            {
                GameObject backgroundObject = Instantiate(backgroundObjects[Random.Range(0, backgroundObjects.Length)], transform);
                float scale = Random.Range(minScale, maxScale);
                Vector2 position = RandomUtils.RandomInRectangleBorder(ref _boxCollider);
                backgroundObject.transform.localScale = new Vector3(scale, scale, scale);
                backgroundObject.transform.position = position;
            }
        }
    }
}
