using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState
{
    public void StartState();
    public void UpdateState();
    public void EndState();
}