using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using usd.Enemies;

namespace usd.Weapons.Projectiles
{
    /// <summary>
    /// A projectile that spins around an actor. The movement is handled by the weapon itself, while the projectile
    /// only accounts for its lifetime and collision handling
    /// </summary>
    public class CircularProjectile : MonoBehaviour
    {
        [HideInInspector]
        public float damage;
        
        public float lifetime;
        
        void Update()
        {
            // The projectile manages its lifetime by itself
            lifetime -= Time.deltaTime;
            if(lifetime < 0)
                Destroy(gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Nmy"))
            {
                other.GetComponent<BasicEnemy>().TakeDamage(damage);
            }
            else if (other.CompareTag("Nmy_Projectile"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
