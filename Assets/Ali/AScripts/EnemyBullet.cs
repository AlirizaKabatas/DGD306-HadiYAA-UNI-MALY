using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 15;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Otomatik yok olma süresi
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // SADECE Player ile temas ettiğinde hasar ver ve yok ol
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        // Diğer tag'lerle çarpışmada hiçbir şey yapma
    }
}
