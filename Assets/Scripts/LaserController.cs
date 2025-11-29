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

    private Vector2 _currentEnd;
    private Tween _moveTween;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialScale = transform.localScale;

        _initialLengthWorld = _spriteRenderer.bounds.size.x;
        if (_initialLengthWorld <= 0f) _initialLengthWorld = 1f;
    }

    private void Start()
    {
        if (SittingPosition) _currentEnd = SittingPosition.position;
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

    public void TweenBeamTo(Vector2 newTarget, float duration = 1f)
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
