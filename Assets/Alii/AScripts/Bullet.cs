using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifetime);
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


}
