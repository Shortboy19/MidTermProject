using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSelector : MonoBehaviour {
    public sceneFader fader;
    public Button[] levelButtons;
    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if(i >= levelReached)
            levelButtons[i].interactable = false;
        }
    }

    public void Select (string levelName)
    {
        fader.FadeTo(levelName);
        //Only the "1" button has a scene attached to it at the moment
    }
    public void AddtoPref()
    {
        if (PlayerPrefs.GetInt("levelReached")>6)
        {
            PlayerPrefs.SetInt("levelReached", 1);
        }
        PlayerPrefs.SetInt("levelReached", PlayerPrefs.GetInt("levelReached") + 1);
        fader.FadeTo(SceneManager.GetActiveScene().name);
    }
}
