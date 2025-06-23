using UnityEngine;

public class ModUIController : MonoBehaviour
{
    [SerializeField] private ModManager modManager;
    [SerializeField] private GameObject modSelectionPanel;
    [SerializeField] private RocketModSlot[] modSlots;
    [SerializeField] private GameObject[] modSlotUIElements;


    private string selectedSlotId;

    public void OnSlotClick(string slotId)
    {
        selectedSlotId = slotId;
        SetModSlotsUIActive(false);
        modSelectionPanel.SetActive(true);
        Debug.Log($"Selected slot for mod: {slotId}");
    }

    public void OnModSelected(RocketModData modData)
    {
        if (string.IsNullOrEmpty(selectedSlotId))
        {
            Debug.LogWarning("No slot selected");
            return;
        }

        modManager.AttachModToSlot(selectedSlotId, modData);
        modSelectionPanel.SetActive(false);
        SetModSlotsUIActive(true);
        selectedSlotId = null;
    }


    public void SetModSlotsUIActive(bool state)
    {
        for (int i = 0; i < modSlotUIElements.Length; i++)
        {
            if (modSlotUIElements[i] == null || modSlots[i] == null)
                continue;

            if (state && modSlots[i].IsOccupied)
            {
                modSlotUIElements[i].SetActive(false);
            }
            else
            {
                modSlotUIElements[i].SetActive(state);
            }
        }
    }
}
