using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using usd.Enemies;

namespace usd.Weapons.Projectiles
{
    public class CircularProjectile : MonoBehaviour
    {
        [HideInInspector]
        public float damage;
        
        // Update is called once per frame
        void Update()
        {
            //transform.position = new Vector3(Mathf.Cos(Time.deltaTime * speed) * 2f, Mathf.Sin(Time.deltaTime * speed) * 2f, 0f);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Nmy"))
            {
                //TODO trigger enemy hit animation
                other.GetComponent<BasicEnemy>().TakeDamage(damage);
            }
            else if (other.CompareTag("Nmy_Projectile"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
