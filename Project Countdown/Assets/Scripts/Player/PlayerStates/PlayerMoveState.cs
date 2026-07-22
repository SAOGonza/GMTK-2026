using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(
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

        player.Move();

        // Stop moving when there is no movement input detected.
        if (player.MoveInput.sqrMagnitude == 0f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
