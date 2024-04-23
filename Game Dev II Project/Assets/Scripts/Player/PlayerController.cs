using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int numberOfJumps = 1;
    private int jumpCounter = 1;
    public float dashForce = 20f;
    public float dashDuration = 0.1f; // Duration of the dash in seconds
    public float dashCooldown = 1f; // Cooldown period for dash in seconds
    public float downDashForce;
    public float normalGravity = 1f;
    public float fallGravity = 2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public float smoothingFactor = 5f;

    public GameObject doubleJumpItem;
    private bool doubleJumpAcquired = false;

    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private bool isGrounded;
    private bool canDash = true;
    private bool dashing = false;
    private bool canDownDash = true;
    private float smoothInput;

    private float horizontalInput;

    public VectorValue startingPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = normalGravity; // Set initial gravity scale
    }

    void Start()
    {
        transform.position = startingPosition.initialValue;
    }

    private void Update()
    {
        // Smooth input handling
        float targetInput = Input.GetAxisRaw("Horizontal");
        smoothInput = Mathf.MoveTowards(smoothInput, targetInput, Time.deltaTime * smoothingFactor);

        // Player movement input
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Flip the player's scale based on horizontal input
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Flip to left
            animator.SetBool("IsRunning", true);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip to right
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        // Jumping input
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryJump();
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryDash();
        }

        // Down dash input
        if (Input.GetKeyDown(KeyCode.S))
        {
            TryDownDash();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(smoothInput * moveSpeed, rb.velocity.y);

        // Player movement
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Adjust gravity when falling
        if (!isGrounded && rb.velocity.y < 0 && !dashing)
        {
            rb.gravityScale = fallGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }

    void TryJump()
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (doubleJumpAcquired && jumpCounter < numberOfJumps)
        {
            Debug.Log("Double Jump Acquired");
            Jump();
        }
    }

    void TryDash()
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void TryDownDash()
    {
        if (canDownDash)
        {
            DownDash();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        isGrounded = false;

        if (jumpCounter < numberOfJumps && doubleJumpAcquired)
        {
            jumpCounter++;
        }
        else
        {
            jumpCounter = 1;
        }
    }

    private IEnumerator Dash()
    {
        rb.velocity = Vector2.zero;
        canDash = false;
        dashing = true;
        float originalSpeed = moveSpeed;
        rb.gravityScale = 0f;
        moveSpeed += dashForce;
        yield return new WaitForSeconds(dashDuration);
        dashing = false;
        moveSpeed = originalSpeed;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void DownDash()
    {
        rb.velocity = new Vector2(rb.velocity.x, downDashForce);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == doubleJumpItem)
        {
            numberOfJumps = 2;
            doubleJumpAcquired = true;
        }
    }
}
