using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class RoverController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxMotorSpeed = 100f;
    [SerializeField] private float motorSmoothTime = 0.5f; // Higher = slower reaction
    [SerializeField] private bool isActive = false;

    [Header("References")]
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private WheelJoint2D[] wheelJoints;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private InputAction moveAction;

    private float moveInput = 0f;
    private float currentMotorSpeed = 0f;
    private float motorVelocity = 0f; // required for SmoothDamp

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null && playerInput.actions != null)
            moveAction = playerInput.actions["Move"];
    }

    private void OnEnable()
    {
        moveAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
    }

    private void Update()
    {
        if (!isActive) return;

        Vector2 input = moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
        moveInput = input.x;
    }

    private void FixedUpdate()
    {
        if (!isActive)
            return;

        float targetSpeed = moveInput * maxMotorSpeed;

        // Smooth acceleration/deceleration and direction switching
        currentMotorSpeed = Mathf.SmoothDamp(currentMotorSpeed, targetSpeed, ref motorVelocity, motorSmoothTime);

        foreach (WheelJoint2D joint in wheelJoints)
        {
            JointMotor2D motor = joint.motor;
            motor.motorSpeed = currentMotorSpeed;
            joint.motor = motor;
            joint.useMotor = Mathf.Abs(moveInput) > 0.01f;
        }
    }

    public void ActivateRover()
    {
        isActive = true;
        rb.simulated = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;

        GameManager.Instance.ChangeState(GameState.RoverDriving);
        cameraFollow.SetFollowTarget(transform, rb);
        cameraFollow.FollowEnabled = true;
        cameraFollow.SetFollowRotation(true);
    }

    public void DeactivateRover()
    {
        isActive = false;
        moveInput = 0f;
        currentMotorSpeed = 0f;
        motorVelocity = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        foreach (WheelJoint2D joint in wheelJoints)
        {
            joint.useMotor = false;
        }

        GameManager.Instance.ChangeState(GameState.Menu);
    }

    public bool IsActive => isActive;
}
