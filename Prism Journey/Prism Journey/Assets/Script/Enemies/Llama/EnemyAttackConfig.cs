using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy/EnemyAttackConfig")]
public class EnemyAttackConfig : ScriptableObject
{
    public bool hasMelee;
    public bool hasSpit;
    public bool hasRoll;
    public bool hasBounce;
}
