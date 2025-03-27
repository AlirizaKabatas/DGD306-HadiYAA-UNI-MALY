using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public float shootSpeed = 10f;  
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public LayerMask targetLayer; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
     
        Vector2 shootDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootPoint.position).normalized;

       
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = shootDirection * shootSpeed;

       
        bullet.GetComponent<Bullet>().targetLayer = targetLayer;
    }
}
