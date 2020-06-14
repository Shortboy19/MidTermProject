
using TMPro;
using UnityEngine;

public class DialogBox : MonoBehaviour
{

    public static DialogBox Instance;
    public GameObject window;
    public TextMeshProUGUI title;
    public TextMeshProUGUI message;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Debug.LogError("0 or multiple " + this + " in the scene."); }
    }

    public void Contiue()
    {
        Time.timeScale = 1;
        SoundManager.Instance.ResumeAllSounds();
        GameState.gamePaused = false;
        window.SetActive(false);
        GameState.canPause = true;
    }

    public static void ShowWindow(string TITLE, string MESSAGE, bool muteSounds = true)
    {
        GameState.canPause = false;
        Time.timeScale = 0;
        if (muteSounds) { SoundManager.Instance.StopAllSounds(); }
        GameState.gamePaused = true;
        Instance.window.SetActive(true);

        Instance.title.text = TITLE;
        Instance.message.text = MESSAGE;
    }
}
