using System.Collections;
using UnityEngine;

namespace usd.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public int numberOfProjectile;
        public float damage;
        public float projectileSpeed;
        public float projectileCooldown;
        public float projectileDuration;
        public GameObject projectilePrefab;

        public WeaponLevel[] upgrades;

        public int _currentLevel;
        
        void Start()
        {
            StartCoroutine(ShootOnCooldown());
        }
        
        public abstract void Shoot();

        private IEnumerator ShootOnCooldown()
        {
            yield return new WaitForSeconds(1f / upgrades[_currentLevel].fireRate);
            while (true)
            {
                Shoot();
                yield return new WaitForSeconds(1f / upgrades[_currentLevel].fireRate);
            }
        }
    }
}
