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
        private int _lastBgm;
        
        void Awake()
        {
            //singleton
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

            _bgm = new AudioSource[6];
            _bgm[0] = sources[2]; //laser
            _bgm[1] = sources[3]; //gatling
            _bgm[2] = sources[4]; //blackhole
            _bgm[3] = sources[5]; //criticalhealth
            _bgm[4] = sources[6]; //maxhealth
            _bgm[5] = sources[7]; //menu and between waves
        }

        /// <summary>
        /// Interpolate between a minimal and maximal sound volume on the master audio mixer
        /// </summary>
        /// <param name="value">callback from the slider value: something between 0 and 1</param>
        public void UpdateMasterVolume(float value)
        {
            float newVolume = Mathf.Lerp(-40f, 20f, value);
            masterVolume.SetFloat("volume", newVolume);
        }
        
        /// <summary>
        /// Same as UpdateMasterVolume, but applied to the SFX audio mixer
        /// </summary>
        /// <param name="value">callback from the slider value: something between 0 and 1</param>
        public void UpdateSfxVolume(float value)
        {
            float newVolume = Mathf.Lerp(-40f, 20f, value);
            vfxVolume.SetFloat("volume", newVolume);
        }

        /// <summary>
        /// Play a once shot sound on the player audio source. Called by the player weapons. 
        /// The sources are splitted since the weapons sounds are very present and would kind of overwhelm a single audio sources
        /// </summary>
        /// <param name="clip">the sound to play</param>
        public void playWeaponSound(AudioClip clip)
        {
            _playerSource.PlayOneShot(clip);
        }
        
        /// <summary>
        /// Play a once shot sound on the weapons audio source. Called by the player himself and the enemies. 
        /// This audio source manages SFX that are called less often than the weapon sounds.
        /// </summary>
        /// <param name="clip">the sound to play</param>
        public void playGeneralSound(AudioClip clip)
        {
            _enemiesSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Fades the current music track out and fades the new music in.
        /// The IDs correspond to:
        /// - 0 : laser weapon music
        /// - 1 : gatling music
        /// - 2 : black hole music
        /// - 3 : low health music
        /// - 4 : high health music
        /// - 5 : between waves and menu music
        /// </summary>
        /// <param name="newWeaponID">ID of the weapon currently equipped</param>
        /// <param name="level">level of the weapon currently equipped</param>
        /// <param name="maxWeaponLevel">max level of all weapons</param>
        public void FadeMusic(int newWeaponID, int level, int maxWeaponLevel)
        {
            // do nothing if the weapon didn't change
            if (newWeaponID == _playingBgm && level > 0 && level < 5)
                return;
            if (maxWeaponLevel == 0)
            {
                StartCoroutine(crossFade(_bgm[_playingBgm], _bgm[3], 2f));
                _playingBgm = 3;
            }
            else if (level == 5 && _playingBgm != 4)
            {
                StartCoroutine(crossFade(_bgm[_playingBgm], _bgm[4], 2f));
                _playingBgm = 4;
            }
            else if (!(level == 5 && _playingBgm == 4))
            {
                StartCoroutine(crossFade(_bgm[_playingBgm], _bgm[newWeaponID], 2f));
                _playingBgm = newWeaponID;
            }
            
        }

        private IEnumerator crossFade(AudioSource from, AudioSource to, float fadeTime)
        {
            foreach (AudioSource source in _bgm)
            {
                if (source != from && source != to)
                    source.volume = 0f;
            }
            
            // linearly interpolate volume in and out
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
        
        public void FadeInMusicMenu()
        {
            StartCoroutine(crossFadeMenu(_bgm[_playingBgm], _bgm[5], 1f));
            _lastBgm = _playingBgm;
            _playingBgm = 5;
        }
        
        public void FadeOutMusicMenu()
        {
            StartCoroutine(crossFadeMenu(_bgm[5], _bgm[_lastBgm], 1f));
            _playingBgm = _lastBgm;
        }
        
        private IEnumerator crossFadeMenu(AudioSource from, AudioSource to, float fadeTime)
        {
            foreach (AudioSource source in _bgm)
            {
                if (source != from && source != to)
                    source.volume = 0f;
            }
            
            // linearly interpolate volume in and out
            float elapsed = 0f;
            while (elapsed < fadeTime)
            {
                elapsed += 1.0f/60.0f;
                from.volume = Mathf.Lerp(.6f, 0f, elapsed / fadeTime);
                to.volume = Mathf.Lerp(0f, .6f, elapsed / fadeTime);
                yield return null;
            }

            yield return null;
        }
        
        public void FadeMusicMax()
        {
            StartCoroutine(crossFadeMenu(_bgm[_playingBgm], _bgm[4], 2f));
            _playingBgm = 4;
        }
        
        public void FadeMusicBase()
        {
            StartCoroutine(crossFadeMenu(_bgm[_playingBgm], _bgm[3], 2f));
            _playingBgm = 3;
        }
    }
}
