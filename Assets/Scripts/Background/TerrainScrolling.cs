using UnityEngine;

namespace usd.Background
{
    public class TerrainScrolling : MonoBehaviour
    {
        public float scrollSpeed = 1f;
        public Vector3 scrollDirection;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(scrollDirection * (Time.deltaTime * scrollSpeed));
        }
    }
}