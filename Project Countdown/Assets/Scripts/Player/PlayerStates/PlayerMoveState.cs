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
        player.Move();

        // Stop moving when there is no movement input detected.
        if (player.MoveInput.sqrMagnitude == 0f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
