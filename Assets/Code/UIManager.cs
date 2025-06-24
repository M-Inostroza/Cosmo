using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rocketRigidbody;
    [SerializeField] private Transform planetTransform;
    [SerializeField] private OrbitManager orbitManager;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI gravityText;
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI altitudeText;
    [SerializeField] private TextMeshProUGUI orbitZoneText;

    [SerializeField] private float planetRadius = 50f;


    private ResourceManager resourceManager;

    void Start()
    {
        resourceManager = ResourceManager.Instance;
    }

    void Update()
    {
        if (rocketRigidbody == null || speedText == null || gravityText == null) return;

        // Calculate and display speed  
        float speed = rocketRigidbody.linearVelocity.magnitude;
        speedText.text = $"Speed: {speed:F1} m/s";

        // Calculate and display gravity  
        gravityText.text = $"Gravity: {rocketRigidbody.GetComponent<Rocket>().GetLastGravityForce().magnitude:F2} N";

        if (resourceManager != null)
            energyText.text = $"Energy: {resourceManager.Energy} / {resourceManager.MaxEnergy}";
        fuelText.text = $"Fuel: {resourceManager.Fuel} / {resourceManager.MaxFuel}";

        float rawDistance = Vector2.Distance(rocketRigidbody.position, planetTransform.position);
        float altitude = Mathf.Max(0f, rawDistance - planetRadius);
        altitudeText.text = $"Altitude: {altitude:F1} m";

        if (orbitZoneText != null && orbitManager != null)
        {
            orbitZoneText.text = $"Zone: {orbitManager.GetCurrentOrbitZone()}";
        }
    }
}
