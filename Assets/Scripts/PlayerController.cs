using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using usd.Weapons;
using Random = UnityEngine.Random;

namespace usd
{
    /// <summary>
    /// Controls the player character, including movement, weapon management, and collision handling
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Movement speed of the player
        /// </summary>
        public float speed = 1f;

        /// <summary>
        /// Duration of invulnerability after being hit
        /// </summary>
        public float invulnerabilityLength;

        /// <summary>
        /// Sound played when the player upgrades a weapon
        /// </summary>
        public AudioClip upgradeSound;

        /// <summary>
        /// Sound played when the player takes damage
        /// </summary>
        public AudioClip damageSound;

        /// <summary>
        /// Sound played when the player dies
        /// </summary>
        public AudioClip deathSound;

        /// <summary>
        /// Particle effect played when the player dies
        /// </summary>
        public GameObject deathParticles;

        /// <summary>
        /// The current weapon the player is using
        /// </summary>
        private Weapon _currentWeapon;

        /// <summary>
        /// Array of all weapons the player can use
        /// </summary>
        private Weapon[] _weapons;

        /// <summary>
        /// Limits of the player's movement area
        /// </summary>
        private Vector2 _playerLimits;

        /// <summary>
        /// Reference to the main camera
        /// </summary>
        private Camera _mainCamera;

        /// <summary>
        /// Reference to the player's mesh renderer
        /// </summary>
        private MeshRenderer _meshRenderer;

        /// <summary>
        /// Indicates whether the player can be hit
        /// </summary>
        private bool canBeHit;

        /// <summary>
        /// Indicates whether the game is over
        /// </summary>
        private bool _isGameOver;

        /// <summary>
        /// The player's current score
        /// </summary>
        private int score;

        /// <summary>
        /// Gets the player's current score
        /// </summary>
        /// <returns>The player's current score.</returns>
        public int _getScore()
        {
            return score;
        }

        /// <summary>
        /// Adds points to the player's score and updates the UI
        /// </summary>
        /// <param name="points">The points to add to the score.</param>
        public void _addScore(int points)
        {
            score += points;
            UIManager.Instance.DisplayScore(score);
        }

        /// <summary>
        /// Gets the array of weapons the player can use
        /// </summary>
        /// <returns>An array of weapons</returns>
        public Weapon[] _getWeapons()
        {
            return _weapons;
        }

        /// <summary>
        /// Gets the current weapon the player is using
        /// </summary>
        /// <returns>The current weapon</returns>
        public Weapon _getCurrentWeapon()
        {
            return _currentWeapon;
        }

        /// <summary>
        /// Downgrades all weapons the player has and updates the UI
        /// </summary>
        public void _DowngradeWeapons()
        {
            foreach (Weapon weapon in _weapons)
            {
                weapon._Downgrade();
            }

            UIManager.Instance.UpdateLevelsOnUI(_weapons[0]._currentLevel, _weapons[1]._currentLevel, _weapons[2]._currentLevel);
        }

        /// <summary>
        /// Registers a hit on the player, handling damage, game over, and invulnerability.
        /// </summary>
        public void RegisterHit()
        {
            if (canBeHit)
            {
                if (CheckGameOver())
                {
                    _isGameOver = true;
                    UIManager.Instance.ShowGameOver();
                    Destroy(gameObject);
                    Instantiate(deathParticles, transform.position, Quaternion.identity);
                    AudioManager.Instance.playGeneralSound(deathSound);
                }
                else
                {
                    AudioManager.Instance.playGeneralSound(damageSound);
                    StartCoroutine(DoInvulnerability());
                    score = Math.Max(0, score - 1000);
                    UIManager.Instance.DisplayScore(score);
                    _DowngradeWeapons();
                    AudioManager.Instance.FadeMusic(_currentWeapon.weaponID - 1, _currentWeapon._currentLevel, GetMaxWeaponLevel());
                }
            }
        }

        /// <summary>
        /// Gets the maximum level of all the player's weapons
        /// </summary>
        /// <returns>The maximum weapon level</returns>
        private int GetMaxWeaponLevel()
        {
            int maxWeaponLevel = -1;
            foreach (Weapon w in _weapons)
            {
                maxWeaponLevel = Math.Max(w._currentLevel, maxWeaponLevel);
            }

            return maxWeaponLevel;
        }

        /// <summary>
        /// Checks if the game is over by verifying if all weapons are at level 0
        /// </summary>
        /// <returns>True if the game is over, otherwise false</returns>
        public bool CheckGameOver()
        {
            foreach (Weapon weapon in _weapons)
            {
                if (weapon._currentLevel > 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Coroutine to handle the player's invulnerability period after being hit
        /// </summary>
        /// <returns>An IEnumerator for the coroutine</returns>
        IEnumerator DoInvulnerability()
        {
            canBeHit = false;
            var hitTime = Time.time;
            // Blink for invulnerabilityLength seconds
            while (Time.time - hitTime < invulnerabilityLength)
            {
                GetComponentInChildren<MeshRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                GetComponentInChildren<MeshRenderer>().enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
            canBeHit = true;
        }
        
        void Start()
        {
            _mainCamera = Camera.main;
            _weapons = transform.GetComponentsInChildren<Weapon>(true);
            _meshRenderer = GetComponent<MeshRenderer>();
            _currentWeapon = _weapons[Random.Range(0, _weapons.Length)];
            _currentWeapon.gameObject.SetActive(true);
            AudioManager.Instance.FadeMusic(_currentWeapon.weaponID - 1, _currentWeapon._currentLevel, GetMaxWeaponLevel());
            UIManager.Instance.SwitchWeapon(_currentWeapon.weaponID, _currentWeapon._currentLevel);

            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            _playerLimits = new Vector2(sizeX, sizeY);

            canBeHit = true;
            score = 0;
            UIManager.Instance.DisplayScore(score);
        }
        
        void Update()
        {
            if (_isGameOver)
                return;

            // Follow the mouse position
            Vector3 lookDirection = Quaternion.Euler(0, 0, 90) * (_mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position);

            if (Time.timeScale > 0)
                transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: lookDirection);

            // Move within the limits of the terrain
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.Translate(speed * Time.deltaTime * input, Space.World);

            if (Input.GetKeyDown(KeyCode.Escape))
                UIManager.Instance.TogglePauseMenu();

            Vector3 newPos = transform.position;
            newPos.x = Mathf.Clamp(transform.position.x, -_playerLimits.x, _playerLimits.x);
            newPos.y = Mathf.Clamp(transform.position.y, -_playerLimits.y, _playerLimits.y);

            transform.position = newPos;
        }

        /// <summary>
        /// Handles collision with other objects
        /// </summary>
        /// <param name="other">The collider of the other object.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Upgrade"))
            {
                // Change weapon to the one associated with the upgrade, then increment its level
                var upgrade = other.GetComponent<Upgrade>();
                if (!upgrade.hasBeenPickedUp)
                {
                    var id = upgrade.weaponID;
                    upgrade.hasBeenPickedUp = true;
                    Destroy(other.gameObject);
                    _currentWeapon.gameObject.SetActive(false);
                    _currentWeapon = _weapons[id - 1];
                    _currentWeapon.gameObject.SetActive(true);
                    _currentWeapon.LevelUp();
                    UIManager.Instance.SwitchWeapon(id, _currentWeapon._currentLevel);
                    AudioManager.Instance.playGeneralSound(upgradeSound);
                    AudioManager.Instance.FadeMusic(_currentWeapon.weaponID - 1, _currentWeapon._currentLevel, GetMaxWeaponLevel());
                }
            }
            else if (other.CompareTag("Nmy_Projectile"))
            {
                // Register hit by enemy projectile
                RegisterHit();
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Nmy"))
            {
                // Register hit by enemy
                RegisterHit();
            }
        }
    }
}