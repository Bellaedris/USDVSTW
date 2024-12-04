using UnityEngine;

namespace usd.Background
{
    /// <summary>
    /// UNUSED, parallax effect for 2D side-scrolling.
    /// </summary>
    public class Parallax : MonoBehaviour
    {
        
        public Vector2 parallaxSpeed;
        public GameObject cam;

        private Vector3 _startPos;

        private BoxCollider2D _bounds;
        
        private void Awake()
        {
            _startPos = transform.position;
            _bounds = GetComponentInParent<BoxCollider2D>();
        }

        private void Update()
        {
            float newX = cam.transform.position.x * parallaxSpeed.x;
            float newY = cam.transform.position.y * parallaxSpeed.y;
            transform.position = new Vector3(_startPos.x + newX, _startPos.y + newY, 0);
            
            if (!_bounds.OverlapPoint(new Vector2(transform.position.x, transform.position.y)))
                Destroy(gameObject);
        }

        public void SetCamera(GameObject camera)
        {
            cam = camera;
        }

        public void SetParallaxSpeed(Vector2 parallaxSpeed)
        {
            this.parallaxSpeed = parallaxSpeed;
        }
    }
}
