using System.Collections;
using UnityEngine;

public class DeersChanging : MonoBehaviour
{
    private int index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitAndDo(1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator WaitAndDo(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeImage();
        StartCoroutine(WaitAndDo(1f));
    }

    private void ChangeImage()
    {
        transform.GetChild(index).gameObject.SetActive(false);
        index++;
        if (index >= transform.childCount)
        {
            index = 0;
        }
        transform.GetChild(index).gameObject.SetActive(true);
    }
}
