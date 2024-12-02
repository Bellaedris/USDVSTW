using UnityEngine;
using usd.Enemies;

namespace usd.Weapons.Projectiles
{
    public class LinearProjectile : MonoBehaviour
    {
        public Bounds limits;
        [HideInInspector]
        public float speed = 1f;
        [HideInInspector]
        public float damage;

        // Update is called once per frame
        void Update()
        {
            if(!limits.Contains(transform.position))
                Destroy(gameObject);
            transform.Translate(transform.right * (speed * Time.deltaTime), Space.World);
            Debug.Log(gameObject.GetComponent<Collider>());
        }

        private void OnTriggerEnter(Collider other)
        {   
            if (other.CompareTag("Nmy"))
            {
                other.GetComponent<BasicEnemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (other.CompareTag("Nmy_Projectile"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
