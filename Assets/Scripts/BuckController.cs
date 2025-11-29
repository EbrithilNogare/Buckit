using UnityEngine;
using UnityEngine.InputSystem;

public class BuckController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float smoothTime = 0.08f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Vector2 moveInput, velRef;
    Vector2 minBound, maxBound;
    Vector2 extent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        var cam = Camera.main;
        var h = cam.orthographicSize;
        var w = h * cam.aspect;

        minBound = new Vector2(-w, -h);
        maxBound = new Vector2(w, h);

        extent = sr.bounds.extents; // world size of sprite / 2
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        animator.Play(moveInput == Vector2.zero ? "Idle" : "Walk");
        if (moveInput.x != 0) sr.flipX = moveInput.x > 0;
    }

    public void OnFight(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<Vector2>() != Vector2.zero)
            animator.Play("Box");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.SmoothDamp(
            rb.linearVelocity,
            moveInput * moveSpeed,
            ref velRef,
            smoothTime
        );

        var p = transform.position;
        p.x = Mathf.Clamp(p.x, minBound.x + extent.x, maxBound.x - extent.x);
        p.y = Mathf.Clamp(p.y, minBound.y + extent.y, maxBound.y - extent.y);
        transform.position = p;
    }
}
