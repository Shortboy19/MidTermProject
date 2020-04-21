using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class sceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve;
    void Start()
    {
        StartCoroutine(FadeInScene());
    }
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOutScene(scene));
    }
    IEnumerator FadeInScene()
    {
        float _delta = 1f;
        while (_delta > 0f)
        {
            _delta -= Time.deltaTime;
            img.color = new Color(0f, 0f, 0f, curve.Evaluate(_delta));
            yield return 0;
        }
    }
    IEnumerator FadeOutScene(string scene)
    {
        float _delta = 0f;
        while (_delta < 1f)
        {
            _delta += Time.deltaTime;
            img.color = new Color(0f, 0f, 0f, curve.Evaluate(_delta));
            yield return 0;
        }
        SceneManager.LoadScene(scene);
    }

}
