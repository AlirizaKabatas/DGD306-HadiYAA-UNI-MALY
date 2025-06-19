using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathHandler : MonoBehaviour
{
    public static DeathHandler Instance;

    public GameObject deathImage; // Ekranda g�sterilecek PNG
    private int deadPlayerCount = 0;

    private void Awake()
    {
        // Singleton benzeri bir kullan�m (tek sahnelik)
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (deathImage != null)
            deathImage.SetActive(false); // Ba�lang��ta kapal�
    }

    public void PlayerDied()
    {
        deadPlayerCount++;

        if (deadPlayerCount >= 2) // �ki karakter de �ld�
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


