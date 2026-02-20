using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState 
{
    public void EixstState(EnemyStateManager enemy);
    public void UpdateState(EnemyStateManager enemy);

    public void EnterState(EnemyStateManager enemy);
}
