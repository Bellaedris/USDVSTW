﻿using System;
using UnityEngine;
using usd.Enemies.Projectiles;

namespace usd.Enemies
{
    /// <summary>
    /// Represents a flyer enemy that moves in a sinusoidal pattern and shoots projectiles.
    /// </summary>
    public class Flyer : BasicEnemy
    {
        /// <summary>
        /// The height of the sinusoidal wave
        /// </summary>
        public float amplitude;

        /// <summary>
        /// The speed of the sinusoidal wave
        /// </summary>
        public float frequency;
        
        /// <summary>
        /// Direction towards the target (Player)
        /// </summary>
        private Vector3 moveDirection;

        /// <summary>
        /// Perpendicular axis for sinusoidal motion
        /// </summary>
        private Vector3 perpendicularDirection;
        
        void Start()
        {
            // Initializes the flyer enemy, setting its difficulty and initial state
            
            timeLastShot = 0.0f;
            player = GameObject.Find("player");

            // Difficulty scaling every 5 waves
            int difficultyModifier = UIManager.Instance.difficultyModifier;
            if (difficultyModifier > 0)
            {
                difficultyModifier = Math.Min(difficultyModifier, 10);
                var difficultyRatio = difficultyModifier / 5.0f;
                health += health * difficultyRatio;
                projectileSpeed += (int) (projectileSpeed * difficultyRatio);
                projectileDamage += (int) (projectileDamage * difficultyRatio);
                fireRate += fireRate * difficultyRatio;
            }
            
            // Calculate the direction towards the player in X-Y plane (ignore Z)
            CalculateDirection();
            
            _uiManager = FindObjectOfType<UIManager>();
            if (_uiManager != null)
                _uiManager.gameOver += OnGameOver;
        }
        
        void Update()
        {
            // Updates the flyer enemy's state, moving in a sinusoidal pattern and shooting if possible
            
            if (_isGameOver)
                return;
            
            timeLastShot += Time.deltaTime;
            playerPosition = player.transform.position;

            // Move towards player
            Move();
            
            // Look at direction
            RotateEntityTowardsDirection(180.0f, 90.0f, moveDirection);

            // Shoot if possible
            if (!(transform.position.x > shootLimits.x && transform.position.x < -shootLimits.x && transform.position.y > shootLimits.y && transform.position.y < -shootLimits.y) 
                && CanShoot())
            {
                Shoot();
            }
            else if (!limits.Contains(transform.position))
            {
                // Recalculate direction if out of bounds
                CalculateDirection();
            }
        }
        
        /// <summary>
        /// Calculates the direction towards the player and the perpendicular direction for sinusoidal motion.
        /// </summary>
        private void CalculateDirection()
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.z = 0; // Ensure movement is constrained to X-Y plane
            moveDirection = directionToPlayer.normalized;
            
            // Calculate the perpendicular direction for sinusoidal oscillation
            perpendicularDirection = Vector3.Cross(moveDirection, Vector3.forward).normalized;
        }

        /// <summary>
        /// Determines whether the flyer enemy can shoot.
        /// </summary>
        /// <returns>True if the flyer enemy can shoot, otherwise false.</returns>
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
        /// Causes the flyer enemy to shoot projectiles towards the player.
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
        /// Moves the flyer enemy towards the player with sinusoidal oscillation.
        /// </summary>
        public override void Move()
        {
            // Forward movement in the X-Y plane
            Vector3 forwardMovement = moveDirection * movementSpeed * Time.deltaTime;

            // Sinusoidal oscillation perpendicular to the movement direction
            float oscillationOffset = Mathf.Sin(Time.time * frequency) * amplitude;
            Vector3 sinusoidalMovement = perpendicularDirection * oscillationOffset;
            sinusoidalMovement = forwardMovement.magnitude <= 0 ? Vector3.zero : sinusoidalMovement;

            // Calculate the new position
            Vector3 newPosition = transform.position + forwardMovement + sinusoidalMovement;
            // Lock the Z-coordinate to 0
            newPosition.z = 0.0f;
            // Apply the new position
            transform.position = newPosition;
        }
    }
}