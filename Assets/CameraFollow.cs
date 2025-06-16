using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // The rocket
    public float smoothSpeed = 5f;      // Higher = snappier follow
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Default 2D camera position

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
