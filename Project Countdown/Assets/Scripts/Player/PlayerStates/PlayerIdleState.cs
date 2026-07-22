using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(
        Player player,
        PlayerStateMachine stateMachine)
        : base(player, stateMachine)
    {
    }

    public override void Update()
    {
        if (player.IsUnderwater)
        {
            stateMachine.ChangeState(player.SwimState);
            return;
        }

        player.ApplyGroundGravity();

        // Begin moving when there is movement input detected.
        if (player.MoveInput.sqrMagnitude > 0f)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }
}
