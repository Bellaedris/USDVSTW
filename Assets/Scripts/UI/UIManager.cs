using System;
using UnityEngine;

namespace usd.UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        
        public UIManager Instance => _instance;
        
        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else 
                Destroy(gameObject);
        }

        public static void LoadGameScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public static void LoadMainMenuScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}
