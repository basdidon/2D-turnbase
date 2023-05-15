using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn { PlayerTurn, EnemyTurn }

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [SerializeField] Turn currentTrun;
    public Turn CurrentTurn { get { return currentTrun; } set { currentTrun = value; } }

    [SerializeField] List<Enemy> enemies;
    public List<Enemy> Enemies { get { return enemies; } }
    [SerializeField] Queue<Enemy> enemiesQueue;
    public Queue<Enemy> EnemiesQueue { get { return enemiesQueue; } private set { enemiesQueue = value; } }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        EnemiesQueue = new Queue<Enemy>();
    }

    private void Start()
    {
        Enemies.AddRange(FindObjectsOfType<Enemy>());
        CurrentTurn = Turn.PlayerTurn;
        Player.Instance.OnStartTurn();
    }

    public void NextTurn()
    {
        Character nextCharacter;
        if(CurrentTurn == Turn.PlayerTurn)
        {
            CurrentTurn = Turn.EnemyTurn;
            
            foreach(var e in Enemies)
            {
                EnemiesQueue.Enqueue(e);
            }

            nextCharacter = EnemiesQueue.Dequeue();
        }
        else // if(CurrentTurn == Turn.EnemyTurn)
        {
            if (EnemiesQueue.TryDequeue(out Enemy enemy))
            {
                nextCharacter = enemy;
            }
            else
            {
                CurrentTurn = Turn.PlayerTurn;
                nextCharacter = Player.Instance;
            }  
        }


        if(nextCharacter == null)
        {
            NextTurn();
        }
        else
        {
            nextCharacter.OnStartTurn();
        }
    }
}
