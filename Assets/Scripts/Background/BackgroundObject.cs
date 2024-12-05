using UnityEngine;
using usd.Utils;

namespace usd
{
    /// <summary>
    /// Represents a background object that moves within the scene.
    /// </summary>
    public class BackgroundObject : MonoBehaviour
    {
        /// <summary>
        /// The maximum speed at which the background object can move.
        /// </summary>
        private float maxSpeed = 10f;

        /// <summary>
        /// The direction in which the background object is moving.
        /// </summary>
        private Vector3 _direction;

        /// <summary>
        /// The speed at which the background object is moving.
        /// </summary>
        private float _speed;

        /// <summary>
        /// The bounds within which the background object can move.
        /// </summary>
        private BoxCollider _bounds;
        
        void Awake()
        {
            // Initializes the background object, setting its movement direction and speed.
            _speed = Random.Range(0f, maxSpeed);
            Vector2 dir = RandomUtils.RandomVectorInRange(-1f, 1f);
            _direction = new Vector3(dir.x, dir.y);
            _bounds = GetComponentInParent<BoxCollider>();
        }
        
        void Update()
        {
            /// Updates the background object's position and destroys it if it leaves the bounds of the game.
            transform.Translate(Time.deltaTime * _speed * _direction);

            // Destroy the object when it leaves the bounds of the game
            if (!_bounds.bounds.Contains(transform.position))
                Destroy(gameObject);
        }
    }
}