using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform orbitCenter;
    public float orbitRadius = 10f;
    public float orbitSpeed = 1f;       // Interpreted as "slow or fast", not degrees/sec
    public float orbitScale = 100f;     // Scales world units for visual clarity
    public float startAngle = 0f;
    public bool clockwise = true;

    private float currentAngle;

    void Start()
    {
        currentAngle = startAngle;
    }

    void Update()
    {
        float direction = clockwise ? -1f : 1f;

        currentAngle += (orbitSpeed * 0.001f) * direction * Time.deltaTime;

        float angleRad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * orbitRadius * orbitScale;
        transform.position = orbitCenter.position + offset;
    }
}
