using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SphereSweepNode : BossAttackBaseNode<SphereSweepConfig, SphereAttackRuntimeData>
{
    public SphereSweepNode(BossContext context, SphereSweepConfig data) : base(context, data)
    {
    }

   

    protected override void  OnEnter()
    {
        Debug.Log("Enter SphereSweep ");
        AdvancePhrase(AttackPhrase.StartAnimation);// Set Phrase Timer = 0 and currentPhrase to next pointing
    }
    protected override void OnStartAnimation()
    {
        //Call the Animation if boos has Animation
        if (context.mechAnimation != null)
        {
            context.mechAnimation.PlayerSphereSweepAttack();

        }
        //Move to Next Phrase After call
        AdvancePhrase(AttackPhrase.WaitAnimation);
    }
    protected override NodeState OnWaitingAnimation()
    {
        PhraseTimer();// Increase Value of timer with time.deltatime
        
                
        //Check Did you have Data and runtime Data if no then return fail 
        if(data==null ||runtimeData==null )
        {
            return NodeState.Failure;
        }

        //Move to next Phrase when time reach the duration time 
        if (runtimeData.PhraseTimer > data.animationDuration)
        {
           
            AdvancePhrase(AttackPhrase.SpawnTelegraph);
            context.mechAnimation.PlayEmpty();
        }

        // Animation Phrase back to Empty
        if (context.mechAnimation != null)
        {
            context.mechAnimation.PlayEmpty();
        }

        //Loop On this Function until Success
        return NodeState.Running;
    }

    protected override NodeState OnSpawnTelegraph()
    {
        //This Prefab Has to use from the local Data 
        //data did not contain the prefab move to the cleanup phrase 
        if (context.sphereSweepData.telegraphPrefab == null)
        {
            FailToCleanup("sphereSweepData Telegraph Prefab are missing");
        }

        runtimeData.TelegraphObject = UnityEngine.GameObject.Instantiate(context.sphereSweepData.telegraphPrefab, context.TelegraphSpawnPosition.position, Quaternion.identity);
        
        runtimeData.Telegraph = runtimeData.TelegraphObject.GetComponentInParent<ITelegraph>();
        runtimeData.Telegraph.Initialize(context.TelegraphSpawnPosition.position, Quaternion.identity);

        //Interface Not found go to Cleanup to skip phrase right after 
        if (runtimeData.Telegraph == null) { return FailToCleanup("Telegraph prefab does not implement ITelegraph."); } 

        runtimeData.Telegraph.SetDimensions(data.telegraphWidth,data.telegraphLength);
        runtimeData.Telegraph.SetRotation(-context.bossTransform.forward);
        runtimeData.Telegraph.SetFill(0f);
            
 
        AdvancePhrase(AttackPhrase.TrackTarget);
        return NodeState.Running;
    }

    protected override NodeState OnTrackTarget()
    {
        // Move to cleanUp if telegraph has no reference
        if (runtimeData.Telegraph == null) { return FailToCleanup("Telegraph Reference missing during Track"); }

        PhraseTimer();
        Vector3 target = new Vector3(context.player.position.x * context.bossTransform.forward.x, context.player.position.y * context.bossTransform.forward.y, context.player.position.z * context.bossTransform.forward.z); //Find targetPosition
        target.y += 0.5f;
        runtimeData.Telegraph.MoveToward(target,data.trackingSpeed); //Tell the Telegraph move to target 

        // Move to next phrase when reach TrackingTime Duration
        if (runtimeData.PhraseTimer > data.trackingDuration)
        {
            AdvancePhrase(AttackPhrase.Fill);
        }

        return NodeState.Running;
    }
    protected override NodeState OnFillTelegraph()
    {
        if(runtimeData.Telegraph == null) { return FailToCleanup("Telegraph missing during charge"); }

        PhraseTimer();
        float percent = Mathf.Clamp01(runtimeData.PhraseTimer / data.fillDuration); // calculate the current percent the fill reach  and set to 1 if it over max value
        runtimeData.Telegraph.SetFill(percent);

        if (runtimeData.PhraseTimer >= data.fillDuration)
        {
           AdvancePhrase(AttackPhrase.SpawnAttack);
        }

        return NodeState.Running;
    }

    protected override NodeState OnSpawnAttack()
    {
        // Telegraph missing? Cleanup.
        if (runtimeData.Telegraph == null)
            return FailToCleanup("Telegraph missing before attack spawn.");

        // Actor prefab missing? Cleanup.
        if (context.sphereAttackPrefab .Length==0)return FailToCleanup("Sphere actor prefab missing.");

        //Spawn attack to correct position 
        //Will only call Once
        if (runtimeData.AttackObject == null)
        {
            runtimeData.leftEdge=runtimeData.Telegraph.GetLeftEdge(context.bossTransform.up);
            runtimeData.rightEdge=runtimeData.Telegraph.GetRightEdge(context.bossTransform.up);
            runtimeData.leftEdge.y += data.actorHeightOffset;
            runtimeData.rightEdge.y += data.actorHeightOffset;

            int index =0;
            if (context.sphereSweepData.sphereActorPrefab.Length > 0) { index = UnityEngine.Random.Range(0, context.sphereSweepData.sphereActorPrefab.Length); } // if prefab is not null get an random from the length 
            runtimeData.AttackObject = UnityEngine.GameObject.Instantiate(context.sphereSweepData.sphereActorPrefab[index], runtimeData.rightEdge, Quaternion.identity); // spawn Attack According to the index on telegraph Edge Position
            runtimeData.AttackActor=runtimeData.AttackObject.GetComponentInParent<IAttackActor>();
            runtimeData.AttackActor.Transform.localScale = Vector3.one;
            runtimeData.AttackActor.Initialize(runtimeData.rightEdge, Quaternion.identity);
        }

        PhraseTimer();
  

        if (runtimeData.PhraseTimer < data.sphereScaleDuration)
        {
           runtimeData.AttackActor.SetScaleOverTime(data.sphereScaleSpeed);
            return NodeState.Running;
        }

        AdvancePhrase(AttackPhrase.AttackActive);
        return NodeState.Running;
    }
    protected override NodeState OnAttackActive()
    {
        //AttackActor 
        if (runtimeData.AttackActor == null)
        {
            FailToCleanup("OnAttackActive  AttackActor  object is missing ");
           
        }
        if (runtimeData.AttackActor.HasReached(runtimeData.leftEdge, 0.05f))
        {
            runtimeData.AttackActor.Cleanup();
            runtimeData.Telegraph.Cleanup();
            AdvancePhrase(AttackPhrase.Cleanup);
        }

        return NodeState.Running;
    }
    protected override NodeState OnCleanUp()
    {
        return base.OnCleanUp();
    }


    
    private NodeState FailToCleanup(string message)
    {
        Debug.LogError("[SphereSweepAttackNode] " + message);
        AdvancePhrase(AttackPhrase.Cleanup);
        return NodeState.Running;
    }
}
