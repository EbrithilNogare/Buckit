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

    [SerializeField]
    private List<AudioClip> reloadList = new List<AudioClip>();

    [SerializeField]
    private List<AudioClip> deerDodgeList = new List<AudioClip>();

    [SerializeField]
    private AudioClip CasingEject;

    [SerializeField]
    private AudioClip CasingDrop;

    [SerializeField]
    private AudioClip WoodHit;

    [SerializeField]
    private AudioClip Aim;

    [SerializeField]
    private AudioClip Gunshot;

    private void Awake()
    {
        listener = FindAnyObjectByType<AudioListener>();
    }

    [ContextMenu(nameof(PlayCasingEject))]
    internal void PlayCasingEject()
    {
        AudioSource.PlayClipAtPoint(CasingEject, listener.transform.position);
    }

    [ContextMenu(nameof(PlayCasingDrop))]
    internal void PlayCasingDrop()
    {
        AudioSource.PlayClipAtPoint(CasingDrop, listener.transform.position);
    }

    [ContextMenu(nameof(PlayWoodHit))]
    internal void PlayWoodHit()
    {
        AudioSource.PlayClipAtPoint(WoodHit, listener.transform.position);
    }

    [ContextMenu(nameof(PlayAim))]
    internal void PlayAim()
    {
        AudioSource.PlayClipAtPoint(Aim, listener.transform.position);
    }

    [ContextMenu(nameof(PlayGunshot))]
    internal void PlayGunshot()
    {
        AudioSource.PlayClipAtPoint(Gunshot, listener.transform.position);
    }

    [ContextMenu(nameof(PlayReload))]
    public void PlayReload()
    {
        PlayReload(listener.transform.position);
    }

    public void PlayReload(Vector3 position)
    {
        PlayRandomSound(reloadList, position);
    }

    [ContextMenu(nameof(PlaySoftStep))]
    public void PlaySoftStep()
    {
        PlaySoftStep(listener.transform.position);
    }

    public void PlaySoftStep(Vector3 position)
    {
        PlayRandomSound(softStepList, position);
    }

    [ContextMenu(nameof(PlayHardStep))]
    public void PlayHardStep()
    {
        PlayHardStep(listener.transform.position);
    }

    public void PlayHardStep(Vector3 position)
    {
        PlayRandomSound(hardStepList, position);
    }

    [ContextMenu(nameof(PlayDeerDeath))]
    public void PlayDeerDeath()
    {
        PlayDeerDeath(listener.transform.position);
    }

    public void PlayDeerDeath(Vector3 position)
    {
        PlayRandomSound(deerDeathList, position);
    }

    [ContextMenu(nameof(PlayDeerCall))]
    public void PlayDeerCall()
    {
        PlayDeerCall(listener.transform.position);
    }

    public void PlayDeerCall(Vector3 position)
    {
        PlayRandomSound(deerCallList, position);
    }

    [ContextMenu(nameof(PlayDeerDodge))]
    public void PlayDeerDodge()
    {
        PlayDeerDodge(listener.transform.position);
    }

    public void PlayDeerDodge(Vector3 position)
    {
        PlayRandomSound(deerDodgeList, position);
    }

    private void PlayRandomSound(List<AudioClip> clips, Vector3 position)
    {
        AudioClip randStep = clips[Random.Range(0, clips.Count)];

        AudioSource.PlayClipAtPoint(randStep, position);
    }
}
