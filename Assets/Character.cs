using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Character : BoardObject
{
    [SerializeField] int hp = 1;
    public int Hp {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                
                Destroy(this.gameObject);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        Hp -= damage;
    }

    [SerializeField] int actionPoint = 1;
    [SerializeField] int remainActionPoint;
    [SerializeField] protected float moveTime = 0.2f;

    protected virtual void Start()
    {
        BoardManager.AddObject(this, transform.position);

        State = IdleState;
    }

    protected virtual void OnDestroy()
    {
        BoardManager.RemoveObject(this);
    }

    #region Turn
    public virtual void OnStartTurn()
    {
        remainActionPoint = actionPoint;
    }

    public virtual void ConsumeActionPoint()
    {
        remainActionPoint--;
        if(remainActionPoint <= 0)
        {
            OnEndTurn();
        }
    }

    [Button]
    public virtual void OnEndTurn()
    {
        TurnManager.Instance.NextTurn();
        Debug.Log(this.gameObject.name + " EndTurn()");
    }
    #endregion
    #region State
    [OdinSerialize] ICharacterState state;
    public ICharacterState State
    {
        get { return state; }
        set
        {
            state?.EndState();
            state = value;
            state.StartState();
        }
    }
    public ICharacterState IdleState = new CharacterIdleState();
    
    class CharacterIdleState : ICharacterState
    {
        public CharacterIdleState()
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
    #endregion

}
