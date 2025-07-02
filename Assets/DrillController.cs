using UnityEngine;
using UnityEngine.InputSystem;

public class DrillController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 input;

    private PlayerInput playerInput;
    private InputAction moveAction;

    private void Awake()
    {
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
        input = moveAction?.ReadValue<Vector2>() ?? Vector2.zero;

    }

    private void FixedUpdate()
    {
        transform.Translate(input.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OilGoal"))
        {
            Debug.Log("Oil found!");
            if (MiningManager.Instance != null)
                MiningManager.Instance.RegisterOilFound();
        }
    }
}
