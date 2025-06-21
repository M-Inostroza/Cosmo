using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField] private float gravityStrength = 200f;
    [SerializeField] private float gravityRange = 2500f;
    [SerializeField] private float gravityStartRadius = 100f; // new

    private void FixedUpdate()
    {
        foreach (Rocket rocket in FindObjectsByType<Rocket>(FindObjectsSortMode.None))
        {
            Vector2 direction = (transform.position - rocket.transform.position);
            float distance = direction.magnitude;

            if (distance > gravityRange || distance < gravityStartRadius)
                continue;

            // Normalize gravity based on distance between start and range
            float t = (distance - gravityStartRadius) / (gravityRange - gravityStartRadius);
            float forceMagnitude = gravityStrength * (1f - t);
            forceMagnitude = Mathf.Max(0f, forceMagnitude);

            Vector2 force = direction.normalized * forceMagnitude;
            rocket.GetComponent<Rigidbody2D>().AddForce(force);
            rocket.SetLastGravityForce(force);
        }
    }
}
