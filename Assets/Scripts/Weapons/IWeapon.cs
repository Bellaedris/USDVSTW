using System.Collections;
using UnityEngine;

namespace usd.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public int weaponID;
        public GameObject projectilePrefab;
        public WeaponLevel[] upgrades;
        public int _currentLevel;
        
        void Start()
        {
            StartCoroutine(ShootOnCooldown());
        }
        
        public abstract void Shoot();

        protected IEnumerator ShootOnCooldown()
        {
            while (true)
            {
                Shoot();
                yield return new WaitForSeconds(1f / upgrades[_currentLevel].fireRate);
            }
        }

        public void LevelUp()
        {
            _currentLevel++;
        }
    }
}
