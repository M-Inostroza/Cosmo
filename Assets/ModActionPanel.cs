using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModActionPanel : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer;

    public void DisplayModButtons(IModTriggerable[] mods)
    {
        // Clear old buttons
        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        foreach (var mod in mods)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            TMP_Text label = button.GetComponentInChildren<TMP_Text>();
            if (label != null) label.text = mod.ActionName;

            button.GetComponent<Button>().onClick.AddListener(mod.Trigger);
        }
    }
}
