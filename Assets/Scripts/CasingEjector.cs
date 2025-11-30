using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CasingEjector : MonoBehaviour
{
    [SerializeField]
    private Transform Casing;
    [SerializeField]
    private AudioController audioController;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        //foreach (Transform t in transform)
        //{
        //    Eject(t.position);
        //    yield return new WaitForSeconds(1f);
        //}
    }

    [ContextMenu(nameof(TestEject))]
    private void TestEject()
    {
        Eject(Vector2.zero);
    }

    public void Eject(Vector2 position)
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
