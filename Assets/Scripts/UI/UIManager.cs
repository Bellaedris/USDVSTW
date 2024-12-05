using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace usd
{
    /// <summary>
    /// manages the UIs of the main menu and the game scene
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Observer pattern. Observers register a callback to this object, that is called when the game is over.
        /// used to update the game state when the game is over so everything can stop spawning and moving.
        /// </summary>
        public event Action gameOver; 
        
        public TMP_Text timerText;
        public TMP_Text waveText;
        public TMP_Text scoreText;

        public RawImage laserLevel;
        public RawImage gatlingLevel;
        public RawImage blackHoleLevel;
        
        public Slider laserLevelSlider;
        public Slider gatlingLevelSlider;
        public Slider blackHoleLevelSlider;

        public GameObject pauseUI;
        public GameObject gameOverUI;
        
        private static UIManager _instance;
        
        private bool _isPaused;
        private float _timeScale;
        public float _GetTimeScale() => _timeScale;
        
        private bool _isGameOver;
        
        public static UIManager Instance => _instance;

        [HideInInspector] public int difficultyModifier = 0;

        private void Awake()
        {
            // singleton
            if(_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            
            if (Time.timeScale != 0)
                _timeScale = Time.timeScale;
        }

        private void OnGUI()
        {
            // do nothing if no timer UI element was given to the script
            if (!timerText)
                return;
            
            //display the game time
            var time = Time.timeSinceLevelLoad;
            float seconds = time % 60;
            float minutes = Mathf.Floor(time / 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        public void SetWaveNumber(int wave)
        {
            waveText.text = $"Wave {wave}";
            if (wave % 5 == 0)
            {
                difficultyModifier++;
            }
        }

        public void SwitchWeapon(int weaponID, int weaponLevel)
        {
            // update the UI to fill the upgrade boxes
            var sliderValue = (weaponLevel + 1) / 6f;
            switch (weaponID)
            {
                case 1:
                    laserLevelSlider.value = sliderValue;
                    laserLevel.color = Color.white;
                    gatlingLevel.color = Color.grey;
                    blackHoleLevel.color = Color.grey;
                    break;
                case 2:
                    gatlingLevelSlider.value = sliderValue;
                    laserLevel.color = Color.grey;
                    gatlingLevel.color = Color.white;
                    blackHoleLevel.color = Color.grey;
                    break;
                case 3:
                    blackHoleLevelSlider.value = sliderValue;
                    laserLevel.color = Color.grey;
                    gatlingLevel.color = Color.grey;
                    blackHoleLevel.color = Color.white;
                    break;
            }
        }

        public void DisplayScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        public void TogglePauseMenu()
        {
            // do nothing if the game is over, there is a menu already
            if (_isGameOver)
            {
                AudioManager.Instance.FadeInMusicMenu();
                return;
            }
            
            // we set the time scale to zero to fake pause the game
            if (!_isPaused)
            {
                AudioManager.Instance.FadeInMusicMenu();
                Time.timeScale = 0;
            }
            else
            {
                AudioManager.Instance.FadeOutMusicMenu();
                Time.timeScale = _timeScale;
            }
            
            _isPaused = !_isPaused;
            pauseUI.SetActive(!pauseUI.activeSelf);
        }
        
        public void UpdateLevelsOnUI(int laserLevel, int gatlingLevel, int blackHoleLevel)
        {
            laserLevelSlider.value = (laserLevel + 1) / 6f;
            gatlingLevelSlider.value = (gatlingLevel + 1) / 6f;
            blackHoleLevelSlider.value = (blackHoleLevel + 1) / 6f;
        }

        /// <summary>
        /// Since the AudioManager is passed through the scene, it is the UI Manager that dispatches the calls to Audio Manager
        /// </summary>
        /// <param name="volume">callback from slider value, between 0 and 1</param>
        public void UpdateMusicVolume(float volume)
        {
            AudioManager.Instance.UpdateMasterVolume(volume);
        }
        
        public void UpdateSfxVolume(float volume)
        {
            AudioManager.Instance.UpdateSfxVolume(volume);
        }
        
        public void ShowGameOver()
        {
            _isGameOver = true;
            gameOver?.Invoke(); // all gameObjects subscribed to this Action will activate their callback method
            StartCoroutine(waitBeforeShowingDeathUI()); // wait 1s before showing the Game Over UI so the player can appreciate our nice particle systems
        }

        IEnumerator waitBeforeShowingDeathUI()
        {
            yield return new WaitForSeconds(1f);
            gameOverUI.SetActive(true);
        }
    }
}
