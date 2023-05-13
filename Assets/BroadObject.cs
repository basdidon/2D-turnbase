using UnityEngine;

public interface IDamageable
{
    public int Hp { get; set; }
    public void TakeDamage(int damage);
}

public class BroadObject : MonoBehaviour
{
    protected BroadManager BroadManager { get { return BroadManager.Instance; } }
    public Vector3Int GridPosition { get { return BroadManager.GetGridPosition(this); } }
    public Vector3 CellCenterWorld { get { return BroadManager.GetCellCenterWolrd(this); } }

}
