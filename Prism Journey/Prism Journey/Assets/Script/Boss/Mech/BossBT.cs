using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBT : MonoBehaviour
{
  

    [SerializeField]private BossContext context;
    public BossContext BossContext => context;

    private Node root; // the top node of the behavior tree
    private void Awake()
    {
        if(context == null) context=GetComponent<BossContext>();
            
    }

    private void Start()
    {
        // ---------------- PHASE 1 ----------------
        // If HP is between 100 and 80, boss only uses Zone Attack
        Node phase1 = new Sequence(new List<Node>
        {
            new CheckBossHPRange(context,context.bossConfig.maxHP,context.bossConfig.minHP), // check if hp is in phase 1 range
            new NodeColdown(2f),
            new SphereSweepAttackNode(context)

                
        });

        // ---------------- PHASE 2 ----------------
        // If HP is between 79.9 and 50, boss randomly uses Zone or Hindrance



        // ---------------- ROOT ----------------
        // Root tries phase1 first, then phase2, then idle
        root = new Selector(new List<Node>
        {
            new IdleNode(context),
            phase1,         
              
        });
    }

    private void Update()
    {
        // Run the whole behavior tree every frame
        if (root != null)
        {
            root.Evaluate();
        }
    }


}
