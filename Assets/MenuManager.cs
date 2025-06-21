using DG.Tweening;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Camera Orbit Settings")]
    [SerializeField] private Transform cameraRig;
    [SerializeField] private Transform planetCenter;
    [SerializeField] private Camera menuCamera;
    [SerializeField] private float rotationSpeed = 0.2f;

    [Header("Launch Pad")]
    [SerializeField] private Collider2D launchPadCollider;

    private Vector2 lastTouchPos;
    private bool isDragging = false;

    void Start()
    {
        PositionCameraAtStart();
    }

    void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
        CheckLaunchPadClick();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPos;
            lastTouchPos = Input.mousePosition;
            RotateRig(delta.x);
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                lastTouchPos = touch.position;
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.position - lastTouchPos;
                lastTouchPos = touch.position;
                RotateRig(delta.x);
            }
        }
    }

    void RotateRig(float horizontalDelta)
    {
        if (cameraRig == null) return;
        float angle = horizontalDelta * (rotationSpeed * 0.01f);
        cameraRig.Rotate(0f, 0f, angle);
    }

    void PositionCameraAtStart()
    {
        if (cameraRig == null || planetCenter == null || menuCamera == null) return;

        cameraRig.position = planetCenter.position;
        cameraRig.rotation = Quaternion.identity;

        menuCamera.transform.SetParent(cameraRig);
        menuCamera.transform.localPosition = new Vector3(0f, 626.4f, -10f);
        menuCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void CheckLaunchPadClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = menuCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 point = new Vector2(worldPos.x, worldPos.y);

            if (launchPadCollider != null && launchPadCollider.OverlapPoint(point))
            {
                EnterFlyMode();
            }
        }
    }

    public void EnterFlyMode()
    {
        Debug.Log("Entering Fly Mode...");

        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.SnapToTargetSmooth(1f); // You can set any transition duration
        }

        // Cambia el tamaño de la cámara con DOTween
        if (menuCamera != null)
            menuCamera.DOOrthoSize(16f, 1f);

        Rocket rocket = FindFirstObjectByType<Rocket>();
        if (rocket != null)
            rocket.flightEnabled = true;

        gameObject.SetActive(false);
    }
}
