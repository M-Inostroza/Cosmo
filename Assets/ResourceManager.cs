using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Fuel Settings")]
    [SerializeField] private int maxFuel = 100;
    [SerializeField] private int fuel = 100;

    // Used to accumulate fractional fuel consumption
    private float fuelRemainder;

    public int Fuel => fuel;
    public int MaxFuel => maxFuel;

    void Awake()
    {
        fuel = Mathf.Clamp(fuel, 0, maxFuel);
    }

    /// <summary>
    /// Consumes fuel over time. The amount parameter is in fuel units per second.
    /// </summary>
    public void Consume(float amountPerSecond)
    {
        float total = amountPerSecond * Time.deltaTime + fuelRemainder;
        int whole = Mathf.FloorToInt(total);
        fuelRemainder = total - whole;

        if (whole <= 0)
            return;

        fuel = Mathf.Max(fuel - whole, 0);
        Debug.Log($"Consumed {whole} fuel. Remaining: {fuel}/{maxFuel}");
    }
}