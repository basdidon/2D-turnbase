using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : Character
{
    public static Player Instance { get; private set; }
    TurnManager TurnManager { get { return TurnManager.Instance; } }
    Inputs Inputs { get; set; }

    private void OnEnable()
    {
        Inputs.Enable();
    }
    private void OnDisable()
    {
        Inputs.Disable();
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Inputs = new Inputs();
        Inputs.Player.Move.performed += ctx =>
        {
            if(TurnManager.CharacterTurn == this && State == IdleState)
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                Vector3Int des = GridPosition + (input.x < 0 ? Vector3Int.left : input.x > 0 ? Vector3Int.right : input.y < 0 ? Vector3Int.down : Vector3Int.up);

                if (BoardManager.TryMoveObject(this, des))
                {
                    State = new MoveState(this, moveTime);
                }
                else if (BoardManager.TryGetBoardObjectOnGridPosition(des, out BoardObject broadObject))
                {
                    if (broadObject is IDamageable damageable)
                    {
                        damageable.TakeDamage(1);
                    }
                }

                ConsumeActionPoint();   
            }
        };
    }

    private void Update()
    {
        State.UpdateState();
    }
}