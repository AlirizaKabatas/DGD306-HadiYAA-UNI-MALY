using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "NextLevel"; // Geçilecek sahne adý

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncuya çarpýnca sahne deðiþtir
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}


