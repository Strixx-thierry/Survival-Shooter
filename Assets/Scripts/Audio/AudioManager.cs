using SurvivalShooter.Core;
using UnityEngine;

namespace SurvivalShooter.Audio
{
    /// <summary>Singleton audio owner, reacts to sound cues.</summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Sources")]
        [SerializeField] AudioSource musicSource;
        [SerializeField] AudioSource sfxSource;

        [Header("Clips")]
        [SerializeField] AudioClip music;
        [SerializeField] AudioClip playerShoot;
        [SerializeField] AudioClip playerDeath;
        [SerializeField] AudioClip enemySpawn;
        [SerializeField] AudioClip enemyShoot;
        [SerializeField] AudioClip meleeHit;
        [SerializeField] AudioClip uiClick;

        [SerializeField, Range(0f, 1f)] float musicVolume = 0.35f;

        const string MusicKey = "music_on";
        const string SfxKey = "sfx_on";

        public bool MusicOn
        {
            get => PlayerPrefs.GetInt(MusicKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(MusicKey, value ? 1 : 0);
                musicSource.mute = !value;
            }
        }

        public bool SfxOn
        {
            get => PlayerPrefs.GetInt(SfxKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(SfxKey, value ? 1 : 0);
                sfxSource.mute = !value;
            }
        }

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.playOnAwake = false;
            sfxSource.playOnAwake = false;
            musicSource.mute = !MusicOn;
            sfxSource.mute = !SfxOn;
        }

        void Start()
        {
            if (music != null) musicSource.Play();
        }

        void OnEnable() => GameEvents.SoundRequested += Play;
        void OnDisable() => GameEvents.SoundRequested -= Play;

        public void SetMusicPaused(bool paused)
        {
            if (paused) musicSource.Pause();
            else musicSource.UnPause();
        }

        void Play(SoundCue cue)
        {
            AudioClip clip = ClipFor(cue);
            if (clip != null) sfxSource.PlayOneShot(clip);
        }

        AudioClip ClipFor(SoundCue cue)
        {
            switch (cue)
            {
                case SoundCue.PlayerShoot: return playerShoot;
                case SoundCue.PlayerDeath: return playerDeath;
                case SoundCue.EnemySpawn: return enemySpawn;
                case SoundCue.EnemyShoot: return enemyShoot;
                case SoundCue.MeleeHit: return meleeHit;
                case SoundCue.UiClick: return uiClick;
                default: return null;
            }
        }
    }
}
