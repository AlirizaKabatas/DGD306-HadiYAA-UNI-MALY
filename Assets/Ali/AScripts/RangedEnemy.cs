using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 7f;
    public float attackInterval = 2f;
    public float health = 100f;
    public float damage = 10f;
    public bool alwaysChase = false;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    private float attackTimer;
    private Transform targetPlayer;

    private Transform[] players;

    private void Start()
    {
        players = new Transform[2];
        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < foundPlayers.Length && i < 2; i++)
            players[i] = foundPlayers[i].transform;
    }

    private void Update()
    {
        if (players[0] == null && players[1] == null)
            return;

        // En yakın oyuncuyu bul
        targetPlayer = GetClosestPlayerInDetectionRange();

        if (targetPlayer != null)
        {
            RotateTowardsTarget();

            float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

            if (alwaysChase || distanceToPlayer <= detectionRange)
            {
                if (distanceToPlayer <= attackRange)
                {
                    attackTimer += Time.deltaTime;

                    if (attackTimer >= attackInterval)
                    {
                        Attack();
                        attackTimer = 0f;
                    }
                }
            }
        }
    }

    Transform GetClosestPlayerInDetectionRange()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var player in players)
        {
            if (player == null) continue;

            float dist = Vector2.Distance(transform.position, player.position);
            if (dist <= detectionRange && dist < minDistance)
            {
                minDistance = dist;
                closest = player;
            }
        }

        return closest;
    }

    void RotateTowardsTarget()
    {
        if (targetPlayer == null) return;

        Vector3 dir = targetPlayer.position - transform.position;
        if (dir.x < 0)
            transform.localScale = new Vector3(1, 1, 1); // sola bak
        else
            transform.localScale = new Vector3(-1, 1, 1); // sağa bak
    }

    void Attack()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Vector2 direction = (targetPlayer.position - bulletSpawnPoint.position).normalized;
            bullet.GetComponent<Bullet>().SetDirection(direction);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
