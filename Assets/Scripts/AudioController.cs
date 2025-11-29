using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioListener listener;

    [SerializeField]
    private List<AudioClip> softStepList = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> hardStepList = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> deerDeathList = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> deerCallList = new List<AudioClip>();

    private void Awake()
    {
        listener = FindAnyObjectByType<AudioListener>();
    }

    public void PlaySoftStep()
    {
        PlaySoftStep(listener.transform.position);
    }

    public void PlaySoftStep(Vector3 position)
    {
        PlayRandomSound(softStepList, position);
    }

    public void PlayHardStep()
    {
        PlayHardStep(listener.transform.position);
    }

    public void PlayHardStep(Vector3 position)
    {
        PlayRandomSound(hardStepList, position);
    }

    public void PlayDeerDeath()
    {
        PlayDeerDeath(listener.transform.position);
    }

    public void PlayDeerDeath(Vector3 position)
    {
        PlayRandomSound(deerDeathList, position);
    }

    public void PlayDeerCall()
    {
        PlayDeerCall(listener.transform.position);
    }

    public void PlayDeerCall(Vector3 position)
    {
        PlayRandomSound(deerCallList, position);
    }

    private void PlayRandomSound(List<AudioClip> clips, Vector3 position)
    {
        AudioClip randStep = clips[Random.Range(0, clips.Count)];

        AudioSource.PlayClipAtPoint(randStep, position);
    }
}
