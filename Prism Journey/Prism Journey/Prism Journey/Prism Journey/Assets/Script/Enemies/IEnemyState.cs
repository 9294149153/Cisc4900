using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState 
{
    public void Eixst(EnemyStateManager enemy);
    public void Tick (EnemyStateManager enemy);

    public void Enter(EnemyStateManager enemy);
}
