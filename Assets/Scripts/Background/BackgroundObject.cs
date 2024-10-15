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
        
        private BoxCollider2D _bounds;
        
        void Awake()
        {
            _speed = Random.Range(0f, maxSpeed);
            Vector2 dir = RandomUtils.RandomVectorInRange(-1f, 1f);
            _direction = new Vector3(dir.x, dir.y);
            _bounds = GetComponentInParent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Time.deltaTime * _speed * _direction);
            
            if (!_bounds.OverlapPoint(new Vector2(transform.position.x, transform.position.y)))
                Destroy(gameObject);
        }
    }
}
