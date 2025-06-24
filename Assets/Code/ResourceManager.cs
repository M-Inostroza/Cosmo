using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    [Header("Fuel Settings")]
    [SerializeField] private int maxFuel = 100;
    [SerializeField] private int fuel = 100;

    [Header("Energy Settings")]
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int energy = 100;

    private float fuelRemainder;
    private float energyRemainder;

    public int Fuel => fuel;
    public int MaxFuel => maxFuel;

    public int Energy => energy;
    public int MaxEnergy => maxEnergy;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        fuel = Mathf.Clamp(fuel, 0, maxFuel);
        energy = Mathf.Clamp(energy, 0, maxEnergy);
    }

    /// <summary>
    /// Consumes fuel over time. The amount parameter is in fuel units per second.
    /// </summary>
    public void ConsumeFuel(float amountPerSecond)
    {
        float total = amountPerSecond * Time.deltaTime + fuelRemainder;
        int whole = Mathf.FloorToInt(total);
        fuelRemainder = total - whole;

        if (whole <= 0) return;

        fuel = Mathf.Max(fuel - whole, 0);
    }

    /// <summary>
    /// Consumes energy over time. The amount parameter is in energy units per second.
    /// </summary>
    public void ConsumeEnergy(float amountPerSecond)
    {
        float total = amountPerSecond * Time.deltaTime + energyRemainder;
        int whole = Mathf.FloorToInt(total);
        energyRemainder = total - whole;

        if (whole <= 0) return;

        energy = Mathf.Max(energy - whole, 0);
    }
}
