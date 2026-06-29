using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SmallHedge.SoundManager;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class VolumeSlider : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider effectSlider;
    [SerializeField] Slider musicSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("EffectVolume"))
            PlayerPrefs.SetFloat("EffectVolume", 1f);

        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetFloat("MusicVolume", 1f);

        LoadVolumeSliders();
        SetEffectVolume(effectSlider.value);
        SetMusicVolume(musicSlider.value);
    }

    public void SetEffectVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("EffectVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    private void LoadVolumeSliders()
    {
        effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    private void SaveEffectVolumeSlider()
    {
        PlayerPrefs.SetFloat("EffectVolume", effectSlider.value);
    }
    private void SaveMusicVolumeSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void OnPointerUp(PointerEventData eventData) //save the volume settings when the user releases the slider
    {
        SaveEffectVolumeSlider();
        SaveMusicVolumeSlider();
    }
}