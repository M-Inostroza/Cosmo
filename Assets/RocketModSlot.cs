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

            var logic = currentMod.GetComponent<RocketModBehaviour>();
            if (logic != null)
            {
                var resource = rocket.GetResourceManager();
                if (resource != null)
                {
                    logic.Initialize(resource); // This is correct
                    Debug.Log($"Initialized {modData.modName} with ResourceManager");
                }
                else
                {
                    Debug.LogError("ResourceManager not found on Rocket");
                }
            }
            else
            {
                Debug.LogWarning("No RocketModBehaviour found on mod prefab");
            }


            Debug.Log($"Mod attached: {modData.modName} to {slotId}");
        }

        currentModData = modData;
    }

    public void ClearMod()
    {
        if (currentMod != null)
            Destroy(currentMod);

        currentMod = null;
        currentModData = null;
    } 
}
