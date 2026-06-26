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

    [Header("GrounbdCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;

    [Header("Dash")]
    public float dashSpeed = 10f;
    public float dashDuration =0.05f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;

    [Header("CoreMech")]
    public GameObject[] objetoscambiar;
    bool isNormal;

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

        if(horizontalMovement == 1)
        {
            isFacingRight = true;
        }else if(horizontalMovement == -1){
            isFacingRight = false;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }

    private bool isGrounded()
    {
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
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
        isNormal = isNormal ? false : true;
        Debug.Log("Hola");
    }

    /*void OnInteract()         //unsure how to implement send messages to the rest of methods
    {
        foreach (GameObject obj in objetoscambiar)
        {
            if (obj != null)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
        isNormal = isNormal ? false : true;
        Debug.Log("Hola");
    }*/
}