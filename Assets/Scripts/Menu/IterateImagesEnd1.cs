using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IterateImagesEnd1 : MonoBehaviour
{
    public GameObject parent;
    public Image Text;
    public GameObject swag;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        parent.transform.GetChild(0).GetComponent<Image>().DOFade(1, 1f).OnComplete(() =>
        {
            parent.transform.GetChild(1).GetComponent<Image>().DOFade(1, 0);
            parent.transform.GetChild(2).GetComponent<Image>().DOFade(1, 0);
            Text.DOFade(1, 2f).OnComplete(() =>
            {
                swag.transform.DOLocalMoveY(-220f, 4f);
            });
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
            parent.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            parent.transform.GetChild(1).gameObject.SetActive(true);
            parent.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
