using System.Collections;
using System.Collections.Generic;
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
    }

    public static void ShowWindow(string TITLE, string MESSAGE)
    {
        Time.timeScale = 0;
        SoundManager.Instance.StopAllSounds();
        GameState.gamePaused = true;
        Instance.window.SetActive(true);

        Instance.title.text = TITLE;
        Instance.message.text = MESSAGE;
    }
}
