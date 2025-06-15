using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyGravity(Vector2 force)
    {
        rb.AddForce(force);
    }
}
