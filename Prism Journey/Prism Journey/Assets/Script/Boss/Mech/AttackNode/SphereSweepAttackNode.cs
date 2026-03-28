using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

public class SphereSweepAttackNode : Node
{
    private BossContext context;

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

    public SphereSweepAttackNode(BossContext bossContext)
    {
        context = bossContext;
    }
    public override NodeState Evaluate()
    {
        
       
        // Phase 1: first time entering node
        if (!started)
        {
            StartAttack();
            return NodeState.Running;
        }

        // Phase 2: Spawn the Telegraph and set the correct position
        //only call once
        if (telegraphReference== null)
        {
            telegraphReference = SpawnTelegraph();
            telegraphVisual = telegraphReference.GetComponent<RectangleTelegrahVisual>();
            if (telegraphVisual != null)
            {
                telegraphVisual.Setup(context.telegraphWidth, context.telegraphLength); // (width , length)
                telegraphVisual.SetFillPercent(0f);
            }
            return NodeState.Running;
        }


        //Phrease3 Telegraph track player position and move toward it
        if (trackingtimer < context.telegraphTrackingDuration)
        {
            trackingtimer += Time.deltaTime;

            Vector3 toPlayerXonly = new Vector3(
                context.player.transform.position.x,
                0.05f,
                0f
            );

            telegraphReference.transform.position = Vector3.Lerp(
                 telegraphReference.transform.position,
                  toPlayerXonly,
                 Time.deltaTime * context.telegraphTrackingSpeed
             );

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

        Vector3 leftedge = telegraphVisual.GetLeftEdgeWorld();
        leftedge.z = leftedge.z - (float)(context.telegraphLength * 2 + 0.5* context.telegraphLength); 
        Vector3 rightedge = telegraphVisual.GetLeftEdgeWorld();
        rightedge.z = rightedge.z + (float)(context.telegraphLength * 2 + 0.5 * context.telegraphLength);



        if (!spheresStarted)
        {
            sphereAttack = GameObject.Instantiate(context.sphereAttackPrefab, leftedge, quaternion.identity);
           spheresStarted = true;
            return NodeState.Running ;
        }

        if(sizeTimer < context.sizeTimerMax)
        {
            sizeTimer += Time.deltaTime;
           
            sphereAttack.transform.localScale += Vector3.one*Time.deltaTime*context.sphereScalespeed;
            return NodeState.Running;
        }

        sphereAttack.transform.position = Vector3.MoveTowards(
        sphereAttack.transform.position,
        rightedge,
        context.sphereAttackMoveSpeed * Time.deltaTime);

      if(Vector3.Distance(sphereAttack.transform.position, rightedge) < 0.05)
        {
            Debug.Log("error");

            EndAttack();
            NodeColdown.SetColdown(false);
            return NodeState.Running ;
        }
  


        return NodeState.Success;
    }

    private void StartAttack()
    {
        started = true;
        lockedPosition = false;
        spheresStarted = false;
        trackingtimer = 0f;
        sizeTimer = 0f;
}

    private void EndAttack()
    {
        if (telegraphReference != null)
        {
            GameObject.Destroy(telegraphReference);
            telegraphReference = null;

        }
        if(sphereAttack != null)
        {
            GameObject.Destroy(sphereAttack);
            sphereAttack = null;
        }

        started = false;
        lockedPosition = false;
        spheresStarted = false;
        trackingtimer = 0f;
        fillTimer = 0f;
        sizeTimer = 0f;
    }

    private GameObject SpawnTelegraph()
    {
        GameObject telegraphObject = GameObject.Instantiate(
        context.rectangleTelegrapgPrefab,
        context.bossTransform.position,
        Quaternion.Euler(0,0,0)  );

        telegraphObject.transform.rotation = Quaternion.Euler(90, 0, 0);



        return telegraphObject;
    }

    
}



