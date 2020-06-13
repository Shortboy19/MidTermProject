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
    }

    public void DisplayNewObjective(string objective, float delay = 1f)
    {
        if(objectiveRoutine == null)
        {
            objectiveRoutine = NewObjective(objective, delay);
            StartCoroutine(objectiveRoutine);
        }
        else
        {
            StopCoroutine(objectiveRoutine);
            objectiveRoutine = NewObjective(objective, delay);
            StartCoroutine(objectiveRoutine);
        }
    }

    IEnumerator objectiveRoutine;

    IEnumerator NewObjective(string message, float delay)
    {
        text.color = Color.green;
        yield return new WaitForSeconds(delay);
        string oldObj = text.text;
        while(oldObj != string.Empty)
        {
            oldObj = oldObj.Remove(oldObj.Length - 1);
            text.text = oldObj;
            yield return new WaitForSeconds(0.025f);
        }

        text.color = Color.white;
        for(int i = 0; i < message.Length; i++)
        {
            text.text += message[i];
            yield return new WaitForSeconds(0.025f);
        }
    }

    public void DisplayOldObjective(string objective, float delay = 1f)
    {
        if (objectiveRoutine == null)
        {
            objectiveRoutine = OldObjective(objective, delay);
            StartCoroutine(objectiveRoutine);
        }
        else
        {
            StopCoroutine(objectiveRoutine);
            objectiveRoutine = OldObjective(objective, delay);
            StartCoroutine(objectiveRoutine);
        }
    }

    IEnumerator OldObjective(string message, float delay)
    {
        text.color = Color.red;
        yield return new WaitForSeconds(delay);
        string oldObj = text.text;
        while (oldObj != string.Empty)
        {
            oldObj = oldObj.Remove(oldObj.Length - 1);
            text.text = oldObj;
            yield return new WaitForSeconds(0.025f);
        }

        text.color = Color.white;
        for (int i = 0; i < message.Length; i++)
        {
            text.text += message[i];
            yield return new WaitForSeconds(0.025f);
        }
    }
}
