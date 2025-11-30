using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CasingEjector : MonoBehaviour
{
    [SerializeField]
    private Transform Casing;
    [SerializeField]
    private AudioController audioController;

    public bool isReloading;
    private int shellIndex;

    void Start()
    {
        isReloading = false;
        shellIndex = transform.childCount - 1;
    }

    // false = empty shells
    public bool UseShellAndContinue()
    {
        if (shellIndex == 0)
            return false;

        Transform shell = transform.GetChild(shellIndex);
        Vector2 position = shell.position;
        EjectAnimation(position);

        shell.gameObject.SetActive(false);

        shellIndex--;

        return shellIndex > 0;
    }

    public void StartReloading(System.Action callback)
    {
        isReloading = true;

        audioController.PlayAction();

        StartCoroutine(RevealBullets(callback));
    }

    private IEnumerator RevealBullets(System.Action callback)
    {

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform shell = transform.GetChild(i);
            shell.gameObject.SetActive(true);

            AudioController.Instance.PlayReload();

            yield return new WaitForSeconds(1.35f);
        }

        shellIndex = transform.childCount - 1;
        isReloading = false;

        callback.Invoke();
    }


    public void EjectAnimation(Vector2 position)
    {
        var casing = Instantiate(Casing, position, Quaternion.identity, transform.parent);

        audioController.PlayCasingEject();

        var seq = DOTween.Sequence();

        seq.Join(casing.DOLocalRotate(new Vector3(0, 0, Random.Range(70, 100)), 0.2f).SetEase(Ease.Linear).SetLoops(9,LoopType.Incremental));
        seq.Join(casing.DOMoveX(Random.Range(400f, 700f), 1f).SetEase(Ease.Linear).SetRelative(true));
        seq.Join(casing.DOMoveY(Random.Range(50f, 100f), 0.2f).SetEase(Ease.OutQuad).SetRelative(true));
        seq.Insert(0.2f, casing.DOMoveY(-1200f, 0.8f).SetEase(Ease.InQuad).SetRelative(true));

        seq.onComplete = () =>
        {
            Destroy(casing.gameObject);
            audioController.PlayCasingDrop();
        };
    }
}
