using UnityEngine;

public class BroadObject : MonoBehaviour{
    public Vector3Int GridPosition { get { return BroadManager.Instance.GetGridPosition(this); } }
    public Vector3 CellCenterWorld { get { return BroadManager.Instance.GetCellCenterWolrd(this); } }

    public void SetObjectToWorldPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }

    /*
    public void Move(Vector2 direction)
    {
        Debug.Log("Move to " + direction);
        Vector3Int newCellPosition = groundTileMap.WorldToCell(transform.position + (Vector3)direction);

        if (CanMove(newCellPosition))
        {
            State = new PlayerMoveState(this, transform.position, groundTileMap.GetCellCenterWorld(newCellPosition), moveTime);
        }
    }
    */
}
