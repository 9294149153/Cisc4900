using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleAttackNode : Node
{

    [Header(" BossContext reference")]
    private BossContext context;
    private bool isRunning;

    private List<GameObject> sphereTelegraph;
    private Vector3[] telegraphTargetPostion;
    private bool getPostion = false;

    private List<GameObject> bubbleList;
    private bool spawnBubble;
   
    [Header("RunTime Calculate value")]
    private float telegraphTrackingTimer;
    private int telegraphSpawnCounter = 0;
    private bool started = false;
    private float animationTime = 0f;
    private float telegraphFillTimer;


    private int finishedBubbleCount;
    private bool bubleMove=false;
    public BubbleAttackNode(BossContext context)
    {
        this.context = context;
        
    }

    public override NodeState Evaluate()
    {

        if (!context.bubbleAttackEnable)  return NodeState.Failure; 

        if (context.isAttackRunning && !isRunning) return NodeState.Failure;

        if (!isRunning)
        {
            Debug.Log("BubbleAttack Start");

            context.isAttackRunning = true;
            isRunning = true;

            return NodeState.Running;
        }
        //First time enter the this node  set the value that was need  before go to next
        if (started==false)
        {
            
            StartAttack();
            Debug.Log("Enter Bubble Attack");
            sphereTelegraph = new List<GameObject>();
            bubbleList=new List<GameObject>();
            context.mechAnimation.PlayerSphereSweepAttack();
            return NodeState.Running;
        }
   
        // Node  animation time before to the attack 
        if (animationTime < context.bubbleAnimationDurationTime)
        {
            animationTime += Time.deltaTime;
            return NodeState.Running;
        }
   
        context.mechAnimation.PlayEmpty();

        //Spawn the telegraph  and set correct scale  also spawn the bubble

        if (context.bubleSpwanTelegraphAmount > telegraphSpawnCounter )
        {
            telegraphSpawnCounter++;
            sphereTelegraph.Add(GameObject.Instantiate(context.sphereTelegraphPrefab, context.TelegraphSpawnPosition.position, Quaternion.identity));
            SphereTelegraphVisual telegrah = sphereTelegraph[telegraphSpawnCounter-1].GetComponent<SphereTelegraphVisual>();
            telegrah.Setup(context.bubleTelegraphtelegraphLength, context.bubleTelegraphtelegraphWidth);
            return NodeState.Running;
        }

        // Get the target Position only once ;
        if (getPostion == false)
        {
            telegraphTargetPostion = RandomPositionInsidePlane(context.plane);
            getPostion = true;
            return NodeState.Running;
        }
        
             //sphereTelegraph is not null and telegraphtargetposition has same amount number of the spheretelegraph
        if (sphereTelegraph.Count == 0 && telegraphTargetPostion.Length!= sphereTelegraph.Count)
        {
            
            Debug.LogError($"[BubbleAttackNode] | Telegraph count={sphereTelegraph.Count} |targetPosition count ={telegraphTargetPostion.Length} did not match ");

            return NodeState.Failure; 
        }

        //Tracking destination position and move toward it 
        if (telegraphTrackingTimer < context.bubleTelegraphTrackingDuration)
        {

            telegraphTrackingTimer += Time.deltaTime;

            for (int i = 0; i < sphereTelegraph.Count; i++)
            {
                SphereTelegraphVisual obj = sphereTelegraph[i].GetComponent<SphereTelegraphVisual>();
                if (obj != null)
                {
                    Vector3 targetPos = new Vector3(telegraphTargetPostion[i].x, telegraphTargetPostion[i].y + 0.2f, telegraphTargetPostion[i].z);
                    obj.MoveToTarget(targetPos, context.bubleTelegraphtelegraphTrackingSpeed);
                }
            }
           return NodeState.Running ;
        }

            // fill the telegraph
            if (telegraphFillTimer < context.bubleTelegraphfillDuration)
            {
                telegraphFillTimer += Time.deltaTime;
                float percent = Mathf.Clamp01(telegraphFillTimer / context.bubleTelegraphfillDuration); // calculate the current percent the fill reach  and set to 1 if it over max value
                for (int i = 0; i < sphereTelegraph.Count; i++)
                {
                    SphereTelegraphVisual obj = sphereTelegraph[i].GetComponent<SphereTelegraphVisual>();
                    if (obj != null)
                    {
                        obj.SetFillPercent(percent);
                    }
                }

                return NodeState.Running ;
            }

        //Spawn the buble attack
        if (spawnBubble == false)
        {
            SpawnBubble();
            return NodeState.Running ;
        }

        // Bubble Move To target Position;

        if (CheckAttackFinished())
        { 
            EndAttack();
            return NodeState.Success;
        }


      
        EndAttack();
        NodeColdown.isColdown = false;



        return NodeState.Running;
    }


   
    private void StartAttack()
    {
        started = true;
        telegraphSpawnCounter = 0;
        getPostion = false;
        animationTime = 0f;
        telegraphTrackingTimer = 0f;
        spawnBubble = false;
        bubleMove=false;
        telegraphFillTimer=0f;
    }

    private void EndAttack()
    {
        started = false;
        context.isAttackRunning = false;
        isRunning= false;

        bubleMove = false;
        spawnBubble = false;
        getPostion = false;

        telegraphFillTimer = 0f;
        telegraphSpawnCounter = 0;
        animationTime = 0f;
        telegraphTrackingTimer = 0f;
        Desotry();
    }

    private void Desotry()
    {
        foreach (var reference in sphereTelegraph)
        {
            GameObject.Destroy(reference.gameObject);
        }

        sphereTelegraph = null;
        telegraphTargetPostion = null;

        bubbleList.Clear();


    }
    private bool CheckAttackFinished()
    {
        for (int i = 0; i < bubbleList.Count; i++)
        {
            BubbleControl bubble = bubbleList[i].GetComponentInParent<BubbleControl>();
            if (bubble != null)
            {
                Vector3 offset = new Vector3(0, 1.5f, 0);
                Vector3 pos = telegraphTargetPostion[i] + offset;
                bubble.MoveToTarget(pos, 10f);
            }
        }
        bubleMove = true;
        return bubleMove == true;

    }
   

    private void SpawnBubble()
    {
        if (context.bubblePrefab == null || context.bubblePrefab.Length == 0)
        {
            Debug.LogError("[BubbleAttackNode] bubblePrefab is null or empty.");
            return;
        }

        if (telegraphTargetPostion == null || telegraphTargetPostion.Length == 0)
        {
            Debug.LogError("[BubbleAttackNode] telegraphTargetPosition is null or empty.");
            return;
        }

        if (spawnBubble == false)
        {
            for (int i = 0; i < context.bubleSpwanTelegraphAmount; i++)
            {
                int random = UnityEngine.Random.Range(0, context.bubblePrefab.Length);

                Vector3 offset = new Vector3(0, 10f, 0);
                Vector3 pos = telegraphTargetPostion[i] + offset;

                bubbleList.Add(GameObject.Instantiate(context.bubblePrefab[random], pos, Quaternion.identity));
                bubbleList[i].transform.localScale = Vector3.one * context.bubleRadius;

                BubbleControl bubble = bubbleList[i].GetComponentInParent<BubbleControl>();
                if (bubble != null)
                {
                    bubble.SetContext(context);
                }

            }
            spawnBubble = true;
        }
    }


    //Return random position around target in vecotr3[] array
    private Vector3[] RandomPositionAroundPlayer(Transform targetPosition)
    {
        float minSpacing = 8f;      // distance between each telegraph
        float maxRadius = 18f;       // max distance from player

        Vector3[] positions = new Vector3[context.bubleSpwanTelegraphAmount];

        int count = 0;
        int safety = 0; // prevent infinite loop

        while (count < context.bubleSpwanTelegraphAmount && safety < 100)
        {
            safety++;

            // random direction on XZ plane
            float angle = Random.Range(0f, Mathf.PI * 2f);

            // random distance from center (player)
            float radius = Random.Range(0.3f, maxRadius);

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 candidate = new Vector3(
                targetPosition.position.x + x,
                targetPosition.position.y,
                targetPosition.position.z + z
            );

            // check spacing with previous positions
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
                positions[count] = candidate;
                count++;
            }
        }

        return positions;
    }

    // return area random position array with 10 scale edgepadding
    private Vector3[] RandomPositionInsidePlane(Transform planeTransform)
    {
        float minSpacing = 8f;   // minimum distance between telegraphs
        float edgePadding = 10f;  // keep some distance from plane edge

        Vector3[] positions = new Vector3[context.bubleSpwanTelegraphAmount];

        // Unity Plane default size is 10x10
        float planeWidth = 10f * planeTransform.localScale.x;
        float planeLength = 10f * planeTransform.localScale.z;

        // half size
        float halfWidth = planeWidth * 0.5f;
        float halfLength = planeLength * 0.5f;

        int count = 0;
        int safety = 0;

        while (count < context.bubleSpwanTelegraphAmount && safety < 200)
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
                positions[count] = candidate;
                count++;
            }
        }

        return positions;
    }
}
