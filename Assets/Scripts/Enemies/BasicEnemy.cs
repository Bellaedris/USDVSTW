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
        [SerializeField] public int health;
        
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
        
        /////////////////////////////////////////// Common Taking Damage and Dying Methods ///////////////////////////////////////////
        public void TakeDamage(int damageTaken)
        {
            health -= damageTaken;
            if (health <= 0)
            {
                Die();
                // TODO +=scoreValue
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
