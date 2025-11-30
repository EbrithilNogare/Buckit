using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxController : MonoBehaviour
{
    [SerializeField] GameObject boxE;
    [SerializeField] GameObject boxQ;
    [SerializeField] GameObject BoxWindow;
    
    [SerializeField] SittingController sittingController;
    [SerializeField] BuckController buckController;

    [SerializeField] private bool pressE;
    [SerializeField] private bool pressQ;

    public bool minigameActive = false;
    
    [Header("---DEBUG---")]
    [SerializeField] private bool debug = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            StartBox();
            debug = false;
        }
    }

    public void StartBox()
    {
        minigameActive = true;
        BoxWindow.SetActive(true);
        pressE = true;
        pressQ = false;
        SwapBox(boxE.transform, true);
        SwapBox(boxQ.transform, false);
        buckController.FightStart();
    }

    public void EndBox()
    {
        if (!minigameActive)
            return;

        minigameActive = false;
        BoxWindow.SetActive(false);
        buckController.EndFight();

        sittingController.boxIsRunning = false;
    }

    private void SwapBox(Transform letter, bool value)
    {
        int indexToConvert = Convert.ToInt32(value);
        letter.GetChild(indexToConvert).gameObject.SetActive(true);
        letter.GetChild(indexToConvert == 0 ? 1 : 0).gameObject.SetActive(false);
    }
    
    public void CallE_left(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && minigameActive) PressedE();
    }

    private void PressedE()
    {
        if (!pressE)
        {
            return;
        }
        Box();
        SwapBox(boxE.transform, false);
        SwapBox(boxQ.transform, true);
        pressE = false;
        pressQ = true;
    }

    public void CallQ_right(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && minigameActive) PressedQ();
    }

    private void Box()
    {
        sittingController.OnSittingGainedDamage?.Invoke();
        buckController.FightSwap();
        AudioController.Instance.PlayWoodHit();
    }

    private void PressedQ()
    {
        if (!pressQ)
        {
            return;
        }
        Box();
        SwapBox(boxE.transform, true);
        SwapBox(boxQ.transform, false);
        pressQ = false;
        pressE = true;
    }
}
