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
        if (other.CompareTag("Enemy"))
        {
            // FlyingEnemy scriptini kontrol et
            FlyingEnemy flyingEnemy = other.GetComponent<FlyingEnemy>();
            if (flyingEnemy != null)
            {
                flyingEnemy.TakeDamage(damage);
            }

            Destroy(gameObject); // Mermiyi yok et
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
