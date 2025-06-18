using UnityEngine;
using UnityEngine.UI;

public class OffscreenTargetIndicator : MonoBehaviour
{
    [SerializeField] private Transform target;      // Planet or object to track
    [SerializeField] private RectTransform icon;    // UI element (arrow image)
    [SerializeField] private float screenEdgeBuffer = 50f;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (target == null || icon == null || mainCam == null)
            return;

        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position);

        // Is the target behind the camera?
        if (screenPos.z < 0)
        {
            screenPos *= -1f;
        }

        // Check if target is off-screen
        bool offscreen = screenPos.x < 0 || screenPos.x > Screen.width ||
                         screenPos.y < 0 || screenPos.y > Screen.height;

        icon.gameObject.SetActive(offscreen);

        if (offscreen)
        {
            // Clamp to screen edge (with buffer)
            screenPos.x = Mathf.Clamp(screenPos.x, screenEdgeBuffer, Screen.width - screenEdgeBuffer);
            screenPos.y = Mathf.Clamp(screenPos.y, screenEdgeBuffer, Screen.height - screenEdgeBuffer);

            icon.position = screenPos;

            // Rotate icon to point toward the target
            Vector3 direction = (target.position - mainCam.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            icon.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // -90 to point up
        }
    }
}
