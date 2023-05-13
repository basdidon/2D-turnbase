using UnityEngine;

public interface IDamageable
{
    public int Hp { get; set; }
    public void TakeDamage(int damage);
}

public class BoardObject : MonoBehaviour
{
    protected BoardManager BoardManager { get { return BoardManager.Instance; } }
    public Vector3Int GridPosition { get { return BoardManager.GetGridPosition(this); } }
    public Vector3 CellCenterWorld { get { return BoardManager.GetCellCenterWolrd(this); } }

}
