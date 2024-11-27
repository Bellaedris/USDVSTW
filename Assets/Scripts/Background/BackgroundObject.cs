using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using usd.Utils;

namespace usd
{
    public class BackgroundObject : MonoBehaviour
    {
        private float maxSpeed = 10f;
        private Vector3 _direction;
        private float _speed;
        
        private BoxCollider _bounds;
        
        void Awake()
        {
            _speed = Random.Range(0f, maxSpeed);
            Vector2 dir = RandomUtils.RandomVectorInRange(-1f, 1f);
            _direction = new Vector3(dir.x, dir.y);
            _bounds = GetComponentInParent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Time.deltaTime * _speed * _direction);
            
            if (!_bounds.bounds.Contains(transform.position))
                Destroy(gameObject);
        }
    }
}
