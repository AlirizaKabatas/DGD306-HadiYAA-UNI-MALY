using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Takip edilecek obje (örneğin karakter)
    public Vector3 offset;   // Kamera ile hedef arasındaki mesafe (isteğe bağlı)

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        }
    }
}
