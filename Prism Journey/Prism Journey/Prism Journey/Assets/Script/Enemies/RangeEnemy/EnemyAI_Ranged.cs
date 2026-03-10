using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Ranged : EnemyAIBase
{

    [Header("Ranged Settings")]
    [SerializeField] private float aimTime = 0.6f;
    [SerializeField] private float aimTurnSpeed = 720f;
    [SerializeField] private float preferredShootDistance = 6f; // keep distance
    [SerializeField] private float attackCooldown = 1.2f;

    [Header("Projectile Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform attackSpawnPoint;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float projectileLifeTime = 2f;
    [SerializeField] private Vector3 projectileEulerOffset = new Vector3(90f, 0f, 0f);


    public Transform patrolStartPosition { get; private set; }



    [Header("Telegraph Line")]
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private float lineMaxDistance = 10f;
    [SerializeField] private LayerMask lineHitMask = ~0; // everything
    [SerializeField] private float lineYOffset = 0.1f;   // lift off ground a bit
     private Vector3 lockedTargetPosition;



    // states for ranged
    private IdleState idle;
    private PatrolState patrol;
    private ChaseState chase;
    private AimState aim;
    private AttackState attack;
    


    protected  void Awake()
    {
        

        if (!agent) agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
        agent.angularSpeed = 360f;

        //TeleGraph
       ;
        if (!aimLine) aimLine = GetComponent<LineRenderer>();
        if (aimLine) aimLine.enabled = false;


    }

    protected virtual void Start()
    {
        patrolStartPosition = transform;
        patrolCenter = transform.position;
        InitStates();
        SetInitialState();
    }
    protected override void InitStates()
    {
        idle = new IdleState();
        patrol = new PatrolState();
        chase = new ChaseState();
        aim= new AimState();
        attack = new AttackState();


        //Assign state Reffernce
        idle.next = patrol;
        patrol.next = chase;
        chase.next = aim;
        chase.exit = patrol;
        aim.next = attack;
        aim.exit = patrol;

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
    public override void SwitchState(IEnemyState next)
    {

        if (next == null) return;

        if (next != null)
        {
            currentState = next;
            currentState.Enter(this);
        }




    }
    
    private void DoShoot()
    {
       
        if (!projectilePrefab || !shootPoint) return;

        Vector3 dir = (lockedTargetPosition - shootPoint.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(90f, 0f, 0f);

        Instantiate(projectilePrefab, shootPoint.position, rot);

    }

    public void SetAimLine(bool on)
    {
        if (!aimLine) return;
        aimLine.enabled = on;
    }

    public void UpdateAimLine()
    {
        if (!aimLine || !aimLine.enabled || !shootPoint || !Player)
            return;

        Vector3 start = shootPoint.position;
        Vector3 end = lockedTargetPosition;

        start.y += lineYOffset;
        end.y += lineYOffset;

        aimLine.SetPosition(0, start);
        aimLine.SetPosition(1, end);
    }
    public void LockAimTarget()
    {
        if (!Player) return;

        lockedTargetPosition = Player.position;
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
        private EnemyAI_Ranged rangeEnemy;
        public IEnemyState next;
        private float waitTime;
        EnemyAI_Ranged ranged;
        public void Enter(EnemyAIBase enemy)
        {
            ranged = (EnemyAI_Ranged)enemy;
            ranged.SetAimLine(false);
            if (enemy.transform.position != enemy.PatrolCenter ){
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
                if (waitTime>= enemy.PatrolWaitTime) { waitTime = 0; SetRandom(enemy); }
            }
        }
        private void SetRandom(EnemyAIBase enemy)
        {
            Vector2 randomPoint = Random.insideUnitCircle * enemy.PatrolRadius;//Random.insideUnitCircle = read random point inside a circle:Range: X and Y are between -1 and 1

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
        EnemyAI_Ranged ranged;

        public void Enter(EnemyAIBase enemy)
        {
           Debug.Log("Enter Chase State");
            ranged = (EnemyAI_Ranged)enemy;
            ranged.SetAimLine(false);
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


    private class AimState : IEnemyState
    {
        public IEnemyState next;
        public IEnemyState exit;
        private float aimTimer;
        EnemyAI_Ranged ranged;


        public void Enter(EnemyAIBase enemy)
        {
            Debug.Log("Enter Aim State");
            aimTimer = 0f;
            
            ranged = (EnemyAI_Ranged)enemy;   // because AimState is inside EnemyAI_Ranged you can also store reference

            ranged.LockAimTarget();   // 🔥 lock position once
            ranged.SetAimLine(true);

            enemy.StopAgent();
        }

        public void Exit(EnemyAIBase enemy)
        {
            


        }

        public void Tick(EnemyAIBase enemy)
        {
            Debug.Log(" Aiming ");
            ranged.UpdateAimLine();

          if(aimTimer < 0.5f)
            {
                aimTimer += Time.deltaTime;
                return;
            }

            enemy.SwitchState(next);
            

        }
    }
        
    private class AttackState : IEnemyState
    {
        public IEnemyState exit;
        EnemyAI_Ranged ranged;
        public void Enter(EnemyAIBase enemy)
        {
            ranged = (EnemyAI_Ranged)enemy;
            ranged.DoShoot();
            enemy.SwitchState(exit);
        }

        public void Exit(EnemyAIBase enemy)
        {
           
        }

        public void Tick(EnemyAIBase enemy)
        {
           
        }
    }


}
