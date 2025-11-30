using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class DeerController : MonoBehaviour
{
    [Header("---DEBUG---")]
    [SerializeField] public bool MoveTest;
    [SerializeField] public bool die;

    public GameObject DeathDeer;
    
    private Animator animator;
    private bool death = false;
    private bool free = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (MoveTest)
        {
            MoveTo(-45,0);
            MoveTest = false;
        }

        if (die)
        {
            Die();
            die = false;
        }

        if (free)
        {
            int random = Random.Range(0, 10000);
            if (random < 2)
            {
                LookAround();
                
            }
            if(random > 9998)
                Eat();
        }
        else
        {
            
        }
    }

    public void MoveTo(float x, float y)
    {
        animator.enabled = true;
        transform.DOLocalMove(new Vector3(x,y,transform.localPosition.z), 4f).OnComplete(()=>animator.enabled = false);
    }

    public void Die()
    {
        if (death)
            return;
        Score.Instance.DoeCount--;

        AudioController.Instance.PlayDeerDeath();
        death = true;
        free = false;
        animator.Play("Die");
        StartCoroutine(WaitAndDo(0.5f));
    }

    public void Eat()
    {
        if (death) return;
        free = false;
        animator.Play("Eat");
        StartCoroutine(Wait(5f));
    }
    
    public void LookAround()
    {
        if (death) return;
        free = false;
        animator.Play("Look");
        StartCoroutine(Wait(5f));
    }

    IEnumerator WaitAndDo(float time)
    {
        yield return new WaitForSeconds(time);
        var deathDeer = Instantiate(DeathDeer, transform.parent.parent.gameObject.GetComponent<DeerManager>().DeathDeersParent.transform);
        deathDeer.transform.position = transform.position;
        transform.parent.parent.gameObject.GetComponent<DeerManager>().DecreaseScore();
        Destroy(gameObject);
    }
    
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        free = true;
    }
    
}
