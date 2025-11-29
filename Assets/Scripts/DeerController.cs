using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DeerController : MonoBehaviour
{
    [Header("---DEBUG---")]
    [SerializeField] public bool MoveTest;
    [SerializeField] public bool die;

    public GameObject DeathDeer;
    
    private Animator animator;

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
    }

    public void MoveTo(float x, float y)
    {
        animator.enabled = true;
        transform.DOLocalMove(new Vector3(x,y,transform.localPosition.z), 4f).OnComplete(()=>animator.enabled = false);
    }

    public void Die()
    {
        animator.Play("Die");
        StartCoroutine(WaitAndDo(1f));
    }

    IEnumerator WaitAndDo(float time)
    {
        yield return new WaitForSeconds(time);
        var deathDeer = Instantiate(DeathDeer, transform.parent.parent.gameObject.GetComponent<DeerManager>().DeathDeersParent.transform);
        deathDeer.transform.position = transform.position;
        Destroy(gameObject);
    }
}
