using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rocketRigidbody;
    [SerializeField] private Transform planetTransform;

    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI gravityText;
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI altitudeText;

    [SerializeField] private float planetRadius = 50f;

    private Vector2 lastGravityForce;

    void Update()
    {
        if (rocketRigidbody == null || speedText == null || gravityText == null) return;

        // Calculate and display speed
        float speed = rocketRigidbody.linearVelocity.magnitude;
        speedText.text = $"Speed: {speed:F1} m/s";

        // Calculate and display gravity
        gravityText.text = $"Gravity: {rocketRigidbody.GetComponent<Rocket>().GetLastGravityForce().magnitude:F2} N";

        var resourceManager = FindObjectOfType<ResourceManager>();
        fuelText.text = $"Fuel: {resourceManager.Fuel} / {resourceManager.MaxFuel}";

        float rawDistance = Vector2.Distance(rocketRigidbody.position, planetTransform.position);
        float altitude = Mathf.Max(0f, rawDistance - planetRadius);
        altitudeText.text = $"Altitude: {altitude:F1} m";
    }
}
