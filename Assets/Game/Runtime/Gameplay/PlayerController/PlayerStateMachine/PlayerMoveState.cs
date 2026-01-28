using System.Collections;
using System.Collections.Generic;
using Game.Runtime.Core;
using Game.Runtime.Data;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player,
        stateMachine,
        animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // AudioManager.Instance.PlaySFX(AudioName.Step);
    }

    public override void Update()
    {
        base.Update();
        player.Move();
        if (GameManager.Instance.CurrentPhase != GamePhase.Gameplay || !player.IsMove())
            stateMachine.ChangeState(player.idleState);
    }


    public override void Exit()
    {
        base.Exit();
        // AudioManager.Instance.PlaySFX(AudioName.None);
    }
}