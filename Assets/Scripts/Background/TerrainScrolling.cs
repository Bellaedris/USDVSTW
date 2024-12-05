using UnityEngine;

namespace usd.Background
{
    /// <summary>
    /// Handles the scrolling of terrain in the background.
    /// </summary>
    public class TerrainScrolling : MonoBehaviour
    {
        /// <summary>
        /// Speed at which the terrain scrolls.
        /// </summary>
        public float scrollSpeed = 1f;

        /// <summary>
        /// Direction in which the terrain scrolls.
        /// </summary>
        public Vector3 scrollDirection;

        // Start is called before the first frame update
        void Start()
        {
            // Initialization code can be added here if needed
        }

        // Update is called once per frame
        void Update()
        {
            // Moves the terrain in the specified direction at the specified speed
            transform.Translate(scrollDirection * (Time.deltaTime * scrollSpeed));
        }
    }
}