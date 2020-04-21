using UnityEngine.UI;
using UnityEngine;

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
    }
}
