using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMTutorialChecker : MonoBehaviour
{
    [SerializeField] GameObject button = null;
    void Start()
    {
        if (PlayerPrefs.GetString("TutorialState") == "Complete")
        {
            button.SetActive(true);
        }
        else
        {
            button.SetActive(false);
        }
    }
}
