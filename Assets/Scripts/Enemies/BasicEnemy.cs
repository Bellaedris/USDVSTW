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
        [SerializeField] public float health;
        
        [SerializeField] public Bounds limits;
        [SerializeField] public float movementSpeed;
        
        [SerializeField] public int projectileSpeed;
        [SerializeField] public int projectileDamage;
        [SerializeField] public float fireRate;
        [SerializeField] public float fireLineSize;
        [SerializeField] public int fireProjectilesCount;
        [SerializeField] public GameObject projectilePrefab;
        
        [SerializeField] public int scoreValue;
        
        // TODO Order refabs and droprates from highest to lowest
        [SerializeField] public List<GameObject> dropPrefab;
        [SerializeField] public List<float> dropRate;
        
        protected GameObject player;
        protected Vector3 playerPosition;
        protected float timeLastShot;
        void Start()
        {

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
            health -= damageTaken;
            if (health <= 0)
            {
                Die();
                // TODO +=scoreValue and animation
            }
        }

        public void Die()
        {
            // Drop loot before death
            var hasDropped = false;
            for (int i = 0; i < dropPrefab.Count; i++)
            {
                if (Random.Range(0f, 1f) < dropRate[i] && !hasDropped)
                {
                    Instantiate(dropPrefab[i], transform.position, Quaternion.identity);
                    hasDropped = true;
                }
            }
            Destroy(gameObject);
        }
    }
}
