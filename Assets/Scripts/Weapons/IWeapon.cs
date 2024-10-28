using UnityEngine;

namespace usd.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public int numberOfProjectile;
        public float damage;
        public float projectileSpeed;
        public float projectileCooldown;
        public GameObject projectilePrefab;
        
        void Start()
        {
            InvokeRepeating("Shoot", 0, projectileCooldown);
        }
        
        public abstract void Shoot();
    }
}
