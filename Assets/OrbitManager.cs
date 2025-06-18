using UnityEngine;
using DG.Tweening;

public class OrbitManager : MonoBehaviour
{
    [Header("Orbit Zones (in meters from planet surface)")]
    [SerializeField] private float lowOrbitMin = 630f;
    [SerializeField] private float lowOrbitMax = 920f;

    [SerializeField] private float midOrbitMin = 1130f;
    [SerializeField] private float midOrbitMax = 1640f;

    [SerializeField] private float highOrbitMin = 1832f;
    [SerializeField] private float highOrbitMax = 2640f;

    [Header("References")]
    [SerializeField] private Transform planetTransform;
    [SerializeField] private Rigidbody2D rocketRigidbody;
    [SerializeField] private float planetRadius = 50f;

    [Header("Camera Control")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float zoomOutSize = 20f;
    [SerializeField] private float zoomDelay = 3f;
    [SerializeField] private float zoomDuration = 1f;

    private string currentZone = "";
    private float timeInZone = 0f;
    private bool hasZoomedOut = false;
    private float originalZoom;

    private void Start()
    {
        if (mainCamera != null)
            originalZoom = mainCamera.orthographicSize;
    }

    private void Update()
    {
        if (rocketRigidbody == null || planetTransform == null || mainCamera == null) return;

        float rawDistance = Vector2.Distance(rocketRigidbody.position, planetTransform.position);
        float altitude = Mathf.Max(0f, rawDistance - planetRadius);

        string orbitZone = GetOrbitZone(altitude);

        if (orbitZone != currentZone)
        {
            currentZone = orbitZone;
            timeInZone = 0f;

            if (hasZoomedOut)
            {
                ZoomTo(originalZoom);
                hasZoomedOut = false;
            }
        }
        else
        {
            timeInZone += Time.deltaTime;

            if (!hasZoomedOut && IsOrbitZone(orbitZone) && timeInZone >= zoomDelay)
            {
                ZoomTo(zoomOutSize);
                hasZoomedOut = true;
            }
        }

        Debug.Log($"Current Orbit: {orbitZone}");
    }

    private string GetOrbitZone(float altitude)
    {
        if (altitude >= lowOrbitMin && altitude <= lowOrbitMax)
            return "Aeris Belt (low orbit)";
        if (altitude >= midOrbitMin && altitude <= midOrbitMax)
            return "Stratus Ring (mid orbit)";
        if (altitude >= highOrbitMin && altitude <= highOrbitMax)
            return "Celestia Orbit (high orbit)";
        return "Outside Orbit Zones";
    }

    private bool IsOrbitZone(string zone)
    {
        return zone.Contains("orbit");
    }

    private void ZoomTo(float size)
    {
        mainCamera.DOOrthoSize(size, zoomDuration).SetEase(Ease.InOutSine);
        Debug.Log($"Zooming camera to {size}");
    }
}
