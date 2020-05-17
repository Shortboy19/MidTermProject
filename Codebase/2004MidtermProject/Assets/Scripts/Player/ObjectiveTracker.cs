using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = string.Empty;
        DisplayNewObjective("Find the key", 1.5f);
    }

    public void DisplayNewObjective(string objective, float delay = 0)
    {
        StartCoroutine(NewObjective(objective, delay));
    }

    IEnumerator NewObjective(string message, float delay = 0)
    {
        text.color = Color.green;
        string oldObj = text.text;
        while(oldObj != string.Empty)
        {
            oldObj = oldObj.Remove(oldObj.Length - 1);
            text.text = oldObj;
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(delay);

        text.color = Color.white;
        for(int i = 0; i < message.Length; i++)
        {
            text.text += message[i];
            yield return new WaitForSeconds(0.025f);
        }
    }
}
