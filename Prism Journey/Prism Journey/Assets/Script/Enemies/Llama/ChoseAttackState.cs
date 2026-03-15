using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChoseAttackState : MonoBehaviour , IEnemyStates
{
    private EnemyContext enemyContext;

    public EnemyState StateType => EnemyState.ChoseAttack;

    [SerializeField]private List<EnemyState> meleeAttackStates;
    [SerializeField] private List<EnemyState> rangeAttackStates;
    private void Awake()
    {
        if (enemyContext == null) enemyContext = GetComponent<EnemyContext>();
        SetupAttacksForMelee();
        SetupAttacksForRange();
    }

    public void Enter()
    {
      
        enemyContext.anim.GetStateForAnimation(StateType);
        enemyContext.enemyMotor.StopAgent();

        // attackStates List empty
        if (meleeAttackStates.Count == 0 && rangeAttackStates.Count==0)
        {
            enemyContext.enemyBrain.ChangeState(EnemyState.Chase);
            
        }

       

    }

    public void Exit()
    {
        enemyContext.anim.animator.SetBool("ChoseAttack", false);
    }

    public void Tick()
    {


        if (rangeAttackStates.Count != 0 && meleeAttackStates.Count != 0)
        {
            bool inMelee = enemyContext.enemySensor.InDisRange(enemyContext.enemyConfig.meleeRangeRadius);
            bool inRange = enemyContext.enemySensor.InDisRange(enemyContext.enemyConfig.RrangeRaidus);

            if (inMelee && inRange)
            {
                List<EnemyState> randomList = RandomForTwoList(meleeAttackStates, rangeAttackStates);
                enemyContext.enemyBrain.ChangeState(randomList[RandomIndexForTheList(randomList)]);
                return;
            }
            else if (!inMelee && inRange)
            {
                enemyContext.enemyBrain.ChangeState(rangeAttackStates[RandomIndexForTheList(rangeAttackStates)]);
                return;
            }
            else if (inMelee)
            {
                enemyContext.enemyBrain.ChangeState(meleeAttackStates[RandomIndexForTheList(meleeAttackStates)]);
                return;
            }
            else
            {
                enemyContext.enemyBrain.ChangeState(EnemyState.Chase);
                return;
            }
        }






    }

    void SetupAttacksForMelee()
    {
        meleeAttackStates = new List<EnemyState>();

        if (enemyContext.enemyAttackConfig.hasMelee)
            meleeAttackStates.Add(EnemyState.MeleeAttack);


        if (enemyContext.enemyAttackConfig.hasBounce)
            meleeAttackStates.Add(EnemyState.BounceAttack);

    }

    void SetupAttacksForRange()
    {
        rangeAttackStates = new List<EnemyState>();

        if (enemyContext.enemyAttackConfig.hasSpit)
            rangeAttackStates.Add(EnemyState.SpitAttack);

        if (enemyContext.enemyAttackConfig.hasRoll)
            rangeAttackStates.Add(EnemyState.RollAttack);
      
    }

  
     List<EnemyState> RandomForTwoList(List<EnemyState> a , List<EnemyState> b)
    {
        int index = UnityEngine.Random.Range(0, 2);

       if(index == 1)
        {
            return a;
        }
        else
        {
            return b;
        }
    }

    int RandomIndexForTheList(List<EnemyState> list)
    {
        int  listIndex;
        if (list.Count != 0)
        {
            
            listIndex = UnityEngine.Random.Range(0, list.Count);
            return listIndex;
        }
        return -1;
    }

}
