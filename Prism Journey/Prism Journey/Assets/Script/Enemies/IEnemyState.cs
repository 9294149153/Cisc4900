using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState 
{

    

    public void Exit(EnemyAIBase enemy);
    public void Tick (EnemyAIBase enemy);

    public void Enter(EnemyAIBase enemy);
}
