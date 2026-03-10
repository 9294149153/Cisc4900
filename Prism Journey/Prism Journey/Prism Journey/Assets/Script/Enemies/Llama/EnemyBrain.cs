using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyBrain : MonoBehaviour
{
    private NavMeshAgent agent;


    


    [Header("Drag state scripts here")]
    [SerializeField] private MonoBehaviour[] stateScripts;

    [SerializeField] private Transform player;

    private Dictionary<EnemyState, IEnemyStates> states = new();

    private IEnemyStates iCurrentState;



    private void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();


        foreach (var script in stateScripts)
        {
            if (script is IEnemyStates state)
            {
                states[state.StateType] = state;
            }
        }
    }
    private void Start()
    {
        iCurrentState = states[0];
        iCurrentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        iCurrentState?.Exit();

        iCurrentState = states[newState];

        iCurrentState.Enter();
    }

    /*
        void OnDrawGizmosSelected()
        {
            // Detect range (yellow)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRange);

            // Melee range (red)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, meleeRange);
        }*/
}
