using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace usd
{
    public class UIManager : MonoBehaviour
    {
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
        private bool _isGameOver;
        
        public static UIManager Instance => _instance;

        [HideInInspector] public int difficultyModifier = 0;

        private void Awake()
        {
            if(_instance == null)
                _instance = this;
            else
                Destroy(gameObject);

            _timeScale = Time.timeScale;
        }

        private void OnGUI()
        {
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
            if (_isGameOver)
                return;
            
            // we set the time scale to zero to fake pause the game
            if (!_isPaused)
                Time.timeScale = 0;
            else
                Time.timeScale = _timeScale;
            _isPaused = !_isPaused;
            pauseUI.SetActive(!pauseUI.activeSelf);
        }
        
        public void UpdateLevelsOnUI(int laserLevel, int gatlingLevel, int blackHoleLevel)
        {
            laserLevelSlider.value = (laserLevel + 1) / 6f;
            gatlingLevelSlider.value = (gatlingLevel + 1) / 6f;
            blackHoleLevelSlider.value = (blackHoleLevel + 1) / 6f;
        }

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
            // delete all enemies on the map? 
            _isGameOver = true;
            StartCoroutine(waitBeforeShowingDeathUI());
        }

        IEnumerator waitBeforeShowingDeathUI()
        {
            yield return new WaitForSeconds(1f);
            gameOverUI.SetActive(true);
        }
    }
}
