using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RoverController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float driveForce = 5f;
    [SerializeField] private bool isActive = false;

    [Header("References")]
    [SerializeField] CameraFollow cameraFollow;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isActive) return;

        float input = 0f;
        if (Input.GetKey(KeyCode.A)) input = -1f;
        else if (Input.GetKey(KeyCode.D)) input = 1f;

        if (input != 0f)
        {
            rb.AddForce(transform.right * input * driveForce, ForceMode2D.Force);
        }
    }

    public void ActivateRover()
    {
        isActive = true;
        rb.simulated = true; // just in case it was off
        GameManager.Instance.ChangeState(GameState.RoverDriving);
        cameraFollow.SetFollowTarget(transform, rb);
    }

    public void DeactivateRover()
    {
        isActive = false;
        rb.linearVelocity = Vector2.zero;
        GameManager.Instance.ChangeState(GameState.Menu); // or whatever fits
    }
}
