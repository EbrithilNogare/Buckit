using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Reference (design-time) resolution")]
    public float targetWidth = 1920f;
    public float targetHeight = 1080f;

    private Camera camera;

    private float targetSize;
    private int lastW;
    private int lastH;

    void Awake()
    {
        camera = Camera.main;
        lastW = Screen.width;
        lastH = Screen.height;
        targetSize = camera.orthographicSize;

        SetOrthographicSize();
    }

    void SetOrthographicSize()
    {
        float targetAspect = targetWidth / targetHeight;
        float windowAspect = (float)Screen.width / Screen.height;

        if (windowAspect >= targetAspect)
        {
            camera.orthographicSize = targetSize;
        }
        else
        {
            float scale = targetAspect / windowAspect;
            camera.orthographicSize = targetSize * scale;
        }
    }

    void Update()
    {
        if (Screen.width != lastW || Screen.height != lastH)
        {
            lastW = Screen.width;
            lastH = Screen.height;
            SetOrthographicSize();
        }
    }
}
