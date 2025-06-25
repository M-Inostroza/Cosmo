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

    private void Start()
    {
        StartCoroutine(WaitForGameManager());
        resourceManager = ResourceManager.Instance;
    }

    private System.Collections.IEnumerator WaitForGameManager()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);

        GameManager.Instance.OnStateChanged += HandleStateChange;

        if (GameManager.Instance.CurrentState == GameState.Fly)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleStateChange;
    }

    private void HandleStateChange(GameState state)
    {
        gameObject.SetActive(state == GameState.Fly);
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy || rocketRigidbody == null) return;

        UpdateSpeed();
        UpdateGravity();
        UpdateFuelAndEnergy();
        UpdateAltitude();
        UpdateOrbitZone();
    }

    private void UpdateSpeed()
    {
        if (speedText == null) return;
        float speed = rocketRigidbody.linearVelocity.magnitude;
        speedText.text = $"Speed: {speed:F1} m/s";
    }

    private void UpdateGravity()
    {
        if (gravityText == null) return;
        float gravity = rocketRigidbody.GetComponent<Rocket>().GetLastGravityForce().magnitude;
        gravityText.text = $"Gravity: {gravity:F2} N";
    }

    private void UpdateFuelAndEnergy()
    {
        if (resourceManager == null)
        {
            Debug.Log("Resource manager = Null");
            return;
        }

        if (fuelText != null)
            fuelText.text = $"Fuel: {resourceManager.Fuel} / {resourceManager.MaxFuel}";

        if (energyText != null)
            energyText.text = $"Energy: {resourceManager.Energy} / {resourceManager.MaxEnergy}";
    }

    private void UpdateAltitude()
    {
        if (altitudeText == null || planetTransform == null) return;

        float rawDistance = Vector2.Distance(rocketRigidbody.position, planetTransform.position);
        float altitude = Mathf.Max(0f, rawDistance - planetRadius);
        altitudeText.text = $"Altitude: {altitude:F1} m";
    }

    private void UpdateOrbitZone()
    {
        if (orbitZoneText == null || orbitManager == null) return;
        orbitZoneText.text = $"Zone: {orbitManager.GetCurrentOrbitZone()}";
    }
}
