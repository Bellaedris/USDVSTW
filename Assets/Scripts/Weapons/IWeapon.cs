using System.Collections;
using UnityEngine;

namespace usd.Weapons
{
    /// <summary>
    /// Generic weapon abstract class
    /// </summary>
    public abstract class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Sound effect for the weapon
        /// </summary>
        public AudioClip sound;

        /// <summary>
        /// Unique identifier for the weapon
        /// </summary>
        public int weaponID;

        /// <summary>
        /// Prefab for the projectile fired by the weapon
        /// </summary>
        public GameObject projectilePrefab;

        /// <summary>
        /// WeaponLevel is an array of scriptable object containing the data of all of the weapon's levels
        /// </summary>
        public WeaponLevel[] upgrades;

        /// <summary>
        /// Current level of the weapon
        /// </summary>
        public int _currentLevel;

        /// <summary>
        /// Limits of the player's movement area
        /// </summary>
        private Vector2 _playerLimits;

        /// <summary>
        /// Reference to the main camera
        /// </summary>
        private Camera _mainCamera;

        /// <summary>
        /// Reference to the audio manager
        /// </summary>
        private AudioManager _audioManager;

        /// <summary>
        /// Checks if any enemies are currently on the screen.
        /// </summary>
        /// <returns>True if an enemy is on the screen, otherwise false.</returns>
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
            
            // Shooting Coroutine
            StartCoroutine(ShootOnCooldown());
        }

        /// <summary>
        /// Downgrades the weapon to the previous level
        /// </summary>
        public void _Downgrade()
        {
            if (_currentLevel > 0)
                _currentLevel--;
        }

        /// <summary>
        /// Abstract method to shoot the weapon. Must be implemented by derived classes
        /// </summary>
        public abstract void Shoot();

        /// <summary>
        /// Coroutine to constantly shoot the weapon at its fire rate frequency
        /// </summary>
        /// <returns>An IEnumerator for the coroutine.</returns>
        protected IEnumerator ShootOnCooldown()
        {
            while (true)
            {
                // Shoot();
                if (isEnemyOnScreen())
                {
                    _audioManager.playWeaponSound(sound);
                    Shoot(); 
                }
                yield return new WaitForSeconds(1f / upgrades[_currentLevel].fireRate);
            }
        }

        /// <summary>
        /// Levels up the weapon to the next level
        /// </summary>
        public void LevelUp()
        {
            _currentLevel++;
            if(_audioManager == null)
                _audioManager = AudioManager.Instance;
            _currentLevel = _currentLevel > 5 ? 5 : _currentLevel;
            // restart the coroutine to resume shooting
            StartCoroutine(ShootOnCooldown());
        }
    }
}