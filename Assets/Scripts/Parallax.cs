using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace usd
{
    public class Parallax : MonoBehaviour
    {
        
        public Vector3 parallaxSpeed;
        public GameObject cam;

        private Vector3 startPos;

        private void Awake()
        {
            startPos = transform.position;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void FixedUpdate()
        {
            float newX = cam.transform.position.x * parallaxSpeed.x;
            float newY = cam.transform.position.y * parallaxSpeed.y;
            float newZ = cam.transform.position.z * parallaxSpeed.z;
            transform.position = new Vector3(startPos.x + newX, startPos.y + newY, startPos.z + newZ);
        }
    }
}
