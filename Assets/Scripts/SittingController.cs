using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SittingController : MonoBehaviour
{
    public GameObject Target;
    public BuckController buckController;
    public CasingEjector casingEjector;
    public BoxController boxController;
    public ParticleSystem DustParticles;

    [FormerlySerializedAs("HitGained")] [Header("---CONST---")] [SerializeField]
    public int MaxHealth;
    public Sprite SittingSprite0;
    public Sprite SittingSprite1;
    public Sprite SittingSprite2;
    public Sprite SittingSprite3;

    [Header("---DEBUG---")] [SerializeField]
    private bool Hit; 

    public Action OnSittingGainedDamage;
    public Action<int> OnGainedMultiplier;
    
    private SpriteRenderer sr;
    private int multiplier = 0;
    private int damagePlus = 5;
    private int damage = 1;
    public const float distanceToTriggerBox = 10f;
    public bool boxIsRunning;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        MaxHealth = 100;
        OnSittingGainedDamage += SittingGainedDamage;
        OnGainedMultiplier += AddMultiplier;
        boxIsRunning = false;
    }

    private void AddMultiplier(int obj)
    {
        multiplier += obj;
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

        if (casingEjector.isReloading && !boxIsRunning)
            if(Vector2.Distance(
               new Vector2(buckController.transform.position.x, buckController.transform.position.y), 
               new Vector2(transform.position.x, transform.position.y)
             ) < distanceToTriggerBox) {
                boxIsRunning = true;
                HideGuidingZone();
                boxController.StartBox();
            }
    }

    public void RevealGuidingZone()
    {
        Target.SetActive(true);
    }

    public void HideGuidingZone()
    {
        Target.SetActive(false);
    }

    Tween shakeTween;

    private void SittingGainedDamage()
    {
        if (shakeTween == null || !shakeTween.IsPlaying())
            shakeTween = transform.DOShakePosition(0.2f);
        MaxHealth -= ((damagePlus * (multiplier > 0 ? 1 : 0)) + damage);
        if (multiplier > 0)
            multiplier--;
        if (MaxHealth >= 80)
        {
            sr.sprite = SittingSprite0;
        }
        else if (MaxHealth < 80 && MaxHealth >= 40)
        {
            AudioController.Instance.PlayCollapse();
            sr.sprite = SittingSprite1;
        }
        else if (MaxHealth < 40 && MaxHealth > 0)
        {
            AudioController.Instance.PlayCollapse();
            sr.sprite = SittingSprite2;
        }
        else
        {
            sr.sprite = SittingSprite3;
            AudioController.Instance.PlayCollapse();
            StartCoroutine(WinScreen());
        }

        // switch (MaxHealth)
        // {
        //     case 0: sr.sprite = SittingSprite0; break;
        //     case 1: sr.sprite = SittingSprite1; break;
        //     case 2: sr.sprite = SittingSprite2; break;
        //     case 3: sr.sprite = SittingSprite3; break;
        //     default: sr.sprite = SittingSprite0; break;
        // }

        DustParticles.Play();
    }

    private IEnumerator WinScreen()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }
}
