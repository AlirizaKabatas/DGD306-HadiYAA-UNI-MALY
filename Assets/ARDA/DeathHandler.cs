using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathHandler : MonoBehaviour
{
    public static DeathHandler Instance;

    public GameObject deathImage; // Ekranda gösterilecek PNG
    private int deadPlayerCount = 0;

    private void Awake()
    {
        // Singleton benzeri bir kullaným (tek sahnelik)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (deathImage != null)
            deathImage.SetActive(false); // Baþlangýçta kapalý
    }

    public void PlayerDied()
    {
        deadPlayerCount++;

        if (deadPlayerCount >= 2) // Ýki karakter de öldü
        {
            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
    {
        if (deathImage != null)
            deathImage.SetActive(true);

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


