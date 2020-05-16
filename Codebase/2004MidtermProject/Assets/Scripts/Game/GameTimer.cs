using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    float timer;

    string hours;
    string minutes;
    string seconds;

    public bool isCounting = false;

    [SerializeField] TextMeshProUGUI timerText;

    private void Start()
    {
        timer = 0;
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

        timerText.text = $" in {hours}:{minutes}:{seconds}";
    }
}
