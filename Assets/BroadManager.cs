using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BroadManager : MonoBehaviour
{
    public static BroadManager Instance { get; private set; }
    [SerializeField] Tilemap groundTileMap;
    [SerializeField] Tilemap colliderTileMap;

    [SerializeField] Dictionary<BroadObject, Vector3Int> broadObjectGridPosition = new Dictionary<BroadObject, Vector3Int>();
    public Vector3Int GetGridPosition (BroadObject broadObject)
    {
        return broadObjectGridPosition[broadObject]; 
    }
    public Vector3 GetCellCenterWolrd (BroadObject broadObject)
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

    public bool CanMove(Vector3Int newCellPosition)
    {
        return groundTileMap.HasTile(newCellPosition) && !colliderTileMap.HasTile(newCellPosition) && !broadObjectGridPosition.ContainsValue(newCellPosition);
    }

    public void AddObject(BroadObject broadObject,Vector3 worldPosition)
    {
        Vector3Int gridPosition = groundTileMap.WorldToCell(worldPosition);

        if (!CanMove(gridPosition))
            Debug.LogError("initPosition is not avariable");

        if (broadObjectGridPosition.TryAdd(broadObject, gridPosition))
        {
            Debug.Log(broadObject.ToString() + " Start at : " + gridPosition);
            // move player to cell potion
            broadObject.transform.position = broadObject.CellCenterWorld;
        }
        else
        {
            Debug.LogError("BroadObject is already in the list");
        }
    }
    public void RemoveObject(BroadObject broadObject)
    {
        broadObjectGridPosition.Remove(broadObject);
    }

    public bool MoveObject(BroadObject broadObject, Vector3Int destination)
    {
        // update gridposition in dict
        // each charecter have a many way to move such as jump, run, blink
        // we will handle it later
        if (CanMove(destination))
        {
            broadObjectGridPosition[broadObject] = destination;
            return true;
        }

        return false;
    }

    public bool TryGetBroadObjectOnGridPosition(Vector3Int gridPosition, out BroadObject broadObject)
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
    
}
