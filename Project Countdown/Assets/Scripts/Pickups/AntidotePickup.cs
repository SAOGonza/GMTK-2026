using UnityEngine;

public class AntidotePickup : Pickup
{
    //[SerializeField] private GameTimer gameTimer;

    public override void Interact(Player player)
    {
        if (player == null)
            return;
        
        // CODE WAS MOVED TO Player.cs
        /*if (gameTimer == null)
            gameTimer = FindAnyObjectByType<GameTimer>();

        if (gameTimer == null)
        {
            Debug.LogWarning("AntidotePickup could not find a GameTimer.");
            return;
        }

        gameTimer.ApplyAntidote();*/

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