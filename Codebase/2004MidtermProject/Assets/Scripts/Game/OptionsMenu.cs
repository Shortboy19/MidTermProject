﻿
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static bool isOpen = false;
    [SerializeField] Slider voiceVol = null;
    [SerializeField] Slider effectsVol = null;
    [SerializeField] Slider ambientVol = null;
    [SerializeField] Slider sensitivitySlider = null;

    [SerializeField] TextMeshProUGUI voiceVolText = null;
    [SerializeField] TextMeshProUGUI effectsVolText = null;
    [SerializeField] TextMeshProUGUI ambientVolText = null;
    [SerializeField] TextMeshProUGUI sensitivityText = null;

    public void Start()
    {
        if (PlayerPrefs.HasKey("Voice Volume"))
        {
            SoundManager.VoiceVolume = PlayerPrefs.GetFloat("Voice Volume");
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
            if (PlayerController.Player != null)
            {
                PlayerController.Player.LookSenstivity = PlayerPrefs.GetFloat("Sensitivity");
            }
        }
        voiceVol.value = SoundManager.VoiceVolume;
        effectsVol.value = SoundManager.EffectsVolume;
        ambientVol.value = SoundManager.AmbientVolume;

        if (PlayerController.Player != null)
            sensitivitySlider.value = PlayerController.Player.LookSenstivity/200;
    }

    // Update is called once per frame
    void Update()
    {
        voiceVolText.text = (voiceVol.value * 100).ToString("F0");
        effectsVolText.text = (effectsVol.value * 100).ToString("F0");
        ambientVolText.text = (ambientVol.value * 100).ToString("F0");
        sensitivityText.text = (sensitivitySlider.value * 100).ToString("F0");
    }

    public void Apply()
    {
        //Debug.Log("Written");
        SoundManager.VoiceVolume = voiceVol.value;
        SoundManager.EffectsVolume = effectsVol.value;
        SoundManager.AmbientVolume = ambientVol.value;

        if (PlayerController.Player != null)
            PlayerController.Player.LookSenstivity = sensitivitySlider.value * 200;

        SoundManager.Instance.UpdateAmbientSoundVolume();
        SoundManager.Instance.UpdateVoiceSoundVolume();
        PlayerPrefs.SetFloat("Voice Volume", SoundManager.VoiceVolume);
        PlayerPrefs.SetFloat("Effects Volume", SoundManager.EffectsVolume);
        PlayerPrefs.SetFloat("Ambient Volume", SoundManager.AmbientVolume);

        if (PlayerController.Player != null)
            PlayerPrefs.SetFloat("Sensitivity", PlayerController.Player.LookSenstivity);
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
