using UnityEngine;

public class EndMusic : MonoBehaviour
{
    public AudioClip Win;
    public AudioClip Lose;

    void Start()
    {
        var audioSource = GetComponent<AudioSource>();
        if (Score.Instance == null || Score.Instance.DoeCount == 0 || !Score.Instance.BuckAlive)
        {
            audioSource.clip = Lose;
        }
        else
        {
            audioSource.clip = Win;
        }

        audioSource.Play();
    }
}
