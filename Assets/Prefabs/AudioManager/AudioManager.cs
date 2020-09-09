using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private AudioSource _audioSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject); 
        }

        _audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("Music")) _audioSource.mute = Convert.ToBoolean(PlayerPrefs.GetString("Music"));
        else
        {
            PlayerPrefs.SetString("Music", "false");
            _audioSource.mute = Convert.ToBoolean(PlayerPrefs.GetString("Music"));
        }
    }
    public void Music()
    {
        _audioSource.mute = !_audioSource.mute;
        PlayerPrefs.SetString("Music", _audioSource.mute.ToString());
    }
}