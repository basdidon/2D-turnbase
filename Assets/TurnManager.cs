using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public enum Turn { PlayerTurn, EnemyTurn }

public class TurnManager : SerializedMonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [OdinSerialize] public Character CharacterTurn { get; private set; }
    [OdinSerialize] public Queue<Character> CharactersQueue { get; private set; }

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
    }

    private void Start()
    {
        var enemies = FindObjectsOfType<Enemy>();

        CharacterTurn = Player.Instance;
        Player.Instance.OnStartTurn();
       
        foreach (var e in enemies)
        {
            CharactersQueue.Enqueue(e);
        }
    }

    public void NextTurn()
    {
        Character nextCharacter = CharactersQueue.Dequeue();
        if(nextCharacter == null)
        {
            NextTurn();
        }
        else
        {
            CharactersQueue.Enqueue(CharacterTurn);
            CharacterTurn = nextCharacter;
            CharacterTurn.OnStartTurn();
        }
    }
}
