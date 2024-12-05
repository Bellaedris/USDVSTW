using System.Collections;
using UnityEngine;
using usd.Utils;

namespace usd.Enemies
{
    /// <summary>
    /// Manages the spawning of enemy waves.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        /// <summary>
        /// Prefab for the normal chaser enemy
        /// </summary>
        [Header("Enemy Prefabs")]
        public GameObject normalChaserPrefab;

        /// <summary>
        /// Prefab for the rare swarm enemy
        /// </summary>
        public GameObject rareSwarmPrefab;

        /// <summary>
        /// Prefab for the rare flyer enemy
        /// </summary>
        public GameObject rareFlyerPrefab;

        /// <summary>
        /// Prefab for the rarest tank enemy
        /// </summary>
        public GameObject rarestTankPrefab;

        /// <summary>
        /// Time interval between enemy spawns
        /// </summary>
        [Header("Wave Settings")]
        public float spawnInterval = 3f;

        /// <summary>
        /// Number of enemies in the first wave
        /// </summary>
        public int initialEnemiesPerWave = 5;

        /// <summary>
        /// Multiplier for increasing the number of enemies per wave
        /// </summary>
        public float waveIncreaseFactor = 1.2f;

        /// <summary>
        /// Maximum number of enemies per wave
        /// </summary>
        public int maxEnemiesPerWave = 25;

        /// <summary>
        /// Delay between waves
        /// </summary>
        public float delayBetweenWaves = 5.0f;

        /// <summary>
        /// Reference to the background object defining the spawn area
        /// </summary>
        [Header("Spawn Area Collider")]
        public GameObject background;

        /// <summary>
        /// Bounds of the spawn area
        /// </summary>
        [HideInInspector]
        public Bounds spawnBounds;

        /// <summary>
        /// Reference to the main camera
        /// </summary>
        private Camera _mainCamera;

        /// <summary>
        /// Current wave number
        /// </summary>
        private int currentWave = 0;

        /// <summary>
        /// Number of enemies to spawn in the current wave
        /// </summary>
        private int enemiesPerWave;

        /// <summary>
        /// Reference to the UI manager
        /// </summary>
        private UIManager _uiManager;

        /// <summary>
        /// Indicates whether the game is over
        /// </summary>
        private bool _isGameOver;
        
        void Start()
        {
            // Initializes the wave manager and starts the wave spawning coroutine

            enemiesPerWave = initialEnemiesPerWave;
            _mainCamera = Camera.main;
            float sizeY = _mainCamera.orthographicSize;
            float sizeX = sizeY * _mainCamera.aspect;
            spawnBounds = new Bounds(_mainCamera.transform.position, new Vector3(sizeX * 2, sizeY * 2, 0));
            StartCoroutine(SpawnWaves());

            _uiManager = FindObjectOfType<UIManager>();
            if (_uiManager != null)
                _uiManager.gameOver += OnGameOver;
        }

        /// <summary>
        /// Coroutine for spawning waves of enemies
        /// </summary>
        IEnumerator SpawnWaves()
        {
            while (!_isGameOver)
            {
                currentWave++;
                UIManager.Instance.SetWaveNumber(currentWave);
                yield return StartCoroutine(SpawnEnemies());
                yield return new WaitForSeconds(delayBetweenWaves);
            }
        }

        /// <summary>
        /// Coroutine for spawning enemies in the current wave
        /// </summary>
        IEnumerator SpawnEnemies()
        {
            int enemiesToSpawn = Mathf.Min(enemiesPerWave, maxEnemiesPerWave);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            enemiesPerWave = Mathf.CeilToInt(enemiesPerWave * waveIncreaseFactor);
        }

        /// <summary>
        /// Spawns a single enemy at a random position within the spawn area
        /// </summary>
        void SpawnEnemy()
        {
            if (_isGameOver)
                return;

            GameObject enemyToSpawn = SelectEnemyPrefab();

            Vector3 spawnPosition = RandomUtils.RandomInRectangleBorder(ref spawnBounds);
            spawnPosition.z = 0;
            Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        }

        /// <summary>
        /// Selects an enemy prefab to spawn based on random chance
        /// </summary>
        /// <returns>The selected enemy prefab.</returns>
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

        /// <summary>
        /// Handles the game over event.
        /// </summary>
        private void OnGameOver()
        {
            _isGameOver = true;
        }
    }
}