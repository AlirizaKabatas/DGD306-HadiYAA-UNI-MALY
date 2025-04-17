using UnityEngine;
using UnityEngine.UI; // Image component'ini kullanmak için

public class Enemy : MonoBehaviour
{
    public int maxHealth = 30;
    public float flashDuration = 0.2f; // 200ms
    private int currentHealth;

    private Image image; // SpriteRenderer yerine Image kullanacağız
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        image = GetComponent<Image>(); // Image component'ini alıyoruz
        originalColor = image.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        image.color = Color.red; // Kırmızı yapıyoruz
        yield return new WaitForSeconds(flashDuration);
        image.color = originalColor; // Orijinal renge geri dönüyoruz
    }

    void Die()
    {
        // Ölüm efekti vs. ekleyebilirsin
        Destroy(gameObject);
    }
}
