using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Position Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Rotation Settings")]
    [SerializeField] private bool followRotation = true;
    [SerializeField] private float rotationSmoothSpeed = 5f;

    [Header("Zoom Settings")]
    [SerializeField] private Rigidbody2D rocketBody;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 30f;
    [SerializeField] private float maxSpeed = 500f; // the speed that triggers max zoom
    [SerializeField] private float zoomSmoothTime = 0.5f;

    [Header("Follow Control")]
    [SerializeField] private bool followEnabled = true;
    public bool FollowEnabled
    {
        get => followEnabled;
        set => followEnabled = value;
    }

    private Tween positionTween;
    private Tween rotationTween;
    private Tween zoomTween;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (!followEnabled || target == null) return;

        // Smooth position
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Smooth rotation
        if (followRotation && rotationTween == null)
        {
            float desiredZ = target.eulerAngles.z;
            float currentZ = transform.eulerAngles.z;
            float smoothedZ = Mathf.LerpAngle(currentZ, desiredZ, rotationSmoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, smoothedZ);
        }

        // Smooth zoom based on speed
        if (rocketBody != null)
        {
            float speed = rocketBody.linearVelocity.magnitude;
            float t = Mathf.Clamp01(speed / maxSpeed);
            float targetSize = Mathf.Lerp(minZoom, maxZoom, t);

            zoomTween?.Kill();
            zoomTween = cam.DOOrthoSize(targetSize, zoomSmoothTime).SetEase(Ease.OutSine);
        }
    }

    public void SnapToTargetSmooth(float duration = 1f)
    {
        if (target == null) return;

        FollowEnabled = false;

        Vector3 targetPos = target.position + offset;
        positionTween?.Kill();
        positionTween = transform.DOMove(targetPos, duration).SetEase(Ease.InOutSine);

        if (followRotation)
        {
            float targetZ = target.eulerAngles.z;
            rotationTween?.Kill();
            rotationTween = transform.DORotate(new Vector3(0f, 0f, targetZ), duration, RotateMode.FastBeyond360);
        }

        Sequence resumeFollow = DOTween.Sequence();
        resumeFollow.AppendInterval(duration);
        resumeFollow.OnComplete(() =>
        {
            FollowEnabled = true;
            rotationTween = null;
        });
    }

    public void SetFollowTarget(Transform newTarget, Rigidbody2D newBody = null)
    {
        target = newTarget;
        rocketBody = newBody;
    }

    public void SetFollowRotation(bool onOff)
    {
        followRotation = onOff;
    }
}
