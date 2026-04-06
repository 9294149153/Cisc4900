using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BossBrain : MonoBehaviour
{
    [SerializeField] private BossContext context;

    private Node idleNode;
    private Node sphereSweepNode;
    private Node bubbleNode;

    private BossPhraseManager phraseManager;
    private MechBossRemoteConfig bossRemoteConfig;

    private BossAttackPool attackPool;
    private AttackSelector attackSelector;

    private Node currentNode;

    private bool waitingInIdle = true;
    private void Start()
    {
        idleNode =new IdleNode(context);
        phraseManager = new BossPhraseManager(context);
        attackPool = new BossAttackPool(context);
        attackSelector = new AttackSelector(context);

        bossRemoteConfig = new MechBossRemoteConfig(); // data container already define in the class

        sphereSweepNode = new SphereSweepNode(context,bossRemoteConfig.sphereSweepConfig);
        bubbleNode=new BubbleNode(context,bossRemoteConfig.bubleAttackConfig);


        context.remoteConfig = bossRemoteConfig;
    }

    private void Update()
    {
        if (context == null)return;

        // retrive Phrase Value Before Enter First Node function even the Idle  / So brain Know What to do in Currency
        context.currentPhrase = phraseManager.EvaluatePhrase();
        
        //Boss First Time Enter Do the Idle First
        if (currentNode == null)
        {
            //Idle State Control , When Idle Finish an Loop return set to false and skip this function for next loop
            if (waitingInIdle)
            {
               NodeState idleState = idleNode.Evaluate();

                if (idleState == NodeState.Success)
                {
                    waitingInIdle = false;
                }

                return;

            }

            BossAttackType nextAttacck = ChooseNextAttack(); // Get BossAtackTpye(Enum) value
            currentNode = RetriveAttackNodeFromAttackType(nextAttacck);//Convert value into Node
            context.currentAttackType = nextAttacck; // On context Set current Attack tpye

            // There Are not Other Node Beside idle loop the idle again 
            // Restart the Update from top to bottom for next condition 
            if (currentNode == null)
            {
                waitingInIdle=true;
                context.currentAttackType = BossAttackType.None;
                return;
            }

        }
        //Execute the Node and return Node progress

        NodeState state = currentNode.Evaluate();   
        
        // currentNode Are holding Node which mean exectuing that node
        // Loop Back to the Idle and repick an attack from the pool when Node are finish

        if(state==NodeState.Success ||state==NodeState.Failure)
        {
            //Reset Value for Function To re Exectue from Idle
            currentNode = null;
            context.currentAttackType = BossAttackType.None;

            //Set Idle True again for exectue Idle Node before Reach Next
            waitingInIdle = true;


           
        }

    }
    private BossAttackType ChooseNextAttack()
    {
        //Receive A reference from the Function of Tpye BossAttackOption with the current Phrase store on context 
        List<BossAttackOption> available = attackPool.GetBossAttackTypesWithPhrase(context.currentPhrase);

        // Attack Selector Decide which Attack will be retrieve from the pool
        return attackSelector.GetAttackFromPoolWithRandom(available);
    }

    //get the node with the Enum(BossAttackTpye) value
    private Node RetriveAttackNodeFromAttackType(BossAttackType value)
    {
        switch (value)
        {
            case BossAttackType.None:
                return idleNode;
            case BossAttackType.SphereSweepAttack:
                return sphereSweepNode;
            case BossAttackType.BubbleAttack:
                return bubbleNode;

        }
        return null;
    }


    public void SetScriptActive(bool active)
    {
        this.enabled=active;
    }

}