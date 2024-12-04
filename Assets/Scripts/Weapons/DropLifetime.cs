using UnityEngine;

namespace usd.Weapons
{
    public class DropLifetime : MonoBehaviour
    {
        private float lifeDuration;
        private float blinkDuration;
        private float spawnTime;
        void Start()
        {
            lifeDuration = 10.0f;
            blinkDuration = 7.0f;
            spawnTime = Time.time;
        }

        void Update()
        {
            if (Time.time - spawnTime > lifeDuration)
            {
                Destroy(gameObject);
                Debug.Log("Destroyed");
            }
            else if (Time.time - spawnTime > blinkDuration)
            {
                if (Time.time % 0.5f < 0.25f)
                {
                    GetComponentInChildren<Renderer>().enabled = false;
                }
                else
                {
                    GetComponentInChildren<Renderer>().enabled = true;
                }
            }
        }
    }
}