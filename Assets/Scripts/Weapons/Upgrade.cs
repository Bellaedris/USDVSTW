using System;
using UnityEngine;

namespace usd.Weapons
{
    [RequireComponent(typeof(BoxCollider))]
    public class Upgrade : MonoBehaviour
    {
        [HideInInspector]
        public bool hasBeenPickedUp;
        
        public int weaponID; 
        
        void Start()
        {
            hasBeenPickedUp = false;
        }
    }
}
