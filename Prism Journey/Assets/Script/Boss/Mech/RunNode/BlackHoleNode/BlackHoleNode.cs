using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleNode : BossAttackBaseNode<BlackHoleAttackConfig, BlackHoleRuntime>
{
    public BlackHoleNode(BossContext context, BlackHoleAttackConfig data) : base(context, data)
    {
    }
    protected override void OnEnter()
    {
        Debug.Log("Enter BlackHole NOde ");
        AdvancePhrase(AttackPhrase.StartAnimation);// Set Phrase Timer = 0 and currentPhrase to next pointing
    }
    protected override void OnStartAnimation()
    {
        //If have animaiton reference then play aniamtion 
        if (context.mechAnimation != null)
        {
            context.mechAnimation.SetCurrentAniamitonStage(BossAnimationStage.Attack);
            Debug.Log("aniamiton played for BlackHole node");
        }
        //Move to Next Phrase After call
        AdvancePhrase(AttackPhrase.WaitAnimation);
    }
    protected override NodeState OnWaitingAnimation()
    {
        PhraseTimer();
        //Check Did you have Data and runtime Data if no then return fail 
        if (data == null || runtimeData == null)
        {
            return NodeState.Failure;
        }
        context.mechAnimation.SetCurrentAniamitonStage(BossAnimationStage.Idle);
        if (runtimeData.PhraseTimer > data.animationDuration)
        {
            AdvancePhrase(AttackPhrase.SpawnTelegraph);
        }
        return NodeState.Running;
    }
    protected override NodeState OnSpawnTelegraph()
    {
        // go to clean up phrase if no prefab and  runtimeData accidently get come thing before real objcet
        if (context.bubbleAttackData.telegraphPrefab == null && runtimeData.telegraphObject != null)
        {
            FailToCleanup("Telegraph prefab missing and  runtimeData didnt clean after loop");
        }

        //Spawn the telegraph and get the reference of the telegraph control script
        for (int i = 0; i < data.spwanAmount; i++)
        {

            if (context.blackHoleAttackData == null)
            {
                FailToCleanup("BlackHole Local Data Reference Storage are missing on context  please go to attach the  data");
            }

            runtimeData.telegraphObject = UnityEngine.GameObject.Instantiate(context.blackHoleAttackData.telegraphPrefab);
            runtimeData.telegraph = runtimeData.telegraphObject.GetComponent<ITelegraph>();

            if (runtimeData.telegraph != null)
            {
                runtimeData.telegraph.Initialize(context.TelegraphSpawnPosition.position, Quaternion.identity);
                runtimeData.telegraph.SetDimensions(context.remoteConfig.blackHoleAttackConfig.telegraphWidth, context.remoteConfig.blackHoleAttackConfig.telegraphLength);
                runtimeData.telegraph.SetFill(0);
                
            }

        }
        AdvancePhrase(AttackPhrase.TrackTarget);
        return NodeState.Running;
    }
    protected override NodeState OnTrackTarget()
    {
        PhraseTimer();
        if(runtimeData.telegraphObject == null || runtimeData.telegraph == null)
        {
            FailToCleanup("TelegraphObject or TelegrahController are missing in this point");
        }

        if (runtimeData.PhraseTimer < data.trackingDuration)
        {
            
                if (runtimeData.telegraph != null)
                {
                    Vector3 pos = context.player.transform.position;
                    pos.y += 0.15f;
                    runtimeData.telegraph.MoveToward(pos, data.trackingSpeed);
                
                }
            return NodeState.Running;
        }
        AdvancePhrase(AttackPhrase.Fill);
        return NodeState.Running;
    }
    protected override NodeState OnFillTelegraph()
    {
        if (runtimeData.telegraphObject == null || runtimeData.telegraph == null)
        {
            FailToCleanup("TelegraphObject or TelegrahController are missing in this point");
        }

        PhraseTimer();
        float percent = Mathf.Clamp01(runtimeData.PhraseTimer / data.fillDuration);
        runtimeData.telegraph.SetFill(percent);

        if (runtimeData.PhraseTimer > data.fillDuration)
        {
            AdvancePhrase(AttackPhrase.SpawnAttack);
        }

        return NodeState.Running;
    }
    protected override NodeState OnSpawnAttack()
    {
        SpawnAttack();
        removeTelegraph();
        AdvancePhrase(AttackPhrase.AttackActive);
        return NodeState.Running;
    }

    protected override NodeState OnAttackActive()
    {
        bool isAttackFinish=false;

        if (runtimeData.attackObject != null)
        {
            BlackHoleZone hole = runtimeData.attackObject.GetComponent<BlackHoleZone>();
            if (hole != null)
            {
                if (hole.CurrentStage() == BlackHoleStage.Finish)
                {
                    isAttackFinish = true;
                }
            }
        }

        if (isAttackFinish)
        {
            AdvancePhrase(AttackPhrase.Cleanup);
        }
       return NodeState.Running;
    }







    public void SpawnAttack()
    {
        if(context.blackHoleAttackData== null)return;
        if (runtimeData.telegraph == null || runtimeData.telegraphObject==null) return;
        if (runtimeData.attackObject != null || runtimeData.attackActor != null) return;


        runtimeData.attackObject = UnityEngine.GameObject.Instantiate(context.blackHoleAttackData.actorPrefab[0], runtimeData.telegraph.Transform.position, Quaternion.identity);
        runtimeData.attackActor =runtimeData.attackObject.GetComponent<IAttackActor>();

        Vector3 scale = new Vector3(context.remoteConfig.blackHoleAttackConfig.telegraphWidth / 3, context.remoteConfig.blackHoleAttackConfig.telegraphWidth / 3, context.remoteConfig.blackHoleAttackConfig.telegraphWidth / 3);
        runtimeData.attackActor.Initialize(runtimeData.telegraph.Transform.position, Quaternion.identity);
        runtimeData.attackActor.SetDimension(scale);

       

    }

    public void removeTelegraph()
    {
        if (runtimeData.telegraph == null || runtimeData.telegraphObject==null)return;

        runtimeData.telegraph.Cleanup();
        runtimeData.telegraphObject = null;
    }

    private NodeState FailToCleanup(string message)
    {
        Debug.LogError("[BlackHoleNode] " + message);
        AdvancePhrase(AttackPhrase.Cleanup);
        return NodeState.Running;
    }
}
