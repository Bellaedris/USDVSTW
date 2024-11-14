using System.Collections.Generic;
using UnityEngine;
using usd.Weapons;

namespace usd
{
    public class PlayerController : MonoBehaviour
    {
        private List<Weapon> _weapons;
        
        // Start is called before the first frame update
        void Start()
        {
            _weapons = new List<Weapon>(transform.GetComponentsInChildren<Weapon>());
        }

        // Update is called once per frame
        void Update()
        {
            // follow the mouse position
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 lookDirection = Quaternion.Euler(0, 0, 90) * (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

            transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: lookDirection);
          
        }
    }
}
