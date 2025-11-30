using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public Image deathScreen;
    public Image deathText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        deathScreen.DOFade(1, 1f).OnComplete(() =>
        {
            deathText.DOFade(1, 2f);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
