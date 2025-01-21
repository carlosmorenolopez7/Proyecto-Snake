using UnityEngine;
using System.Collections;
using TMPro;

public class Aviso : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeOutAndDeactivate(3f));
    }

    private IEnumerator FadeOutAndDeactivate(float duration)
    {
        Color startColor = textMeshPro.color;
        float rate = 1.0f / duration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            textMeshPro.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), progress);
            progress += rate * Time.deltaTime;

            yield return null;
        }

        textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, 0);
        gameObject.SetActive(false);
    }
}