using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using usd.Enemies.Projectiles;

namespace usd.Enemies
{
    public class SwarmUnit : BasicEnemy
    {
        private SwarmSpawner spawnerReference;
        private Vector3 moveDirection;
        
        public void SetMoveDrection(Vector3 direction)
        {
            moveDirection = direction;
        }
        
        void Start()
        {
            timeLastShot = 0.0f;
            player = GameObject.Find("player");
            playerPosition = player.transform.position;
            
            _uiManager = FindObjectOfType<UIManager>();
            if (_uiManager != null)
                _uiManager.gameOver += OnGameOver;
        }
        
        public void InitializeValues(SwarmSpawner spawner)
        {
            // Set the player reference to the player reference of the spawner
            // Initialize all public fields
            health = spawner.health;
            limits = spawner.limits;
            movementSpeed = spawner.movementSpeed;
            projectileSpeed = spawner.projectileSpeed;
            projectileDamage = spawner.projectileDamage;
            fireRate = spawner.fireRate;
            fireLineSize = spawner.fireLineSize;
            fireProjectilesCount = spawner.fireProjectilesCount;
            projectilePrefab = spawner.projectilePrefab;
            scoreValue = spawner.scoreValue;
            dropPrefab = new List<GameObject>(spawner.dropPrefab);
            dropRate = new List<float>(spawner.dropRate);
            spawnerReference = spawner;
        }
        void Update()
        {
            if (_isGameOver)
                return;
            
            timeLastShot += Time.deltaTime;
            playerPosition = player.transform.position;
            
            // Move towards player
            Move();
            // Look at player
            RotateEntityTowardsPlayer(180.0f, 90.0f);
            
            // // Shoot if possible
            if (!(transform.position.x > shootLimits.x && transform.position.x < -shootLimits.x && transform.position.y > shootLimits.y && transform.position.y < -shootLimits.y) 
                && CanShoot())
            {
                Shoot();
            }
            // else if (!limits.Contains(transform.position))
            // {
            //     Destroy(gameObject);
            //     spawnerReference.RemoveUnit(this);
            // }
        }
        
        // Override take damage and die methods
        public new void TakeDamage(float damageTaken)
        {
            health -= damageTaken;
            if (health <= 0)
            {
                // TODO animation
                Destroy(gameObject);
                spawnerReference.RemoveUnit(this);
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
                projectile.GetComponent<NmyLinearProjectile>().speed = projectileSpeed;
                projectile.GetComponent<NmyLinearProjectile>().damage = projectileDamage;
                projectile.GetComponent<NmyLinearProjectile>().target = playerPosition;
            }
        }

        public override void Move()
        {
            // Move according to swarm direction
            Vector3 newPosition = transform.position + moveDirection * movementSpeed * Time.deltaTime;
            newPosition.z = 0.0f;
            transform.position = newPosition;
        }
    }
}