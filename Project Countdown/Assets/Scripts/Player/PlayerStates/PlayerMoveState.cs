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

        if (player.MoveInput.sqrMagnitude == 0f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
