using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Rocket : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float thrustForce = 5f;
    [SerializeField] private float fuelEfficiency = 8f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float thrustAccelerationTime = 2f;
    [SerializeField] private float thrustSmoothing = 5f;
    [SerializeField] private float fallDamageThreshold = 8f;


    [SerializeField] private AnimationCurve thrustCurve = AnimationCurve.Linear(0, 0, 1, 1);


    [Header("Visuals")]
    [SerializeField] private SpriteRenderer thrustVisual;
    [SerializeField] private SpriteRenderer thrustVisualAvatar;

    [Header("References")]
    [SerializeField] private ResourceManager resourceManager;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction moveAction;


    private Vector2 lastGravityForce;
    private float currentThrustPower = 0f;
    private bool isThrusting = false;
    public bool flightEnabled = false;


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
        moveAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        isThrusting = false;
    }

    public void ApplyGravity(Vector2 force)
    {
        rb.AddForce(force);
        lastGravityForce = force;
    }

    private void HandleInput()
    {
        if (!flightEnabled)
            return;

        Vector2 input = Vector2.zero;

        if (moveAction != null)
            input = moveAction.ReadValue<Vector2>();

        if (input == Vector2.zero)
        {
            input.x = (Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f) +
                      (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);
            input.y = Input.GetKey(KeyCode.UpArrow) ? 1f : 0f;
        }

        // Rotation
        float rotationInput = -input.x;
        rb.MoveRotation(rb.rotation + rotationInput * rotationSpeed * Time.deltaTime);

        // Thrust
        if (input.y > 0f)
        {
            bool hasFuel = resourceManager != null && resourceManager.Fuel > 0;

            if (hasFuel)
            {
                isThrusting = true;

                SetThrustVisuals(true);

                // Smooth buildup
                currentThrustPower += Time.deltaTime / thrustAccelerationTime;
                currentThrustPower = Mathf.Clamp01(currentThrustPower);

                float velocityInThrustDir = Vector2.Dot(rb.linearVelocity, transform.up);

                if (velocityInThrustDir < maxSpeed)
                {
                    float curved = thrustCurve.Evaluate(currentThrustPower);
                    rb.AddForce(transform.up * thrustForce * curved);
                }

                resourceManager.Consume(fuelEfficiency);
                return;
            }
        }

        // Reset if not thrusting
        currentThrustPower = 0f;
        SetThrustVisuals(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 impactVelocity = collision.relativeVelocity;
        float verticalSpeed = Mathf.Abs(Vector2.Dot(impactVelocity, transform.up));

        string targetTag = collision.collider.tag;
        Debug.Log($"Landed on: {targetTag}");

        if (verticalSpeed > fallDamageThreshold)
        {
            Debug.Log("explode");
        }
        else
        {
            Debug.Log("Clean landing");
        }
    }



    private void SetThrustVisuals(bool state)
    {
        if (thrustVisual != null)
            thrustVisual.enabled = state;
        if (thrustVisualAvatar != null)
            thrustVisualAvatar.enabled = state;
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
