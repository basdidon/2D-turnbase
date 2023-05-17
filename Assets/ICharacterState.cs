using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public interface ICharacterState
{
    public void StartState();
    public void UpdateState();
    public void EndState();
}

class MoveState : ICharacterState
{
    Character Character { get; }
    float LerpDuration { get; }

    public MoveState(Character character, float lerpDuration)
    {
        Character = character;
        LerpDuration = lerpDuration;
    }

    IEnumerator MoveIE()
    {
        Vector3 start = Character.transform.position;
        Vector3 des = Character.CellCenterWorld;
        float timeElapsed = 0;
        while (timeElapsed < LerpDuration)
        {
            Character.transform.position = Vector3.Lerp(start, des, timeElapsed / LerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Character.transform.position = des;
        Debug.Log(Character.name+ " are at : " + (Vector2Int) Character.GridPosition);
        Character.State = Character.IdleState;
    }


    public void StartState()
    {
        Character.StartCoroutine(MoveIE());
    }

    public void UpdateState()
    {

    }

    public void EndState()
    {

    }
}