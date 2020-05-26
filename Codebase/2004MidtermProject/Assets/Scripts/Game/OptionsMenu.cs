using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static bool isOpen = false;
    [SerializeField] Slider musicVol;
    [SerializeField] Slider effectsVol;
    [SerializeField] Slider ambientVol;
    [SerializeField] Slider sensitivitySlider;

    [SerializeField] TextMeshProUGUI musicVolText;
    [SerializeField] TextMeshProUGUI meffectsVolText;
    [SerializeField] TextMeshProUGUI ambientVolText;
    [SerializeField] TextMeshProUGUI sensitivityText;

    void Start()
    {
        musicVol.value = SoundManager.MusicVolume;
        effectsVol.value = SoundManager.EffectsVolume;
        ambientVol.value = SoundManager.AmbientVolume;
        sensitivitySlider.value = PlayerController.Player.LookSenstivity/200;
    }

    // Update is called once per frame
    void Update()
    {
        musicVolText.text = (musicVol.value * 100).ToString("F0");
        meffectsVolText.text = (effectsVol.value * 100).ToString("F0");
        ambientVolText.text = (ambientVol.value * 100).ToString("F0");
        sensitivityText.text = (sensitivitySlider.value *100).ToString("F0");
    }

    public void Apply()
    {
        SoundManager.MusicVolume = musicVol.value;
        SoundManager.EffectsVolume = effectsVol.value;
        SoundManager.AmbientVolume = ambientVol.value;
        PlayerController.Player.LookSenstivity = sensitivitySlider.value * 200;

        SoundManager.Instance.UpdateAmbientSoundVolume();
        SoundManager.Instance.UpdateMusicSoundVolume();
    }

    private void OnEnable()
    {
        isOpen = true;
    }
    private void OnDisable()
    {
        isOpen = false;
    }
}
