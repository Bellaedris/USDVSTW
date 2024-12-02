using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace usd.Enemies
{
    public class WaveManager : MonoBehaviour
    {
        [Header("Enemy Prefabs")]
        public GameObject normalChaserPrefab;
        public GameObject rareSwarmPrefab;
        public GameObject rareFlyerPrefab;
        public GameObject rarestTankPrefab;

        [Header("Wave Settings")]
        public float spawnInterval = 2f; // Time between spawns
        public int initialEnemiesPerWave = 5; // Enemies in the first wave
        public float waveIncreaseFactor = 1.2f; // Multiplier for enemies per wave
        public int maxEnemiesPerWave = 50; // Cap for max enemies in a wave

        [Header("Spawn Area")]
        public Vector2 spawnAreaMin; // Bottom-left corner
        public Vector2 spawnAreaMax; // Top-right corner

        private int currentWave = 0;
        private int enemiesPerWave;

        void Start()
        {
            enemiesPerWave = initialEnemiesPerWave;
            StartCoroutine(SpawnWaves());
        }

        IEnumerator SpawnWaves()
        {
            while (true)
            {
                currentWave++;
                GameManager.Instance.SetWaveNumber(currentWave);
                Debug.Log($"Starting Wave {currentWave}");
                yield return StartCoroutine(SpawnEnemies());
                yield return new WaitForSeconds(5f); // Delay between waves
            }
        }

        IEnumerator SpawnEnemies()
        {
            int enemiesToSpawn = Mathf.Min(enemiesPerWave, maxEnemiesPerWave);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            // Increase wave difficulty
            enemiesPerWave = Mathf.CeilToInt(enemiesPerWave * waveIncreaseFactor);
        }

        void SpawnEnemy()
        {
            GameObject enemyToSpawn = SelectEnemyPrefab();

            // Randomize spawn position within the defined area
            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }

        GameObject SelectEnemyPrefab()
        {
            float randomValue = Random.value;
            
            //TODO TUNE THESE VALUES
            if (randomValue < 0.4f)
                return normalChaserPrefab;
            else if (randomValue < 0.7f)
                return rareSwarmPrefab;
            else if (randomValue < 0.9f)
                return rareFlyerPrefab;
            else
                return rarestTankPrefab;
        }
    }
}
