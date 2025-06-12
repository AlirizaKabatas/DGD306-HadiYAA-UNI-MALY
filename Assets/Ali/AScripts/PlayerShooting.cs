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

    [Header("Üst Vücut")]
    [SerializeField] private SpriteRenderer upperBodyRenderer;

    [SerializeField] private Sprite spriteRight;
    [SerializeField] private Sprite spriteRightUp;
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteLeftUp;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteLeftDown;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteRightDown;

    void Update()
    {
        Vector2 shootDirection = GetShootDirection();
        UpdateUpperBodySprite(shootDirection);

        if (Input.GetMouseButton(0) && Time.time >= lastFireTime + (1f / fireRate))
        {
            if (shootDirection != Vector2.zero)
            {
                Shoot(shootDirection);
                lastFireTime = Time.time;
            }
        }
    }

    Vector2 GetShootDirection()
    {
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        if (right && up) return new Vector2(1, 1).normalized;
        if (right && down) return new Vector2(1, -1).normalized;
        if (left && up) return new Vector2(-1, 1).normalized;
        if (left && down) return new Vector2(-1, -1).normalized;
        if (right) return Vector2.right;
        if (left) return Vector2.left;

        if (transform.localScale.x > 0) return Vector2.right;
        else return Vector2.left;
    }

    void Shoot(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
            bulletRb.linearVelocity = direction * bulletSpeed;

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

        Vector2 recoilDir = Vector2.zero;

        if (direction.x > 0)
            recoilDir = Vector2.left;
        else if (direction.x < 0)
            recoilDir = Vector2.right;

        if (recoilDir != Vector2.zero)
            playerRb.AddForce(recoilDir * recoilForce, ForceMode2D.Impulse);
    }

    void UpdateUpperBodySprite(Vector2 direction)
    {
        direction = direction.normalized;

        if (direction == new Vector2(1, 1).normalized) upperBodyRenderer.sprite = spriteRightUp;
        else if (direction == new Vector2(1, -1).normalized) upperBodyRenderer.sprite = spriteRightDown;
        else if (direction == new Vector2(-1, 1).normalized) upperBodyRenderer.sprite = spriteLeftUp;
        else if (direction == new Vector2(-1, -1).normalized) upperBodyRenderer.sprite = spriteLeftDown;
        else if (direction == Vector2.right) upperBodyRenderer.sprite = spriteRight;
        else if (direction == Vector2.left) upperBodyRenderer.sprite = spriteLeft;
        else if (direction == Vector2.up) upperBodyRenderer.sprite = spriteUp;
        else if (direction == Vector2.down) upperBodyRenderer.sprite = spriteDown;
    }
}
