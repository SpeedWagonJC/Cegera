using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    public Rigidbody rb;
    bool isFacingRight = true;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Jump")]
    public float fuerzaSalto = 6f;

    [Header("Dash")]
    public float dashSpeed = 10f;
    public float dashDuration =0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        float dashDirection = isFacingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y); //Dash movement

        yield return new WaitForSeconds(dashDuration * 10);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y); //Reset Horizontal Velocity

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}