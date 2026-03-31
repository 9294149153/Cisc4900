using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SphereSweepAttackNode : Node
{
    private BossContext context;
    private bool isRunning;

    private bool started; // did node Start
    
    private bool spheresStarted; // did the attack Spawn

    [Header("Telegraph Track and lockPosition")]
    private Vector3 lockedTargetPosition;
    private float trackingtimer; // need to reset after attack
    private bool lockedPosition; // did the playerlastPosition lock

    [Header("Fill the telegraph")]
    float fillTimer ; // need to reset after attack

    [Header("SphereScaleCountdown")]
    private float sizeTimer; // need to reset the value when end


    [Header("Spawn && Refference")]
    private GameObject telegraphReference;
    private RectangleTelegrahVisual telegraphVisual; //  read the scipt the telegraph spawn
    private GameObject sphereAttack;
    private SphereAttackControl sphereAttackControl;

    private Vector3 targetPos;


    private float animationTimer;
    public SphereSweepAttackNode(BossContext bossContext)
    {
        context = bossContext;
    }
    public override NodeState Evaluate()
    {
        //  if disabled → skip
        if (!context.sphereSweepEnabled) return NodeState.Failure;

        //  if another attack running → do nothing
        if (context.isAttackRunning && !isRunning)  return NodeState.Failure;

        // Phase 1: first time entering node

        if (!isRunning)
        {
            Debug.Log("SphereAttack Start");
            context.isAttackRunning = true;
            isRunning = true;
        }

            if (!started)
        {
            StartAttack();
            context.mechAnimation.PlayerSphereSweepAttack();
            return NodeState.Running;
        }

        if (animationTimer < context.sphereAnimationDuration)
        {
            animationTimer += Time.deltaTime; 
            return NodeState.Running;
        }

        context.mechAnimation.PlayEmpty();


        // Phase 2: Spawn the Telegraph and set the correct position
        //only call once
        if (telegraphReference== null)
        {
            telegraphReference = SpawnTelegraph();

            telegraphVisual = telegraphReference.GetComponent<RectangleTelegrahVisual>();
            if (telegraphVisual != null)
            {
                telegraphVisual.SetRotation(-context.bossTransform.forward);
                telegraphVisual.Setup(context.telegraphWidth, context.telegraphLength); // (width , length)
                telegraphVisual.SetFillPercent(0f);
                
            }
            return NodeState.Running;
        }


        //Phrease3 Telegraph track player position and move toward it
        if (trackingtimer < context.telegraphTrackingDuration)
        {
            trackingtimer += Time.deltaTime;

            Vector3 toPlayer = PlayerForwardAxis(context.bossTransform.forward);
            toPlayer.y += 0.05f;

            telegraphVisual.MoveToward(toPlayer, context.telegraphTrackingSpeed);

            return NodeState.Running;
        }

        //only call once before this node completely finish
        if (!lockedPosition)
        {
            lockedPosition= true;
            lockedTargetPosition = telegraphReference.transform.position;
            fillTimer = 0f;
        }

        // Phrase4: fill the telegprah
        
            if(fillTimer < context.fillDuration)
            {
                fillTimer += Time.deltaTime;

                float percent = Mathf.Clamp01(fillTimer / context.fillDuration); // calculate the current percent the fill reach  and set to 1 if it over max value

                if (telegraphVisual != null)
                {
                    telegraphVisual.SetFillPercent(percent);
                }

            return NodeState.Running;
        }


                
        //Phrase 5 Spawn the attack 

        Vector3 leftedge = telegraphVisual.GetLeftEdgeOfTelegraph(context.bossTransform.up);
        leftedge.y += 1.5f;
        Vector3 rightedge = telegraphVisual.GetRightEdgeOfTelegraph(context.bossTransform.up);
        rightedge.y += 1.5f;

        if (!spheresStarted)
        {
            int index = 0;
            if (context.sphereAttackPrefab != null)
            {
                index = UnityEngine.Random.Range(0, context.sphereAttackPrefab.Length);
            }   
            sphereAttack = GameObject.Instantiate(context.sphereAttackPrefab[index], leftedge, quaternion.identity);
            sphereAttackControl=sphereAttack.GetComponent<SphereAttackControl>();
            spheresStarted = true;
            return NodeState.Running ;
        }

        if(sizeTimer < context.sizeTimerMax)
        {
            
            sizeTimer += Time.deltaTime;
            if (sphereAttackControl == null)
            {
                Debug.LogError("SphereAttackControl missing!");
                return NodeState.Failure;
            }
            sphereAttackControl.SetSphereAttackScale(context.sphereScaleSpeed);
            return NodeState.Running;
        }
        sphereAttackControl.MoveToTarget(rightedge,context.sphereAttackMoveSpeed);

        //Check did the attack Finish

        if (Vector3.Distance(sphereAttack.transform.position, rightedge) < 0.05)
        {
            context.isAttackRunning = false;
            context.sphereSweepEnabled = true; // disable after use (optional)
            isRunning = false;
            EndAttack();
            return NodeState.Success;
          
        }

        return NodeState.Running;
    }

    private void StartAttack()
    {
        started = true;
        lockedPosition = false;
        spheresStarted = false;
        trackingtimer = 0f;
        sizeTimer = 0f;
        animationTimer=0f;

    }

    //reset refference for next loop of sequence 
    private void EndAttack()
    {
        if (telegraphReference != null)GameObject.Destroy(telegraphReference);

        if (sphereAttack != null) GameObject.Destroy(sphereAttack);
        
        ResetNodeState();
    }

    private void ResetNodeState()
    {
        started = false;
        isRunning = false;

        animationTimer = 0f;
        trackingtimer = 0f;
        fillTimer = 0f;
        sizeTimer = 0f;

        lockedPosition = false;
        spheresStarted = false;

        telegraphReference = null;
        telegraphVisual = null;
        sphereAttack = null;
        sphereAttackControl = null;
    }

    private GameObject SpawnTelegraph()
    {
        Vector3 pos;
        if (context.TelegraphSpawnPosition != null)
        {
             pos = context.TelegraphSpawnPosition.position;
        }
        else
        {
            pos =context.bossTransform.position;
        }
            GameObject telegraphObject = GameObject.Instantiate(
            context.rectangleTelegrapgPrefab,
            pos,
            Quaternion.Euler(0, 0, 0));
        return telegraphObject;
    }

    private Vector3 PlayerForwardAxis(Vector3 forward)
    {
        if(context.player == null)
        {
            Debug.LogError($"[BossContext]| {context.player} + refference Missing");
        }
        Vector3 pos = new Vector3(context.player.position.x*forward.x,context.player.position.y*forward.y,context.player.position.z*forward.z);
        return pos;
    }


}



