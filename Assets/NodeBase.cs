using System.Collections.Generic;
using UnityEngine;

public class NodeBase
{
    public static BoardManager BoardManager { get { return BoardManager.Instance;} }
    public Vector3Int CellPosition;
    public List<Vector3Int> directionMoveToNode; // like, go left, left, up, left
    public int G { get; set; }  // cumulative cost to this node, In this context mean ActionPoint
    public float H { get; set; }  // cost to targetCell,that ignore all obstacles
    public float F => G + H;

    public NodeBase(Vector3Int cellPosition,int g,Vector3Int targetCell)
    {
        CellPosition = cellPosition;
        G = g;
        H = Mathf.Abs(CellPosition.x - targetCell.x) + Mathf.Abs(CellPosition.y - targetCell.y);
    }

    public NodeBase(Vector3Int cellPosition, int g, Vector3Int targetCell, List<Vector3Int> direcions)
    {
        CellPosition = cellPosition;
        G = g;
        H = Mathf.Abs(CellPosition.x - targetCell.x) + Mathf.Abs(CellPosition.y - targetCell.y);
        directionMoveToNode = direcions;
    }
    /*
    public static void FindDirectionMovePath(Vector3Int startCell, Vector3Int targetCell)
    {
        NodeBase startNode = new NodeBase(startCell,0,targetCell);
        List<NodeBase> toSearch = new List<NodeBase>() { startNode };
        List<NodeBase> processed = new List<NodeBase>();

        while (toSearch.Count > 0)
        {
            NodeBase currentNode = toSearch[0];
            foreach (var t in toSearch)
                if (t.F < currentNode.F || t.F == currentNode.F && t.H < currentNode.H)
                    currentNode = t;

            processed.Add(currentNode);
            toSearch.Remove(currentNode);

            // move 4 direction
            foreach(var moveableDirection in BoardManager.GetMoveableDirection(currentNode.CellPosition))
            {
                var newNodePath = new List<Vector3Int>(currentNode.directionMoveToNode) { moveableDirection };
                toSearch.Add(new NodeBase(currentNode.CellPosition + moveableDirection, currentNode.G + 1, targetCell, newNodePath));
                //**** It include processed
            }
        }
    }
    */
}
