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
    public int maxJumps = 2;
    private int intmaxJumps ;     //uso interno, para funcionamiento de cambio de cambio de estado
    int jumpsRemaining;

    [Header("GrounbdCheck")]
    public Transform groundCheckPos;
    public Vector3 groundCheckSize = new Vector3(0.5f, 0.05f, 0.5f);
    public LayerMask groundLayer;

    [Header("Dash")]
    public float dashSpeed = 10f;
    public float dashDuration =0.05f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;

    [Header("CoreMech")]
    public GameObject[] objetoscambiar;
    bool isBall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        intmaxJumps = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        }
        GroundCheck();
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;

        if(horizontalMovement == 1)
        {
            isFacingRight = true;
        }else if(horizontalMovement == -1){
            isFacingRight = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
                jumpsRemaining--;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
            }
        }
    }

    private void GroundCheck()
    {
        if (Physics.CheckBox(
            groundCheckPos.position,
            groundCheckSize,
            Quaternion.identity,
            groundLayer))
        {
            jumpsRemaining = intmaxJumps;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
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

    public void Cambio(InputAction.CallbackContext context)
    {
        foreach (GameObject obj in objetoscambiar)
        {
            if (obj != null)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
        if (isBall)
        {
            isBall = false;
            canDash = true;
            intmaxJumps = 1;
        }
        else
        {
            isBall = true;
            canDash = false;
            intmaxJumps = maxJumps;
        }
    }

}