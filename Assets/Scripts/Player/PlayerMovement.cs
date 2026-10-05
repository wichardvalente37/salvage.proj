using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float dashForce = 10f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.2f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool dashRequested;
    private bool stopDashRequested;
    private float dashTimer;
    private float dashCooldownTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            moveInput.y += 1;

        if (Keyboard.current.sKey.isPressed)
            moveInput.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            moveInput.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            moveInput.x += 1;

        moveInput = moveInput.normalized;

        Vector2 dir = moveInput;
        Vector2 Playerdir = dir - new Vector2(transform.position.x, transform.position.y);

        transform.up = new Vector2(Playerdir.x, Playerdir.y);

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }


        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
            {
                isDashing = false;
                stopDashRequested = true;
            }

            return;
        }


        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            moveInput != Vector2.zero &&
            dashCooldownTimer <= 0)
        {
            dashDirection = moveInput;
            dashRequested = true;
        }
    }

    private void FixedUpdate()
    {
        if (stopDashRequested)
        {
            rb.linearVelocity = Vector2.zero;
            stopDashRequested = false;
        }


        if (dashRequested)
        {
            rb.linearVelocity = Vector2.zero;

            rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse
            );
           
        
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            dashRequested = false;
        }
    }
}