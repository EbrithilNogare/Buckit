using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class LaserController : MonoBehaviour
{
    public TelescopeController telescopeController;
    public Transform SittingPosition;
    public Transform Buck;
    public MinigameController minigameController;
    public CasingEjector casingEjector;

    private const float aimingTimeBeforeHit = 3f;

    private SpriteRenderer _spriteRenderer;
    private Vector3 _initialScale;
    private float _initialLengthWorld;

    private Vector2 _currentEnd;
    private Tween _moveTween;
    private bool isAiming;
    private float hitRadius = 1.0f;
    private float aimingDuration = 0;
    private DeerController selectedTarget;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialScale = transform.localScale;

        _initialLengthWorld = _spriteRenderer.bounds.size.x;
        if (_initialLengthWorld <= 0f) _initialLengthWorld = 1f;
    }

    private void Start()
    {
        isAiming = false;
        if (SittingPosition)
            _currentEnd = SittingPosition.position;
    }
    void Update()
    {
        if (!isAiming) return;

        aimingDuration += Time.deltaTime;

        if (aimingDuration > aimingTimeBeforeHit - 3f)
        {
            AudioController.Instance.PlayAim();
        }

        if (IsBuckOnBeam())
        {
            EndLaser();
            // ### give control to minigame
            minigameController.MinigameStart(selectedTarget);
        }
        else if (aimingDuration > aimingTimeBeforeHit)
        {
            // end self
            EndLaser();

            //kill deer
            AudioController.Instance.PlayGunshot();
            selectedTarget.Die();

            // play sound
            // todo pridat zvuk vystrelu

            // play animation
            bool canContinue = casingEjector.UseShellAndContinue();


            if (canContinue)
            {
                // ### give control to telescope
                telescopeController.StartLooking();
            }
            else
            {
                // ### give control to ejector
                casingEjector.StartReloading(telescopeController.StartLooking);
            }
        }
    }

    bool IsBuckOnBeam()
    {
        Vector2 start = SittingPosition.position;
        Vector2 end = _currentEnd;
        Vector2 buckPos = Buck.position;

        Vector2 seg = end - start;
        float segLenSq = seg.sqrMagnitude;
        if (segLenSq < 0.0001f) return false;

        float t = Vector2.Dot(buckPos - start, seg) / segLenSq;
        t = Mathf.Clamp01(t);

        Vector2 closest = start + seg * t;
        float dist = Vector2.Distance(buckPos, closest);

        return dist <= hitRadius;
    }

    public void TriggerLaser(DeerController _selectedTarget)
    {
        selectedTarget = _selectedTarget;
        gameObject.SetActive(true);
        MoveBeamTo(SittingPosition.position);
        TweenBeamTo(selectedTarget.transform.position);
        isAiming = true;
        aimingDuration = 0;
    }

    private void EndLaser()
    {
        isAiming = false;
        gameObject.SetActive(false);
        AudioController.Instance.StopAim();
    }

    public void Test()
    {
        TweenBeamTo(new Vector2(30,-10), .1f);
    }

    public void MoveBeamTo(Vector2 target)
    {
        if (!SittingPosition) return;

        UpdateBeam(target);
        _currentEnd = target;
    }

    public void TweenBeamTo(Vector2 newTarget, float duration = .1f)
    {
        if (!SittingPosition) return;

        _moveTween?.Kill();

        _moveTween = DOTween.To(
                () => _currentEnd,
                v => { _currentEnd = v; UpdateBeam(v); },
                newTarget,
                duration
            )
            .SetEase(Ease.Linear);
    }

    private void UpdateBeam(Vector2 target)
    {
        Vector3 start = SittingPosition.position;
        Vector3 end = new Vector3(target.x, target.y, start.z);

        Vector3 dir = end - start;
        float distance = dir.magnitude;

        if (distance < 0.0001f)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        transform.position = start + dir * 0.5f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float scaleFactor = distance / _initialLengthWorld;
        transform.localScale = new Vector3(
            _initialScale.x * scaleFactor,
            _initialScale.y,
            _initialScale.z
        );
    }
}
