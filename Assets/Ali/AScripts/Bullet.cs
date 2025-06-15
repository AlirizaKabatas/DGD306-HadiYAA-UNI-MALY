using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifetime); // Mermiyi belirtilen süre sonra yok et
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Merminin çarptığı obje Enemy tag'ine sahip mi kontrol et
        if (other.CompareTag("Enemy"))
        {
            // DottopusBoss objesine hasar vermek için
            DottopusBoss boss = other.GetComponent<DottopusBoss>();
            if (boss != null)
            {
                Debug.Log("DottopusBoss'a hasar verildi!");
                boss.TakeDamage(damage);  // Boss'a hasar ver
            }

            // FlyingEnemy objelerine hasar vermek için
            FlyingEnemy flyingEnemy = other.GetComponent<FlyingEnemy>();
            if (flyingEnemy != null)
            {
                Debug.Log("FlyingEnemy'ye hasar verildi!");
                flyingEnemy.TakeDamage(damage);  // FlyingEnemy'e hasar ver
            }

            // Mermiyi yok et
            Destroy(gameObject);
        }
    }

    // Bu fonksiyon, merminin yönüne göre sprite'ı döndürüyor
    public void SetDirection(Vector2 direction)
    {
        // Yönü derecelere çeviriyoruz
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Sprite'ı yönlendiriyoruz
    }
}
