using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Shooting : MonoBehaviour
{
    [Header("Mermi Ayarları")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    public int magazineSize = 24;
    public float reloadDuration = 2f;
    public bool autoReload = true;
    private int currentAmmo;
    private float lastFireTime;
    private bool isReloading = false;

    [Header("Fire Points (Açı Bazlı)")]
    public List<Transform> firePoints_Right;
    public List<Transform> firePoints_RightUp;
    public List<Transform> firePoints_RightDown;
    public List<Transform> firePoints_Left;
    public List<Transform> firePoints_LeftUp;
    public List<Transform> firePoints_LeftDown;

    [Header("Geri Tepme")]
    public bool enableRecoil = true;
    public float recoilForce = 5f;
    public Rigidbody2D playerRb;

    [Header("VFX & SFX")]
    public GameObject muzzleFlashPrefab;
    public AudioClip shootSfx;
    public AudioClip reloadSfx;
    public AudioSource audioSource;

    [Header("UI")]
    public Text ammoText;
    public GameObject reloadUI;

    [Header("Üst Vücut Sprite")]
    [SerializeField] private SpriteRenderer upperBodyRenderer;
    [SerializeField] private Sprite spriteRight;
    [SerializeField] private Sprite spriteRightUp;
    [SerializeField] private Sprite spriteRightDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteLeftUp;
    [SerializeField] private Sprite spriteLeftDown;

    void Start()
    {
        currentAmmo = magazineSize;
        UpdateAmmoUI();
        if (reloadUI != null) reloadUI.SetActive(false);
    }

    void Update()
    {
        if (isReloading) return;

        Vector2 shootDirection = GetShootDirection();
        UpdateUpperBodySprite(shootDirection);

        if (Input.GetKey(KeyCode.I) && Time.time >= lastFireTime + (1f / fireRate))
        {
            if (shootDirection != Vector2.zero && currentAmmo > 0)
            {
                StartCoroutine(ShootFromDirection(shootDirection));
                lastFireTime = Time.time;
            }
            else if (currentAmmo <= 0 && autoReload)
            {
                StartCoroutine(Reload());
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    Vector2 GetShootDirection()
    {
        bool up = Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.DownArrow);
        bool left = Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.RightArrow);

        if (right && up) return new Vector2(1, 1).normalized;
        if (right && down) return new Vector2(1, -1).normalized;
        if (left && up) return new Vector2(-1, 1).normalized;
        if (left && down) return new Vector2(-1, -1).normalized;
        if (right) return Vector2.right;
        if (left) return Vector2.left;

        return transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }

    IEnumerator ShootFromDirection(Vector2 direction)
    {
        List<Transform> selectedFirePoints = GetFirePointsByDirection(direction);

        foreach (Transform firePoint in selectedFirePoints)
        {
            if (currentAmmo <= 0) break;

            Shoot(direction, firePoint);
            currentAmmo--;
            UpdateAmmoUI();
            yield return new WaitForSeconds(0.03f); // aralıklı atış
        }

        if (currentAmmo <= 0 && autoReload)
        {
            StartCoroutine(Reload());
        }
    }

    List<Transform> GetFirePointsByDirection(Vector2 dir)
    {
        dir = dir.normalized;

        if (dir == new Vector2(1, 1).normalized) return firePoints_RightUp;
        if (dir == new Vector2(1, -1).normalized) return firePoints_RightDown;
        if (dir == new Vector2(-1, 1).normalized) return firePoints_LeftUp;
        if (dir == new Vector2(-1, -1).normalized) return firePoints_LeftDown;
        if (dir == Vector2.right) return firePoints_Right;
        if (dir == Vector2.left) return firePoints_Left;

        return firePoints_Right; // default fallback
    }

    void Shoot(Vector2 direction, Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = direction * bulletSpeed;

        if (muzzleFlashPrefab != null)
            Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);

        if (shootSfx != null && audioSource != null)
            audioSource.PlayOneShot(shootSfx);

        ApplyRecoil(direction);
    }

    void ApplyRecoil(Vector2 direction)
    {
        if (!enableRecoil || playerRb == null) return;

        Vector2 recoilDir = direction.x >= 0 ? Vector2.left : Vector2.right;
        playerRb.AddForce(recoilDir * recoilForce, ForceMode2D.Impulse);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if (reloadUI != null)
            reloadUI.SetActive(true);

        if (reloadSfx != null && audioSource != null)
            audioSource.PlayOneShot(reloadSfx);

        yield return new WaitForSeconds(reloadDuration);

        currentAmmo = magazineSize;
        UpdateAmmoUI();

        if (reloadUI != null)
            reloadUI.SetActive(false);

        isReloading = false;
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo + "/" + "∞";
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
    }
}
