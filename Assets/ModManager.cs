using UnityEngine;

public class ModManager : MonoBehaviour
{
    public bool CanLaunch { get; private set; } = false;

    [SerializeField] private RocketModSlot[] modSlots;

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
