using UnityEngine;

public class AntidotePickup : Pickup
{
    [SerializeField] private GameTimer gameTimer;

    public override void Interact(Player player)
    {
        if (player == null)
            return;

        if (gameTimer == null)
            gameTimer = FindAnyObjectByType<GameTimer>();

        if (gameTimer == null)
        {
            Debug.LogWarning("AntidotePickup could not find a GameTimer.");
            return;
        }

        gameTimer.ApplyAntidote();
        base.Interact(player);
    }
}