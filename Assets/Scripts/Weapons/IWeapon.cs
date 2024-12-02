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

        public void _Downgrade()
        {
            if (_currentLevel > 1)
                _currentLevel--;
            
        }
        
        public abstract void Shoot();

        protected IEnumerator ShootOnCooldown()
        {
            while (true)
            {
                Debug.Log("WEE");
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
