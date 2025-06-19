using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public Transform target1;
    public Transform target2;

    public Vector3 offset = new Vector3(0f, 0f, -10f); // Z genelde sabit kalýr (2D kameralar için)
    public float smoothTime = 0.3f;
    public float zoomLimiter = 5f;

    public float minZoom = 5f;
    public float maxZoom = 10f;

    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        int activeTargets = GetActiveTargetCount();

        if (activeTargets == 0)
            return;

        Move(activeTargets);
        Zoom(activeTargets);
    }

    void Move(int activeTargets)
    {
        Vector3 targetPosition = GetCenterPoint(activeTargets) + offset;

        // Sadece Z sabit, X ve Y takip eder
        Vector3 fixedZPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, fixedZPosition, ref velocity, smoothTime);
    }

    void Zoom(int activeTargets)
    {
        if (activeTargets == 2)
        {
            float distance = (target1.position - target2.position).magnitude;
            float newZoom = Mathf.Lerp(maxZoom, minZoom, distance / zoomLimiter);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, minZoom, Time.deltaTime);
        }
    }

    Vector3 GetCenterPoint(int activeTargets)
    {
        if (activeTargets == 2)
        {
            return (target1.position + target2.position) / 2f;
        }
        else if (target1 != null && target1.gameObject.activeInHierarchy)
        {
            return target1.position;
        }
        else if (target2 != null && target2.gameObject.activeInHierarchy)
        {
            return target2.position;
        }
        else
        {
            return transform.position; // fallback
        }
    }

    int GetActiveTargetCount()
    {
        int count = 0;
        if (target1 != null && target1.gameObject.activeInHierarchy) count++;
        if (target2 != null && target2.gameObject.activeInHierarchy) count++;
        return count;
    }
}
