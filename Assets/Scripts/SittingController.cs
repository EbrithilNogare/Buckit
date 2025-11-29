using System;
using UnityEngine;

public class SittingController : MonoBehaviour
{
    [Header("---CONST---")] [SerializeField]
    public int HitGained;
    public Sprite SittingSprite0;
    public Sprite SittingSprite1;
    public Sprite SittingSprite2;
    public Sprite SittingSprite3;

    [Header("---DEBUG---")] [SerializeField]
    private bool Hit; 

    public Action OnSittingGainedDamage;
    
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        HitGained = 0;
        OnSittingGainedDamage += SittingGainedDamage;
    }

    private void Update()
    {
        #region TEST

        if (Hit)
        {
            OnSittingGainedDamage?.Invoke();
            Hit = false;
        }
        #endregion
    }

    private void SittingGainedDamage()
    {
        HitGained++;
        switch (HitGained)
        {
            case 0: sr.sprite = SittingSprite0; break;
            case 1: sr.sprite = SittingSprite1; break;
            case 2: sr.sprite = SittingSprite2; break;
            case 3: sr.sprite = SittingSprite3; break;
            default: sr.sprite = SittingSprite0; break;
        }
    }
}
