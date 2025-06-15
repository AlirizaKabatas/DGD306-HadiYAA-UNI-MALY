using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public static void CheckAllPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                // En az 1 oyuncu hâlâ yaşıyor → oyun devam
                return;
            }
        }

        // Hepsi ölü → DeathScreen sahnesine geç
        SceneManager.LoadScene("DeathScreen");
    }
}
