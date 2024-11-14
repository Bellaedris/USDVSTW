using System.Collections.Generic;
using UnityEngine;
using usd.Weapons;

namespace usd
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 1f;
        
        private List<Weapon> _weapons;
        private Vector2 _playerLimits;
        
        // Start is called before the first frame update
        void Start()
        {
            _weapons = new List<Weapon>(transform.GetComponentsInChildren<Weapon>());

            float sizeY = Camera.main.orthographicSize;
            float sizeX = sizeY * Camera.main.aspect;
            _playerLimits = new Vector2(sizeX, sizeY);
        }

        // Update is called once per frame
        void Update()
        {
            // follow the mouse position
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 lookDirection = Quaternion.Euler(0, 0, 90) * (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

            transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: lookDirection);
            
            // move in the limits of the terrain
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.Translate(speed * Time.deltaTime * input, Space.World);
            
            Vector3 newPos = transform.position;
            newPos.x = Mathf.Clamp(transform.position.x, -_playerLimits.x, _playerLimits.x);
            newPos.y = Mathf.Clamp(transform.position.y, -_playerLimits.x, _playerLimits.y);
        }
    }
}
