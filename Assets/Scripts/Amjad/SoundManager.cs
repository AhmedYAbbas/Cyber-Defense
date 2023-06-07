using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    // Public Variables
    public AudioClip MainMenuMusic;
    public AudioClip GameplayMusic;

    // Private Variables
    [HideInInspector] private AudioSource _audioSource;

    public static SoundManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic(GameplayMusic, .1f);
    }

    public void PlaySoundEffect(AudioClip SFX, float volume = 1f)
    {
        _audioSource.PlayOneShot(SFX, volume);
    }

    public void PlayBackgroundMusic(AudioClip BackgroundMusic, float volume = 1f, bool loop = true)
    {
        _audioSource.clip = BackgroundMusic;
        _audioSource.volume = volume;
        _audioSource.loop = loop;
        _audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        if (_audioSource != null)
            _audioSource.Stop();
    }

    public void SetBackgroundMusicVolume(float volume)
    {
        if (_audioSource != null)
            _audioSource.volume = volume;
    }
}