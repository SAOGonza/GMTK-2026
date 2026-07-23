using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowerCore : MonoBehaviour, IInteractable, IContextualInteractable
{
    [Header("Settings")]
    [SerializeField] private bool consumePowerCells = true;

    private PlayerInventory playerInventory;
    private bool hasBeenOverloaded;


    public string InteractionPrompt
    {
        get
        {
            if (hasBeenOverloaded)
                return "Power Core overloaded!";

            if (playerInventory == null || !playerInventory.HasEnoughPowerCells)
                return "You need more power cells to overload the power core.";

            return "Press 'E' to overload the power core.";
        }
    }

    public void SetInteractor(Player player)
    {
        if (player == null)
        {
            playerInventory = null;
            return;
        }

        playerInventory = player.GetComponent<PlayerInventory>();
    }

    public void Interact(Player player)
    {
        if (hasBeenOverloaded || player == null)
            return;

        playerInventory = player.GetComponent<PlayerInventory>();

        if (playerInventory == null)
        {
            Debug.LogWarning("Player is missing PlayerInventory component.");
            return;
        }

        if (!playerInventory.HasEnoughPowerCells)
            return;

        OverloadCore();
    }

    private void OverloadCore()
    {
        hasBeenOverloaded = true;

        if (consumePowerCells)
            playerInventory.RemovePowerCells(playerInventory.RequiredPowerCells);

        Debug.Log("Power core overloaded.");
        GameManager.Instance?.TriggerWin();
    }

    public void SetPlayerInventory(PlayerInventory inventory)
    {
        playerInventory = inventory;
    }
}
