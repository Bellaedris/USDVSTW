using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using usd.Weapons;
using Random = UnityEngine.Random;

namespace usd
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 1f;
        public float invulnerabilityLength;

        private Weapon _currentWeapon;
        private Weapon[] _weapons;
        private Vector2 _playerLimits;
        private Camera _mainCamera;
        
        private bool canBeHit;
        private int score;
        
        public int _getScore()
        {
            return score;
        }
        
        public void _addScore(int points)
        {
            score += points;
            UIManager.Instance.DisplayScore(score);
        }
        
        public Weapon[] _getWeapons()
        {
            return _weapons;
        }
        
        public Weapon _getCurrentWeapon()
        {
            return _currentWeapon;
        }
        
        public void _DowngradeWeapons()
        {
            foreach (Weapon weapon in _weapons)
            {
                weapon._Downgrade();
                // Debug.Log(weapon._currentLevel);
            }
            
            UIManager.Instance.UpdateLevelsOnUI(_weapons[0]._currentLevel, _weapons[1]._currentLevel, _weapons[2]._currentLevel);
            // foreach (var weapon2 in _weapons)
            // {
            //     Debug.Log("ID : " + weapon2.weaponID + "--Cur lvl : " + weapon2._currentLevel);
            // }
            // Debug.Log("---------------------------------------------------------------------");
        }

        public void RegisterHit()
        {
            if (canBeHit)
            {      
                if (CheckGameOver())
                {
                    //TODO Game Manager call game over
                    Debug.Log("Game Over");
                }
                else
                {
                    StartCoroutine(DoInvulnerability());
                    score = Math.Max(0, score - 1000);
                    UIManager.Instance.DisplayScore(score);
                    _DowngradeWeapons();
                }
            }
        }
        
        public bool CheckGameOver()
        {
            foreach (Weapon weapon in _weapons)
            {
                if (weapon._currentLevel > 0)
                    return false;
            }
            return true;
        }
        
        IEnumerator DoInvulnerability()
        {
            canBeHit = false;
            var hitTime = Time.time;
            // Blink invulnerabilityLength seconds
            while (Time.time - hitTime < invulnerabilityLength)
            {
                GetComponentInChildren<MeshRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
                GetComponentInChildren<MeshRenderer>().enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
            canBeHit = true;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = Camera.main;
            _weapons = transform.GetComponentsInChildren<Weapon>(true);
            _currentWeapon = _weapons[Random.Range(0, _weapons.Length)];
            _currentWeapon.gameObject.SetActive(true);
            UIManager.Instance.SwitchWeapon(_currentWeapon.weaponID, _currentWeapon._currentLevel);

            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            _playerLimits = new Vector2(sizeX, sizeY);
            
            canBeHit = true;
            score = 0;
            UIManager.Instance.DisplayScore(score);
        }

        // Update is called once per frame
        void Update()
        {
            // follow the mouse position
            Vector3 lookDirection = Quaternion.Euler(0, 0, 90) * (_mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position);

            transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: lookDirection);
            
            // move in the limits of the terrain
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.Translate(speed * Time.deltaTime * input, Space.World);
            
            if(Input.GetKeyDown(KeyCode.Escape))
                UIManager.Instance.TogglePauseMenu();
            
            Vector3 newPos = transform.position;
            newPos.x = Mathf.Clamp(transform.position.x, -_playerLimits.x, _playerLimits.x);
            newPos.y = Mathf.Clamp(transform.position.y, -_playerLimits.y, _playerLimits.y);
            
            transform.position = newPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Upgrade"))
            {
                // change weapon to the one associated with the upgrade, then increment its level
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
                }
            } 
            else if (other.CompareTag("Nmy_Projectile"))
            {
                // Debug.Log(name + " hit by " + other.name);
                //TODO trigger ally hit animation
                RegisterHit();
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Nmy"))
            {
                // Debug.Log(name + " hit by " + other.name);
                //TODO trigger ally hit animation
                RegisterHit();
            }
        }
    }
}
