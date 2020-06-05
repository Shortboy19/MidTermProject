using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveTracker : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = string.Empty;
        if (SceneManager.GetActiveScene().name== "ProtoypeMilestoneScene")
        {
            DisplayNewObjective("Find the key", 1.5f);
        }
    }

    public void DisplayNewObjective(string objective, float delay = 1.5f)
    {
        StartCoroutine(NewObjective(objective, delay));
    }

    IEnumerator NewObjective(string message, float delay = 1.5f)
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

    public void DisplayOldObjective(string objective, float delay = 1.5f)
    {
        StartCoroutine(OldObjective(objective, delay));
    }

    IEnumerator OldObjective(string message, float delay = 1.5f)
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
