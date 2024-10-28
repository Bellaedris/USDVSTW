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
        
        }
    }
}
