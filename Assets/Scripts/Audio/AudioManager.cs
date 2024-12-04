using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace usd
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        public static AudioManager Instance => _instance;
        
        public AudioMixer masterVolume;
        public AudioMixer vfxVolume;
        
        private AudioSource _playerSource;
        private AudioSource _enemiesSource;
        // okay this is ugly but we want smooth transitions between contextual musics
        // and we don't have fmod or wwise
        private AudioSource[] _bgm; // holds music for laser/gatling/blackHole/criticalHP/maxHP
        private int _playingBgm;
        
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
            _playerSource = sources[0];
            _enemiesSource = sources[1];

            _bgm = new AudioSource[5];
            _bgm[0] = sources[2]; //laser
            _bgm[1] = sources[3]; //gatling
            _bgm[2] = sources[4]; //blackhole
            _bgm[3] = sources[5]; //criticalhealth
            _bgm[4] = sources[6]; //maxhealth
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

        public void FadeMusic(int newWeaponID, int level)
        {
            // do nothing if the weapon didn't change
            if (newWeaponID == _playingBgm && level > 0)
                return;
            if (level == 0)
            {
                StartCoroutine(crossFade(_bgm[_playingBgm], _bgm[3], 1f));
                _playingBgm = 3;
            }
            else if (level == 5)
            {
                StartCoroutine(crossFade(_bgm[_playingBgm], _bgm[4], 1f));
                _playingBgm = 4;
            }
            else
            {
                StartCoroutine(crossFade(_bgm[_playingBgm], _bgm[newWeaponID], 1f));
                _playingBgm = newWeaponID;
            }
            
        }

        private IEnumerator crossFade(AudioSource from, AudioSource to, float fadeTime)
        {
            float elapsed = 0f;
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                from.volume = Mathf.Lerp(.6f, 0f, elapsed / fadeTime);
                to.volume = Mathf.Lerp(0f, .6f, elapsed / fadeTime);
                yield return null;
            }

            yield return null;
        }
    }
}
