using UnityEngine;
using usd.Weapons.Projectiles;

namespace usd.Weapons
{
    public class Radial : Weapon
    {
        public float shootRadius = 60f;
        
        public override void Shoot()
        {
            int projCount = upgrades[_currentLevel].weaponProjectiles;
            float angleOffset = shootRadius / (projCount + 1f);
            float startAngle = -shootRadius / 2f;

            for (int i = 0; i < projCount; i++)
            {
                // Calculate the angle for each projectile in the fan (around the Z-axis in 2D)
                float angle = startAngle + angleOffset * (i + 1);

                // Calculate the rotation for each projectile (around the Z-axis for 2D)
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                // Instantiate the projectile with calculated position and rotation
                var projObj = Instantiate(projectilePrefab, transform.position, transform.rotation * rotation);
                var proj = projObj.GetComponent<LinearProjectile>();
                proj.damage = upgrades[_currentLevel].projectileDamage;
                proj.speed = upgrades[_currentLevel].projectileSpeed;
            }
        }
    }
}
