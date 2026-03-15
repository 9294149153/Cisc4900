using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState 
{
    // Start is called before the first frame update
    void Exit(EnemyAIBase enemy);
    void Tick(EnemyAIBase enemy);
    void Enter(EnemyAIBase enemy);
}
