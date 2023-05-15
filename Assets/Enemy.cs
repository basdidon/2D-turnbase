using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, IDamageable
{
    public override void OnStartTurn()
    {
        base.OnStartTurn();
        // move to player
        State = new MoveState(this, moveTime);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        TurnManager.Instance.Enemies.Remove(this);
    }
}
