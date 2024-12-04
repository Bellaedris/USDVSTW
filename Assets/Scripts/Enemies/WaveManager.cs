using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using usd.Utils;

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
        public float spawnInterval = 3f; // Time between spawns
        public int initialEnemiesPerWave = 5; // Enemies in the first wave
        public float waveIncreaseFactor = 1.2f; // Multiplier for enemies per wave
        public int maxEnemiesPerWave = 25; // Cap for max enemies in a wave
        public float delayBetweenWaves = 5.0f; // Delay between waves
        
        [Header("Spawn Area Collider")]
        public GameObject background;

        [HideInInspector]
        public Bounds spawnBounds;
        private Camera _mainCamera;
        
        // [Header("Spawn Area")]
        // public Vector2 spawnAreaMin; // Bottom-left corner
        // public Vector2 spawnAreaMax; // Top-right corner

        private int currentWave = 0;
        private int enemiesPerWave;

        void Start()
        {
            enemiesPerWave = initialEnemiesPerWave;
            _mainCamera = Camera.main;
            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            spawnBounds = new Bounds(_mainCamera.transform.position, new Vector3(sizeX * 2, sizeY * 2, 0));
            StartCoroutine(SpawnWaves());
        }

        IEnumerator SpawnWaves()
        {
            while (true)
            {
                currentWave++;
                UIManager.Instance.SetWaveNumber(currentWave);
                Debug.Log($"Starting Wave {currentWave}");
                yield return StartCoroutine(SpawnEnemies());
                yield return new WaitForSeconds(delayBetweenWaves); // Delay between waves
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
            Vector3 spawnPosition = RandomUtils.RandomInRectangleBorder(ref spawnBounds);
            spawnPosition.z = 0;
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
