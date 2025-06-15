using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();

    public Vector3 offset;
    public float smoothTime = 0.5f;

    public float maxZoom = 10f;   // Daha yakýn
    public float minZoom = 5f;    // Daha uzak
    public float zoomLimiter = 20f;

    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        CleanNullTargets();

        if (targets == null || targets.Count == 0)
            return;

        Move();
        Zoom();
    }

    void CleanNullTargets()
    {
        // targets listesindeki null veya aktif olmayan nesneleri temizler
        targets.RemoveAll(t => t == null || !t.gameObject.activeInHierarchy);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;

        // Y eksenini sabit tutmak için burasý önemli
        newPosition.y = transform.position.y;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float distance = GetGreatestDistanceX(); // Sadece yatay mesafeye göre zoom
        float targetZoom = Mathf.Lerp(maxZoom, minZoom, distance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 3f); // daha hýzlý yumuþama
    }

    float GetGreatestDistanceX()
    {
        if (targets.Count == 1) return 0f;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (Transform target in targets)
        {
            bounds.Encapsulate(target.position);
        }

        return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
            return targets[0].position;

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (Transform target in targets)
        {
            bounds.Encapsulate(target.position);
        }

        return bounds.center;
    }

    // Dýþarýdan hedefler atanmasýný saðlar
    public void SetTargets(List<Transform> newTargets)
    {
        targets = newTargets;
    }
}




