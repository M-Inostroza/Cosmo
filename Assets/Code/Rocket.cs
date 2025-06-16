using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Movement Settings")]
    public float thrustForce = 5f;
    public float rotationSpeed = 180f; // Degrees per second

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
    }

    public void ApplyGravity(Vector2 force)
    {
        rb.AddForce(force);
    }

    void HandleInput()
    {
        // Rotation (left/right arrows)
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
            rotationInput = 1f; // Rotate counter-clockwise
        else if (Input.GetKey(KeyCode.RightArrow))
            rotationInput = -1f; // Rotate clockwise

        rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.deltaTime);

        // Thrust (up arrow)
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.up * thrustForce);
            Debug.Log("Thrust applied: " + thrustForce);
        }
    }
}
