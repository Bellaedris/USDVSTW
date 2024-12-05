using UnityEngine;

namespace usd.Background
{
    /// <summary>
    /// UNUSED, parallax effect for 2D side-scrolling.
    /// </summary>
    public class Parallax : MonoBehaviour
    {
        /// <summary>
        /// Speed of the parallax effect in the x and y directions.
        /// </summary>
        public Vector2 parallaxSpeed;

        /// <summary>
        /// Reference to the camera object.
        /// </summary>
        public GameObject cam;

        /// <summary>
        /// Initial position of the parallax object.
        /// </summary>
        private Vector3 _startPos;

        /// <summary>
        /// Bounds within which the parallax object can move.
        /// </summary>
        private BoxCollider2D _bounds;
        
        private void Awake()
        {
            // Initializes the parallax object, setting its start position and bounds.
            _startPos = transform.position;
            _bounds = GetComponentInParent<BoxCollider2D>();
        }
        
        private void Update()
        {
            // Updates the parallax object's position based on the camera's position and destroys it if it leaves the bounds.
            float newX = cam.transform.position.x * parallaxSpeed.x;
            float newY = cam.transform.position.y * parallaxSpeed.y;
            transform.position = new Vector3(_startPos.x + newX, _startPos.y + newY, 0);

            if (!_bounds.OverlapPoint(new Vector2(transform.position.x, transform.position.y)))
                Destroy(gameObject);
        }

        /// <summary>
        /// Sets the camera object for the parallax effect.
        /// </summary>
        /// <param name="camera">The camera object to set.</param>
        public void SetCamera(GameObject camera)
        {
            cam = camera;
        }

        /// <summary>
        /// Sets the speed of the parallax effect.
        /// </summary>
        /// <param name="parallaxSpeed">The speed of the parallax effect in the x and y directions.</param>
        public void SetParallaxSpeed(Vector2 parallaxSpeed)
        {
            this.parallaxSpeed = parallaxSpeed;
        }
    }
}