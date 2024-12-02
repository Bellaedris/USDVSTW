using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace usd
{
    public class CircularProjectile : MonoBehaviour
    {
        public float lifetime;
        // Update is called once per frame
        void Update()
        {
            lifetime -= Time.deltaTime;
            if(lifetime < 0)
                Destroy(gameObject);
            //transform.position = new Vector3(Mathf.Cos(Time.deltaTime * speed) * 2f, Mathf.Sin(Time.deltaTime * speed) * 2f, 0f);
        }
    }
}
