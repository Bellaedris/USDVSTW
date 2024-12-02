using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace usd.Enemies.Projectiles
{
    public class NmyLinearProjectile : MonoBehaviour
    {
        public Bounds limits;
        [HideInInspector]
        public float speed = 1f;
        [HideInInspector]
        public float damage;
        [HideInInspector]
        public Vector3 target;

        public float tolerance = 0.05f;
        private Vector3 moveDirection;
        private bool hasReachedTarget = false;
        
        void Start()
        {
            moveDirection = (target- transform.position).normalized;
        }
        // Update is called once per frame
        void Update()
        {
            if(!limits.Contains(transform.position))
                Destroy(gameObject);

            MoveTowardsTarget();
        }
        
        private void MoveTowardsTarget()
        {
            if (!hasReachedTarget)
            {
                // Check if the object is near the target
                if (Vector3.Distance(transform.position, target) <= tolerance)
                {
                    hasReachedTarget = true; 
                }
                else
                {
                    // Move towards the target
                    moveDirection = (target - transform.position).normalized;
                }
            }

            // Move in the current direction
            transform.position += moveDirection * speed * Time.deltaTime;
        }
        
        private void OnTriggerEnter(Collider other)
        {   
            if (other.CompareTag("Player"))
            {
                //TODO trigger ally hit animation
                // Debug.Log("TOUCHEEEEE");
                PlayerController pController = other.GetComponent<PlayerController>();
                pController.RegisterHit();
                Destroy(gameObject);
            }
        }
    }
}
