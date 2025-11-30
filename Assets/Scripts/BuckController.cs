using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuckController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float smoothTime = 0.08f;

    public Sprite Fight0;
    public Sprite Fight1;
    private int fight = 0;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Vector2 moveInput, prevInput, velRef;
    Vector2 minBound, maxBound;

    Coroutine activeStepCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        var cam = Camera.main;
        var h = cam.orthographicSize;
        var w = h * cam.aspect;

        var extent = sr.bounds.extents;

        float heighOfSky = 6.6f;

        minBound = new Vector2(-w + extent.x, -h + extent.y);
        maxBound = new Vector2(w - extent.x, h - extent.y - heighOfSky);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        prevInput = moveInput;
        moveInput = ctx.ReadValue<Vector2>();
        animator.Play(moveInput == Vector2.zero ? "Idle" : "Walk");
        if (moveInput.x != 0) sr.flipX = moveInput.x > 0;

        if (prevInput.magnitude == 0 && moveInput.magnitude != 0)
        {
            activeStepCoroutine = StartCoroutine(StepCoroutine());
        }
        else if (prevInput.magnitude != 0 && moveInput.magnitude == 0)
        {
            StopCoroutine(activeStepCoroutine);
        }
    }

    public void OnFight(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<Vector2>() != Vector2.zero)
            animator.Play("Box");
    }

    public void FightSwap(){
        switch (fight)
        {
            case 0:
                sr.sprite = Fight0;
                fight = 1;
                break;
            case 1:
                sr.sprite = Fight1;
                fight = 0;
                break;
            default:
                sr.sprite = Fight0; 
                break;
        }
        
    }

    public void FightStart()
    {
        animator.enabled = false;
        gameObject.GetComponent<PlayerInput>().enabled = false;
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
        p.x = Mathf.Clamp(p.x, minBound.x, maxBound.x);
        p.y = Mathf.Clamp(p.y, minBound.y, maxBound.y);
        transform.position = p;
    }

    private IEnumerator StepCoroutine()
    {
        while(true)
        {
            AudioController.Instance.PlayHardStep();

            yield return new WaitForSeconds(0.1f);

            AudioController.Instance.PlayHardStep();

            yield return new WaitForSeconds(0.3f);
        }
    }
}
