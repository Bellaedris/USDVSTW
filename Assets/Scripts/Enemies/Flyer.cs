using UnityEngine;
using usd.Enemies.Projectiles;

namespace usd.Enemies
{
    public class Flyer : BasicEnemy
    {
        // The height of the wave
        public float amplitude; // Height of the sinusoidal wave
        public float frequency; // Speed of the sinusoidal wave
        
        private Vector3 moveDirection; // Direction towards the target (player)
        private Vector3 perpendicularDirection; // Perpendicular axis for sinusoidal motion

        void Start()
        {
            timeLastShot = 0.0f;
            player = GameObject.Find("player");

            // Calculate the direction towards the player in X-Y plane (ignore Z)
            CalculateDirection();
        }

        void Update()
        {
            timeLastShot += Time.deltaTime;
            playerPosition = player.transform.position;

            // Move towards player
            Move();
            // Look at direction
            RotateEntityTowardsDirection(180.0f, 90.0f, moveDirection);

            // // Shoot if possible
            if (!(transform.position.x > shootLimits.x && transform.position.x < -shootLimits.x && transform.position.y > shootLimits.y && transform.position.y < -shootLimits.y) 
                && CanShoot())
            {
                Shoot();
            }
            else if (!limits.Contains(transform.position))
            {
                // Destroy(gameObject);
                CalculateDirection();
            }
        }
        
        private void CalculateDirection()
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.z = 0; // Ensure movement is constrained to X-Y plane
            moveDirection = directionToPlayer.normalized;
            
            // Calculate the perpendicular direction for sinusoidal oscillation
            perpendicularDirection = Vector3.Cross(moveDirection, Vector3.forward).normalized;
        }
        // Specific Methods
        public override bool CanShoot()
        {
            if (timeLastShot >= 1.0f / fireRate)
            {
                timeLastShot = 0.0f;
                return true;
            }

            return false;
        }

        public override void Shoot()
        {
            //Todo maybe remove fireLine for basic nmy
            if (fireLineSize > 0 && fireProjectilesCount > 1)
            {
                Vector3 fireDirection = playerPosition - transform.position;
                fireDirection.z = 0;
                fireDirection = fireDirection.normalized;

                float spacing = fireLineSize / (fireProjectilesCount - 1);

                // Calculate perpendicular direction for spreading projectiles along the fire line
                Vector3 perpendicular = new Vector3(-fireDirection.y, fireDirection.x, 0);

                // Start position relative to the center of the line
                Vector3 startPosition = transform.position - perpendicular * (fireLineSize / 2f);

                for (int i = 0; i < fireProjectilesCount; i++)
                {
                    // Calculate the position for each projectile
                    Vector3 spawnPosition = startPosition + perpendicular * (spacing * i);

                    // Instantiate the projectile at the calculated world position
                    GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
                    projectile.GetComponent<NmyLinearProjectile>().speed = projectileSpeed;
                    projectile.GetComponent<NmyLinearProjectile>().damage = projectileDamage;
                    projectile.GetComponent<NmyLinearProjectile>().target = playerPosition;
                }
            }
            else
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                // Todo change sript to linear projectile enemy
                projectile.GetComponent<NmyLinearProjectile>().speed = projectileSpeed;
                projectile.GetComponent<NmyLinearProjectile>().damage = projectileDamage;
                projectile.GetComponent<NmyLinearProjectile>().target = playerPosition;
            }
        }

        public override void Move()
        {
            // Debug.Log(transform.position);
            // Forward movement in the X-Y plane
            Vector3 forwardMovement = moveDirection * movementSpeed * Time.deltaTime;

            // Sinusoidal oscillation perpendicular to the movement direction
            float oscillationOffset = Mathf.Sin(Time.time * frequency) * amplitude;
            Vector3 sinusoidalMovement = perpendicularDirection * oscillationOffset;

            // Calculate the new position
            Vector3 newPosition = transform.position + forwardMovement + sinusoidalMovement;
            // Lock the Z-coordinate to 0
            newPosition.z = 0.0f;
            // Apply the new position
            transform.position = newPosition;
            // Debug.Log(transform.position);
        }
    }
}