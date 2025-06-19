
using UnityEngine;
public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor = 0.5f; // Manzaranın ne kadar yavaş hareket edeceğini kontrol eder.
    private Vector3 lastCameraPosition;

    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;
        float deltaY = cameraTransform.position.y - lastCameraPosition.y;

        // Manzara hareketi
        transform.position = new Vector3(transform.position.x + deltaX * parallaxFactor,
                                          transform.position.y + deltaY * parallaxFactor,
                                          transform.position.z);

        lastCameraPosition = cameraTransform.position;
    }
}
