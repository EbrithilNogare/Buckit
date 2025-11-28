using UnityEngine;
using UnityEngine.InputSystem;

public class BuckController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Called automatically from PlayerInput component (Unity Events mode)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
