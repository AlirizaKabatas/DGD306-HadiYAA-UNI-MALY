using UnityEngine;
using System.Collections;

public class DottopusMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveDuration = 2f;
    public float waitDuration = 1f;

    [Header("Movement Bounds")]
    public Collider2D movementBounds;

    private Vector2 moveDirection;

    void Start()
    {
        StartCoroutine(MovementRoutine());
    }

    IEnumerator MovementRoutine()
    {
        while (true)
        {
            moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            float moveTime = 0f;

            while (moveTime < moveDuration)
            {
                Vector2 newPos = (Vector2)transform.position + moveDirection * moveSpeed * Time.deltaTime;

                if (movementBounds.bounds.Contains(newPos))
                {
                    transform.position = newPos;
                }
                else
                {
                    // Eğer dışarı çıkarsa yönü tersine çevir
                    moveDirection = -moveDirection;
                }

                moveTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(waitDuration);
        }
    }
}
