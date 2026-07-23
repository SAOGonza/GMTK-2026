using TMPro;
using UnityEngine;

public class PowerCellCounterUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TMP_Text counterText;

    private void OnEnable()
    {
        if (playerInventory != null)
        {
            playerInventory.OnPowerCellCountChanged += UpdateCounter;
        }
    }

    private void Start()
    {
        if (playerInventory != null)
        {
            UpdateCounter(playerInventory.PowerCellCount);
        }
    }

    private void OnDisable()
    {
        if (playerInventory != null)
        {
            playerInventory.OnPowerCellCountChanged -= UpdateCounter;
        }
    }

    private void UpdateCounter(int amount)
    {
        if (counterText == null || playerInventory == null)
            return;

        counterText.text = $"Power Cells: {amount}/" + $"{playerInventory.RequiredPowerCells}";
    }
}
