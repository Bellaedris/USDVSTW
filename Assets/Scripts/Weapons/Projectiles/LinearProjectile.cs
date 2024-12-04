using UnityEngine;
using usd.Enemies;

namespace usd.Weapons.Projectiles
{
    public class LinearProjectile : MonoBehaviour
    {
        private Vector2 limits;
        private Camera _mainCamera;
        [HideInInspector]
        public float speed = 1f;
        [HideInInspector]
        public float damage;

        void Start()
        {
            _mainCamera = Camera.main;
            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            limits = new Vector2(sizeX, sizeY);
        }
        
        // Update is called once per frame
        void Update()
        {
            if (transform.position.x > limits.x || transform.position.x < -limits.x || transform.position.y > limits.y || transform.position.y < -limits.y)
            {
                Destroy(gameObject);
            }
            transform.Translate(transform.right * (speed * Time.deltaTime), Space.World);
            // Debug.Log(gameObject.GetComponent<Collider>());
        }

        private void OnTriggerEnter(Collider other)
        {   
            if (other.CompareTag("Nmy"))
            {
                //TODO trigger enemy hit animation
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
