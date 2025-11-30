using System.Collections;
using DG.Tweening;
using UnityEngine;

public class IterateImagesEnd : MonoBehaviour
{
    public GameObject parent;
    public SpriteRenderer Text;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        parent.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(1, 1f).OnComplete(() =>
        {
            parent.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(1, 0);
            Text.DOFade(1, 2f);
            StartCoroutine(WaitAndDo(1f));
        });
    }

    IEnumerator WaitAndDo(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeImage();
        StartCoroutine(WaitAndDo(1f));
    }

    private void ChangeImage()
    {
        if (parent.transform.GetChild(1).gameObject.activeSelf)
        {
            parent.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            parent.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
