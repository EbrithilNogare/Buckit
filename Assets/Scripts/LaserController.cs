using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class LaserController : MonoBehaviour
{
    [Header("Fixed start of the beam")]
    public Transform SittingPosition;

    private SpriteRenderer _spriteRenderer;
    private Vector3 _initialScale;
    private float _initialLengthWorld;

    private Vector2 _currentEnd;     // used for tweening
    private Tween _moveTween;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialScale = transform.localScale;

        _initialLengthWorld = _spriteRenderer.bounds.size.x;
        if (_initialLengthWorld <= 0f)
            _initialLengthWorld = 1f;
    }

    private void Start()
    {
        // Initialize to start position on first frame
        if (SittingPosition != null)
            _currentEnd = SittingPosition.position;
    }

    /// <summary>
    /// Instantly moves & stretches the beam so it connects SittingPosition to target.
    /// </summary>
    public void MoveBeamTo(Vector2 target)
    {
        if (SittingPosition == null)
            return;

        UpdateBeam(target);
        _currentEnd = target;
    }

    /// <summary>
    /// Smoothly animates the beam so that its target moves from the old endpoint to the new one.
    /// </summary>
    public void TweenBeamTo(Vector2 newTarget, float duration = 1f)
    {
        if (SittingPosition == null)
            return;

        // Kill previous tween if active
        _moveTween?.Kill();

        // Animate _currentEnd from current value to newTarget
        _moveTween = DOTween.To(
                () => _currentEnd,
                v => { _currentEnd = v; UpdateBeam(v); },
                newTarget,
                duration
            )
            .SetEase(Ease.Linear);
    }

    /// <summary>
    /// Applies geometry: center, rotation, scale.
    /// </summary>
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

        // center
        transform.position = start + dir * 0.5f;

        // rotation
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // stretch
        float scaleFactor = distance / _initialLengthWorld;
        transform.localScale = new Vector3(
            _initialScale.x * scaleFactor,
            _initialScale.y,
            _initialScale.z
        );
    }
}
