using DG.Tweening;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public SpriteRenderer deathScreen;

    public SpriteRenderer deathText;
    
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
