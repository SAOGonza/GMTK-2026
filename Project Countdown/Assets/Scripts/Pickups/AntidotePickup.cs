using UnityEngine;

public class AntidotePickup : Pickup
{
    public override void Interact(Player player)
    {
        if (player == null)
            return;

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        // Error handling if the PlayerInventory component is missing
        if (inventory == null)
        {
            Debug.LogWarning("Player is missing PlayerInventory component.");
            return;
        }

        inventory.AddAntidote();
        base.Interact(player);
    }
}