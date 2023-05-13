using UnityEngine;

public class Character : BroadObject
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
                BroadManager.RemoveObject(this);
                Destroy(this.gameObject);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        Hp -= damage;
    }

    protected virtual void Start()
    {
        BroadManager.AddObject(this, transform.position);

        State = IdleState;
    }

    #region State
    ICharacterState state;
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
