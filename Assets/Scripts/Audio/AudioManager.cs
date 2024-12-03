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
        private AudioSource _source;
        
        void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
            
            _source = GetComponent<AudioSource>();
        }

        public void UpdateMasterVolume(float value)
        {
            float newVolume = Mathf.Lerp(-80f, 20f, value);
            masterVolume.SetFloat("volume", newVolume);
        }

        public void playPlayerSound(AudioClip clip)
        {
            _source.PlayOneShot(clip);
        }
    }
}
