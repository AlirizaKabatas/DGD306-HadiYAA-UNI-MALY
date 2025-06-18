using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target1;
    public Transform target2;

    public float smoothTime = 0.2f;
    public Vector3 offset = new Vector3(0, 0, -10);
    public Vector2 deadZone = new Vector2(0.5f, 0.5f);

    private Vector3 velocity;

    void LateUpdate()
    {
        // GameManager varsa ve playerlar sahnedeyse güncelle
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.player1Instance != null)
                target1 = GameManager.Instance.player1Instance.transform;
            else
                target1 = null;

            if (GameManager.Instance.player2Instance != null)
                target2 = GameManager.Instance.player2Instance.transform;
            else
                target2 = null;
        }

        // Hiç oyuncu yoksa hareket etme
        if (target1 == null && target2 == null)
            return;

        Vector3 desiredPosition;

        // Ýki oyuncu hayattaysa ortasýný bul
        if (target1 != null && target2 != null)
        {
            Vector3 center = (target1.position + target2.position) / 2f;
            desiredPosition = center + offset;
        }
        // Sadece biri hayattaysa onu takip et
        else if (target1 != null)
        {
            desiredPosition = target1.position + offset;
        }
        else // sadece target2 != null
        {
            desiredPosition = target2.position + offset;
        }

        // Dead zone kontrolü
        Vector3 delta = desiredPosition - transform.position;
        if (Mathf.Abs(delta.x) < deadZone.x && Mathf.Abs(delta.y) < deadZone.y)
            return;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}



