using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>(); 
    }

    public void EnableCollider(bool enable)
    {
        boxCollider.enabled = enable;
    }
}
