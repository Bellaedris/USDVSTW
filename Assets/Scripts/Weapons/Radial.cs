using UnityEngine;

namespace usd.Weapons
{
    public class Radial : Weapon
    {
        public float shootRadius = 60f;
        
        public override void Shoot()
        {
            float angleOffset = shootRadius / (numberOfProjectile + 1f);
            float startAngle = -shootRadius / 2f;

            for (int i = 0; i < numberOfProjectile; i++)
            {
                // Calculate the angle for each projectile in the fan (around the Z-axis in 2D)
                float angle = startAngle + angleOffset * (i + 1);

                // Calculate the rotation for each projectile (around the Z-axis for 2D)
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                // Instantiate the projectile with calculated position and rotation
                Instantiate(projectilePrefab, transform.position, rotation, transform);
            }
        }
    }
}
