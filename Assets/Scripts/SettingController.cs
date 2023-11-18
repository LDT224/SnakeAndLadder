using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public static SettingController instance;

    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider audioSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("audioVolume"))
            LoadVolume();
        else
        {
            SetAudioVolume();
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetAudioVolume()
    {
        float volume = audioSlider.value;
        audioMixer.SetFloat("Audio", volume);
        PlayerPrefs.SetFloat("audioVolume", volume);
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void LoadVolume()
    {
        audioSlider.value = PlayerPrefs.GetFloat("audioVolume");
        SetAudioVolume();

        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();

        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSFXVolume();
    }
}
