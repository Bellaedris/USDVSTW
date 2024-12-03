using System.Collections;
using UnityEngine;

namespace usd.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public AudioClip sound;
        public int weaponID;
        public GameObject projectilePrefab;
        public WeaponLevel[] upgrades;
        public int _currentLevel;
        
        private AudioManager _audioManager;
        
        void Start()
        {
            _audioManager = AudioManager.Instance;
            StartCoroutine(ShootOnCooldown());
        }
        
        public abstract void Shoot();

        protected IEnumerator ShootOnCooldown()
        {
            while (true)
            {
                Shoot();
                _audioManager.playPlayerSound(sound);
                yield return new WaitForSeconds(1f / upgrades[_currentLevel].fireRate);
            }
        }

        public void LevelUp()
        {
            _currentLevel++;
            if(_audioManager == null)
                _audioManager = AudioManager.Instance;
            // restart the coroutine to resume shooting
            StartCoroutine(ShootOnCooldown());
        }
    }
}
