using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    public TextMeshProUGUI leaderboardText;
    void Start()
    {
        List<string> sortedList= BubbleSort(Save.ReadString());
        string finalText="";
        for (int i = 0; i < 11; i++)
        {
            if (i<sortedList.Count&&i>0)
            {
                finalText = finalText + (i) + ": "+ sortedList[i] + "\n";
            }
        }
        leaderboardText.text = finalText;
    }
    private static List<string> BubbleSort(List<string> titles)
    {
        bool hasSwapped;
        List<string> newList = new List<string>();
        for (int i = 0; i < titles.Count; i++)
        {
            newList.Add(titles[i]);
        }
        do
        {
            hasSwapped = false;
            for (int i = 0; i < newList.Count - 1; i++)
            {
                if (newList[i].CompareTo(newList[i + 1]) > 0)
                {
                    string temp = newList[i + 1];
                    newList[i + 1] = newList[i];
                    newList[i] = temp;
                    hasSwapped = true;
                }
            }
        } while (hasSwapped);
        return newList;
    }
}
