using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class TelescopeController : MonoBehaviour
{
    public Transform SittingPosition;
    public DeerManager deerManager;
    public Transform Buck;
    public LaserController laserController;

    public Transform View;
    public Transform Cone;

    private const float tooCloseToView = 15f;

    private SpriteRenderer _coneRenderer;
    private Vector3 _coneInitialScale;
    private float _coneInitialWorldLength;
    private Vector2 _currentTarget;
    private Tween _moveTween;
    private float MoveDuration = .5f;
    private float WaitDuration = 1.0f;

    private List<DeerController> deers;
    private DeerController selectedTarget;
    private bool avoidingDeer;
    private float _fixedZ;

    void Awake()
    {
        avoidingDeer = true;

        if (!Cone || !View || !SittingPosition)
        {
            Debug.LogWarning("TelescopeController: Missing SittingPosition / View / Cone reference.");
            return;
        }

        _fixedZ = View.position.z;

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

    private void Start()
    {
        deers = deerManager.GetDeers();

        Automation();
    }

    private void Update()
    {
        if (avoidingDeer)
        {
            if (Vector2.Distance(View.position, Buck.position) < tooCloseToView)
            {
                transform.DOKill();
                Vector2 moveTo = GetAwayPositionFromBuck(30f);
                TweenTo(moveTo, .2f)
                    .OnComplete(() => DOVirtual.DelayedCall(0.2f, () =>
                    {
                        lookupsLeft++;
                        Automation();
                    }));
            }
        }
    }

    private Vector2 GetAwayPositionFromBuck(float distance)
    {
        if (!Buck || !View) return Vector2.zero;

        Vector2 buckPos = Buck.position;
        Vector2 viewPos = View.position;

        Vector2 dir = viewPos - buckPos;      // direction from buck to current view
        if (dir.sqrMagnitude < 0.001f)
            dir = Random.insideUnitCircle;    // fallback if on top

        dir.Normalize();
        return buckPos + dir * distance;      // point "distance" units away from buck
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

    private Vector2 GeSafeLocationOnScreen()
    {
        Vector2 proposedCoordination = Vector2.zero;
        do
        {
            proposedCoordination = GetRandomLocationOnScreen();
        } while (!IsLocationSafe(proposedCoordination));
        return proposedCoordination;
    }

    private bool IsLocationSafe(Vector2 pos)
    {
        if (Vector2.Distance(new Vector2(Buck.position.x, Buck.position.y), pos) < 3.0f)
            return false;

        foreach (DeerController t in deers)
            if (Vector2.Distance(new Vector2(t.transform.position.x, t.transform.position.y), pos) < 2.0f)
                return false;

        return true;
    }

    private int lookupsLeft = 4;
    public void Automation()
    {
        deers = deerManager.GetDeers();

        if (lookupsLeft <= 0)
        {
            if (deers.Count <= 0)
            {
                Debug.LogError("game should have ended already");
                return;
            }

            lookupsLeft = 4;
            avoidingDeer = false;
            selectedTarget = deers[Random.Range(0, deers.Count)];
            TweenTo(new Vector2(selectedTarget.transform.position.x, selectedTarget.transform.position.y), MoveDuration)
                .OnComplete(() => DOVirtual.DelayedCall(1.0f, () => TriggerLaser(selectedTarget)));

        }
        else
        {
            lookupsLeft--;
            avoidingDeer = true;
            Vector2 next = GeSafeLocationOnScreen();
            TweenTo(next, MoveDuration)
                .OnComplete(() => DOVirtual.DelayedCall(WaitDuration, Automation));
        }
    }

    private void TriggerLaser(DeerController selectedTarget)
    {
        Cone.gameObject.SetActive(false);
        View.gameObject.SetActive(false);
        laserController.TriggerLaser(selectedTarget);
    }

    public void LaserEnded()
    {
        Cone.gameObject.SetActive(true);
        View.gameObject.SetActive(true);
        Automation();
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
        Vector3 start = new Vector3(SittingPosition.position.x, SittingPosition.position.y, _fixedZ);
        Vector3 end = new Vector3(target.x, target.y, _fixedZ);
        Vector3 dir = end - start;
        float distance = dir.magnitude;

        Cone.gameObject.SetActive(true);

        var rot = Quaternion.FromToRotation(Vector3.up, dir) * Quaternion.Euler(0f, 0f, -180f);

        View.position = end;
        View.rotation = rot;

        Cone.position = new Vector3(
            (start.x + end.x) * 0.5f,
            (start.y + end.y) * 0.5f,
            _fixedZ
        );
        Cone.rotation = rot;

        float scaleFactor = distance / _coneInitialWorldLength;
        Cone.localScale = new Vector3(
            _coneInitialScale.x,
            _coneInitialScale.y * scaleFactor,
            _coneInitialScale.z
        );
    }
}
