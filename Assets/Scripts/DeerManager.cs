using UnityEngine;

public class DeerManager : MonoBehaviour
{
    [Header("---CONST---")]
    public GameObject deerPrefab3;
    public GameObject deerPrefab4;
    public GameObject deerPrefab5;
    public GameObject SpawnPointsParent;
    [SerializeField] private DeerController AlphaDeer;
    
    [Header("---DEBUG---")]
    [SerializeField] private bool spawnStag;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AlphaDeer = transform.childCount >= 2 ? transform.GetChild(1).gameObject.GetComponent<DeerController>() : null;
    }
    
    // Update is called once per frame
    void Update()
    {
        #region TEST

        if (spawnStag)
        {
            CreateStado();
            spawnStag = false;
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
}
