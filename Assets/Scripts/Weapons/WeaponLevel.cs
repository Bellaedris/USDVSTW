using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace usd.Weapons
{
    /// <summary>
    /// Scriptable object used to create datas on each weapon levels.
    /// Can be created as an asset and edited in the editor.
    /// </summary>
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
