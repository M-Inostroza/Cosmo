using System.Net.Sockets;
using UnityEngine;

public class RocketModSlot : MonoBehaviour
{
    public string slotId;
    public bool IsOccupied => currentMod != null;
    public RocketModData currentModData { get; private set; }

    private GameObject currentMod;
    private Rocket rocket;
    private ModManager modManager;

    private void Start()
    {
        rocket = GetComponentInParent<Rocket>();
        modManager = FindFirstObjectByType<ModManager>();
    }

    public void AttachMod(RocketModData modData)
    {
        ClearMod();

        if (modData != null && modData.prefab != null)
        {
            currentMod = Instantiate(modData.prefab, transform);
            currentMod.transform.localPosition = Vector3.zero;
            currentMod.SetActive(true);
        }

        currentModData = modData;

        if (rocket != null)
            rocket.RecalculateMass(modManager.GetTotalModWeight());
    }

    public void ClearMod()
    {
        if (currentMod != null)
            Destroy(currentMod);

        currentMod = null;
        currentModData = null;
    } 
}
