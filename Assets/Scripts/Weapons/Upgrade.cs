using System;
using UnityEngine;

namespace usd.Weapons
{
    [RequireComponent(typeof(BoxCollider))]
    public class Upgrade : MonoBehaviour
    {
        [HideInInspector]
        public bool hasBeenPickedUp = false;
        
        public int weaponID; 
        
        void Start()
        {
        }
    }
}
