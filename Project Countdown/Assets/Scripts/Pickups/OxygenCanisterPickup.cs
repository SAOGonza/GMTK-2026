using UnityEngine;

public class OxygenCanisterPickup : Pickup
{
    public override void Interact(Player player)
    {
        if (player == null)
            return;

        GameTimer.Instance.ResetGauge();
        base.Interact(player);
    }
}