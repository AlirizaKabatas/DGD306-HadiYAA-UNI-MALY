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

        // Hareket ve d�n��� engelle
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Yer�ekimini s�f�rla
        rb.gravityScale = 0;
    }
}
