
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public static GameState Instance;
    public static bool gamePaused = false;
    public static bool canPause = true;

    public GameObject pauseMenu = null;
    public GameObject deathMenu = null;
    public GameObject winMenu = null;
    public GameObject tutorialExit = null;
    public Image fadeImg = null;
    [SerializeField] OptionsMenu options = null;

    public static bool gameWon = false;
    public bool isMainMenu = false;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Debug.LogError("0 or multiple " + this + " in the scene."); }

        Time.timeScale = 1;
        gamePaused = false;
        if(!isMainMenu)
        {
            deathMenu.SetActive(false);
            winMenu.SetActive(false);
            gameWon = false;
            tutorialExit.SetActive(false);
        }
       // SoundManager.Instance.ResumeAllSounds(); 
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
        SoundManager.Instance.StopAllSounds();
    }
    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gamePaused = false;
        SoundManager.Instance.ResumeAllSounds();
    }

    private void Update()
    {
        if (gamePaused || isMainMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (isMainMenu || !canPause)
            return;

        if (Input.GetButtonDown("Pause") && !OptionsMenu.isOpen && !deathMenu.activeSelf && !winMenu.activeSelf && (fadeImg.color.a==0))
        {
            TogglePause();
        }

    }

    public static void ShowDeathMenu()
    {
        SoundManager.Instance.PlayEffectAtPoint(SoundManager.Instance.PlayerHurt, PlayerController.Player.transform.position);
        gamePaused = true;
        Instance.deathMenu.SetActive(true);
        GameTimer.Instance.AttemptFailed();
        SoundManager.Instance.StopAllSounds(); 
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

    public static void ShowTutorialExit()
    {
        if(PlayerController.Player.hitExit)
        {
            gameWon = true;
            gamePaused = true;
            Instance.tutorialExit.SetActive(true);
            PlayerPrefs.SetString("TutorialState","Complete");
        }
    }

    public static void ResetState()
    {
        SoundManager.Instance.ResumeAllSounds();
        Time.timeScale = 1;
        if(Instance != null && Instance.pauseMenu != null)
            Instance.pauseMenu.SetActive(false);
        gamePaused = false;
        AudioListener.pause = false;
        canPause = true;
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
