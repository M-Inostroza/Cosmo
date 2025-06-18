using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    [Header("Orbit Settings")]
    [SerializeField] private Transform orbitCenter;
    [SerializeField] private float orbitRadius = 10f;
    [SerializeField] private float orbitSpeed = 1f;       
    [SerializeField] private float startAngle = 0f;
    [SerializeField] private bool clockwise = true;

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
        Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f) * (orbitRadius * 1000);
        transform.position = orbitCenter.position + offset;
    }
}
