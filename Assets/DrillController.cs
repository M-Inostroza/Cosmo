using UnityEngine;

public class DrillController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 input;

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        transform.Translate(input.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OilGoal"))
        {
            Debug.Log("Oil found!");
            //EndMiningGame();
        }
    }
}
