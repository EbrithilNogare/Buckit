using System;
using DG.Tweening;
using UnityEngine;

public class DeerController : MonoBehaviour
{
    [Header("---DEBUG---")]
    [SerializeField] public bool MoveTest;
    
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
    }

    public void MoveTo(float x, float y)
    {
        animator.enabled = true;
        transform.DOLocalMove(new Vector3(x,y,transform.localPosition.z), 4f).OnComplete(()=>animator.enabled = false);
    }
}
