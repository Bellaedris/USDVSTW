using System.Collections.Generic;
using UnityEngine;
using usd.Weapons.Projectiles;

namespace usd.Weapons
{
    /// <summary>
    /// Represents a circular weapon that shoots projectiles in a circular pattern
    /// </summary>
    public class Circular : Weapon
    {
        /// <summary>
        /// List of projectiles instantiated by this weapon
        /// </summary>
        private List<GameObject> _projectiles;
        
        private void Awake()
        {
            // Initializes the weapon and its projectiles list.

            _projectiles = new List<GameObject>();
        }

        /// <summary>
        /// Shoots projectiles in a circular pattern based on the current upgrade level.
        /// </summary>
        public override void Shoot()
        {
            int projCount = upgrades[_currentLevel].weaponProjectiles;
            float projRange = upgrades[_currentLevel].projectileRange;
            float angleOffset = 360f / projCount * Mathf.Deg2Rad;
            
            for (int i = 0; i < projCount; i++)
            {
                // Calculate the angle for each projectile in the circle (around the Z-axis in 2D)
                float angle = angleOffset * i;

                var position = transform.position +
                               new Vector3(Mathf.Cos(angle) * projRange, Mathf.Sin(angle) * projRange, 0f);

                // Instantiate the projectile with calculated position and rotation
                var proj = Instantiate(projectilePrefab, position, projectilePrefab.transform.rotation, transform);
                proj.GetComponent<CircularProjectile>().lifetime = upgrades[_currentLevel].projectileDuration;
                proj.GetComponent<CircularProjectile>().damage = upgrades[_currentLevel].projectileDamage;
                _projectiles.Add(proj);
            }
        }

        private void Update()
        {
            // Updates the weapon's state, rotating the projectiles around the weapon
            transform.Rotate(Vector3.forward, Time.deltaTime * upgrades[_currentLevel].projectileSpeed);
        }
    }
}