using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace usd
{
    public class AudioManager : MonoBehaviour
    {

        private static AudioManager _instance;

        public static AudioManager Instance => _instance;
        
        public AudioMixer masterVolume;
        public AudioMixer vfxVolume;
        
        private AudioSource _source;
        private AudioSource _playerSource;
        private AudioSource _enemiesSource;
        
        void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            var sources = GetComponents<AudioSource>();
            _source = sources[0];
            _playerSource = sources[1];
            _enemiesSource = sources[2];
        }

        public void UpdateMasterVolume(float value)
        {
            float newVolume = Mathf.Lerp(-80f, 20f, value);
            masterVolume.SetFloat("volume", newVolume);
        }
        
        public void UpdateSfxVolume(float value)
        {
            float newVolume = Mathf.Lerp(-80f, 20f, value);
            vfxVolume.SetFloat("volume", newVolume);
        }

        public void playWeaponSound(AudioClip clip)
        {
            _playerSource.PlayOneShot(clip);
        }
        
        public void playGeneralSound(AudioClip clip)
        {
            _enemiesSource.PlayOneShot(clip);
        }
    }
}
