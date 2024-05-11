using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;

    private void Awake()
    {
        float volume = 1;
        if (PlayerPrefs.HasKey("volume"))
            volume = PlayerPrefs.GetFloat("volume");
        volumeSlider.value = volume;
        volumeSlider.onValueChanged.AddListener((float _) => SetVolume(_));
    }
    public void SetVolume(float value)
    {
        PlayerPrefs.SetFloat("volume", value);
    }
}
