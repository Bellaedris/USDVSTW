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

        public RawImage laserLevel;
        public RawImage gatlingLevel;
        public RawImage blackHoleLevel;
        
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

        public void SwitchWeapon(int weaponID)
        {
            switch (weaponID)
            {
                case 0:
                    //laserLevel.material.SetVector("_glowDirection", new Vector4(0, 1, 0, 0));
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }
    }
}
