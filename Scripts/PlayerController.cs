using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    public bool isFacingRight = true; // Biến để theo dõi hướng của nhân vật

    
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A)) {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D)) {
            moveInput = 1f;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }

        Flip();

        if (Mathf.Abs(moveInput) != 0f) {
            animator.SetBool("isRunning", true);
        } else {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("AA1");
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            animator.SetTrigger("S");
        }

        // Ham Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded) {
            animator.SetTrigger("Dash");
            Dash();
        }

    }

    void Flip()
    {
        // Quay nhân vật theo hướng di chuyển
        if (Input.GetKey(KeyCode.D) && !isFacingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // đảm bảo scale.x dương
            transform.localScale = scale;
            isFacingRight = true;
        }
        if (Input.GetKey(KeyCode.A) && isFacingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // đảm bảo scale.x âm
            transform.localScale = scale;
            isFacingRight = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                animator.SetBool("isJumping", false);
            }
        }
    }

    void Dash() {
        float dashSpeed = 10f; // bạn có thể chỉnh số này
        float dashDuration = 0.2f;

        Vector2 dashDirection = isFacingRight ? Vector2.right : Vector2.left;
        rb.linearVelocity = dashDirection * dashSpeed;
        Invoke(nameof(StopDash), dashDuration);
    }

    void StopDash()
    {
        rb.linearVelocity = Vector2.zero; // dừng lại sau khi dash
    }

}
