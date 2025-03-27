using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 3f;  

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        Destroy(gameObject);
    }
}
