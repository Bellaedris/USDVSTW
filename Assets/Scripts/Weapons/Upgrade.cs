using System;
using UnityEngine;

namespace usd.Weapons
{
    /// <summary>
    /// An upgrade. Only wraps the weapon ID and a boolean to ensure the upgrade can only be picked up once before being destroyed.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Upgrade : MonoBehaviour
    {
        [HideInInspector]
        public bool hasBeenPickedUp = false;
        
        public int weaponID; 
    }
}
