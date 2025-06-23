using UnityEngine;

public class CompassSystem : MonoBehaviour
{
    [SerializeField] private Transform target;            // The asteroid or point of interest
    [SerializeField] private Transform rocketTransform;   // The rocket
    [SerializeField] private RectTransform pointerUI;     // The pointer icon
    [SerializeField] private float radius = 150f;         // Distance from rocket

    void LateUpdate()
    {
        if (rocketTransform == null || target == null || pointerUI == null) return;

        // Calculate direction from rocket to target
        Vector2 direction = (target.position - rocketTransform.position).normalized;

        // Calculate world-space pointer position
        Vector3 pointerWorldPos = rocketTransform.position + (Vector3)(direction * radius);

        // Set position and rotation
        pointerUI.position = pointerWorldPos;
        pointerUI.up = direction; // Rotate arrow to face the target
    }
}
