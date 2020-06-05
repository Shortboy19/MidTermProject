using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;
    public static bool gamePaused = false;

    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject winMenu;
    [SerializeField] OptionsMenu options;

    public static bool gameWon = false;

    public GameTimer timer;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Debug.LogError("0 or multiple " + this + " in the scene."); }

        Time.timeScale = 1;
        gamePaused = false;
        deathMenu.SetActive(false);
        winMenu.SetActive(false);
        gameWon = false;
    }

    public void TogglePause()
    {
        if (gamePaused)
            Unpause();
        else
            Pause();
    }

    public void Pause()
    {
        SoundManager.Instance.StopAllSounds();
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gamePaused = true;
        AudioListener.pause = true; 
    }
    public void Unpause()
    {
        SoundManager.Instance.ResumeAllSounds();
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gamePaused = false;
        AudioListener.pause = false; 
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !OptionsMenu.isOpen)
        {
            TogglePause();
        }

        if (!gamePaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public static void ShowDeathMenu()
    {
        //SoundManager.Instance.PlayGlobalEffect(SoundManager.Instance.PlayerHurt);
        gamePaused = true;
        Instance.deathMenu.SetActive(true);
        GameTimer.Instance.AttemptFailed();
        AudioListener.pause = true;
    }

    public static void ShowWinMenu()
    {
        if(PlayerController.Player.hitExit)
        {
            gameWon = true;
            gamePaused = true;
            Instance.winMenu.SetActive(true);
            GameTimer.isCounting = false;
            GameTimer.Instance.GetTime();
        }
    }

    public static void ResetState()
    {
        gamePaused = false;
        Time.timeScale = 1;
    }

    public GameObject optionsMenu;

    public void ShowOptions()
    {
        optionsMenu.SetActive(true);
    }

    public void HideOptions()
    {
        optionsMenu.SetActive(false);
    }

    private void Start()
    {
        options.Start();
        options.Apply();
    }
}
