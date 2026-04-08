using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class LaserNode : BossAttackBaseNode<LaserAttackConfig, LaserAttackRunTime>
{

    public LaserNode(BossContext context, LaserAttackConfig data) : base(context, data)
    {
    }
    protected override void OnEnter()
    {
        Debug.Log("Enter LaserNode ");
        runtimeData.InitData();
        AdvancePhrase(AttackPhrase.StartAnimation);
    }
    protected override void OnStartAnimation()
    {
        //If have animaiton reference then play aniamtion 
        if (context.mechAnimation != null)
        {
            Debug.Log("aniamiton played for Laser node");// has not make it yet just use debug.log for currency
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
        context.mechAnimation.PlayEmpty();
        if (runtimeData.PhraseTimer > data.animationDuration)
        {
            AdvancePhrase(AttackPhrase.SpawnTelegraph);
        }
        return NodeState.Running;
    }   

    protected override NodeState OnSpawnTelegraph()
    {

        // go to clean up phrase if no prefab and  runtimeData accidently get come thing before real objcet
        if (context.laserAttackData.telegraphPrefab == null && runtimeData.TelegraphObject.Count > 0)
        {
            FailToCleanup("Telegraph prefab missing and  runtimeData didnt clean after loop");
        }

        for (int i = 0; i < data.spwanAmount; i++)
        {
            runtimeData.TelegraphObject.Add(UnityEngine.GameObject.Instantiate(context.laserAttackData.telegraphPrefab, context.TelegraphSpawnPosition.position, Quaternion.identity));
            runtimeData.Telegraph.Add(runtimeData.TelegraphObject[i].GetComponent<ITelegraph>());
            if (runtimeData.Telegraph[i] != null)
            {
                runtimeData.Telegraph[i].Initialize(context.TelegraphSpawnPosition.position, Quaternion.identity); // set telegraph position 
                runtimeData.Telegraph[i].SetDimensions(data.telegraphWidth, data.telegraphLength); // set telegraph the size 
                runtimeData.Telegraph[i].SetFill(0f);// and set the fill value
            }

        }

        AdvancePhrase(AttackPhrase.TrackTarget); // move to spawn attack
        return NodeState.Running;
    }


    protected override NodeState OnTrackTarget()
    {
        
        if (runtimeData.Telegraph == null || runtimeData.TelegraphObject == null)  
        {
            FailToCleanup("telegraph or telegraph object reference missing ");
        }
        float delayBetweenTelegraphs = 1f;
            
        bool anyTelegraphStillTracking = false;

        PhraseTimer();
        // move  the telegraph to the Position 

        for (int i = 0; i < runtimeData.Telegraph.Count; i++)
        {
            if (runtimeData.Telegraph[i] == null)
                continue;

            float startDelay = i * delayBetweenTelegraphs;
            float endTime = startDelay + data.trackingDuration*0.3f;

            // this telegraph has not started yet
            if (runtimeData.PhraseTimer < startDelay)
                continue;

            // this telegraph is currently allowed to track
            if (runtimeData.PhraseTimer < endTime)
            {
                Vector3 pos = context.player.position;
                pos.y += 0.3f;

                runtimeData.Telegraph[i].MoveToward(pos, data.trackingSpeed);
                anyTelegraphStillTracking = true;
            }
        }


        if (anyTelegraphStillTracking)
        {
            return NodeState.Running;
        }


        AdvancePhrase(AttackPhrase.Fill);
        return NodeState.Running;
    }
    protected override NodeState OnFillTelegraph()
    {

        if (runtimeData.Telegraph == null || runtimeData.TelegraphObject == null)
        {
            FailToCleanup("telegraph or telegraph object reference missing ");
        }
    

        PhraseTimer();

        // Fill  all the graph with for loop  in lieu of  fill one by one
        if (runtimeData.PhraseTimer < data.fillDuration)
        {
            float percent = Mathf.Clamp01(runtimeData.PhraseTimer / data.fillDuration);

            for (int i = 0; i < runtimeData.Telegraph.Count; i++)
            {
                runtimeData.Telegraph[i].SetFill(percent);

            }
            return NodeState.Running;
        }
       
        AdvancePhrase(AttackPhrase.SpawnAttack);
        return NodeState.Running;
    }

    protected override NodeState OnSpawnAttack()
    {
        // the attack object did not clean  from the previous same attack or something wrong with the object. move to clean 
        if (runtimeData.AttackObject == null|| runtimeData.AttackObject.Count >0 )
        {
           
            FailToCleanup("OnSpwanAttack Attack Object exist before spawn  , remove it ");
        }

        for(int i = 0; i < runtimeData.Telegraph.Count; i++)
        {
            runtimeData.AttackObject.Add(UnityEngine.GameObject.Instantiate(context.laserAttackData.laserActorPrefab[0], runtimeData.Telegraph[i].Transform.position, Quaternion.identity));
            if (runtimeData.AttackObject[i] != null)
            {
                runtimeData.AttackActor.Add(runtimeData.AttackObject[i].GetComponentInParent<IAttackActor>());
                runtimeData.AttackActor[i].Initialize(runtimeData.Telegraph[i].Transform.position, Quaternion.identity);
            }
        }
        AdvancePhrase(AttackPhrase.AttackActive);
        return NodeState.Running;
    }

    protected override NodeState OnAttackActive()
    {
        if (runtimeData.AttackObject == null || runtimeData.AttackObject.Count == 0)
        {
            FailToCleanup("OnattackActive  attackObject did not spawn or lost reference ");
        }

  
        PhraseTimer();

        if (runtimeData.PhraseTimer < data.attackDuration)
        {
            return NodeState.Running;   
        }

        LaserAttackActor obj = runtimeData.AttackObject[runtimeData.count].GetComponentInParent<LaserAttackActor>();
        if (obj != null)
        {
            obj.SetActive();
            runtimeData.Telegraph[runtimeData.count].Cleanup();
            runtimeData.count++;
            runtimeData.PhraseTimer = 0f;
        }


        if (runtimeData.count == runtimeData.AttackActor.Count)
        {
            AdvancePhrase(AttackPhrase.Cleanup);
        }
       // AdvancePhrase(AttackPhrase.Cleanup);
        return NodeState.Running;
    }

    
    private NodeState FailToCleanup(string message)
    {
        Debug.LogError("[BubleNode] " + message);
        AdvancePhrase(AttackPhrase.Cleanup);
        return NodeState.Running;
    }

}
