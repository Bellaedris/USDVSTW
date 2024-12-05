using System;
using UnityEngine;

namespace usd.UI
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;
        
        public SceneLoader Instance => _instance;
        
        private void Awake()
        {
            //singleton 
            if(_instance == null)
                _instance = this;
            else 
                Destroy(gameObject);
        }

        public static void LoadGameScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            // Time.timeScale = UIManager.Instance._GetTimeScale();
        }

        public static void LoadMainMenuScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            AudioManager.Instance.FadeMusicMax();
        }

        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}
