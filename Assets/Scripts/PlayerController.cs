using System;
using System.Collections.Generic;
using UnityEngine;
using usd.Weapons;
using Random = UnityEngine.Random;

namespace usd
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 1f;

        private Weapon _currentWeapon;
        private Weapon[] _weapons;
        private Vector2 _playerLimits;
        private Camera _mainCamera;
        
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
                _currentWeapon.LevelUp();
                var upgrade = other.GetComponent<Upgrade>();
                _currentWeapon.gameObject.SetActive(false);
                _currentWeapon = _weapons[upgrade.weaponID - 1];
                _currentWeapon.gameObject.SetActive(true);
                
                Destroy(other.gameObject);
            }
        }
    }
}
