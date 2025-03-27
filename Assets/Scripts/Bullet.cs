using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask targetLayer;
    public float damage = 1f;  

    void OnCollisionEnter2D(Collision2D collision)
    {
    
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            Target target = collision.gameObject.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

      
            Destroy(gameObject);
        }
    }
}
