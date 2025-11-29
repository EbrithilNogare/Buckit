using UnityEngine;
using UnityEngine.InputSystem;

public class BuckController : MonoBehaviour
{
    public float moveSpeed = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 moveInput;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Called automatically from PlayerInput component (Unity Events mode)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
        }
        if (moveInput.x > 0)
        {
            sr.flipX = true;
        }
        else
        {
            if(moveInput.x < 0)
                sr.flipX = false;
        }
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
