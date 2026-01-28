using System.Collections;
using System.Collections.Generic;
using Game.Runtime.Core;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player,
        stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (GameManager.Instance.CurrentPhase != GamePhase.Gameplay) return;
        //if (player.moveInputValue == Vector2.zero) return;

        if (player.IsMove()) stateMachine.ChangeState(player.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}