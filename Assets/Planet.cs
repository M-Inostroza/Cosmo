using UnityEngine;

public class Planet : MonoBehaviour
{
    public float gravityStrength = 9.8f; // Adjustable in Inspector

    void FixedUpdate()
    {
        // Use FindObjectsByType instead of the deprecated FindObjectsOfType
        foreach (var body in Object.FindObjectsByType<Rocket>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            Vector2 direction = (Vector2)(transform.position - body.transform.position);
            float distance = direction.magnitude;

            if (distance == 0) continue;

            // Optional: gravity falloff with distance squared
            Vector2 force = direction.normalized * gravityStrength;

            // Apply the force to the object
            body.ApplyGravity(force);
        }
    }
}
