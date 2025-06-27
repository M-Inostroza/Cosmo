using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField] private float gravityStrength = 200f;
    [SerializeField] private float gravityRange = 2500f;
    [SerializeField] private float gravityStartRadius = 100f;

    private void FixedUpdate()
    {
        ApplyGravityToRockets();
        ApplyGravityToRovers();
    }

    private void ApplyGravityToRockets()
    {
        foreach (Rocket rocket in FindObjectsByType<Rocket>(FindObjectsSortMode.None))
        {
            if (rocket == null) continue;

            Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 force = CalculateGravityForce(rb.position, "Rocket");
                if (force != Vector2.zero)
                {
                    rb.AddForce(force);
                    rocket.SetLastGravityForce(force);
                }
            }
        }
    }

    private void ApplyGravityToRovers()
    {
        foreach (RoverController rover in FindObjectsByType<RoverController>(FindObjectsSortMode.None))
        {
            if (rover == null) continue;

            Rigidbody2D rb = rover.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 force = CalculateGravityForce(rb.position, "Rover");
                if (force != Vector2.zero)
                {
                    rb.AddForce(force);
                }
            }
        }
    }

    private Vector2 CalculateGravityForce(Vector2 objectPosition, string label)
    {
        Vector2 direction = (Vector2)transform.position - objectPosition;
        float distance = direction.magnitude;


        if (distance > gravityRange)
        {
            return Vector2.zero;
        }

        if (distance < gravityStartRadius)
        {
            return Vector2.zero;
        }

        float t = (distance - gravityStartRadius) / (gravityRange - gravityStartRadius);
        float forceMagnitude = gravityStrength * (1f - t);
        forceMagnitude = Mathf.Max(0f, forceMagnitude);

        Vector2 force = direction.normalized * forceMagnitude;

        return force;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravityStartRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, gravityRange);
    }
}
