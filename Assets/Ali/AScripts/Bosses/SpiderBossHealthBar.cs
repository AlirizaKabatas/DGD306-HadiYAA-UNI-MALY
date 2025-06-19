using UnityEngine;
using UnityEngine.UI;

public class SpiderBossHealthBar : MonoBehaviour
{
    public Image healthBarImage;  // Health bar'ın Image component'ı
    private SpiderBoss boss;      // SpiderBoss referansı

    void Start()
    {
        boss = FindObjectOfType<SpiderBoss>();  // Sahnedeki SpiderBoss'u bul
    }

    void Update()
    {
        if (boss != null)
        {
            float healthPercentage = (float)boss.currentHealth / boss.maxHealth;  // Sağlık oranını hesapla
            healthBarImage.fillAmount = healthPercentage;  // Barı güncelle
        }
    }
}
