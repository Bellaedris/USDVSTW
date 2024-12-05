using UnityEngine;

namespace usd.Enemies.Projectiles
{
    /// <summary>
    /// Represents a linear projectile fired by an enemy.
    /// </summary>
    public class NmyLinearProjectile : MonoBehaviour
    {
        /// <summary>
        /// Speed at which the projectile moves.
        /// </summary>
        [HideInInspector]
        public float speed = 1f;

        /// <summary>
        /// Damage dealt by the projectile.
        /// </summary>
        [HideInInspector]
        public float damage;

        /// <summary>
        /// Target position the projectile is moving towards.
        /// </summary>
        [HideInInspector]
        public Vector3 target;

        /// <summary>
        /// Tolerance for reaching the target.
        /// </summary>
        public float tolerance = 0.05f;

        /// <summary>
        /// Reference to the main camera.
        /// </summary>
        private Camera _mainCamera;

        /// <summary>
        /// Direction in which the projectile is moving.
        /// </summary>
        private Vector3 moveDirection;

        /// <summary>
        /// Bounds within which the projectile can move.
        /// </summary>
        private Bounds limits;
        
        void Start()
        {
            // Initializes the projectile, setting its movement direction and bounds.
            _mainCamera = Camera.main;
            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            limits = new Bounds(_mainCamera.transform.position, new Vector3(sizeX * 2, sizeY * 2, 50));

            moveDirection = (target - transform.position).normalized;
        }
        
        void Update()
        {
            // Updates the projectile's position and destroys it if it leaves the bounds.
            if (!limits.Contains(transform.position))
            {
                Destroy(gameObject);
            }

            MoveTowardsTarget();
        }

        /// <summary>
        /// Moves the projectile towards its target.
        /// </summary>
        private void MoveTowardsTarget()
        {
            // Move in the current direction
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }
}