using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Salida : MonoBehaviour
{
    public GameObject victoryCanvas;

    private void Start()
    {
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            if (victoryCanvas != null)
            {
                victoryCanvas.SetActive(true);
            }
            StartCoroutine(RestartGameAfterDelay(3f));
        }
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}