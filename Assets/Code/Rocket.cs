using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Rocket : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float thrustForce = 5f;
    [SerializeField] private float rotationSpeed = 180f; // Degrees per second

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction moveAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null && playerInput.actions != null)
        {
            moveAction = playerInput.actions["Move"];
        }
    }

    void OnEnable()
    {
        if (moveAction != null)
            moveAction.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null)
            moveAction.Disable();
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
        Vector2 input = Vector2.zero;
        if (moveAction != null)
        {
            input = moveAction.ReadValue<Vector2>();
        }

        // Fallback for desktop keyboard
        if (input == Vector2.zero)
        {
            input.x = (Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f) +
                      (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);
            input.y = Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;
        }

        float rotationInput = -input.x; // Left = -1 -> rotate counter-clockwise
        rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.deltaTime);

        if (input.y > 0f)
        {
            float adjustedThrust = thrustForce * 0.2f; // Adjust sensitivity here
            rb.AddForce(transform.up * adjustedThrust * input.y);
        }
    }
}
