using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    int attempts=1;

    public static bool isCounting = false;

    [SerializeField] TextMeshProUGUI timerText;
    public static GameTimer Instance;
    TimeSpan timeCounter;
    DateTime lastChecked;
    public float updateFrequency = 0.1f;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Debug.LogError("0 or multiple " + this + " in the scene."); }
    }
    
    void Start()
    {
        long ticks = 0;
        timeCounter = new TimeSpan(ticks);
        lastChecked = DateTime.Now;

        StartCoroutine(CalculateTime());
    }

    IEnumerator CalculateTime()
    {
        if (!isCounting)
            yield return null;

        bool bRun = true;

        while (bRun)
        {
            DateTime now = DateTime.Now;

            timeCounter += now - lastChecked;

            lastChecked = now;
            yield return new WaitForSeconds(updateFrequency);
        }
    }

    public void GetTime()
    {
        if (timerText == null)
        {
            if (GameObject.Find("Timer"))
                timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        }

        timerText.text = $" in {timeCounter.Hours}:{timeCounter.Minutes}:{timeCounter.Seconds} with {attempts} attempts";
        Save.WriteString(timeCounter.Hours.ToString("00") + ":" + timeCounter.Minutes.ToString("00") + ":" + timeCounter.Seconds.ToString("00") + "/" + attempts);
        Debug.Log(Save.ReadString());
    }
    public void AttemptFailed()
    {
        attempts++;
    }
}
