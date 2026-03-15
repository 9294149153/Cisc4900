using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum StateVisual {Idle,Patrol,Chase,Attack }
public class EnemyAi_Blade : EnemyAIBase
{

    private IdleState idle;
    private PatrolState patrol;
    private ChaseState chase;
    private AttackState attack;

    public Transform patrolStartPosition { get; private set; }

    public event EventHandler<OnEnemyVisualChangeArgs> OnEnemyVisualChange;

    public class OnEnemyVisualChangeArgs: EventArgs { public StateVisual stateVisual; }

    protected void Awake()
    {


        if (!agent) agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
        agent.angularSpeed = 360f;

    }

    protected virtual void Start()
    {
        patrolStartPosition = transform;
        patrolCenter = transform.position;
        InitStates();
        SetInitialState();
        OnEnemyVisualChange?.Invoke(this, new OnEnemyVisualChangeArgs { stateVisual = StateVisual.Idle });
    }

    protected override void InitStates()
    {

        idle = new IdleState();
        patrol = new PatrolState();
        chase = new ChaseState();
        attack= new AttackState();
        

        //Assign state Reffernce

        idle.next = patrol;
        patrol.next = chase;
        chase.next = attack;
        chase.exit = patrol;
        attack.exit = chase;
    }
    protected override void SetInitialState()
    {
        SwitchState(idle);
    }

    protected override void Update()
    {
        if (currentState != null)
        {
            currentState.Tick(this);
        }
    }
    public override void SwitchState(IEnemyState next )
    {

        if (next == null) return;

        if (next != null)
        {
            currentState = next;
            currentState.Enter(this);
        }

       

    }








    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = Application.isPlaying ? patrolCenter : transform.position;
        Gizmos.DrawWireSphere(center, patrolRadius);
    }



    // ===================== STATES =====================
    private class IdleState : IEnemyState
    {
        public IEnemyState next;
        private float timer;
        private float timerMax = 2f;

        public void Enter(EnemyAIBase e) { timer = 0f; e.StopAgent(); }
        public void Tick(EnemyAIBase e)
        {
            timer += Time.deltaTime;
            if (timer >= timerMax) e.SwitchState(next);
            Debug.Log("current in Idle State");
        }
        public void Exit(EnemyAIBase e) { }
    }

    private class PatrolState : IEnemyState
    {
     
        public IEnemyState next;
        private float waitTime;
       private EnemyAi_Blade bladeEnemy;

        public void Enter(EnemyAIBase enemy)
        {
            bladeEnemy=(EnemyAi_Blade)enemy;
            bladeEnemy.OnEnemyVisualChange?.Invoke(this, new OnEnemyVisualChangeArgs { stateVisual = StateVisual.Patrol});

            if (enemy.transform.position != enemy.PatrolCenter)
            {
                enemy.Agent.SetDestination(enemy.PatrolCenter);

            }
            Debug.Log("Enter patrol State");
            waitTime = 0f;
            enemy.SetSpeed(4f);
            SetRandom(enemy);
        }

        public void Exit(EnemyAIBase enemy)
        {

        }

        public void Tick(EnemyAIBase enemy)
        {
            //Player in chase Range
            if (enemy.InChaseRange)
            {
                enemy.SwitchState(next);
            }

            //Wait until path calculation finishes
            if (enemy.Agent.pathPending) return;
            if (enemy.Agent.remainingDistance <= enemy.PatrolPointTolerance)
            {
                waitTime += Time.deltaTime;
                if (waitTime >= enemy.PatrolWaitTime) { waitTime = 0; SetRandom(enemy); }
            }
        }
        private void SetRandom(EnemyAIBase enemy)
        {
            Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * enemy.PatrolRadius;//Random.insideUnitCircle = read random point inside a circle:Range: X and Y are between -1 and 1

            Vector3 p = new Vector3(enemy.PatrolCenter.x + randomPoint.x,
                enemy.PatrolCenter.y,
                enemy.PatrolCenter.z + randomPoint.y);

            //Check the next move position are valid  not in wall , not in void ..etc.
            if (NavMesh.SamplePosition(p, out NavMeshHit hit, enemy.PatrolRadius, NavMesh.AllAreas))
                enemy.Agent.SetDestination(hit.position);
        }
    }

    private class ChaseState : IEnemyState
    {
        public IEnemyState next;
        public IEnemyState exit;
        private float awareTimer;
        private float awareTimerMax = 0.5f;
       

        public void Enter(EnemyAIBase enemy)
        {
            Debug.Log("Enter Chase State");

           EnemyAi_Blade bladeEnemy = (EnemyAi_Blade)enemy;
            bladeEnemy.OnEnemyVisualChange?.Invoke(this, new OnEnemyVisualChangeArgs { stateVisual = StateVisual.Chase });


            awareTimer = 0;
            enemy.SetSpeed(enemy.ChaseSpeed);
            enemy.ResumeAgent();
        }

        public void Exit(EnemyAIBase enemy)
        {

        }

        public void Tick(EnemyAIBase enemy)
        {
            Debug.Log("Chasing");

            //player still in chase Zone
            //enemy aware the player in zone 
            //then chase 
            if (enemy.InChaseRange && awareTimer > awareTimerMax)
            {
                enemy.ResumeAgent();
                enemy.Agent.SetDestination(enemy.Player.position);

                //Player Reach the attackZone then go to aim 
                if (enemy.InAttackRange)
                {
                    enemy.SwitchState(next);
                }
            }
            else if (!enemy.InChaseRange)
            {
                enemy.SwitchState(exit);
            }
            else
            {
                awareTimer += Time.deltaTime;
            }
        }
    }

    private class AttackState : IEnemyState
    {
        public IEnemyState exit;
        public void Enter(EnemyAIBase enemy)
        {
            Debug.Log("Attack");
            EnemyAi_Blade bladeEnemy = (EnemyAi_Blade)enemy;
            bladeEnemy.OnEnemyVisualChange?.Invoke(this, new OnEnemyVisualChangeArgs { stateVisual = StateVisual.Attack});

        }

        public void Exit(EnemyAIBase enemy)
        {
            
        }

        public void Tick(EnemyAIBase enemy)
        {
           
        }
    }
}
