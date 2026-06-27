using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

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
    private int intmaxJumps = 1;     //uso interno, para funcionamiento de cambio de cambio de estado, strats with one for being a ball
    int jumpsRemaining = 1;

    [Header("GrounbdCheck")]
    public Transform groundCheckPos;
    public Vector3 groundCheckSize = new Vector3(0.5f, 0.05f, 0.5f);
    public LayerMask groundLayer;

    [Header("Positions")]
    public Collider Coll;
    public Transform Sprt;
    public Vector3[] sprtPos = new Vector3[9];
    public Vector3[] collPos = new Vector3[9];

    [Header("Dash")]
    public float dashSpeed = 10f;
    public float dashDuration =0.05f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;

    [Header("CoreMech")]
    public GameObject[] objetoscambiar;
    bool isBall = true;

    [Header("Animations")]
    public SpriteRenderer sr;
    public Animator animator;
    private float xPosLastFrame;

    void Start()
    {
        intmaxJumps = maxJumps;
        Sprt.localPosition = sprtPos[0];
    }

    void Update()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        }
        GroundCheck();
        FlipcharacterX();
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;

        if(horizontalMovement == 1)
        {
            isFacingRight = true;
            animator.SetBool("isRoll", true);
            animator.SetBool("isWalk", true);
        }
        else if(horizontalMovement == -1){
            isFacingRight = false;
            animator.SetBool("isRoll", true);
            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isRoll", false);
            animator.SetBool("isWalk", false);
        }
        if (horizontalMovement != 0)
        {
            if (isBall)
            {
                chPositions(2);
            }
            else
            {
                chPositions(3);
            }
        }
    }

    private void FlipcharacterX()
    {
        if(transform.position.x > xPosLastFrame)
        {
            sr.flipX = false;
        }else if (transform.position.x < xPosLastFrame)
        {
            sr.flipX = true;
        }
        xPosLastFrame = transform.position.x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
                jumpsRemaining--;
                animator.SetTrigger("isJump");
                chPositions(5);
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
            animator.SetTrigger("isDash");
            chPositions(4);
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
            canDash = false;
            intmaxJumps = maxJumps;
            animator.SetBool("isBall", false);
            chPositions(1);
        }
        else
        {
            isBall = true;
            canDash = true;
            intmaxJumps = 1;
            animator.SetBool("isBall", true);
            chPositions(0);
        }
        animator.SetTrigger("isTrans");
    }

    private async void chPositions(int stt)
    {
        if (stt == 4 || stt == 5 || stt == 7 || stt == 8)
        {
            Sprt.localPosition = sprtPos[stt];
            await Task.Delay(600);
        }
        if(stt == 0 || stt == 1)
        {
            Sprt.localPosition = sprtPos[6];
            await Task.Delay(600);
            Sprt.localPosition = sprtPos[stt];
        }
        if(stt == 2 || stt == 3)
        {
            Sprt.localPosition = sprtPos[stt];
        }
    }
}