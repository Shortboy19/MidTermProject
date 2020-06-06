using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    float timer=0;
    int attempts=1;

    string hours;
    string minutes;
    string seconds;

    public static bool isCounting = false;

    [SerializeField] TextMeshProUGUI timerText;
    public static GameTimer Instance;
    private void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Debug.LogError("0 or multiple " + this + " in the scene."); }
    }
    private void Start()
    {

    }

    private void Update()
    {
        if(isCounting)
            timer += Time.deltaTime;
    }

    public void GetTime()
    {
        hours = Mathf.Floor(timer / 3600).ToString("00");
        minutes = Mathf.Floor(timer / 60).ToString("00");
        seconds = (timer % 60).ToString("00");

        if (timerText == null)
        {
            if (GameObject.Find("Timer"))
                timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        }
        timerText.text = $" in {hours}:{minutes}:{seconds} \n with {attempts} attempts";
        Save.WriteString(hours + ":" + minutes + ":" + seconds + "/" + attempts);
        Debug.Log(Save.ReadString());
    }
    public void AttemptFailed()
    {
        attempts++;
    }
}
