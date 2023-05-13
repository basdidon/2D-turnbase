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

        if (broadObjectGridPosition.ContainsKey(broadObject))
            Debug.LogError("BroadObject is already in the list");

        if (!CanMove(gridPosition))
            Debug.LogError("initPosition is already used");

        broadObjectGridPosition.Add(broadObject, gridPosition);
        Debug.Log(broadObject.ToString()+" Start at : " + gridPosition);
        // move player to cell potion
        broadObject.SetObjectToWorldPosition(groundTileMap.GetCellCenterWorld(gridPosition));
    }
    public void RemoveObject(BroadObject broadObject)
    {
        broadObjectGridPosition.Remove(broadObject);
    }

    public void MoveObject(BroadObject broadObject, Vector3Int destination)
    {
        // update gridposition in dict
        // each charecter have a many way to move such as jump, run, blink
        // we will handle it later
        if (CanMove(destination))
            broadObjectGridPosition[broadObject] = destination;

    }
    
}
