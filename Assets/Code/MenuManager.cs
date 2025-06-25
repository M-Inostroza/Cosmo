using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuManager : MonoBehaviour
{
    [Header("Camera Orbit Settings")]
    [SerializeField] private Transform cameraRig;
    [SerializeField] private Transform planetCenter;
    [SerializeField] private Camera menuCamera;
    [SerializeField] private float rotationSpeed = 0.2f;
    [SerializeField] private float cameraZoomSpeed = 1f;
    private bool canRotate = true;

    [Header("Launch Pad")]
    [SerializeField] private Collider2D launchPadCollider;
    [SerializeField] private Button flyUI;

    [Header("Workshop")]
    [SerializeField] private Collider2D workshopCollider;
    [SerializeField] private Button driveUI;

    [Header("Menu")]
    [SerializeField] private Canvas modPanel;
    [SerializeField] private Canvas HUDPanel;
    [SerializeField] private Canvas workshopPanel;

    [SerializeField] private ModManager modManager;
    [SerializeField] private ModActionPanel modActionPanel;

    private Vector2 lastTouchPos;
    private bool isDragging = false;

    private void Start()
    {
        StartCoroutine(WaitForGameManager());
    }

    private System.Collections.IEnumerator WaitForGameManager()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);

        GameManager.Instance.OnStateChanged += HandleStateChanged;

        if (GameManager.Instance.CurrentState == GameState.Menu)
            ActivateMenuUI();
        else
            gameObject.SetActive(false);

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

        CheckWorkshopClick();
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

        if (isDragging && canRotate)
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
            else if (touch.phase == TouchPhase.Moved && canRotate)
            {
                Vector2 delta = touch.position - lastTouchPos;
                lastTouchPos = touch.position;
                RotateRig(delta.x);
            }
        }
    }

    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.Menu)
        {
            gameObject.SetActive(true);
            ActivateMenuUI();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void ActivateMenuUI()
    {
        canRotate = true;
        modPanel.gameObject.SetActive(false);
        flyUI.gameObject.SetActive(false);
        HUDPanel.gameObject.SetActive(false);
    }


    public void ShowModActionUI()
    {
        var mods = modManager.GetAllTriggerableMods();
        modActionPanel.DisplayModButtons(mods);
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
                PrepareForLaunch();
            }
        }
    }

    void CheckWorkshopClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = menuCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 point = new Vector2(worldPos.x, worldPos.y);

            if (workshopCollider != null && workshopCollider.OverlapPoint(point))
            {
                workshopPanel.gameObject.SetActive(true);
            }
        }
    }

    public void EnterFlyMode()
    {
        GameManager.Instance.ChangeState(GameState.Fly);

        ShowModActionUI();
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
            rocket.EnableFlight();

        HUDPanel.gameObject.SetActive(true);
        modPanel.gameObject.SetActive(false);
        flyUI.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    public void PrepareForLaunch()
    {
        GameManager.Instance.ChangeState(GameState.Preparation);
        canRotate = false;

        // Zoom camera
        if (menuCamera != null)
            menuCamera.DOOrthoSize(3f, 1f);
            menuCamera.transform.DOMove(new Vector3(0f, 616.5f, -10f), cameraZoomSpeed);

        modPanel.gameObject.SetActive(true);
        flyUI.gameObject.SetActive(true);
    }


    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }

}
