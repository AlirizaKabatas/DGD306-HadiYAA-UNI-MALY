using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "NextLevel"; // Ge�ilecek sahne ad�

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncuya �arp�nca sahne de�i�tir
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}


