using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{ 
    public static BoardManager Instance { get; private set; }
    [SerializeField] Tilemap groundTileMap;
    [SerializeField] Tilemap colliderTileMap;
    [SerializeField] Tilemap overlayTilemap;

    [SerializeField] TileBase tileBase;
      
    [SerializeField] Dictionary<BoardObject, Vector3Int> broadObjectGridPosition = new Dictionary<BoardObject, Vector3Int>();
    [SerializeField] Vector3Int focusCell;
    public Vector3Int FocusCell {
        get { return focusCell; } 
        set { 
            overlayTilemap.ClearAllTiles();
            focusCell = value;
            overlayTilemap.SetTile(focusCell, tileBase);
        } 
    }

    public Vector3Int GetGridPosition (BoardObject broadObject)
    {
        return broadObjectGridPosition[broadObject]; 
    }

    public Vector3 GetCellCenterWolrd (BoardObject broadObject)
    {
        return groundTileMap.GetCellCenterWorld(GetGridPosition(broadObject));
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
        return groundTileMap.HasTile(cellPosition) && !colliderTileMap.HasTile(cellPosition) && !broadObjectGridPosition.ContainsValue(cellPosition);
    }

    public void AddObject(BoardObject broadObject,Vector3 worldPosition)
    {
        Vector3Int gridPosition = groundTileMap.WorldToCell(worldPosition);

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

    public bool TryDirectionalMove(BoardObject boardObject,Vector3Int direction)
    {
        return TryMoveObject(boardObject, boardObject.GridPosition + direction);
    }

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
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        FocusCell = groundTileMap.WorldToCell(mouseWorldPos);
    }

    public List<Vector3Int> GetMoveableDirection(Vector3Int cell)
    {
        List<Vector3Int> moveableDirection = new List<Vector3Int>();
        List<Vector3Int> directions = new List<Vector3Int>() { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        foreach (var direction in directions)
            if (IsFreeTile(cell + direction))
                moveableDirection.Add(direction);

        return moveableDirection;
    }
}