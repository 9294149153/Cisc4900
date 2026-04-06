using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BubbleNode : BossAttackBaseNode<BubbleAttackConfig, BubbleAttackRuntimeData>
{

  
    public BubbleNode(BossContext context, BubbleAttackConfig bubbleAttackConfig):base(context,bubbleAttackConfig)
    {

    }

    protected override void OnEnter()
    {
        Debug.Log("Enter BubbleNode ");
        runtimeData.InitData();// init the List on the runtimeData Container 
        AdvancePhrase(AttackPhrase.StartAnimation);// Set Phrase Timer = 0 and currentPhrase to next pointing
    }

    protected override void OnStartAnimation()
    {
        //If have animaiton reference then play aniamtion 
        if (context.mechAnimation != null)
        {
            Debug.Log("aniamiton played for buble node");// has not make it yet just use debug.log for currency
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
        if (context.bubbleAttackData.telegraphPrefab == null  && runtimeData.TelegraphObject.Count>0)
        {
            FailToCleanup("Telegraph prefab missing and  runtimeData didnt clean after loop");
        }

        //Spawn the telegraph and get the reference of the telegraph control script
        for (int i = 0; i < data.spwanAmount; i++)
        {
            runtimeData.TelegraphObject.Add(UnityEngine.GameObject.Instantiate(context.bubbleAttackData.telegraphPrefab,context.TelegraphSpawnPosition.position,Quaternion.identity));
            runtimeData.Telegraph.Add(runtimeData.TelegraphObject[i].GetComponent<ITelegraph>());
            if (runtimeData.Telegraph[i] != null)
            {
                runtimeData.Telegraph[i].Initialize(context.TelegraphSpawnPosition.position, Quaternion.identity); // set telegraph position 
                runtimeData.Telegraph[i].SetDimensions(data.telegraphWidth,data.telegraphLength); // set telegraph the size 
                runtimeData.Telegraph[i].SetFill(0f);// and set the fill value
            }
           
        }

        // fetch target position for each telegraph 
        if (runtimeData.targetPosition == null || runtimeData.targetPosition.Count == 0)
        {
            runtimeData.targetPosition = RandomPositionInsidePlane(context.plane, data.spwanAmount);
        }
        
        AdvancePhrase(AttackPhrase.TrackTarget);
        return NodeState.Running;
    }

    protected override NodeState OnTrackTarget()
    {
        PhraseTimer();
        if(runtimeData.targetPosition ==null ||runtimeData.targetPosition.Count>data.spwanAmount || runtimeData.targetPosition.Count < data.spwanAmount)
        {
            FailToCleanup("telegrah Spawn position is null nor the amount are not equal to the telegraph");
        }


        // move  the telegraph to the each targetPosition 
        if(runtimeData.PhraseTimer<data.trackingDuration)
        {
            for (int i = 0; i < runtimeData.targetPosition.Count; i++)
            {
                if (runtimeData.Telegraph != null)
                {
                    Vector3 pos = runtimeData.targetPosition[i];
                    pos.y += 0.3f;
                    runtimeData.Telegraph[i].MoveToward(pos, data.trackingSpeed);
                }
            }
            return NodeState.Running;
        }
        AdvancePhrase(AttackPhrase.Fill);
        return NodeState.Running;
    }

    

   

    protected override NodeState OnFillTelegraph()
    {
       
        if(runtimeData.Telegraph.Count==0 || runtimeData.Telegraph == null)
        {
            FailToCleanup("Telegraph reference are missing");
        }

        PhraseTimer();
        float percent = Mathf.Clamp01(runtimeData.PhraseTimer / data.fillDuration);
        for (int i = 0;i < runtimeData.Telegraph.Count; i++)
        {
            runtimeData.Telegraph[i].SetFill(percent);  
        }

        if (runtimeData.PhraseTimer > data.fillDuration)
        {
            AdvancePhrase(AttackPhrase.SpawnAttack);
        }


        return NodeState.Running;
    }

    protected override NodeState OnSpawnAttack()
    {
        SpawnBubble();
        AdvancePhrase(AttackPhrase.AttackActive);
        return NodeState.Running;
    }


    protected override NodeState OnAttackActive()
    {
        PhraseTimer();
        if (runtimeData.AttackActor == null || runtimeData.AttackActor.Count == 0)
        {
            FailToCleanup("OnAttackActive AttackActor was null move to cleanUp Phrase");
        }


        if (runtimeData.PhraseTimer < context.remoteConfig.bubleAttackConfig.bubbleAttackDuration)
        {
            for (int i = 0; i < runtimeData.AttackActor.Count; i++)
                {
                    Vector3 pos = runtimeData.targetPosition[i];
                    pos.y += 1.5f;
                    runtimeData.AttackActor[i].MoveToward(pos, context.remoteConfig.bubleAttackConfig.fallSpeed);
                   
              }
                return NodeState.Running ;
        }

     

        AdvancePhrase(AttackPhrase.Cleanup);
        
        
        return NodeState.Running;
    }


    protected override NodeState OnCleanUp()
    {
        //Clean the telegraph after attack 
        //Also call the Base Class CleanUP to Enter Fisnish  Phrase
        if (runtimeData.Telegraph != null)
        {
            foreach (var reference in runtimeData.Telegraph)
            {
                if (reference != null)
                {
                    reference.Cleanup();
                }
            }
            
        }
        return base.OnCleanUp();
    }

    protected override void Finisih()
    {
        base.Finisih();
    }


    //Helper Method
    ///////////////////////////////////////////
    private NodeState FailToCleanup(string message)
    {
        Debug.LogError("[BubleNode] " + message);
        AdvancePhrase(AttackPhrase.Cleanup);
        return NodeState.Running;
    }
    private void SpawnBubble()
    {
        if (context.bubbleAttackData.actorPrefab == null || context.bubblePrefab.Length == 0)
        {
            Debug.LogError("[BubbleAttackNode] bubblePrefab is null or empty.");
            return;
        }

        if (runtimeData.targetPosition== null || (runtimeData.targetPosition.Count == 0))
        {
            Debug.LogError("[BubbleAttackNode] telegraphTargetPosition is null or empty.");
            return;
        }

        for (int i = 0; i < context.remoteConfig.bubleAttackConfig.spwanAmount; i++)
        {
            int random = UnityEngine.Random.Range(0, context.bubblePrefab.Length);

            Vector3 offset = new Vector3(0, context.remoteConfig.bubleAttackConfig.bubbleSpawnHeight, 0);
            Vector3 pos = runtimeData.targetPosition[i] + offset;

            runtimeData.AttackObject.Add(GameObject.Instantiate(context.bubbleAttackData.actorPrefab[random], pos, Quaternion.identity));
            runtimeData.AttackActor.Add(runtimeData.AttackObject[i].GetComponent<IAttackActor>());
            runtimeData.AttackActor[i].Transform.localScale = Vector3.one * context.remoteConfig.bubleAttackConfig.bubbleRadius;

            BubbleSphereController bubble = runtimeData.AttackObject[i].GetComponentInParent<BubbleSphereController>();
            if (bubble != null)
            {
                bubble.SetContext(context);
            }

        }
    }




    // return area random position array with 10 scale edgepadding
    private List<Vector3> RandomPositionInsidePlane(Transform planeTransform, int amountToGenerate)
    {
        float minSpacing = 8f;   // minimum distance between telegraphs
        float edgePadding = 10f;  // keep some distance from plane edge

        List<Vector3> positions = new List<Vector3>();

        // Unity Plane default size is 10x10
        float planeWidth = 10f * planeTransform.localScale.x;
        float planeLength = 10f * planeTransform.localScale.z;

        // half size
        float halfWidth = planeWidth * 0.5f;
        float halfLength = planeLength * 0.5f;

        int count = 0;
        int safety = 0;

        while (count < amountToGenerate && safety < 200)
        {
            safety++;

            float randomX = Random.Range(
                planeTransform.position.x - halfWidth + edgePadding,
                planeTransform.position.x + halfWidth - edgePadding
            );

            float randomZ = Random.Range(
                planeTransform.position.z - halfLength + edgePadding,
                planeTransform.position.z + halfLength - edgePadding
            );

            Vector3 candidate = new Vector3(
                randomX,
                planeTransform.position.y,
                randomZ
            );

            bool valid = true;

            for (int i = 0; i < count; i++)
            {
                if (Vector3.Distance(candidate, positions[i]) < minSpacing)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                positions.Add(candidate);
                count++;
            }
        }

        return positions;
    }
}
