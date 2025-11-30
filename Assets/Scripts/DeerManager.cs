using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeerManager : MonoBehaviour
{
    [Header("---CONST---")]
    public GameObject deerPrefab3;
    public GameObject deerPrefab4;
    public GameObject deerPrefab5;
    public GameObject SpawnPointsParent;
    public GameObject DeathDeersParent;
    public TextMeshProUGUI deerCountText;
    
    [Header("---DEBUG---")]
    [SerializeField] private bool getDeers;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AlphaDeer = transform.childCount >= 2 ? transform.GetChild(1).gameObject.GetComponent<DeerController>() : null;
    }
    
    // Update is called once per frame
    void Update()
    {
        #region TEST

        if (getDeers)
        {
            GetDeers();
            getDeers = false;
        }
        
        #endregion
    }

    private void CreateStado()
    {
        int random = Random.Range(0, 3);
        int randomPosition = Random.Range(0, 4);
        GameObject newDeer;
        switch (random)
        {
            case 0:
                newDeer = Instantiate(deerPrefab3, transform);
                break;
            case 1:
                newDeer = Instantiate(deerPrefab4, transform);
                break;
            case 2:
                newDeer = Instantiate(deerPrefab5, transform);
                break;
            default:
                newDeer = Instantiate(deerPrefab3, transform);
                break;
        }
        newDeer.transform.position = SpawnPointsParent.transform.GetChild(randomPosition).position;
    }

    public List<DeerController> GetDeers()
    {
        return FindObjectsByType<DeerController>(FindObjectsSortMode.None).ToList();
    }

    public void DecreaseScore()
    {
        deerCountText.text = Int32.TryParse(deerCountText.text, out var deerCount ) ? (deerCount - 1).ToString() : "0";
    }
}
