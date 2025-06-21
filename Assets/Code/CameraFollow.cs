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

    [Header("Follow Control")]
    [SerializeField] private bool followEnabled = true;
    public bool FollowEnabled
    {
        get => followEnabled;
        set => followEnabled = value;
    }

    private Tween positionTween;
    private Tween rotationTween;

    void LateUpdate()
    {
        if (!followEnabled || target == null) return;

        // Position follow
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Rotation follow
        if (followRotation && rotationTween == null)
        {
            float desiredZ = target.eulerAngles.z;
            float currentZ = transform.eulerAngles.z;
            float smoothedZ = Mathf.LerpAngle(currentZ, desiredZ, rotationSmoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, smoothedZ);
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

        // After both tweens finish, resume full follow
        Sequence resumeFollow = DOTween.Sequence();
        resumeFollow.AppendInterval(duration);
        resumeFollow.OnComplete(() =>
        {
            FollowEnabled = true;
            rotationTween = null;
        });
    }

}
