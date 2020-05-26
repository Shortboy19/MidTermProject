﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !PlayerController.Player.cc.isGrounded)
        {
            SoundManager.Instance.PlayGlobalEffect(SoundManager.Instance.PlayerHurt); 
            GameState.ShowDeathMenu();
        }
    }
}
