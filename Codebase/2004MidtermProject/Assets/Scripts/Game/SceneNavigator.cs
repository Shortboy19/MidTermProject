using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void ReloadScene()
    {
        GameState.ResetState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReloadSceneWithFade()
    {
        GameState.ResetState();
        StartCoroutine(FadeOutScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadScene(int index)
    {
        GameState.ResetState();
        SceneManager.LoadScene(index);
    }
    public void LoadScene(string name)
    {
        GameState.ResetState();
        SceneManager.LoadScene(name);
    }

    public void TutorialLoadScene(string name)
    {
        if(PlayerPrefs.GetString("TutorialState") != "Complete")
        {
            StartCoroutine(FadeOutScene("TutorialScene"));
        }
        else
        {
            StartCoroutine(FadeOutScene(name));
        }
    }

    public void LoadSceneWithFade(int index)
    {
        GameState.ResetState();
        StartCoroutine(FadeOutScene(index));
    }
    public void LoadSceneWithFade(string name)
    {
        GameState.ResetState();
        StartCoroutine(FadeOutScene(name));
    }

    public void LoadNextScene()
    {
        GameState.ResetState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadPreviousScene()
    {
        GameState.ResetState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadNextSceneWithFade()
    {
        GameState.ResetState();
        StartCoroutine(FadeOutScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void LoadPreviousSceneWithFade()
    {
        GameState.ResetState();
        StartCoroutine(FadeOutScene(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void LoadMainMenu()
    {
        GameState.ResetState();
        SceneManager.LoadScene("Main Menu");
        GameState.gamePaused = false;
    }
    public void LoadMainMenuWithFade()
    {
        GameState.ResetState();
        StartCoroutine(FadeOutScene("Main Menu"));
        GameState.gamePaused = false;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    #region SceneFade
    public Image fadeImg;
    public AnimationCurve curve;
    void Awake()
    {
        if(fadeImg != null)
        {
            fadeImg.color = new Color(0f, 0f, 0f, 255);
            StartCoroutine(FadeIn());
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator FadeIn()
    {
        float _delta = 1f;
        while (_delta > 0f)
        {
            _delta -= Time.deltaTime;
            fadeImg.color = new Color(0f, 0f, 0f, curve.Evaluate(_delta));
            yield return 0;
        }
    }

    IEnumerator FadeOutScene(string name)
    {
        float _delta = 0f;
        while (_delta < 1f)
        {
            _delta += Time.deltaTime;
            fadeImg.color = new Color(0f, 0f, 0f, curve.Evaluate(_delta));
            yield return 0;
        }
        SceneManager.LoadScene(name);
    }
    IEnumerator FadeOutScene(int index)
    {
        float _delta = 0f;
        while (_delta < 1f)
        {
            _delta += Time.deltaTime;
            fadeImg.color = new Color(0f, 0f, 0f, curve.Evaluate(_delta));
            yield return 0;
        }
        SceneManager.LoadScene(index);
    }
    #endregion
}
