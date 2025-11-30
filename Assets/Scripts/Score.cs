using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public static Score Instance;

    public int DoeCount = 15;

    public bool BuckAlive = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [ContextMenu("test")]
    private void Test()
    {
        SceneManager.LoadScene(1);
    }
}
