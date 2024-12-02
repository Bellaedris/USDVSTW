using System;
using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("Score : " + score);
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
        }

        public void RegisterHit()
        {
            if (canBeHit)
            {      
                Debug.Log("Hit registered");
                _DowngradeWeapons();
                StartCoroutine(DoInvulnerability());
            }
        }
        
        IEnumerator DoInvulnerability()
        {
            canBeHit = false;
            var hitTime = Time.time;
            // Blink invulnerabilityLength seconds
            Debug.Log(Time.time - hitTime);
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

            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            _playerLimits = new Vector2(sizeX, sizeY);
            
            canBeHit = true;
            score = 0;
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
            
            Vector3 newPos = transform.position;
            newPos.x = Mathf.Clamp(transform.position.x, -_playerLimits.x, _playerLimits.x);
            newPos.y = Mathf.Clamp(transform.position.y, -_playerLimits.x, _playerLimits.y);
            
            transform.position = newPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Upgrade"))
            {
                var upgrade = other.GetComponent<Upgrade>();
                _currentWeapon.gameObject.SetActive(false);
                _currentWeapon = _weapons[upgrade.weaponID - 1];
                _currentWeapon.gameObject.SetActive(true);
                _currentWeapon.LevelUp();
                _currentWeapon.Shoot();
                
                Destroy(other.gameObject);
            } 
            else if (other.CompareTag("Nmy"))
            {
                RegisterHit();
            }
        }
    }
}
