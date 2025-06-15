using UnityEngine;

public class BurnZone : MonoBehaviour
{
    public int damagePerSecond = 10;
    public float lifetime = 3f;
    private float tickTimer = 1f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tickTimer -= Time.deltaTime;
            if (tickTimer <= 0f)
            {
                PlayerHealth health = other.GetComponent<PlayerHealth>();
                if (health != null)
                    health.TakeDamage(damagePerSecond);

                tickTimer = 1f; // 1 saniyede bir vurur
            }
        }
    }
}
