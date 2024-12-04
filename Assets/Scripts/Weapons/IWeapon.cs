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

        private Vector2 _playerLimits;
        private Camera _mainCamera;
        
        private AudioManager _audioManager;
        
        private bool isEnemyOnScreen()
        {
            // get all enemies and see if they are into the camera view
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Nmy");
            
            foreach (GameObject enemy in enemies)
            {
                Vector3 enemyPos = enemy.transform.position;
                if (enemyPos.x < _playerLimits.x - 0.5f && enemyPos.x > -_playerLimits.x + 0.5f && enemyPos.y < _playerLimits.y - 0.5f && enemyPos.y > -_playerLimits.y + 0.5f)
                    return true;
            }

            return false;
        }
        
        void Start()
        {
            _audioManager = AudioManager.Instance;
            _mainCamera = Camera.main;
            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            _playerLimits = new Vector2(sizeX, sizeY);
            
            StartCoroutine(ShootOnCooldown());
        }

        public void _Downgrade()
        {
            if (_currentLevel > 0)
                _currentLevel--;
            
        }
        
        public abstract void Shoot();

        protected IEnumerator ShootOnCooldown()
        {
            while (true)
            {
                Shoot();
                _audioManager.playPlayerSound(sound);
                if (isEnemyOnScreen()) 
                    Shoot();
                yield return new WaitForSeconds(1f / upgrades[_currentLevel].fireRate);
            }
        }

        public void LevelUp()
        {
            _currentLevel++;
            if(_audioManager == null)
                _audioManager = AudioManager.Instance;
            _currentLevel = _currentLevel > 5 ? 5 : _currentLevel;
            // restart the coroutine to resume shooting
            StartCoroutine(ShootOnCooldown());
            // Shoot();
        }
    }
}
