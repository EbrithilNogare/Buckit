using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MinigameController : MonoBehaviour
{
    [SerializeField] GameObject minigameWindow;
    [SerializeField] TextMeshProUGUI minigameText;
    [SerializeField] Image progressBar;
    
    private string minigameChar1 = "E";
    private string minigameChar2 = "Q";

    private float minRadialValue = 0.1f;
    private float maxRadialValue = 0.95f;
    
    private bool minigameActive;

    private void Start()
    {
        RestartQuickTimeEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.fillAmount >= maxRadialValue)
        {
            CallEnd();
        }
        if(minigameActive)
            SetRadialValue(progressBar.fillAmount + (Time.deltaTime / 2));
    }

    private void CallEnd()
    {
        Debug.Log("DEATH!!!!");
    }

    public void MinigameStart()
    {
        minigameWindow.SetActive(true);
        minigameActive = true;
    }

    public void SetMinigameText()
    {
        int random = Random.Range(0, 2);
        minigameText.text = random == 0 ? minigameChar1 : minigameChar2;
    }

    private void RestartQuickTimeEvent()
    {
        SetRadialValue(0f);
        SetMinigameText();
        
    }

    private void SetRadialValue(float radialValue)
    {
        progressBar.fillAmount = Mathf.Clamp(radialValue, minRadialValue, maxRadialValue);
    }

    public void CallE_left(InputAction.CallbackContext ctx)
    {
        Debug.Log("LEFT");
    }

    public void CallQ_right(InputAction.CallbackContext ctx)
    {
        Debug.Log("RIGHT");
    }
}
