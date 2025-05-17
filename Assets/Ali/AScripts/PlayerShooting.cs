using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Mermi Ayarları")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    private float lastFireTime;

    [Header("Recoil")]
    public bool enableRecoil = true;
    public float recoilForce = 5f;
    public Rigidbody2D playerRb;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= lastFireTime + (1f / fireRate))
        {
            Vector2 shootDirection = GetShootDirection();

            if (shootDirection != Vector2.zero)
            {
                Shoot(shootDirection);
                lastFireTime = Time.time;
            }
        }
    }

    Vector2 GetShootDirection()
    {
        // WASD input kontrolü
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        // 6 yön belirleme (yukarı ve aşağı yok)
        if (right && up) return new Vector2(1, 1).normalized;     // Sağ üst
        if (right && down) return new Vector2(1, -1).normalized;  // Sağ alt
        if (left && up) return new Vector2(-1, 1).normalized;     // Sol üst
        if (left && down) return new Vector2(-1, -1).normalized;  // Sol alt
        if (right) return Vector2.right;                          // Sağ
        if (left) return Vector2.left;                            // Sol

        // WASD basılmıyorsa karakter yönüne göre ateş et
        if (transform.localScale.x > 0) return Vector2.right;
        else return Vector2.left;
    }

    void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
            bulletRb.linearVelocity = direction * bulletSpeed;

        // Merminin yönünü doğru şekilde ayarla
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }

        ApplyRecoil(direction);
    }

    void ApplyRecoil(Vector2 direction)
    {
        if (!enableRecoil || playerRb == null) return;

        // Sadece sola veya sağa recoil uygula
        Vector2 recoilDir = Vector2.zero;

        if (direction.x > 0)
            recoilDir = Vector2.left;
        else if (direction.x < 0)
            recoilDir = Vector2.right;

        if (recoilDir != Vector2.zero)
            playerRb.AddForce(recoilDir * recoilForce, ForceMode2D.Impulse);
    }
}
