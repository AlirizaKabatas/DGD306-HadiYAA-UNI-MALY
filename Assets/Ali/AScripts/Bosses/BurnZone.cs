using UnityEngine;

public class BurnZone : MonoBehaviour
{
    public int damagePerSecond = 10;  // Her saniyede verilecek hasar
    public float lifetime = 3f;  // Zone'un ömrü
    private float tickTimer = 0f;  // Zamanlayıcı başlangıç değeri

    void Start()
    {
        Destroy(gameObject, lifetime);  // Zone belirli bir süre sonra yok olacak
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tickTimer += Time.deltaTime;  // Zamanı artır

            // Eğer bir saniye geçerse, hasar ver
            if (tickTimer >= 1f)
            {
                PlayerHealth health = other.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damagePerSecond);  // Hasar ver
                }

                tickTimer = 0f;  // Zamanlayıcıyı sıfırla
            }
        }
    }
}
