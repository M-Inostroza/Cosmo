using UnityEngine;
using System.Collections.Generic;


public class ModManager : MonoBehaviour
{
    public static ModManager Instance { get; private set; }
    public bool CanLaunch { get; private set; } = false;

    [SerializeField] private RocketModSlot[] modSlots;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        foreach (var slot in modSlots)
        {
            if (slot.IsOccupied && slot.GetComponentInChildren<RocketModBehaviour>() is RocketModBehaviour mod)
            {
                mod.OnModUpdate();
            }
        }
    }

    public IModTriggerable[] GetAllTriggerableMods()
    {
        List<IModTriggerable> result = new List<IModTriggerable>();

        foreach (var slot in modSlots)
        {
            if (slot.IsOccupied)
            {
                var mod = slot.GetComponentInChildren<IModTriggerable>();
                if (mod != null)
                    result.Add(mod);
            }
        }

        return result.ToArray();
    }


    public void EnableLaunch()
    {
        CanLaunch = true;
        Debug.Log("Launch enabled!");
    }

    public void AttachModToSlot(string slotId, RocketModData modData)
    {
        Debug.Log($"Attaching mod {modData.modName} to slot {slotId}");
        foreach (var slot in modSlots)
        {
            Debug.Log($"Checking slot: {slot.slotId}");

            if (slot.slotId == slotId)
            {
                Debug.Log($"Match found for slot {slotId}");
                slot.AttachMod(modData);
                return;
            }
        }
    }

    public float GetTotalModWeight()
    {
        float total = 0f;
        foreach (var slot in modSlots)
        {
            if (slot.IsOccupied && slot.currentModData != null)
                total += slot.currentModData.weight;
        }
        return total;
    }
}
