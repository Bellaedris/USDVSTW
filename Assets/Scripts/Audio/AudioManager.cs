using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace usd
{
    /// <summary>
    /// Manages audio for the game, including background music, sound effects, and volume control.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        /// <summary>
        /// Singleton instance of the AudioManager.
        /// </summary>
        public static AudioManager Instance => _instance;
        
        public AudioMixer masterVolume;
        public AudioMixer vfxVolume;
        
        private AudioSource _playerSource;
        private AudioSource _enemiesSource;
        private AudioSource[] _bgm; // holds music for laser/gatling/blackHole/criticalHP/maxHP
        private int _playingBgm;
        private int _lastBgm;
        
        void Awake()
        {
            // Singleton pattern implementation
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
            _bgm[0] = sources[2]; // laser
            _bgm[1] = sources[3]; // gatling
            _bgm[2] = sources[4]; // blackhole
            _bgm[3] = sources[5]; // critical health
            _bgm[4] = sources[6]; // max health
            _bgm[5] = sources[7]; // menu and between waves
        }

        /// <summary>
        /// Interpolates between a minimal and maximal sound volume on the master audio mixer.
        /// </summary>
        /// <param name="value">Callback from the slider value: something between 0 and 1.</param>
        public void UpdateMasterVolume(float value)
        {
            float newVolume = Mathf.Lerp(-40f, 20f, value);
            masterVolume.SetFloat("volume", newVolume);
        }
        
        /// <summary>
        /// Same as UpdateMasterVolume, but applied to the SFX audio mixer.
        /// </summary>
        /// <param name="value">Callback from the slider value: something between 0 and 1.</param>
        public void UpdateSfxVolume(float value)
        {
            float newVolume = Mathf.Lerp(-40f, 20f, value);
            vfxVolume.SetFloat("volume", newVolume);
        }

        /// <summary>
        /// Plays a one-shot sound on the player audio source. Called by the player weapons.
        /// </summary>
        /// <param name="clip">The sound to play.</param>
        public void playWeaponSound(AudioClip clip)
        {
            _playerSource.PlayOneShot(clip);
        }
        
        /// <summary>
        /// Plays a one-shot sound on the general audio source. Called by the player and enemies.
        /// </summary>
        /// <param name="clip">The sound to play.</param>
        public void playGeneralSound(AudioClip clip)
        {
            _enemiesSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Fades the current music track out and fades the new music in.
        /// </summary>
        /// <param name="newWeaponID">ID of the weapon currently equipped.</param>
        /// <param name="level">Level of the weapon currently equipped.</param>
        /// <param name="maxWeaponLevel">Max level of all weapons.</param>
        public void FadeMusic(int newWeaponID, int level, int maxWeaponLevel)
        {
            // Do nothing if the weapon didn't change
            if (newWeaponID == _playingBgm && level > 0 && level < 5)
                return;
            if (maxWeaponLevel == 0)
            {
                StopAllCoroutines();
                StartCoroutine(crossFade(_bgm[_playingBgm], _bgm[3], 2f));
                _playingBgm = 3;
            }
            else if (level == 5 && _playingBgm != 4)
            {
                StopAllCoroutines();
                StartCoroutine(crossFade(_bgm[_playingBgm], _bgm[4], 2f));
                _playingBgm = 4;
            }
            else if (!(level == 5 && _playingBgm == 4))
            {
                StopAllCoroutines();
                StartCoroutine(crossFade(_bgm[_playingBgm], _bgm[newWeaponID], 2f));
                _playingBgm = newWeaponID;
            }
        }

        /// <summary>
        /// Crossfades between two audio sources over a specified duration.
        /// </summary>
        /// <param name="from">The audio source to fade out.</param>
        /// <param name="to">The audio source to fade in.</param>
        /// <param name="fadeTime">The duration of the fade.</param>
        private IEnumerator crossFade(AudioSource from, AudioSource to, float fadeTime)
        {
            // Kills all other music
            foreach (AudioSource source in _bgm)
            {
                if (source != from && source != to)
                    source.volume = 0f;
            }
            
            // Linearly interpolate volume in and out
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
        
        /// <summary>
        /// Fades in the menu music.
        /// </summary>
        public void FadeInMusicMenu()
        {
            StopAllCoroutines();
            StartCoroutine(crossFadeMenu(_bgm[_playingBgm], _bgm[5], 2f));
            _lastBgm = _playingBgm;
            _playingBgm = 5;
        }
        
        /// <summary>
        /// Fades out the menu music.
        /// </summary>
        public void FadeOutMusicMenu()
        {
            StopAllCoroutines();
            StartCoroutine(crossFadeMenu(_bgm[5], _bgm[_lastBgm], 2f));
            _playingBgm = _lastBgm;
        }
        
        /// <summary>
        /// Crossfades between two audio sources over a specified duration for menu music.
        /// </summary>
        /// <param name="from">The audio source to fade out.</param>
        /// <param name="to">The audio source to fade in.</param>
        /// <param name="fadeTime">The duration of the fade.</param>
        private IEnumerator crossFadeMenu(AudioSource from, AudioSource to, float fadeTime)
        {
            // Kills all other music
            foreach (AudioSource source in _bgm)
            {
                if (source != from && source != to)
                    source.volume = 0f;
            }
            
            // Linearly interpolate volume in and out
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
        
        /// <summary>
        /// Fades in the max health music.
        /// </summary>
        public void FadeMusicMax()
        {
            StopAllCoroutines();
            StartCoroutine(crossFadeMenu(_bgm[_playingBgm], _bgm[4], 2f));
            _playingBgm = 4;
        }
        
        /// <summary>
        /// Fades in the base health music.
        /// </summary>
        public void FadeMusicBase()
        {
            StopAllCoroutines();
            StartCoroutine(crossFadeMenu(_bgm[_playingBgm], _bgm[3], 2f));
            _playingBgm = 3;
        }
    }
}