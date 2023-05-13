using System.Collections;
using UnityEngine;

public class Player : Character
{
    [SerializeField] Inputs Inputs { get; set; }
    [SerializeField] float moveTime = 0.2f;

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
        Inputs = new Inputs();
        Inputs.Player.Move.performed += ctx =>
        {
            if(State == IdleState)
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                Vector3Int des = GridPosition + (input.x < 0 ? Vector3Int.left : input.x > 0 ? Vector3Int.right : input.y < 0 ? Vector3Int.down : Vector3Int.up);

                if (BroadManager.MoveObject(this, des))
                {
                    State = new PlayerMoveState(this, transform.position, CellCenterWorld, moveTime);
                }
                else if (BroadManager.TryGetBroadObjectOnGridPosition(des, out BroadObject broadObject))
                {
                    if (broadObject is IDamageable damageable)
                    {
                        damageable.TakeDamage(1);
                    }
                }
            }
        };
    }

    private void Update()
    {
        State.UpdateState();    
    }

    #region State
    class PlayerMoveState : ICharacterState
    {
        Player Player { get; }
        Vector3 From { get; }
        Vector3 To { get; }
        float LerpDuration { get; }

        public PlayerMoveState(Player player,Vector3 from,Vector3 to,float lerpDuration)
        {
            Player = player;
            From = from;
            To = to;
            LerpDuration = lerpDuration;
        }

        IEnumerator MoveIE()
        {
            float timeElapsed = 0;
            while (timeElapsed < LerpDuration)
            {
                Player.transform.position = Vector3.Lerp(From, To, timeElapsed / LerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            Player.transform.position = To;
            Debug.Log("you are at : " + (Vector2Int)Player.GridPosition);
            Player.State = Player.IdleState;
        }


        public void StartState()
        {
            Player.StartCoroutine(MoveIE());
        }

        public void UpdateState()
        {

        }

        public void EndState()
        {

        }
    }
    #endregion
}
