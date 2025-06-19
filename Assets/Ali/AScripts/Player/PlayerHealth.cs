using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public Image leftHealthBar;
    public Image rightHealthBar;
    public AudioClip healSound;
    public AudioClip deathSound;
    public float flashDuration = 0.2f;
    public Renderer[] renderers;

    private int currentHealth;
    private bool isDead = false;
   
    [SerializeField] private AudioSource audioSource;


   void Start()
{
    currentHealth = maxHealth;

    // Eğer audioSource atanmadıysa, bileşen ekleyin
    if (audioSource == null)
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Eğer yine yoksa, bir tane ekleriz
        }
    }

    UpdateHealthUI();
}


    void UpdateHealthUI()
    {
        float fill = (float)currentHealth / maxHealth;
        if (leftHealthBar != null) leftHealthBar.fillAmount = fill;
        if (rightHealthBar != null) rightHealthBar.fillAmount = fill;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        if (audioSource && deathSound) audioSource.PlayOneShot(deathSound);
        gameObject.SetActive(false);  // Oyundan yok olur
        GameManager.Instance.PlayerDied();
        
    }

    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Heal"))
    {
        int healAmount = 20;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // Heal sesinin çalması için kontrol ekleyelim
        if (audioSource && healSound)
        {
            audioSource.PlayOneShot(healSound);
        }

        StartCoroutine(FlashGreen());
        Destroy(collision.gameObject);
    }
}


    IEnumerator FlashGreen()
    {
        foreach (var r in renderers)
            r.material.color = Color.green;

        yield return new WaitForSeconds(flashDuration);

        foreach (var r in renderers)
            r.material.color = Color.white;
    }
}
