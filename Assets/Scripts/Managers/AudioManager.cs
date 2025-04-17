using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioClip _gameplayMusic;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("SFX")]
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioClip _buttonClickSound;
    [SerializeField] private AudioClip _unrollSound;
    [SerializeField] private AudioClip _mummyWrappedSound;

    [Range(0f, 1f)] public float sfxVolume = 0.7f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
        }

        if (_sfxSource == null)
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.playOnAwake = false;
        }

        UpdateVolumes();
        PlayGameplayMusic();
    }
    public void UpdateVolumes()
    {
        _musicSource.volume = musicVolume;
        _sfxSource.volume = sfxVolume;
    }

    #region === Music ===
    public void PlayGameplayMusic() => PlayMusic(_gameplayMusic);

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void StopMusic() => _musicSource.Stop();

    #endregion
    #region === SFX ===
    public void PlayButtonClick() => PlaySFX(_buttonClickSound);
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        _sfxSource.PlayOneShot(clip);
    }

    public void PlayUnrollSound()
    {
        if (_unrollSound == null) return;
        _sfxSource.PlayOneShot(_unrollSound);
    }

    public void PlayMummyWrappedSound()
    {
        if (_unrollSound == null) return;
        _sfxSource.PlayOneShot(_mummyWrappedSound);
    }
    #endregion
}