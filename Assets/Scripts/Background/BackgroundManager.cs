using UnityEngine;
using usd.Utils;

namespace usd.Background
{
    /// <summary>
    /// Manages background objects that over aroud the scene. 
    /// </summary>
    public class BackgroundManager : MonoBehaviour
    {
        public GameObject[] backgroundObjects;
        public int maxNumberOfBackgroundObjects = 2;
        public float minScale = .1f;
        public float maxScale = .8f;
        
        private BoxCollider _boxCollider;
        
        // Start is called before the first frame update
        void Start()
        {
            _boxCollider = GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            // since the background objects are deleting themselves upon leaving the game area, we must spawn others
            if (transform.childCount < maxNumberOfBackgroundObjects)
            {
                Vector3 position = RandomUtils.RandomInRectangleBorder(ref _boxCollider);
                GameObject backgroundObject = Instantiate(backgroundObjects[Random.Range(0, backgroundObjects.Length)], position + Vector3.forward * 0.3f, Quaternion.identity, transform);
                float scale = Random.Range(minScale, maxScale);
                backgroundObject.transform.localScale = new Vector3(scale, scale, scale);
                backgroundObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }
}
