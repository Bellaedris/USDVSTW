using System;
using System.Collections.Generic;
using UnityEngine;
using usd.Enemies.Projectiles;
using Random = UnityEngine.Random;

namespace usd.Enemies
{
    /// <summary>
    /// Spawns and manages a swarm of enemy units
    /// </summary>
    public class SwarmSpawner : BasicEnemy
    {
        /// <summary>
        /// The number of units in the swarm
        /// </summary>
        public int numberOfUnits;

        /// <summary>
        /// The prefab for the swarm units
        /// </summary>
        public GameObject unitPrefab;
        
        /// <summary>
        /// The list of swarm units
        /// </summary>
        private List<GameObject> swarmUnits;

        /// <summary>
        /// The current movement direction of the swarm
        /// </summary>
        private Vector3 moveDirection;

        /// <summary>
        /// Indicates whether the spawner has dropped loot
        /// </summary>
        private bool hasDropped;

        /// <summary>
        /// Gets the current movement direction of the swarm
        /// </summary>
        /// <returns>The current movement direction.</returns>
        public Vector3 GetMoveDirection()
        {
            return moveDirection;
        }

        /// <summary>
        /// Calculates a random direction for the swarm to move in
        /// </summary>
        public void CalculateRandomDirection()
        {
            if (_isGameOver)
            {
                CancelInvoke("CalculateRandomDirection");
                return;
            }
            
            // Calculate direction towards player position
            Vector3 playerDirection = (player.transform.position - transform.position).normalized;

            // Determine possible directions based on player position
            List<Vector3> directions = new List<Vector3>();

            if (playerDirection.x < 0)
            {
                directions.Add(Vector3.left);
                directions.Add(new Vector3(-1, 1, 0).normalized);
                directions.Add(new Vector3(-1, -1, 0).normalized);
            }
            else if (playerDirection.x > 0)
            {
                directions.Add(Vector3.right);
                directions.Add(new Vector3(1, 1, 0).normalized);
                directions.Add(new Vector3(1, -1, 0).normalized);
            }

            if (playerDirection.y < 0)
            {
                directions.Add(Vector3.down);
            }
            else if (playerDirection.y > 0)
            {
                directions.Add(Vector3.up);
            }

            moveDirection = directions[Random.Range(0, directions.Count)];

            // Give direction to swarm units
            foreach (var swarmUnit in swarmUnits)
            {
                if (swarmUnit != null)
                    swarmUnit.GetComponent<SwarmUnit>().SetMoveDrection(moveDirection);
            }
        }
        
        /// <summary>
        /// Removes a unit from the swarm.
        /// </summary>
        /// <param name="swarmUnit">The swarm unit to remove.</param>
        public void RemoveUnit(SwarmUnit swarmUnit)
        {
            swarmUnits.Remove(swarmUnit.gameObject);
            numberOfUnits--;
        }
        
        void Start()
        {            
            // Initializes the swarm spawner and spawns the swarm units
            
            // Difficulty scaling every 5 waves
            int difficultyModifier = UIManager.Instance.difficultyModifier;
            if (difficultyModifier > 0)
            {
                difficultyModifier = Math.Min(difficultyModifier, 10);
                var difficultyRatio = difficultyModifier / 5.0f;
                // for scaling number of units : Un+1 = (n+1)²
                for (int i = 0; i < difficultyModifier; i++)
                {
                    numberOfUnits = numberOfUnits + 2 * Mathf.CeilToInt(Mathf.Sqrt(numberOfUnits)) + 1;
                }
                health += health * difficultyRatio;
                projectileSpeed += (int) (projectileSpeed * difficultyRatio);
                projectileDamage += (int) (projectileDamage * difficultyRatio);
                fireRate += fireRate * difficultyRatio;
            }
            
            // Offset position by a fixed value depending on number of units, so that it spawns around the spawner
            player = GameObject.Find("player");
            int unitsPerSide = Mathf.CeilToInt(Mathf.Sqrt(numberOfUnits));
            float spacing = 0.5f;
            swarmUnits = new List<GameObject>();
            
            for (int i = 0; i < numberOfUnits; i++)
            {
                int row = i / unitsPerSide;
                int col = i % unitsPerSide;
                float xOffset = (col - row) * spacing;
                float yOffset = (col + row) * spacing;
                Vector3 offset = new Vector3(xOffset, yOffset, 0);
                GameObject swarmUnit = Instantiate(unitPrefab, transform.position + offset, Quaternion.identity);
                swarmUnits.Add(swarmUnit);
                swarmUnit.GetComponent<SwarmUnit>().InitializeValues(this);
            }
            
            _uiManager = FindObjectOfType<UIManager>();
            if (_uiManager != null)
                _uiManager.gameOver += OnGameOver;
            
            CalculateRandomDirection();
            // Call CalculateRandomDirection every 6 seconds
            InvokeRepeating("CalculateRandomDirection", 0.0f, Random.Range(4.0f, 8.0f));
        }
        
        void Update()
        {
            // Updates the swarm spawner's state

            if (_isGameOver)
                return;
            
            transform.position += moveDirection * (movementSpeed * Time.deltaTime);
            
            if(transform.position.x > shootLimits.x || transform.position.x < -shootLimits.x || transform.position.y > shootLimits.y || transform.position.y < -shootLimits.y)
                CalculateRandomDirection();
            
            // Death of spawner if all units dead
            if (numberOfUnits <= 0 && !hasDropped)
            {
                SpawnerDieAndDrop();
            }
        }

        /// <summary>
        /// Handles the death of the spawner and drops loot.
        /// </summary>
        void SpawnerDieAndDrop()
        {
            // Drop loot before death
            var dropRateCum = 0.0f;
            for (int i = 0; i < dropPrefab.Count; i++)
            {
                dropRateCum += dropRate[i];
                if (Random.Range(0f, 1f) < dropRateCum && !hasDropped)
                {
                    Instantiate(dropPrefab[i], transform.position, Quaternion.identity);
                    hasDropped = true;
                }
            }
            Destroy(gameObject);
            player.GetComponent<PlayerController>()._addScore(scoreValue);
        }
        
        /// <summary>
        /// Overrides the TakeDamage method to prevent the spawner from triggering taking damage events
        /// </summary>
        /// <param name="damageTaken">The amount of damage taken.</param>
        public new void TakeDamage(float damageTaken)
        {
            return;
        }
    }
}