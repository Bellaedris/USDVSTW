using UnityEngine;

namespace usd.Utils
{
    public class RandomUtils
    {
        /*
         * Generate a vector with each coordinates between min and max
         */
        public static Vector2 RandomVectorInRange(float min, float max)
        {
            return new Vector2(
                UnityEngine.Random.Range(min, max),
                UnityEngine.Random.Range(min, max)
            );
        }

        /*
         * Generate a random powition on the border of a rectangle
         */
        public static Vector2 RandomInRectangleBorder(ref BoxCollider2D collider)
        {
            int side = UnityEngine.Random.Range(0, 4);
            float minX = collider.bounds.min.x;
            float maxX = collider.bounds.max.x;
            float minY = collider.bounds.min.y;
            float maxY = collider.bounds.max.y;

            // We pick a side at random, then place ourselves on that side and randomize the other axis
            Vector3 ret;
            switch (side)
            {
                case 0: // left
                    ret = new Vector2(minX, Random.Range(minY, maxY));
                    break;
                case 1: // right
                    ret = new Vector2(maxX, Random.Range(minY, maxY));
                    break;
                case 2: // top
                    ret = new Vector2(Random.Range(minX, maxX), minY);
                    break;
                default: // bottom
                    ret = new Vector2(Random.Range(minX, maxX), maxY);
                    break;
            }

            return ret;
        }
    }
}
