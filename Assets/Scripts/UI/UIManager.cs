using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace usd
{
    /// <summary>
    /// Manages the UIs of the main menu and the game scene.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Observer pattern. Observers register a callback to this object, that is called when the game is over.
        /// Used to update the game state when the game is over so everything can stop spawning and moving.
        /// </summary>
        public event Action gameOver; 
        
        /// <summary>
        /// Text element to display the timer
        /// </summary>
        public TMP_Text timerText;

        /// <summary>
        /// Text element to display the wave number
        /// </summary>
        public TMP_Text waveText;

        /// <summary>
        /// Text element to display the score
        /// </summary>
        public TMP_Text scoreText;

        /// <summary>
        /// Image element to display the laser level
        /// </summary>
        public RawImage laserLevel;

        /// <summary>
        /// Image element to display the gatling level
        /// </summary>
        public RawImage gatlingLevel;

        /// <summary>
        /// Image element to display the black hole level
        /// </summary>
        public RawImage blackHoleLevel;
        
        /// <summary>
        /// Slider element to display the laser level
        /// </summary>
        public Slider laserLevelSlider;

        /// <summary>
        /// Slider element to display the gatling level
        /// </summary>
        public Slider gatlingLevelSlider;

        /// <summary>
        /// Slider element to display the black hole level
        /// </summary>
        public Slider blackHoleLevelSlider;

        /// <summary>
        /// UI element for the pause menu
        /// </summary>
        public GameObject pauseUI;

        /// <summary>
        /// UI element for the game over screen
        /// </summary>
        public GameObject gameOverUI;
        
        /// <summary>
        /// Singleton instance of the UIManager
        /// </summary>
        private static UIManager _instance;
        
        /// <summary>
        /// Indicates whether the game is paused
        /// </summary>
        private bool _isPaused;

        /// <summary>
        /// Stores the time scale of the game
        /// </summary>
        private float _timeScale;

        /// <summary>
        /// Gets the time scale of the game
        /// </summary>
        /// <returns>The time scale of the game</returns>
        public float _GetTimeScale() => _timeScale;
        
        /// <summary>
        /// Indicates whether the game is over
        /// </summary>
        private bool _isGameOver;
        
        /// <summary>
        /// Gets the singleton instance of the UIManager
        /// </summary>
        public static UIManager Instance => _instance;

        /// <summary>
        /// Difficulty modifier for the game
        /// </summary>
        [HideInInspector] public int difficultyModifier = 0;
        
        private void Awake()
        {
            // Singleton pattern implementation
            if(_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            
            if (Time.timeScale != 0)
                _timeScale = Time.timeScale;
        }

        /// <summary>
        /// Updates the timer text with the current game time
        /// </summary>
        private void OnGUI()
        {
            // Do nothing if no timer UI element was given to the script
            if (!timerText)
                return;
            
            // Display the game time
            var time = Time.timeSinceLevelLoad;
            float seconds = time % 60;
            float minutes = Mathf.Floor(time / 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// Sets the wave number and updates the wave text
        /// </summary>
        /// <param name="wave">The current wave number.</param>
        public void SetWaveNumber(int wave)
        {
            waveText.text = $"Wave {wave}";
            if (wave % 5 == 0)
            {
                difficultyModifier++;
            }
        }

        /// <summary>
        /// Switches the weapon and updates the UI to reflect the new weapon level.
        /// </summary>
        /// <param name="weaponID">The ID of the weapon.</param>
        /// <param name="weaponLevel">The level of the weapon.</param>
        public void SwitchWeapon(int weaponID, int weaponLevel)
        {
            // Update the UI to fill the upgrade boxes
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

        /// <summary>
        /// Displays the score on the UI.
        /// </summary>
        /// <param name="score">The current score.</param>
        public void DisplayScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        /// <summary>
        /// Toggles the pause menu and pauses or resumes the game
        /// </summary>
        public void TogglePauseMenu()
        {
            // Do nothing if the game is over, there is a menu already
            if (_isGameOver)
            {
                AudioManager.Instance.FadeInMusicMenu();
                return;
            }
            
            // Set the time scale to zero to fake pause the game
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
        
        /// <summary>
        /// Updates the weapon levels on the UI.
        /// </summary>
        /// <param name="laserLevel">The level of the laser weapon.</param>
        /// <param name="gatlingLevel">The level of the gatling weapon.</param>
        /// <param name="blackHoleLevel">The level of the black hole weapon.</param>
        public void UpdateLevelsOnUI(int laserLevel, int gatlingLevel, int blackHoleLevel)
        {
            laserLevelSlider.value = (laserLevel + 1) / 6f;
            gatlingLevelSlider.value = (gatlingLevel + 1) / 6f;
            blackHoleLevelSlider.value = (blackHoleLevel + 1) / 6f;
        }

        /// <summary>
        /// Updates the music volume through the AudioManager.
        /// </summary>
        /// <param name="volume">The new music volume, between 0 and 1.</param>
        public void UpdateMusicVolume(float volume)
        {
            AudioManager.Instance.UpdateMasterVolume(volume);
        }
        
        /// <summary>
        /// Updates the sound effects volume through the AudioManager.
        /// </summary>
        /// <param name="volume">The new sound effects volume, between 0 and 1.</param>
        public void UpdateSfxVolume(float volume)
        {
            AudioManager.Instance.UpdateSfxVolume(volume);
        }
        
        /// <summary>
        /// Shows the game over screen and stops the game.
        /// </summary>
        public void ShowGameOver()
        {
            _isGameOver = true;
            gameOver?.Invoke(); // All gameObjects subscribed to this Action will activate their callback method
            StartCoroutine(waitBeforeShowingDeathUI()); // Wait 1s before showing the Game Over UI so the player can appreciate our nice particle systems
        }

        /// <summary>
        /// Waits for a short duration before showing the game over UI.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine.</returns>
        IEnumerator waitBeforeShowingDeathUI()
        {
            yield return new WaitForSeconds(1f);
            gameOverUI.SetActive(true);
        }
    }
}