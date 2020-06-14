using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static float MusicVolume = 0.75f;
    public static float EffectsVolume = 0.75f;
    public static float AmbientVolume = 0.75f;

    //audio players component.
    [SerializeField] AudioSource EffectSound;
    [SerializeField] AudioSource MusicSound;
    [SerializeField] AudioSource RainSound;
    [SerializeField] AudioSource NightSound;
    [SerializeField] AudioSource ThunderSound;
    [SerializeField] AudioSource HeartBeatSound;
    public AudioSource BreathSound;
    public AudioSource VoiceLineSound;
    public AudioSource GazeSound;

    public AudioClip[] ThunderClap;
    public AudioClip[] PlayerWalking;
    public AudioClip PlayerRunning;
    public AudioClip Rain;
    public AudioClip MetalGate;
    public AudioClip GhostPassThroughWalls;
    public AudioClip GhostBreath;
    public AudioClip FlashLightClick;
    public AudioClip MetalGateClose;
    public AudioClip GhostFade;
    public AudioClip PlayerSliding;
    public AudioClip TrapButton;
    public AudioClip PlayerHurt; 
    public AudioClip MonolithActivate;
    public AudioClip Shard;
    public AudioClip Battery;
    public AudioClip Heartbeat;
    public AudioClip OutOfBreath; 
    public AudioClip TeleportTrap; 
    public AudioClip MonsterKill; 

    public AudioClip[] effects;
    public AudioClip[] voiceLines;

    //singleton instance
    public static SoundManager Instance;

    //Initialize the singleton instance
    private void Awake()
    {
        //If there is not already an instance of SoundManager, set it to this.
        if (Instance == null) { Instance = this; } else if (Instance != this) { Debug.LogError("0 or multiple " + this + " in the scene."); }
    }

    //Play a single Clip through the sound effects source.
    public void PlayEffectAtPoint(AudioClip sound, Vector3 point)
    {
        AudioSource.PlayClipAtPoint(sound, point, 1 * EffectsVolume);
    }
    public void PlayEffectAtPoint(AudioClip sound, Vector3 point, float volume)
    {
        AudioSource.PlayClipAtPoint(sound, point, volume * EffectsVolume);
    }

    public void PlayGlobalEffect(AudioClip clip)
    {
        EffectSound.volume = 1 * EffectsVolume;
        EffectSound.clip = clip;
        EffectSound.Play();
    }

    public void PlayGlobalEffect(AudioClip clip, float volume)
    {
        EffectSound.volume = volume * EffectsVolume;
        EffectSound.clip = clip;
        EffectSound.Play();
    }

    public void PlayEffect(AudioSource source, AudioClip clip)
    {
        source.volume = 1 * EffectsVolume;
        source.clip = clip;
        source.Play();
    }
    public void PlayGaze()
    {
        GazeSound.volume = 1 * EffectsVolume;
        GazeSound.Play();
    }

    //Play a single Clip through the music source.
    public void PlayMusic(AudioClip song)
    {
        MusicSound.volume = 1 * MusicVolume;
        MusicSound.clip = song;
        MusicSound.Play(); 
    }

    public void StartRain()
    {
        if (RainSound.isPlaying)
            return;
        
        RainSound.clip = Rain;
        RainSound.loop = true;
        RainSound.volume = 0;
        RainSound.Play();
        StartCoroutine(LerpRainVolume());
        NightSound.Play();
    }

    float volMod = 0;
    IEnumerator LerpRainVolume()
    {
        while(volMod < 1)
        {
            RainSound.volume = Mathf.Lerp(0, 0.15f * AmbientVolume, volMod);
            yield return new WaitForEndOfFrame();
            volMod += 0.01f;
        }
    }

    public void PlayThunderEffect(AudioClip clip, float volume)
    {
        ThunderSound.volume = volume * AmbientVolume;
        ThunderSound.clip = clip;
        ThunderSound.Play();
    }

    float EffectSoundVol;
    float MusicSoundVol;
    float RainSoundVol;
    float NightSoundVol;
    float ThunderSoundVol;
    float HeartbeatSoundVol;
    float BreathVol;

    public void StopAllSounds()
    {
        GetSoundVolumes();

        EffectSound.volume = 0;
        MusicSound.volume = 0;
        RainSound.volume = 0;
        NightSound.volume = 0;
        ThunderSound.volume = 0;
        HeartBeatSound.volume = 0;
        BreathSound.volume = 0;
    }

    public void ResumeAllSounds()
    {
        EffectSound.volume = EffectSoundVol;
        MusicSound.volume = MusicSoundVol;
        RainSound.volume = RainSoundVol;
        NightSound.volume = NightSoundVol;
        ThunderSound.volume = ThunderSoundVol;
        HeartBeatSound.volume = HeartbeatSoundVol;
        BreathSound.volume = BreathVol; 
    }

    void GetSoundVolumes()
    {
        EffectSoundVol = EffectSound.volume;
        MusicSoundVol = MusicSound.volume;
        RainSoundVol = RainSound.volume;
        NightSoundVol = NightSound.volume;
        ThunderSoundVol = ThunderSound.volume;
        HeartbeatSoundVol = HeartBeatSound.volume;
        BreathVol = BreathSound.volume; 
    }

    public void UpdateMusicSoundVolume()
    {
        MusicSound.volume = 1 * MusicVolume;
        MusicSoundVol = 1 * MusicVolume;
    }

    public void UpdateAmbientSoundVolume()
    {
        RainSoundVol = 0.15f * AmbientVolume;
        NightSoundVol = 0.15f * AmbientVolume;
    }

    public void PlayHeartBeatEffect(AudioClip clip)
    {
        HeartBeatSound.volume = 1 * EffectsVolume;
        HeartBeatSound.clip = clip;
        HeartBeatSound.Play();
    }

    public void PlayBreath(AudioClip clip)
    {
        BreathSound.volume = 1 * EffectsVolume;
        BreathSound.clip = clip;
        BreathSound.Play();
    }

    public void PlayVoiceLine(int i)
    {
        VoiceLineSound.volume = 1;
        VoiceLineSound.clip = voiceLines[i];
        VoiceLineSound.Play();
    }
}
