using UnityEngine;

namespace usd.Weapons
{
    public class Linear : Weapon
    {
        public override void Shoot()
        {
            float spawnOffset = 1f / (numberOfProjectile + 1f);
            for (int i = 0; i < numberOfProjectile; i++)
            {
                Instantiate(projectilePrefab, transform.position + new Vector3(0f, -.5f + spawnOffset * (i + 1), 0f), Quaternion.Euler(90f, 0f, 0f), transform);
            }
        }
    }
}
