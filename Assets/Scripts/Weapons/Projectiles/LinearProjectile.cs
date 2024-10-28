using UnityEngine;

namespace usd.Weapons.Projectiles
{
    public class LinearProjectile : MonoBehaviour
    {
        public float speed = 1f;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(transform.right * (speed * Time.deltaTime));
        }
    }
}
