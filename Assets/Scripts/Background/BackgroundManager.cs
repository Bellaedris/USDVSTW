using UnityEngine;
using usd.Utils;

namespace usd.Background
{
    /// <summary>
    /// Manages background objects that move around the scene.
    /// </summary>
    public class BackgroundManager : MonoBehaviour
    {
        /// <summary>
        /// Array of background objects to be instantiated.
        /// </summary>
        public GameObject[] backgroundObjects;

        /// <summary>
        /// Maximum number of background objects allowed in the scene.
        /// </summary>
        public int maxNumberOfBackgroundObjects = 2;

        /// <summary>
        /// Minimum scale for the background objects.
        /// </summary>
        public float minScale = .1f;

        /// <summary>
        /// Maximum scale for the background objects.
        /// </summary>
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
            // Since the background objects are deleting themselves upon leaving the game area, we must spawn others
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