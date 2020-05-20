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
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gamePaused = true;
    }
    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gamePaused = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
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
    }

    public static void ShowWinMenu()
    {
        gameWon = true;
        gamePaused = true;
        Instance.timer.isCounting = false;
        Instance.timer.GetTime();
        Instance.winMenu.SetActive(true);
    }

    public static void ResetState()
    {
        gamePaused = false;
        Time.timeScale = 1;
    }
}
