using UnityEngine;

namespace usd.Weapons.Projectiles
{
    public class LinearProjectile : MonoBehaviour
    {
        [HideInInspector]
        public float speed = 1f;
        [HideInInspector]
        public float damage;

        // Update is called once per frame
        void Update()
        {
            transform.Translate(transform.right * (speed * Time.deltaTime), Space.World);
        }
    }
}
