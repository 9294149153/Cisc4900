using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBrain : MonoBehaviour
{
   

    private EnemyContext context;

    


    [Header("Drag state scripts here")]
    [SerializeField] private MonoBehaviour[] stateScripts;


    [SerializeField] private Transform player;

    private Dictionary<EnemyState, IEnemyStates> states = new();

    private IEnemyStates iCurrentState;
   
    private void Awake()
    {
     
        if(context == null) context = GetComponent<EnemyContext>();

        if (stateScripts.Length > 0)
        {
            foreach (var script in stateScripts)
            {
                if (script is IEnemyStates state)
                {
                    states[state.StateType] = state;
                }
            }
        }
       

      
    }
    private void Start()
    {
        iCurrentState = states[EnemyState.Idle];
        iCurrentState.Enter();
    }

    private void Update()
    {
        iCurrentState.Tick();
    }

    public void ChangeState(EnemyState newState)
    {
        iCurrentState?.Exit();

        iCurrentState = states[newState];

        iCurrentState.Enter();
    }

   

    
     void OnDrawGizmosSelected()
        {
        if (context == null || context.enemyConfig == null) return;

        // Chase in Detect range (yellow)
        Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, context.enemyConfig.inChaseRange);

            // Chase out range (red)
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, context.enemyConfig.outChaseRange);

        // 
       

       


    }
}
