using UnityEngine;

public class PlayerSwimState : PlayerState
{
    public PlayerSwimState(
        Player player,
        PlayerStateMachine
        stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Update()
    {
        player.Swim();
    }
}
