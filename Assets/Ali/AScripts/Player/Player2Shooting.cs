using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player2Shooting : MonoBehaviour
{
    [Header("Mermi Ayarları")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 5f;
    public int magazineSize = 24;  // This can be changed dynamically at runtime if needed
    public float reloadDuration = 2f;
    private int currentAmmo;
    private float lastFireTime;
    private bool isReloading = false;

    [Header("Fire Points (Yönlere Göre)")]
    public List<Transform> firePoints_Right;
    public List<Transform> firePoints_RightUp;
    public List<Transform> firePoints_RightDown;
    public List<Transform> firePoints_Left;
    public List<Transform> firePoints_LeftUp;
    public List<Transform> firePoints_LeftDown;

    [Header("VFX & SFX")]
    public GameObject muzzleFlashPrefab;
    public AudioClip shootSfx;
    public AudioSource audioSource;

    [Header("Üst Vücut Sprite")]
    [SerializeField] private SpriteRenderer upperBodyRenderer;
    [SerializeField] private Sprite spriteRight;
    [SerializeField] private Sprite spriteRightUp;
    [SerializeField] private Sprite spriteRightDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteLeftUp;
    [SerializeField] private Sprite spriteLeftDown;

    [Header("UI")]
    public Text ammoText;              // Ammo: 24/∞ gibi gözükecek
    public GameObject reloadImage;     // Reload sırasında aktif olan Image (başta gizli)

    private PlayerInput playerInput;

    void Start()
    {
        currentAmmo = magazineSize;
        playerInput = GetComponent<PlayerInput>();
        UpdateAmmoText();

        if (reloadImage != null)
            reloadImage.SetActive(false);  // Başta reload göstergesi görünmesin
    }

    void Update()
    {
        if (isReloading) return;

        Vector2 shootDirection = GetShootDirection();
        UpdateUpperBodySprite(shootDirection);

        if (playerInput.actions["Fire2"].ReadValue<float>() > 0.1f && Time.time >= lastFireTime + (1f / fireRate))
        {
            if (shootDirection != Vector2.zero && currentAmmo > 0)
            {
                StartCoroutine(ShootFromDirection(shootDirection));
                lastFireTime = Time.time;
            }
            else if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }

        if (playerInput.actions["Reload2"].triggered && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    Vector2 GetShootDirection()
    {
        Vector2 input = playerInput.actions["Aim2"].ReadValue<Vector2>();

        if (input.magnitude < 0.3f)
            return transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        input = input.normalized;

        if (input.x > 0.5f && input.y > 0.5f) return new Vector2(1, 1).normalized;
        if (input.x > 0.5f && input.y < -0.5f) return new Vector2(1, -1).normalized;
        if (input.x < -0.5f && input.y > 0.5f) return new Vector2(-1, 1).normalized;
        if (input.x < -0.5f && input.y < -0.5f) return new Vector2(-1, -1).normalized;
        if (input.x > 0.5f) return Vector2.right;
        if (input.x < -0.5f) return Vector2.left;

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
            UpdateAmmoText();
            yield return new WaitForSeconds(0.03f);
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

        return firePoints_Right;
    }

    void Shoot(Vector2 direction, Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = direction * bulletSpeed; // Corrected to use velocity instead of linearVelocity

        if (muzzleFlashPrefab != null)
            Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);

        if (shootSfx != null && audioSource != null)
            audioSource.PlayOneShot(shootSfx);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if (reloadImage != null)
            reloadImage.SetActive(true); // Reload UI image active

        yield return new WaitForSeconds(reloadDuration);

        currentAmmo = magazineSize;  // Reset ammo count after reload
        UpdateAmmoText();

        if (reloadImage != null)
            reloadImage.SetActive(false); // Reload complete, hide UI image

        isReloading = false;
    }

    void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + "/∞"; // Shows current ammo and infinite max
        }
    }

    void UpdateUpperBodySprite(Vector2 direction)
    {
        if (direction == new Vector2(1, 1).normalized) upperBodyRenderer.sprite = spriteRightUp;
        else if (direction == new Vector2(1, -1).normalized) upperBodyRenderer.sprite = spriteRightDown;
        else if (direction == new Vector2(-1, 1).normalized) upperBodyRenderer.sprite = spriteLeftUp;
        else if (direction == new Vector2(-1, -1).normalized) upperBodyRenderer.sprite = spriteLeftDown;
        else if (direction == Vector2.right) upperBodyRenderer.sprite = spriteRight;
        else if (direction == Vector2.left) upperBodyRenderer.sprite = spriteLeft;
    }
}
