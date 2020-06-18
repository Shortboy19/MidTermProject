
using UnityEngine;
using UnityEngine.UI;

public class BatteryScript : MonoBehaviour
{
    public GameObject[] batteryGauge = null;

    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = PlayerController.Player.currBattery / PlayerController.Player.maxBattery;
        //int j = 0;
        //for (int i = 4; i > 0; i--)
        //{
        //    if (PlayerController.Player.currBattery >= PlayerController.Player.maxBattery * i / 5)
        //    {
        //        batteryGauge[j].SetActive(true);
        //    }
        //    else
        //    {
        //        batteryGauge[j].SetActive(false);
        //    }
        //    j++;
        //}
    }
}
