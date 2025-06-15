using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target1;
    public Transform target2;

    public float smoothTime = 0.2f;
    public Vector3 offset = new Vector3(0, 0, -10);
    public Vector2 deadZone = new Vector2(0.5f, 0.5f); // Küçük hareketleri yoksay

    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target1 == null || target2 == null)
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.player1Instance != null)
                    target1 = GameManager.Instance.player1Instance.transform;
                if (GameManager.Instance.player2Instance != null)
                    target2 = GameManager.Instance.player2Instance.transform;
            }
            return;
        }

        Move();
    }

    void Move()
    {
        Vector3 centerPoint = (target1.position + target2.position) / 2f;
        Vector3 desiredPosition = centerPoint + offset;

        // Dead zone: çok küçük hareketlerde kamera yer deðiþtirme
        Vector3 delta = desiredPosition - transform.position;
        if (Mathf.Abs(delta.x) < deadZone.x && Mathf.Abs(delta.y) < deadZone.y)
            return;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}

