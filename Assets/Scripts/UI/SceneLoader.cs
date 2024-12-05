using System;
using UnityEngine;

namespace usd.UI
{
    /// <summary>
    /// Manages scene loading and provides methods to load different scenes
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance of the SceneLoader
        /// </summary>
        private static SceneLoader _instance;

        /// <summary>
        /// Gets the singleton instance of the SceneLoader
        /// </summary>
        public SceneLoader Instance => _instance;
        
        private void Awake()
        {
            // Singleton pattern implementation
            // To be sure that only one instance of the SceneLoader exists

            if(_instance == null)
                _instance = this;
            else 
                Destroy(gameObject);
        }

        /// <summary>
        /// Loads the game scene and resumes the game time
        /// </summary>
        public static void LoadGameScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            Time.timeScale = 1.0f;
            AudioManager.Instance.FadeMusicBase();
        }

        /// <summary>
        /// Loads the main menu scene and sets the music volume to maximum
        /// </summary>
        public static void LoadMainMenuScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            AudioManager.Instance.FadeMusicMax();
        }

        /// <summary>
        /// Quits the application
        /// </summary>
        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}