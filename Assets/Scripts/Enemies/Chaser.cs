using System;
using UnityEngine;
using usd.Enemies.Projectiles;

namespace usd.Enemies
{
    /// <summary>
    /// Represents a chaser enemy that follows the player and shoots projectiles.
    /// </summary>
    public class Chaser : BasicEnemy
    {
        void Start()
        {
            // Initializes the chaser enemy, setting its difficulty and initial state.

            
            // Difficulty scaling every 5 waves
            int difficultyModifier = UIManager.Instance.difficultyModifier;
            if (difficultyModifier > 0)
            {
                difficultyModifier = Math.Min(difficultyModifier, 10);
                var difficultyRatio = difficultyModifier / 5.0f;
                health += health * difficultyRatio;
                movementSpeed += movementSpeed * difficultyRatio;
                projectileSpeed += (int) (projectileSpeed * difficultyRatio);
                projectileDamage += (int) (projectileDamage * difficultyRatio);
                fireRate += fireRate * difficultyRatio;
            }
            
            timeLastShot = 0.0f;
            player = GameObject.Find("player");
            playerPosition = player.transform.position;
            
            _uiManager = FindObjectOfType<UIManager>();
            if (_uiManager != null)
                _uiManager.gameOver += OnGameOver;
        }

        void Update()
        {
            // Updates the chaser enemy's state, moving towards the player and shooting if possible.
            
            if (_isGameOver)
                return;
            
            timeLastShot += Time.deltaTime;
            playerPosition = player.transform.position;
            
            // Move towards player
            Move();
            
            // Look at player
            RotateEntityTowardsPlayer(180.0f, 90.0f);
            
            // Shoot if possible
            if (!(transform.position.x > shootLimits.x && transform.position.x < -shootLimits.x && transform.position.y > shootLimits.y && transform.position.y < -shootLimits.y) 
                && CanShoot())
            {
                Shoot();
            }
        }
        
        /// <summary>
        /// Determines whether the chaser enemy can shoot.
        /// </summary>
        /// <returns>True if the chaser enemy can shoot, otherwise false</returns>
        public override bool CanShoot()
        {
            if (timeLastShot >= 1.0f / fireRate)
            {
                timeLastShot = 0.0f;
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Causes the chaser enemy to shoot projectiles towards the player
        /// </summary>
        public override void Shoot()
        {   
            // Todo maybe remove fireLine for basic enemy
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
                projectile.GetComponent<NmyLinearProjectile>().speed = projectileSpeed;
                projectile.GetComponent<NmyLinearProjectile>().damage = projectileDamage;
                projectile.GetComponent<NmyLinearProjectile>().target = playerPosition;
            }
        }

        /// <summary>
        /// Moves the chaser enemy towards the player
        /// </summary>
        public override void Move()
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, playerPosition, movementSpeed * Time.deltaTime);
            newPosition.z = 0.0f;
            transform.position = newPosition;
        }
    }
}