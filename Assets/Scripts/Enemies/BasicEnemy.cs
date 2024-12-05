using System.Collections.Generic;
using UnityEngine;

namespace usd.Enemies
{
    /// <summary>
    /// Abstract class representing a basic enemy in the game.
    /// </summary>
    public abstract class BasicEnemy : MonoBehaviour
    {
        [Header("Enemy Stats")]
        /// <summary>
        /// The health of the enemy
        /// </summary>
        public float health;

        /// <summary>
        /// The movement speed of the enemy
        /// </summary>
        public float movementSpeed;

        /// <summary>
        /// The speed of the enemy's projectiles
        /// </summary>
        public int projectileSpeed;

        /// <summary>
        /// The damage dealt by the enemy's projectiles
        /// </summary>
        public int projectileDamage;

        /// <summary>
        /// The rate at which the enemy fires
        /// </summary>
        public float fireRate;

        /// <summary>
        /// The size of the line of shooting
        /// </summary>
        public float fireLineSize;

        /// <summary>
        /// The number of projectiles fired by the enemy
        /// </summary>
        public int fireProjectilesCount;

        /// <summary>
        /// The prefab for the enemy's projectiles
        /// </summary>
        public GameObject projectilePrefab;

        /// <summary>
        /// The score value awarded for defeating the enemy.
        /// </summary>
        [Header("Enemy Loot")]
        public int scoreValue;

        /// <summary>
        /// The list of prefabs that the enemy can drop as loot
        /// </summary>
        public List<GameObject> dropPrefab;

        /// <summary>
        /// The list of drop rates corresponding to each loot prefab
        /// </summary>
        public List<float> dropRate;

        /// <summary>
        /// The particles to be instantiated upon the enemy's death
        /// </summary>
        [Header("Misc")] 
        public GameObject deathParticles;

        /// <summary>
        /// The sound to be played upon the enemy's death
        /// </summary>
        public AudioClip deathSound;
        
        /// <summary>
        /// Reference to the main camera
        /// </summary>
        private Camera _mainCamera;

        /// <summary>
        /// The bounds within which the enemy can move
        /// </summary>
        [HideInInspector]
        public Bounds limits;

        /// <summary>
        /// The limits within which the enemy can shoot
        /// </summary>
        [HideInInspector]
        public Vector2 shootLimits;

        /// <summary>
        /// Indicates whether the enemy has died
        /// </summary>
        private bool hasDied;
        
        /// <summary>
        /// Reference to the Player object
        /// </summary>
        protected GameObject player;

        /// <summary>
        /// The position of the Player
        /// </summary>
        protected Vector3 playerPosition;

        /// <summary>
        /// The time enemy last shot
        /// </summary>
        protected float timeLastShot;
        
        /// <summary>
        /// Reference to the UI manager
        /// </summary>
        protected UIManager _uiManager;

        /// <summary>
        /// Indicates whether the game is over
        /// </summary>
        protected bool _isGameOver;

        void Start()
        {
            // Initializes the enemy, setting its bounds and shoot limits
            _mainCamera = Camera.main;
            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            limits = new Bounds(_mainCamera.transform.position, new Vector3(sizeX * 2, sizeY * 2, 0) + new Vector3(4.0f, 4.0f, 0));
            sizeY = _mainCamera.orthographicSize;
            sizeX = sizeY * _mainCamera.aspect;
            shootLimits = new Vector2(sizeX + 1.0f, sizeY + 1.0f);
            hasDied = false;
        }
        
        void Update()
        {
            
        }
                
        /////////////////////////////////////////// Specific per Enemy Methods ///////////////////////////////////////////
        
        /// <summary>
        /// Determines whether the enemy can shoot
        /// </summary>
        /// <returns>True if the enemy can shoot, otherwise false</returns>
        public virtual bool CanShoot() { return true; }
        
        /// <summary>
        /// Causes the enemy to shoot
        /// </summary>
        public virtual void Shoot() { }

        /// <summary>
        /// Causes the enemy to move
        /// </summary>
        public virtual void Move() { }
        
        /////////////////////////////////////////// Common Rotating, Taking Damage and Dying Methods ///////////////////////////////////////////
        
        /// <summary>
        /// Rotates the enemy towards the player
        /// </summary>
        /// <param name="x_offset">The offset in the x direction.</param>
        /// <param name="z_offset">The offset in the z direction.</param>
        public void RotateEntityTowardsPlayer(float x_offset, float z_offset)
        {
            // Get the direction from this object to the player
            Vector3 directionToPlayer = player.transform.position - transform.position;

            // Calculate the angle in degrees (atan2 handles direction correctly)
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            // Apply the rotation around the Z-axis
            transform.rotation = Quaternion.Euler(x_offset, 0, -angle + z_offset);
        }
        
        /// <summary>
        /// Rotates the enemy towards a specified direction.
        /// </summary>
        /// <param name="x_offset">The offset in the x direction.</param>
        /// <param name="z_offset">The offset in the z direction.</param>
        /// <param name="direction">The direction to rotate towards.</param>
        public void RotateEntityTowardsDirection(float x_offset, float z_offset, Vector3 direction)
        {
            // Calculate the angle in degrees (atan2 handles direction correctly)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation around the Z-axis
            transform.rotation = Quaternion.Euler(x_offset, 0, -angle + z_offset);
        }
        
        /// <summary>
        /// Causes the enemy to take damage
        /// </summary>
        /// <param name="damageTaken">The amount of damage taken.</param>
        public void TakeDamage(float damageTaken)
        {
            if (_isGameOver)
                return;
            
            if (gameObject.GetComponent<SwarmUnit>() != null)
            {
                gameObject.GetComponent<SwarmUnit>().TakeDamage(damageTaken);
            }
            else
            {
                health -= damageTaken;
                if (health <= 0)
                {
                    if (!hasDied)
                        player.GetComponent<PlayerController>()._addScore(scoreValue);
                    hasDied = true;
                    Die();
                }
            }
        }

        /// <summary>
        /// Causes the enemy to die
        /// </summary>
        public void Die()
        {
            DieWithParticles();
        }
        
        /// <summary>
        /// Causes the enemy to die with particles sfx
        /// </summary>
        private void DieWithParticles()
        {
            // yield return DropLoot();
            DropLoot();
            Destroy(gameObject);
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            AudioManager.Instance.playGeneralSound(deathSound);
        }
        
        /// <summary>
        /// Causes the enemy to drop loot.
        /// </summary>
        private void DropLoot()
        {
            // Drop loot before death
            var hasDropped = false;
            var dropRateCum = 0.0f;
            for (int i = 0; i < dropPrefab.Count; i++)
            {
                dropRateCum += dropRate[i];
                if (Random.Range(0f, 1f) < dropRateCum && !hasDropped)
                {
                    Instantiate(dropPrefab[i], transform.position - Vector3.forward * 0.3f, Quaternion.identity);
                    hasDropped = true;
                }
            }
        }

        /// <summary>
        /// Handles the game over event
        /// </summary>
        protected void OnGameOver()
        {
            _isGameOver = true;
        }
    }
}