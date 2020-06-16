using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour
{
    [SerializeField] Vector3 start = Vector3.zero, End = Vector3.zero;
    private float T = 0;
    public TextMeshProUGUI Text = null;
    void Start()
    {
        StartCoroutine("Animate");
    }
    public IEnumerator Animate()
    {
        do
        {
            Text.rectTransform.anchoredPosition = Vector3.Lerp(start, End, T);

            T += Time.deltaTime * 0.05f;
            yield return null;
        } while (T < 1);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

}
