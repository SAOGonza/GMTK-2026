using UnityEngine;

public class OxygenCanisterPickup : Pickup
{
    public override void Interact(Player player)
    {
        if (player == null)
            return;

        Game.Manager.Oxygen = 100f;
        base.Interact(player);
    }
}