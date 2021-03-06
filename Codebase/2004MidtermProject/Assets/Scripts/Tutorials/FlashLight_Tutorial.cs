using UnityEngine;

public class FlashLight_Tutorial : MonoBehaviour
{
    public Light[] tutorialLights = null;
    [SerializeField] GameObject battery = null;
    [SerializeField] bool trigger = false;
    bool triggered = false;

    private void Start()
    {
        battery.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !triggered)
        {
            if(!trigger)
            {
                for (int i = 0; i < tutorialLights.Length; i++)
                {
                    tutorialLights[i].gameObject.SetActive(false);
                }
                TutorialManager.PlayVoiceLine(4);
                PlayerController.Player.objective.DisplayNewObjective("Pickup the battery");
                PlayerController.tutorialBattery = true;
                battery.SetActive(true);
                triggered = true;
            }
            else
            {
                if(PlayerController.Player.currBattery <= 1)
                {
                    PlayerController.Player.currBattery = 1;
                }
                TutorialManager.PlayVoiceLine(6);
                PlayerController.Player.objective.DisplayNewObjective("Turn on the lights");
                triggered = true;
            }
        }
    }
}
