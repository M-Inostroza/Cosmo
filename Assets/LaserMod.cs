using UnityEngine;

public class LaserMod : RocketModBehaviour, IModTriggerable
{
    [SerializeField] private float energyPerSecond = 4f;

    public string ActionName => "Shoot";

    public void Trigger()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (resourceManager.Energy > 0)
        {
            resourceManager.ConsumeEnergy(energyPerSecond);
            Debug.Log("Laser: Shooting");
            Debug.DrawRay(transform.position, transform.up * 100f, Color.red, 0.1f);
        }
    }

    public override void OnModUpdate()
    {
        // No update needed
    }
}
