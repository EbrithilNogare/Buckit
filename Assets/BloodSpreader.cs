using UnityEngine;
using DG.Tweening;

public class BloodSpreader : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1f, 1f, 1f);
    public float duration = 1f;

    void Start()
    {
        transform.DOScale(targetScale, duration);
    }
}
