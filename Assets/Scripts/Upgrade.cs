using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace usd.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
    public class WeaponLevel : ScriptableObject
    {
        public int weaponProjectiles;
        public float projectileSpeed;
        public float projectileRange;
        public float projectileDuration;
        public float projectileDamage;
        public float fireRate;
    }
}
