using UnityEngine;

public class BossCanvasTrigger : MonoBehaviour
{
    public GameObject bossHealthCanvas;

    private void OnTriggerEnter2D(Collider2D other)

    {
        if (other.CompareTag("Player"))
        {
            bossHealthCanvas.SetActive(true);
        }
    }
}

