using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using usd.Enemies.Projectiles;

namespace usd.Enemies
{
    // Todo Abstract class
    public abstract class BasicEnemy : MonoBehaviour
    {
        [Header("Enemy Stats")]
        public float health;
        public float movementSpeed;
        public int projectileSpeed;
        public int projectileDamage;
        public float fireRate;
        public float fireLineSize;
        public int fireProjectilesCount;
        public GameObject projectilePrefab;
        [Header("Enemy Loot")]
        public int scoreValue;
        public List<GameObject> dropPrefab;
        public List<float> dropRate;
        [Header("Misc")] 
        public GameObject deathParticles;
        public AudioClip deathSound;
        
        private Camera _mainCamera;
        [HideInInspector]
        public Bounds limits;
        [HideInInspector]
        public Vector2 shootLimits;
        
        protected GameObject player;
        protected Vector3 playerPosition;
        protected float timeLastShot;
        void Start()
        {
            _mainCamera = Camera.main;
            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            limits = new Bounds(_mainCamera.transform.position, new Vector3(sizeX * 2, sizeY * 2, 0) + new Vector3(4.0f, 4.0f, 0));
            sizeY = _mainCamera.orthographicSize;
            sizeX = sizeY * _mainCamera.aspect;
            shootLimits = new Vector2(sizeX + 1.0f, sizeY + 1.0f);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
                
        /////////////////////////////////////////// Specific per Enemy Methods ///////////////////////////////////////////
        public virtual bool CanShoot() { return true; }
        
        public virtual void Shoot() { }

        public virtual void Move() { }
        
        /////////////////////////////////////////// Common Rotating, Taking Damage and Dying Methods ///////////////////////////////////////////
        
        public void RotateEntityTowardsPlayer(float x_offset, float z_offset)
        {
            // Get the direction from this object to the player
            Vector3 directionToPlayer = player.transform.position - transform.position;

            // Calculate the angle in degrees (atan2 handles direction correctly)
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            // Apply the rotation around the Z-axis
            transform.rotation = Quaternion.Euler(x_offset, 0, -angle + z_offset);
        }
        
        public void RotateEntityTowardsDirection(float x_offset, float z_offset, Vector3 direction)
        {
            // Calculate the angle in degrees (atan2 handles direction correctly)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation around the Z-axis
            transform.rotation = Quaternion.Euler(x_offset, 0, -angle + z_offset);
        }
        
        public void TakeDamage(float damageTaken)
        {
            if (gameObject.GetComponent<SwarmUnit>() != null)
            {
                gameObject.GetComponent<SwarmUnit>().TakeDamage(damageTaken);
            }
            else
            {
                health -= damageTaken;
                if (health <= 0)
                {
                    Die();
                    // TODO animation
                    player.GetComponent<PlayerController>()._addScore(scoreValue);
                }
            }
        }

        public void Die()
        {
            StartCoroutine(DieCoroutine());
        }
        
        private IEnumerator DieCoroutine()
        {
            yield return DropLoot();
            Destroy(gameObject);
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            AudioManager.Instance.playGeneralSound(deathSound);
        }
        
        private IEnumerator DropLoot()
        {
            yield return new WaitForSeconds(0.75f);
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
    }
}
