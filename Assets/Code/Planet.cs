using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Gravity Settings")]
    public float gravityStrength = 100f;         // Base gravity strength
    public float maxGravityForce = 50f;          // Clamp max force
    public float minGravityDistance = 1f;        // Avoid divide-by-zero and too strong pull

    void FixedUpdate()
    {
        foreach (var body in Object.FindObjectsByType<Rocket>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            Vector2 direction = (Vector2)(transform.position - body.transform.position);
            float distance = direction.magnitude;

            if (distance < minGravityDistance) continue;

            // Gravity falloff: smoother gameplay-friendly version (1 / distance)
            float forceMagnitude = gravityStrength / distance;

            // Optional: Clamp force
            forceMagnitude = Mathf.Min(forceMagnitude, maxGravityForce);

            Vector2 force = direction.normalized * forceMagnitude;
            body.ApplyGravity(force);
        }
    }
}
