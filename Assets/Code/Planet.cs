using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField] private float gravityStrength = 200f;
    [SerializeField] private float gravityRange = 2500f;

    private void FixedUpdate()
    {
        foreach (Rocket rocket in FindObjectsOfType<Rocket>())
        {
            Vector2 direction = (transform.position - rocket.transform.position);
            float distance = direction.magnitude;

            if (distance > gravityRange) continue;

            float forceMagnitude = gravityStrength * (1f - (distance / gravityRange));
            forceMagnitude = Mathf.Max(0f, forceMagnitude);

            Vector2 force = direction.normalized * forceMagnitude;
            rocket.GetComponent<Rigidbody2D>().AddForce(force);
            rocket.SetLastGravityForce(force);
        }
    }
}
