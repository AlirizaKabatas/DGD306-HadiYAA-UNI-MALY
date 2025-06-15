using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject burnZonePrefab;
    public LayerMask groundLayer;
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player'a çarparsa
        if (collision.CompareTag(playerTag))
        {
            SpawnBurnZone();
            Destroy(gameObject);
        }

        // Ground Layer'ına çarparsa
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            SpawnBurnZone();
            Destroy(gameObject);
        }
    }

    void SpawnBurnZone()
    {
        if (burnZonePrefab)
        {
            Instantiate(burnZonePrefab, transform.position, Quaternion.identity);
        }
    }
}
