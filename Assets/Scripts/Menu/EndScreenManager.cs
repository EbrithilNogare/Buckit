using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreenManager : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject deathDoesScreen;
    public GameObject WinLess;
    public GameObject WinMid;
    public GameObject WinUltra;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Score.Instance == null) return;
        
        if (!Score.Instance.BuckAlive)
        {
            CallDeathScreen();
        }
        else
        {
            if (Score.Instance.DoeCount == 15)
            {
                CallWinUltra();
            }
            else
            {
                if (Score.Instance.DoeCount >= 8)
                {
                    CallWinMid();
                }
                else
                {
                    CallWinLess();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallDeathScreen()
    {
        deathScreen.SetActive(true);
    }
    
    public void CallDeathDoesScreen()
    {
        deathDoesScreen.SetActive(true);
    }

    public void CallWinLess()
    {
        WinLess.SetActive(true);
    }

    public void CallWinMid()
    {
        WinMid.SetActive(true);
    }

    public void CallWinUltra()
    {
        WinUltra.SetActive(true);
    }
    
}
