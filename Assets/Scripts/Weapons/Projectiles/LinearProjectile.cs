using UnityEngine;

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
        }
    }
}
