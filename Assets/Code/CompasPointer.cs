using UnityEngine;
using TMPro;

public class CompasPointer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;               // The asteroid or point of interest
    [SerializeField] private Transform rocketTransform;      // The rocket
    [SerializeField] private RectTransform pointerUI;        // The pointer icon
    [SerializeField] private TextMeshProUGUI distanceText;   // Distance label
    [SerializeField] private Vector2 distanceTextOffset = new Vector2(0f, -20f); // Offset below arrow

    [Header("Display Settings")]
    [SerializeField] private float radius = 150f;            // Distance from rocket
    [SerializeField] private float rotationOffset = 0f;      // Adjust if sprite is rotated

    void LateUpdate()
    {
        if (rocketTransform == null || target == null || pointerUI == null) return;

        // Calculate direction from rocket to target
        Vector2 direction = (target.position - rocketTransform.position).normalized;

        // Position the pointer around the rocket
        Vector3 pointerWorldPos = rocketTransform.position + (Vector3)(direction * radius);
        pointerUI.position = pointerWorldPos;
        pointerUI.rotation = Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0f, 0f, rotationOffset);

        // Update the distance label
        if (distanceText != null)
        {
            float distance = Vector2.Distance(rocketTransform.position, target.position);
            distanceText.text = $"{distance:F0} m";

            Vector3 screenPos = pointerUI.position + (Vector3)distanceTextOffset;
            distanceText.rectTransform.position = screenPos;
        }
    }
}
