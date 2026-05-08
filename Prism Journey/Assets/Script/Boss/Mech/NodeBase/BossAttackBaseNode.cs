using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class BossAttackBaseNode <TData , TRuntime> : Node 
    where TData : class
    where TRuntime : BossAttackRuntimeBase, new()

{
    
    protected readonly BossContext context;
    protected readonly TData data;
    protected readonly TRuntime runtimeData;

    protected BossAttackBaseNode(BossContext context,TData data)
        
    { 
        this.context = context;
        this.data = data;
       this.runtimeData = new TRuntime();
    }

    public override NodeState Evaluate()
    {
        //No Reference , no Comnplie data , no Data Container   Node can not be run  return fail
       if(context==null || data==null || runtimeData == null)
        {
            return NodeState.Failure;
        }

        // current No Other Node are running  && this node has not start
        //Clam and Start to attack
        if (context.isAttackRunning == false && runtimeData.currentPhrase == AttackPhrase.None)
        {
            context.isAttackRunning = true;
            runtimeData.currentPhrase = AttackPhrase.Enter;
            runtimeData.PhraseTimer = 0;
        }
        // If another attack is already running and this node is not started,
        // fail so other attack keeps control.
        else if (context.isAttackRunning && runtimeData.currentPhrase == AttackPhrase.None)
        {
            return NodeState.Failure;
        }

        switch (runtimeData.currentPhrase)
        {
            case AttackPhrase.Enter:
                OnEnter();
                return NodeState.Running;
              
            case AttackPhrase.StartAnimation:
                OnStartAnimation();
                return NodeState.Running;

            case AttackPhrase.WaitAnimation:
                return OnWaitingAnimation();    

            case AttackPhrase.SpawnTelegraph:
                return OnSpawnTelegraph();

            case AttackPhrase.TrackTarget:
                return OnTrackTarget();

            case AttackPhrase.Fill:
                return OnFillTelegraph();

            case AttackPhrase.SpawnAttack:
                return OnSpawnAttack();

            case AttackPhrase.AttackActive: 
                return OnAttackActive();

            case AttackPhrase.Cleanup:
                return OnCleanUp();

            case AttackPhrase.Finished:
                 Finisih();
                return NodeState.Success;
        }


        return NodeState.Failure;
    }


    //Child Must Implement and Customize how funciton work 
    protected abstract void OnEnter();
    protected abstract void OnStartAnimation();
    protected abstract NodeState OnWaitingAnimation();
    protected abstract NodeState OnSpawnTelegraph();
    protected abstract NodeState OnTrackTarget();
    protected abstract NodeState OnFillTelegraph();
    protected abstract NodeState OnSpawnAttack();
    protected abstract NodeState OnAttackActive();
    

    protected virtual NodeState OnCleanUp()
    {

        // Move to finished state.
        runtimeData.currentPhrase = AttackPhrase.Finished;
        return NodeState.Running;
    }
    protected virtual void Finisih()
    {
        // Boss can now allow another attack later.
        context.isAttackRunning = false;

        // Reset runtime data for future reuse.
        runtimeData.Reset();
       
    }

    protected void AdvancePhrase(AttackPhrase nextPhrase)
    {
        runtimeData.currentPhrase = nextPhrase;
        runtimeData.PhraseTimer = 0f;
    }

    protected void PhraseTimer()
    {
        runtimeData.PhraseTimer += Time.deltaTime;
    }
}
