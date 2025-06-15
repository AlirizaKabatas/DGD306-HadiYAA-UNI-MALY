using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StaticObstacle : MonoBehaviour
{
    void Awake()
    {
        // Rigidbody2D yoksa ekle
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        // Kinematic yap
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Hareket ve dönüþü engelle
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Yerçekimini sýfýrla
        rb.gravityScale = 0;
    }
}
