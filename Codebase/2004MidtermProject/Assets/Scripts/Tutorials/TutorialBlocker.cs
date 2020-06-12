using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBlocker : MonoBehaviour
{
    Collider col;
    [SerializeField] Collider prevCol;
    [SerializeField] bool triggered = false;
    [SerializeField] bool playVoiceLine = true;
    void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !triggered)
        {
            if (prevCol != null)
            { prevCol.isTrigger = false; }
            triggered = true;
            
            if (SoundManager.Instance.VoiceLineSound.isPlaying)
            {
                if(playVoiceLine)
                    TutorialManager.PlayCutOffLine();
            }
        }
    }
}
