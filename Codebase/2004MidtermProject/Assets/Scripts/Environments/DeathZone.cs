using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    [SerializeField] bool showMenu = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !PlayerController.Player.cc.isGrounded)
        {
            SoundManager.Instance.PlayGlobalEffect(SoundManager.Instance.PlayerHurt);
            if (showMenu)
                GameState.ShowDeathMenu();
            else
            {
                SoundManager.Instance.VoiceLineSound.Stop();
                SoundManager.Instance.VoiceLineSound.volume = 0;
                SoundManager.Instance.VoiceLineSound.clip = null;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                TutorialManager.pitDeath = true;
                GameState.ResetState();
            }
        }
    }
}
