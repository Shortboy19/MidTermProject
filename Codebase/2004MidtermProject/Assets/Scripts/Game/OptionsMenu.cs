﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Slider musicVol;
    [SerializeField] Slider effectsVol;
    [SerializeField] Slider ambientVol;

    [SerializeField] TextMeshProUGUI musicVolText;
    [SerializeField] TextMeshProUGUI meffectsVolText;
    [SerializeField] TextMeshProUGUI ambientVolText;

    void Start()
    {
        musicVol.value = SoundManager.MusicVolume;
        effectsVol.value = SoundManager.EffectsVolume;
        ambientVol.value = SoundManager.AmbientVolume;
    }

    // Update is called once per frame
    void Update()
    {
        musicVolText.text = (musicVol.value * 100).ToString("F0");
        meffectsVolText.text = (effectsVol.value * 100).ToString("F0");
        ambientVolText.text = (ambientVol.value * 100).ToString("F0");
    }

    public void Apply()
    {
        SoundManager.MusicVolume = musicVol.value;
        SoundManager.EffectsVolume = effectsVol.value;
        SoundManager.AmbientVolume = ambientVol.value;

        SoundManager.Instance.UpdateAmbientSoundVolume();
        SoundManager.Instance.UpdateMusicSoundVolume();
    }


}