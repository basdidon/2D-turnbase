using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] Inputs Inputs { get; set; }
    [SerializeField] Tilemap groundTileMap;
    [SerializeField] Tilemap colliderTileMap;

    ICharacterState state;
    public ICharacterState State {
        get { return state; }
        set { 
            state?.EndState();
            state = value;
            state.StartState();
        }
    }
    public ICharacterState IdleState = new PlayerIdleState();
    private Vector3Int CellPosition { get; set; }

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
                Move(ctx.ReadValue<Vector2>());
        };
    }

    void Start()
    {
        CellPosition = groundTileMap.WorldToCell(transform.position);
        Debug.Log("Player Start at : " + CellPosition);
        // move player to cell potion
        transform.position = groundTileMap.GetCellCenterWorld(CellPosition);

        State = IdleState;
    }



    private void Move(Vector2 direction)
    {
        Debug.Log("Move to " + direction);
        Vector3Int newCellPosition = groundTileMap.WorldToCell(transform.position + (Vector3)direction);

        if (CanMove(newCellPosition))
        {
            State = new PlayerMoveState(this,transform.position, groundTileMap.GetCellCenterWorld(newCellPosition),1f);
        }
    }

    private bool CanMove(Vector3Int newCellPosition)
    {
        return groundTileMap.HasTile(newCellPosition) && !colliderTileMap.HasTile(newCellPosition);
    }

    #region State
    class PlayerIdleState : ICharacterState
    {
        public PlayerIdleState()
        {

        }

        public void StartState()
        {

        }

        public void UpdateState()
        {

        }

        public void EndState()
        {

        }
    }

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
            Debug.Log("you are at : " + To);
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
