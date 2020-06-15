using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAnim : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> text;
    [SerializeField] List<Button> button;
    SceneNavigator nav;
    Camera cam;
    void Start()
    {
        nav = GetComponentInParent<SceneNavigator>();
        source = gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
        cam = Camera.main;
    }

    public void Play(string name)
    {
        StartCoroutine(FadeElements(name));
    }

    public AnimationCurve curve;

    IEnumerator FadeText()
    {
        float _delta = 1f;
        while (_delta > 0f)
        {
            _delta -= Time.deltaTime;
            for(int i = 0; i < text.Count; i++)
            {
                Color color = text[i].color;
                text[i].color = new Color(color.r, color.g, color.b, curve.Evaluate(_delta));
            }
            
            yield return null;
        }
        textRoutine = null;
    }

    IEnumerator FadeButton()
    {
        float _delta = 1f;
        while (_delta > 0f)
        {
            _delta -= Time.deltaTime;
            for (int i = 0; i < button.Count; i++)
            {
                ColorBlock colors = button[i].colors;
                Color color = button[i].colors.normalColor;
                colors.normalColor = new Color(color.r, color.g, color.b, curve.Evaluate(_delta));
                colors.highlightedColor = new Color(color.r, color.g, color.b, curve.Evaluate(_delta));
                colors.selectedColor = new Color(color.r, color.g, color.b, curve.Evaluate(_delta));
                colors.disabledColor = new Color(color.r, color.g, color.b, curve.Evaluate(_delta));
                button[i].colors = colors;
            }

            yield return null;
        }
        buttonRoutine = null;
    }

    IEnumerator textRoutine;
    IEnumerator buttonRoutine;

    [SerializeField] Transform endpoint;
    IEnumerator FadeElements(string name)
    {
        float _delta = 0;
        PlaySound();
        textRoutine = FadeText();
        buttonRoutine = FadeButton();

        StartCoroutine(textRoutine);
        StartCoroutine(buttonRoutine);

        while(textRoutine != null && buttonRoutine != null)
        {
            yield return null;
        }

        nav.TutorialLoadScene(name, source);
        cam.transform.LookAt(endpoint);
        while (_delta < 1)
        {
            _delta += Time.deltaTime * 0.01f;
            cam.transform.position = Vector3.Lerp(cam.transform.position, endpoint.position, curve.Evaluate(_delta));
            yield return null;
        }
    }

    public AudioClip sound;
    private AudioSource source;

    void PlaySound()
    {
        //SoundManager.Instance.PlayEffectAtPoint(source.clip, transform.position, 1);
        source.Play();
    }
}
