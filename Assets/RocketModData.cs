using UnityEngine;

[CreateAssetMenu(fileName = "NewMod", menuName = "Rocket/Mod")]
public class RocketModData : ScriptableObject
{
    public string modName;
    public Sprite icon;
    public GameObject prefab;

    [TextArea]
    public string description;

    public ModType type;

    // Shared attributes
    public float powerUsage;
    public float weight;

    // Type-specific attributes
    public float damage;         // for lasers
    public float scanRange;      // for scanners
    public float extraFuel;      // for fuel tanks
}

public enum ModType
{
    Laser,
    Scanner,
    FuelTank,
}
