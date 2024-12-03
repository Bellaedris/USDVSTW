using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace usd.Enemies.Projectiles
{
    public class NmyLinearProjectile : MonoBehaviour
    {
        [HideInInspector]
        public float speed = 1f;
        [HideInInspector]
        public float damage;
        [HideInInspector]
        public Vector3 target;
        
        public float tolerance = 0.05f;
        
        private Camera _mainCamera;
        private Vector3 moveDirection;
        private bool hasReachedTarget = false;
        private Bounds limits;
        
        void Start()
        {
            _mainCamera = Camera.main;
            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            limits = new Bounds(_mainCamera.transform.position, new Vector3(sizeX * 2, sizeY * 2, 50));
            
            Debug.Log(limits.center);
            moveDirection = (target - transform.position).normalized;
        }
        // Update is called once per frame
        void Update()
        {
            if (!limits.Contains(transform.position))
            {
                Destroy(gameObject);
            }

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
    }
}
