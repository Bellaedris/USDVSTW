using UnityEngine;
using usd.Weapons.Projectiles;

namespace usd.Weapons
{
    public class Linear : Weapon
    {
        public override void Shoot()
        {
            float spawnOffset = 1f / (upgrades[_currentLevel].weaponProjectiles + 1f);
            for (int i = 0; i < upgrades[_currentLevel].weaponProjectiles; i++)
            {
                GameObject projObj = Instantiate(projectilePrefab, transform.position + transform.rotation * new Vector3(0f, -.5f + spawnOffset * (i + 1), 0f), transform.rotation );
                var proj = projObj.GetComponent<LinearProjectile>();
                proj.damage = upgrades[_currentLevel].projectileDamage;
                proj.speed = upgrades[_currentLevel].projectileSpeed;
            }
        }
    }
}
