using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using usd.Weapons.Projectiles;

namespace usd.Ennemies
{
    // Todo Abstract class
    public class BasicEnnemy : MonoBehaviour
    {
        private GameObject player;
        private Vector3 playerPosition;
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
        
        private float timeLastShot;
        void Start()
        {
            timeLastShot = 0.0f;
            player = GameObject.Find("player");
            playerPosition = player.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            timeLastShot += Time.deltaTime;
            playerPosition = player.transform.position;
            
            // Move towards player
            Debug.Log("Moving");
            Move();
            
            // // Shoot if possible
            Debug.Log(limits.Contains(transform.position));
            if (limits.Contains(transform.position) && CanShoot())
            {
                Shoot();
            }
            else if (!limits.Contains(transform.position))
            {
                Destroy(gameObject);
            }
        }
        
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
            Debug.Log("Dying");
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

        private bool CanShoot()
        {
            if (timeLastShot >= 1.0f/fireRate)
            {
                timeLastShot = 0.0f;
                return true;
            }
            
            return false;
        }
        
        public void Shoot()
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
                Debug.Log("Shooting");
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                // Todo change sript to linear projectile enemy
                projectile.GetComponent<NmyLinearProjectile>().speed = projectileSpeed;
                projectile.GetComponent<NmyLinearProjectile>().damage = projectileDamage;
                projectile.GetComponent<NmyLinearProjectile>().target = playerPosition;
            }
        }

        public void Move()
        {
            Debug.Log("Moving to player : " + playerPosition);
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, movementSpeed * Time.deltaTime);            
        }
    }
}
