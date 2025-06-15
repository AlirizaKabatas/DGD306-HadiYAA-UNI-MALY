using UnityEngine;

public class Heal : MonoBehaviour
{
    [Header("Prefab Ayarları")]
    public GameObject spawnPrefab;  // Düşman öldüğünde spawnlanacak prefab
    public Transform spawnLocation; // Prefab'ın spawnlanacağı konum

    private void OnDeath()
    {
        if (spawnPrefab != null && spawnLocation != null)
        {
            Instantiate(spawnPrefab, spawnLocation.position, Quaternion.identity);
        }

        // Düşman öldü, öldürme işlemi (örneğin, düşmanı yok et)
        Destroy(gameObject);
    }

    // Bu fonksiyon bir düşmanın öldüğünü tespit eder ve OnDeath fonksiyonunu tetikler
    public void TakeDamage(int damage)
    {
        // Düşman sağlık mekanizması (örneğin bir health değişkeni ile)
        int currentHealth = 100; // Örnek health, sizin mekanizmanıza bağlı olarak değiştirebilirsiniz

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            OnDeath();  // Düşman öldü, prefab spawnla
        }
    }
}
