using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [Header("Mermi Ayarları")]
    public GameObject bulletPrefab;
    public List<Transform> firePoints = new List<Transform>();
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    public int magazineSize = 24;
    public float reloadDuration = 2f;
    public bool autoReload = true;

    private float lastFireTime;
    private int currentAmmo;
    private bool isReloading = false;

    [Header("Geri Tepme")]
    public bool enableRecoil = true;
    public float recoilForce = 5f;
    public Rigidbody2D playerRb;

    [Header("VFX ve SFX")]
    public GameObject muzzleFlashPrefab;
    public AudioClip shootSfx;
    public AudioClip reloadSfx;
    public AudioSource audioSource;

    [Header("UI")]
    public Text ammoText;           // Örn: 24/∞
    public GameObject reloadUI;     // Reload görseli

    [Header("Üst Vücut Spriteları")]
    [SerializeField] private SpriteRenderer upperBodyRenderer;

    [SerializeField] private Sprite spriteRight;
    [SerializeField] private Sprite spriteRightUp;
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteLeftUp;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteLeftDown;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteRightDown;

    void Start()
    {
        currentAmmo = magazineSize;
        UpdateAmmoUI();
        if (reloadUI != null)
            reloadUI.SetActive(false);
    }

    void Update()
    {
        if (isReloading) return;

        Vector2 shootDirection = GetShootDirection();
        UpdateUpperBodySprite(shootDirection);

        if (Input.GetMouseButton(0) && Time.time >= lastFireTime + (1f / fireRate))
        {
            if (shootDirection != Vector2.zero && currentAmmo > 0)
            {
                StartCoroutine(ShootSequence(shootDirection));
                lastFireTime = Time.time;
            }
            else if (currentAmmo <= 0 && autoReload)
            {
                StartCoroutine(Reload());
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
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
        if (up) return Vector2.up;
        if (down) return Vector2.down;

        return transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }

    IEnumerator ShootSequence(Vector2 direction)
    {
        foreach (Transform firePoint in firePoints)
        {
            if (currentAmmo <= 0) break;

            Shoot(direction, firePoint);
            currentAmmo--;
            UpdateAmmoUI();
            yield return new WaitForSeconds(0.03f); // aradaki gecikme
        }

        if (currentAmmo <= 0 && autoReload)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot(Vector2 direction, Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
            bulletRb.linearVelocity = direction * bulletSpeed;

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
        else if (direction == Vector2.up) upperBodyRenderer.sprite = spriteUp;
        else if (direction == Vector2.down) upperBodyRenderer.sprite = spriteDown;
    }
}
