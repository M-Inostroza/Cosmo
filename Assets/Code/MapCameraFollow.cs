using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class MapCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private RawImage mapDisplayUI;
    [SerializeField] private int resolutionWidth = 1920;
    [SerializeField] private int resolutionHeight = 1080;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomStep = 500f;
    [SerializeField] private float zoomDuration = 0.5f;

    private Camera cam;
    private Tween zoomTween;
    private RenderTexture mapTexture;

    void Awake()
    {
        cam = GetComponent<Camera>();

        float aspect = (float)Screen.width / Screen.height;
        resolutionWidth = Mathf.RoundToInt(resolutionHeight * aspect);

        mapTexture = new RenderTexture(resolutionWidth, resolutionHeight, 16, RenderTextureFormat.ARGB32);
        mapTexture.Create();

        cam.targetTexture = mapTexture;

        if (mapDisplayUI != null)
            mapDisplayUI.texture = mapTexture;
    }


    void LateUpdate()
    {
        if (target != null)
            transform.position = target.position + offset;
    }

    public void ZoomIn()
    {
        float targetSize = Mathf.Max(100f, cam.orthographicSize - zoomStep);
        StartZoom(targetSize);
    }

    public void ZoomOut()
    {
        float targetSize = cam.orthographicSize + zoomStep;
        StartZoom(targetSize);
    }

    private void StartZoom(float targetSize)
    {
        if (zoomTween != null && zoomTween.IsActive())
            zoomTween.Kill();

        zoomTween = cam.DOOrthoSize(targetSize, zoomDuration).SetEase(Ease.InOutSine);
    }

    // ✅ Public function to enable the map
    public void ShowMap()
    {
        if (cam != null)
        {
            cam.enabled = true;

            // Reassign the target texture if needed
            if (cam.targetTexture != mapTexture)
                cam.targetTexture = mapTexture;
        }

        if (mapDisplayUI != null)
        {
            mapDisplayUI.texture = mapTexture;
            mapDisplayUI.enabled = true;
        }
        else
        {
            Debug.LogWarning("MapCanvas is not assigned in MapCameraFollow.");
        };
    }



    // ✅ Public function to disable the map
    public void HideMap()
    {
        Debug.Log("Map off");
        if (cam != null)
            cam.enabled = false;

        if (mapDisplayUI != null)
        {
            mapDisplayUI.texture = null;
            mapDisplayUI.enabled = false;
        }
    }


}
