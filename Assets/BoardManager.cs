using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class BoardManager : SerializedMonoBehaviour
{ 
    public static BoardManager Instance { get; private set; }

    [BoxGroup("TileMap"),OdinSerialize, ShowInInspector, Required] public Tilemap GroundTileMap { get; private set; }
    [BoxGroup("TileMap"),OdinSerialize, ShowInInspector, Required] public Tilemap ColliderTileMap { get; private set; }
    [BoxGroup("TileMap"),OdinSerialize, ShowInInspector, Required] public Tilemap OverlayTilemap { get; private set; }

    public TileBase focusTile;
    public List<Vector3Int> directionalMoves;

    float updateFucusCellTime = 0.5f;
    float updateFucusCellTimeElapsed = 0f;

    [OdinSerialize] readonly Dictionary<BoardObject, Vector3Int> broadObjectGridPosition;
    [OdinSerialize] Vector3Int focusCell;
    public Vector3Int FocusCell {
        get { return focusCell; }
        set { 
            OverlayTilemap.ClearAllTiles();
            focusCell = value;
            OverlayTilemap.SetTile(focusCell, focusTile);
        } 
    }

    public Vector3Int GetGridPosition (BoardObject broadObject)
    {
        return broadObjectGridPosition[broadObject]; 
    }

    public Vector3 GetCellCenterWolrd (BoardObject broadObject)
    {
        return GroundTileMap.GetCellCenterWorld(GetGridPosition(broadObject));
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public bool IsFreeTile(Vector3Int cellPosition)
    {
        return GroundTileMap.HasTile(cellPosition) && !ColliderTileMap.HasTile(cellPosition) && !broadObjectGridPosition.ContainsValue(cellPosition);
    }

    public void AddObject(BoardObject broadObject,Vector3 worldPosition)
    {
        Vector3Int gridPosition = GroundTileMap.WorldToCell(worldPosition);

        if (!IsFreeTile(gridPosition))
            Debug.LogError("initPosition is not avariable");

        if (broadObjectGridPosition.TryAdd(broadObject, gridPosition))
        {
            Debug.Log(broadObject.ToString() + " Start at : " + gridPosition);
            // move player to cell potion
            broadObject.transform.position = broadObject.CellCenterWorld;
        }
        else
        {
            Debug.LogError("BoardObject is already in the list");
        }
    }

    public void RemoveObject(BoardObject broadObject)
    {
        broadObjectGridPosition.Remove(broadObject);
    }
    /*
    public bool TryDirectionalMove(BoardObject boardObject,Vector3Int direction)
    {
        return TryMoveObject(boardObject, boardObject.GridPosition + direction);
    }*/

    public bool TryMoveObject(BoardObject boardObject, Vector3Int destination)
    {
        // update gridposition in dict
        // each charecter have a many way to move such as jump, run, blink
        // we will handle it later
        if (IsFreeTile(destination))
        {
            broadObjectGridPosition[boardObject] = destination;
            return true;
        }

        return false;
    }

    public bool TryGetBoardObjectOnGridPosition(Vector3Int gridPosition, out BoardObject broadObject)
    {
        broadObject = null;

        foreach (var pair in broadObjectGridPosition)
        {
            if(pair.Value == gridPosition)
            {
                broadObject = pair.Key;
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if(updateFucusCellTimeElapsed > updateFucusCellTime)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            FocusCell = GroundTileMap.WorldToCell(mouseWorldPos);

            // find the way from 0,0 to focus cell
            directionalMoves = PathFinder.FindDirectionMovePath(Player.Instance.GridPosition, FocusCell);

            Vector3 currentPos = Player.Instance.CellCenterWorld + Vector3.forward * 2;
            for (int i = 0; i < directionalMoves.Count; i++)
            {
                Vector3 nextPos = currentPos + directionalMoves[i];
                Debug.DrawLine(currentPos , nextPos,Color.black,updateFucusCellTime);
                currentPos = nextPos;
            }

            updateFucusCellTimeElapsed = 0f;
        }
        updateFucusCellTimeElapsed += Time.deltaTime;
    }

    // when you move to any direction in game, may be active some event like IceFloor that won't let you stop and keep you going in same direction,
    public bool TryDirectionalMove(Vector3Int startCell,Vector3Int direction,out Vector3Int resultCell)
    {
        resultCell = Vector3Int.zero;

        if (IsFreeTile(startCell+direction))
        {
            resultCell = startCell + direction;
            return true;
        }
        return false;
    }
}