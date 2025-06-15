using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem;
using System;


public class Player1Input : MonoBehaviour
{
    private Vector2 moveInput;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump()
    {
        Debug.Log("Player1 Jump!");
    }

    private void OnShoot()
    {
        Debug.Log("Player1 Shoot!");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
}
