using UnityEngine;

namespace usd.Weapons
{
    /// <summary>
    /// Manages the lifetime of a dropped item, including its blinking effect before disappearing.
    /// </summary>
    public class DropLifetime : MonoBehaviour
    {
        /// <summary>
        /// The total duration the item will exist before being destroyed.
        /// </summary>
        private float lifeDuration;

        /// <summary>
        /// The duration after which the item will start blinking.
        /// </summary>
        private float blinkDuration;

        /// <summary>
        /// The time when the item was spawned.
        /// </summary>
        private float spawnTime;
        
        void Start()
        {
            lifeDuration = 10.0f;
            blinkDuration = 7.0f;
            spawnTime = Time.time;
        }
        
        void Update()
        {
            // Updates the item's state, handling its destruction and blinking effect when near destruction
            if (Time.time - spawnTime > lifeDuration)
            {
                Destroy(gameObject);
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