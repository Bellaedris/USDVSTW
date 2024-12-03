using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace usd
{
    public class GameManager : MonoBehaviour
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
        
        private static GameManager _instance;
        
        public static GameManager Instance => _instance;

        private void Awake()
        {
            if(_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }

        private void OnGUI()
        {
            var time = Time.timeSinceLevelLoad;
            float seconds = time % 60;
            float minutes = Mathf.Floor(time / 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        public void SetWaveNumber(int wave)
        {
            waveText.text = $"Wave {wave}";
        }

        public void SwitchWeapon(int weaponID, int weaponLevel)
        {
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
        
        public void UpdateLevelsOnUI(int laserLevel, int gatlingLevel, int blackHoleLevel)
        {
            laserLevelSlider.value = (laserLevel + 1) / 6f;
            gatlingLevelSlider.value = (gatlingLevel + 1) / 6f;
            blackHoleLevelSlider.value = (blackHoleLevel + 1) / 6f;
        }

        public void displayScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}
