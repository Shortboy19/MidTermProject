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

    public void Start()
    {
        if (PlayerPrefs.HasKey("Music Volume"))
        {
            SoundManager.MusicVolume = PlayerPrefs.GetFloat("Music Volume");
        }
        if (PlayerPrefs.HasKey("Effects Volume"))
        {
            SoundManager.EffectsVolume = PlayerPrefs.GetFloat("Effects Volume");
        }
        if (PlayerPrefs.HasKey("Ambient Volume"))
        {
            SoundManager.AmbientVolume = PlayerPrefs.GetFloat("Ambient Volume");
        }
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerController.Player.LookSenstivity = PlayerPrefs.GetFloat("Sensitivity");
        }
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
        PlayerPrefs.SetFloat("Music Volume", musicVol.value);
        PlayerPrefs.SetFloat("Effects Volume", effectsVol.value);
        PlayerPrefs.SetFloat("Ambient Volume", ambientVol.value);
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value * 200);
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
