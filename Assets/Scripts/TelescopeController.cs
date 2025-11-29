using UnityEngine;
using DG.Tweening;

public class TelescopeController : MonoBehaviour
{
    public Transform SittingPosition;
    
    public Transform View;
    public Transform Cone;

    
    private SpriteRenderer _coneRenderer;
    private Vector3 _coneInitialScale;
    private float _coneInitialWorldLength;
    private Vector2 _currentTarget;
    private Tween _moveTween;

    void Awake()
    {
        if (!Cone || !View || !SittingPosition)
        {
            Debug.LogWarning("TelescopeController: Missing SittingPosition / View / Cone reference.");
            return;
        }

        _coneRenderer = Cone.GetComponent<SpriteRenderer>();
        _coneInitialScale = Cone.localScale;

        if (_coneRenderer && _coneRenderer.sprite != null)
        {
            float spriteLocalLength = _coneRenderer.sprite.bounds.size.y;
            if (spriteLocalLength <= 0f) spriteLocalLength = 1f;
            _coneInitialWorldLength = spriteLocalLength * _coneInitialScale.y;
        }
        else
        {
            _coneInitialWorldLength = 1f;
        }

        var p = View ? View.position : SittingPosition.position;
        _currentTarget = new Vector2(p.x, p.y);
    }

    private Vector2 GetRandomLocationOnScreen()
    {
        Vector2 AreaFrom = new Vector2(-80, -40);
        Vector2 AreaTo = new Vector2(80, 10);

        return new Vector2(
            Random.Range(AreaFrom.x, AreaTo.x),
            Random.Range(AreaFrom.y, AreaTo.y)
        );
    }

    public void Test()
    {
        float MoveDuration = .5f;
        float WaitDuration = 2.0f;
        Vector2 next = GetRandomLocationOnScreen();

        TweenTo(next, MoveDuration)
            .OnComplete(() => DOVirtual.DelayedCall(WaitDuration, Test));
    }

    public void MoveTo(Vector2 target)
    {
        if (!SittingPosition || !View || !Cone) return;
        _moveTween?.Kill();
        _currentTarget = target;
        UpdateGeometry(target);
    }

    public Tween TweenTo(Vector2 target, float duration = 1f)
    {
        var vp = View.position;
        _currentTarget = new Vector2(vp.x, vp.y);

        _moveTween?.Kill();
        _moveTween = DOTween.To(
            () => _currentTarget,
            v => { _currentTarget = v; UpdateGeometry(v); },
            target,
            duration
        ).SetEase(Ease.Linear);

        return _moveTween;
    }

    void UpdateGeometry(Vector2 target)
    {
        Vector3 start = SittingPosition.position;
        Vector3 end = new Vector3(target.x, target.y, start.z);
        Vector3 dir = end - start;
        float distance = dir.magnitude;

        Cone.gameObject.SetActive(true);

        var rot = Quaternion.FromToRotation(Vector3.up, dir) * Quaternion.Euler(0f, 0f, -180f);

        View.position = end;
        View.rotation = rot;

        Cone.position = (start + end) * 0.5f;
        Cone.rotation = rot;

        float scaleFactor = distance / _coneInitialWorldLength;
        Cone.localScale = new Vector3(
            _coneInitialScale.x,
            _coneInitialScale.y * scaleFactor,
            _coneInitialScale.z
        );
    }
}
