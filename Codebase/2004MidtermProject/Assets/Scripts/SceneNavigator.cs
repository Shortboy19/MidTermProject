using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReloadSceneWithFade()
    {
        StartCoroutine(FadeOutScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadSceneWithFade(int index)
    {
        StartCoroutine(FadeOutScene(index));
    }
    public void LoadSceneWithFade(string name)
    {
        StartCoroutine(FadeOutScene(name));
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadNextSceneWithFade()
    {
        StartCoroutine(FadeOutScene(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void LoadPreviousSceneWithFade()
    {
        StartCoroutine(FadeOutScene(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void LoadMainMenuWithFade()
    {
        StartCoroutine(FadeOutScene("Main Menu"));
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
