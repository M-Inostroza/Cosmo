using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;            // The rocket

    [Header("Position Settings")]
    [SerializeField] private float smoothSpeed = 5f;      // Higher = snappier follow
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Rotation Settings")]
    [SerializeField] private bool followRotation = true;
    [SerializeField] private float rotationSmoothSpeed = 5f; // Degrees per second

    void LateUpdate()
    {
        if (target == null) return;

        // Smooth position follow
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Smooth rotation follow
        if (followRotation)
        {
            float desiredZ = target.eulerAngles.z;
            float currentZ = transform.eulerAngles.z;

            float smoothedZ = Mathf.LerpAngle(currentZ, desiredZ, rotationSmoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, smoothedZ);
        }
    }
}
