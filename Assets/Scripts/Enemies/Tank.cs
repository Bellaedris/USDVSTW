using UnityEngine;
using UnityEngine.UIElements;
using usd.Enemies.Projectiles;

namespace usd.Enemies
{
    public class Tank : BasicEnemy
    {
        void Start()
        {
            timeLastShot = 0.0f;
            player = GameObject.Find("player");
            playerPosition = player.transform.position;
        }
        
        void Update()
        {
            timeLastShot += Time.deltaTime;
            playerPosition = player.transform.position;
            
            // Move towards player
            Move();
            // Look at player
            RotateEntityTowardsPlayer(180.0f, 90.0f);
            
            // // Shoot if possible
            if (limits.Contains(transform.position) && CanShoot())
            {
                Shoot();
            }
            else if (!limits.Contains(transform.position))
            {
                Destroy(gameObject);
            }
        }
        
        // Specific Methods
        public override bool CanShoot()
        {
            if (timeLastShot >= 1.0f/fireRate)
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
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, movementSpeed * Time.deltaTime);
        }
    }
}