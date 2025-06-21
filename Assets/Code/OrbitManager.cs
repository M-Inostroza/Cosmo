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
    [SerializeField] private float fasterZoomDuration = 0.5f;


    [Header("Orbital Representation")]
    [SerializeField] private GameObject orbitalAvatar;
    [SerializeField] private SpriteRenderer orbitalFire;

    private Tween currentZoomTween;
    private Sequence avatarSequence;

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
        bool isOrbit = IsOrbitZone(orbitZone);

        if (orbitZone != currentZone)
        {
            currentZone = orbitZone;
            timeInZone = 0f;

            if (!isOrbit && hasZoomedOut)
            {
                ZoomTo(originalZoom, fasterZoomDuration);
                hasZoomedOut = false;
            }
        }
        else
        {
            timeInZone += Time.deltaTime;

            if (isOrbit && !hasZoomedOut && timeInZone >= zoomDelay)
            {
                ZoomTo(zoomOutSize, zoomDuration);
                hasZoomedOut = true;
            }
        }
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

    private void ZoomTo(float size, float duration)
    {
        if (currentZoomTween != null && currentZoomTween.IsActive())
            currentZoomTween.Kill();

        currentZoomTween = mainCamera
            .DOOrthoSize(size, duration)
            .SetEase(Ease.InOutSine);

        if (size == zoomOutSize)
            ShowOrbitalAvatar();
        else
            HideOrbitalAvatar();
    }

    private void ShowOrbitalAvatar()
    {
        if (orbitalAvatar == null) return;

        orbitalAvatar.SetActive(true);
        var avatarRenderer = orbitalAvatar.GetComponent<SpriteRenderer>();
        if (avatarRenderer == null) return;

        if (orbitalFire != null)
        {
            Color fireStartColor = orbitalFire.color;
            fireStartColor.a = 0f;
            orbitalFire.color = fireStartColor;
            orbitalFire.transform.localScale = Vector3.zero;
        }

        Color avatarStartColor = avatarRenderer.color;
        avatarStartColor.a = 0f;
        avatarRenderer.color = avatarStartColor;

        orbitalAvatar.transform.localScale = Vector3.zero;

        if (avatarSequence != null && avatarSequence.IsActive()) avatarSequence.Kill();

        avatarSequence = DOTween.Sequence();
        avatarSequence.Append(avatarRenderer.DOFade(1f, 1.7f));
        avatarSequence.Join(orbitalAvatar.transform.DOScale(10f, 2f).SetEase(Ease.InCubic));

        if (orbitalFire != null)
        {
            avatarSequence.Join(orbitalFire.DOFade(1f, 1.7f));
            avatarSequence.Join(orbitalFire.transform.DOScale(2.8f, 2f).SetEase(Ease.OutBack));
        }
    }


    private void HideOrbitalAvatar()
    {
        if (orbitalAvatar == null) return;

        if (avatarSequence != null && avatarSequence.IsActive()) avatarSequence.Kill();

        var avatarRenderer = orbitalAvatar.GetComponent<SpriteRenderer>();
        if (avatarRenderer == null) return;

        avatarSequence = DOTween.Sequence();
        avatarSequence.Append(avatarRenderer.DOFade(0f, 0.3f));
        avatarSequence.Join(orbitalAvatar.transform.DOScale(0f, 0.3f));

        if (orbitalFire != null)
        {
            avatarSequence.Join(orbitalFire.DOFade(0f, 0.3f));
            avatarSequence.Join(orbitalFire.transform.DOScale(0f, 0.3f));
        }

        avatarSequence.OnComplete(() => orbitalAvatar.SetActive(false));
    }

    public string GetCurrentOrbitZone()
    {
        return currentZone;
    }
}

