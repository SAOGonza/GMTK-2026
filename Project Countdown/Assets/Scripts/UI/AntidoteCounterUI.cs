using TMPro;
using UnityEngine;

public class AntidoteCounterUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TMP_Text counterText;

    private void OnEnable()
    {
        if (playerInventory != null)
        {
            playerInventory.OnAntidoteCountChanged += UpdateCounter;
        }
    }

    private void Start()
    {
        if (playerInventory != null)
        {
            UpdateCounter(playerInventory.AntidoteCount);
        }
    }

    private void OnDisable()
    {
        if (playerInventory != null)
        {
            playerInventory.OnAntidoteCountChanged -= UpdateCounter;
        }
    }

    private void UpdateCounter(int amount)
    {
        if (counterText == null || playerInventory == null)
            return;

        counterText.text = $"Antidotes: {amount}";
    }
}
