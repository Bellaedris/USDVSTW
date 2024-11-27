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
        
        private BoxCollider _boxCollider;
        private Vector2 _boxCenter;
        
        // Start is called before the first frame update
        void Start()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _boxCenter = new Vector2(_boxCollider.bounds.center.x, _boxCollider.bounds.center.y);
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.childCount < maxNumberOfBackgroundObjects)
            {
                Vector3 position = RandomUtils.RandomInRectangleBorder(ref _boxCollider);
                GameObject backgroundObject = Instantiate(backgroundObjects[Random.Range(0, backgroundObjects.Length)], position + Vector3.back * 0.01f, Quaternion.identity, transform);
                float scale = Random.Range(minScale, maxScale);
                backgroundObject.transform.localScale = new Vector3(scale, scale, scale);
                backgroundObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }
}
