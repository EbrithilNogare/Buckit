using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MinigameController : MonoBehaviour
{
    [SerializeField] GameObject minigameWindow;
    [SerializeField] GameObject minigameLetters;
    [SerializeField] Image progressBar;
    [SerializeField] TelescopeController telescopeController;
    [SerializeField] SittingController sittingController;
    [SerializeField] private GameObject Moved;
    [SerializeField] private GameObject SuperMoved;
    [SerializeField] private GameObject BuckImage;
    [SerializeField] private RectTransform Bg;

    private float minRadialValue = 0.1f;
    private float maxRadialValue = 0.95f;
    
    private bool minigameActive;

    private int numOfBullets = 3;
    private int quickValue = 0;
    
    private DeerController deerController;

    private void Start()
    {
        RestartQuickTimeEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (!minigameWindow.activeSelf) return;
        if (progressBar.fillAmount >= maxRadialValue)
        {
            CallDeath();
        }

        if (minigameActive)
        {
            SetRadialValue(progressBar.fillAmount + (Time.deltaTime / 2f)); // moznost delit nejakou const
        }
    }

    private void CallDeath()
    {
        Debug.Log("DEATH!!!!");
        minigameWindow.SetActive(false);
        numOfBullets = 3;
        minigameActive = false;
        SetRadialValue(0f);
        //TODO
        //SCENE CHANGE!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void CallEnd()
    {
        Debug.Log("END!!!!");
        minigameWindow.SetActive(false);
        numOfBullets = 3;
        minigameActive = false;
        SetRadialValue(0f);
        telescopeController.LaserEnded();
    }

    public void MinigameStart(DeerController controller)
    {
        deerController = controller;
        minigameWindow.SetActive(true);
        minigameActive = true;
    }

    public void SetMinigameText()
    {
        int random = Random.Range(0, 2);
        quickValue = random;
        for (int i = 0; i < minigameLetters.transform.childCount; i++)
        {
            minigameLetters.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        minigameLetters.transform.GetChild(random).gameObject.SetActive(true);
    }

    private void RestartQuickTimeEvent()
    {
        if (numOfBullets == 0)
        {
            CallEnd();
            return;
        }
        SetRadialValue(0f);
        SetMinigameText();
    }

    private void SetRadialValue(float radialValue)
    {
        progressBar.fillAmount = Mathf.Clamp(radialValue, minRadialValue, maxRadialValue);
    }

    public void CallE_left(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Check(0);
    }
    
    public void CallQ_right(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) Check(1);
    }
    
    private void Check(int i)
    {
        if (!minigameWindow.activeSelf) return;
        minigameActive = false;
        
        //TODO
        //CALL AUDIO
        
        if (i != quickValue)
        {
            CallDeath();
        }
        else
        {
            AudioController.Instance.PlayGunshot();
            if (progressBar.fillAmount >= minRadialValue && progressBar.fillAmount <= 0.51f)
            {
                //KILL DEER
                Debug.Log("Kill deer");
                deerController.Die();
                CallEnd();
            }
            else if (progressBar.fillAmount > 0.51 && progressBar.fillAmount <= 0.85f)
            {
                //MISS and Continue
                Miss(i, false);
                Debug.Log("green");
                
            }
            else if(progressBar.fillAmount > 0.85f && progressBar.fillAmount <= maxRadialValue)
            {
                //Boost Damage and Miss
                Debug.Log("red");
                Miss(i, true);
                sittingController.OnGainedMultiplier?.Invoke(1);
            }
        }
    }

    private void Miss(int i, bool extraMiss)
    {
        BuckImage.SetActive(false);
        RectTransform image; 
        if (extraMiss)
        {
            image = SuperMoved.transform.GetChild(i).GetComponent<RectTransform>();
            SuperMoved.transform.GetChild(i).gameObject.SetActive(true);
            AudioController.Instance.PlayUberDodge();
        }
        else
        {
            image = Moved.transform.GetChild(i).GetComponent<RectTransform>();
            Moved.transform.GetChild(i).gameObject.SetActive(true);
            AudioController.Instance.PlayDeerDodge();
        }

        int movedValue = i == 0 ? 217 : -217;

        image.DOLocalMoveX(movedValue, .33f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            //Clean Up
            var vector3 = BuckImage.GetComponent<RectTransform>().localPosition;
            vector3.x = movedValue;
            BuckImage.GetComponent<RectTransform>().localPosition = vector3;
            BuckImage.SetActive(true);
            
            SuperMoved.transform.GetChild(i).gameObject.SetActive(false);
            Moved.transform.GetChild(i).gameObject.SetActive(false);
            
            vector3 = SuperMoved.GetComponent<RectTransform>().localPosition;
            vector3.x = movedValue;
            SuperMoved.GetComponent<RectTransform>().localPosition = vector3;
            
            vector3 = Moved.GetComponent<RectTransform>().localPosition;
            vector3.x = movedValue;
            Moved.GetComponent<RectTransform>().localPosition = vector3;

            int bgMove = i == 0 ? -25 : 25;
            Bg.DOLocalMoveX(Bg.localPosition.x + bgMove, .33f).SetEase(Ease.OutBounce);
            BuckImage.GetComponent<RectTransform>().DOLocalMoveX(0, 0.33f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                minigameActive = true;
                numOfBullets--;
                Debug.Log(numOfBullets);
                RestartQuickTimeEvent();
            });
            
        });
    }
}
