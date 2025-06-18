using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Rocket : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float thrustForce = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float maxSpeed = 10f; // Max forward velocity
    [SerializeField] private SpriteRenderer thrustVisual;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector2 lastGravityForce;

    private bool isThrusting = false;


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

    void FixedUpdate()
    {

        isThrusting = false; // reset every physics frame
    }

    public void ApplyGravity(Vector2 force)
    {
        rb.AddForce(force);
        lastGravityForce = force; // Store for debugging
    }

    private void HandleInput()
    {
        Vector2 input = Vector2.zero;

        // Unity Input System
        if (moveAction != null)
            input = moveAction.ReadValue<Vector2>();

        // Fallback for keyboard input
        if (input == Vector2.zero)
        {
            input.x = (Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f) +
                      (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);
            input.y = Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;
        }

        // Rotation (left/right)
        float rotationInput = -input.x;
        rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.deltaTime);

        // Thrust
        if (input.y > 0f)
        {
            isThrusting = true;

            if (thrustVisual != null)
                thrustVisual.enabled = true;

            float velocityInThrustDir = Vector2.Dot(rb.linearVelocity, transform.up);

            if (velocityInThrustDir < maxSpeed)
            {
                rb.AddForce(transform.up * thrustForce * input.y);
            }
        }
        else
        {
            if (thrustVisual != null)
                thrustVisual.enabled = false;
        }
    }

    public void SetLastGravityForce(Vector2 force)
    {
        lastGravityForce = force;
    }

    public Vector2 GetLastGravityForce()
    {
        return lastGravityForce;
    }
}
