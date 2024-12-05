using UnityEngine;
using usd.Weapons.Projectiles;

namespace usd.Weapons
{
    /// <summary>
    /// A weapon that shoots projectiles in a straight line. Projectiles will stack on top of each other.
    /// </summary>
    public class Linear : Weapon
    {
        public override void Shoot()
        {
            // Shoots a number of projectiles in a straight line based on the current upgrade level
            float spawnOffset = 1f / (upgrades[_currentLevel].weaponProjectiles + 1f);
            for (int i = 0; i < upgrades[_currentLevel].weaponProjectiles; i++)
            {
                Vector3 spawnPos = transform.position +
                                   transform.rotation * new Vector3(0f, -.5f + spawnOffset * (i + 1), 0f);
                spawnPos.z = 0.0f;
                GameObject projObj = Instantiate(projectilePrefab, spawnPos, transform.rotation );
                var proj = projObj.GetComponent<LinearProjectile>();
                proj.damage = upgrades[_currentLevel].projectileDamage;
                proj.speed = upgrades[_currentLevel].projectileSpeed;
            }
        }
    }
}
