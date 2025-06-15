using UnityEngine;
using UnityEngine.UI;

public class DottopusHealthBar : MonoBehaviour
{
    public Image healthBarImage;  // Health bar'ın Image component'ı
    private DottopusBoss boss;    // Boss referansı

    void Start()
    {
        boss = FindObjectOfType<DottopusBoss>();  // Boss objesini buluyoruz
    }

    void Update()
    {
        // Boss'un sağlığını health bar ile eşleştiriyoruz
        if (boss != null)
        {
            float healthPercentage = (float)boss.currentHealth / boss.maxHealth;  // Sağlık oranını hesapla
            healthBarImage.fillAmount = healthPercentage;  // Sağlık barını güncelle
        }
    }
}
