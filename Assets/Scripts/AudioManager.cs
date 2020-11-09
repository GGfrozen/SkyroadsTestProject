using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicPlayer.volume = PlayerPrefs.GetFloat(PREFS_MUSIC_VOLUME, 0.5f);
            effectsPlayer.volume = PlayerPrefs.GetFloat(PREFS_MUSIC_VOLUME, 0.5f);
        }
    }
    #endregion

    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource effectsPlayer;

    private const string PREFS_MUSIC_VOLUME = "Volume";
    private const string PREFS_EFFECTS_VOLUME = "Effects";
    
    public void PlayEffect(AudioClip effect)
    {
        effectsPlayer.PlayOneShot(effect);
    }
    public void SetMusicVolume(float volume)
    {
        musicPlayer.volume = volume;
        PlayerPrefs.SetFloat(PREFS_MUSIC_VOLUME, volume);
    }

    public void SetEffectsVolume(float volume)
    {
        effectsPlayer.volume = volume;
        PlayerPrefs.SetFloat(PREFS_EFFECTS_VOLUME, volume);
    }
    public float GetMusicVolume()
    {
        return musicPlayer.volume;
    }

    public float GetEffectsVolume()
    {
        return effectsPlayer.volume;
    }
    
}
