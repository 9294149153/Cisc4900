
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy/Config")]
public class EnemysConfig :ScriptableObject
{
    [Header("Patrol")]
    public float patrolRadius ;
    public float patrolPointTolerance ;
    public float patrolWaitTime ;

    [Header("Attack")]
    public GameObject attackPrefab;

    
    [Header("Movement")]
    public float patrolSpeed ;
    public float chaseSpeed;

    [Header("Ranges")]
    public float alarmRange ;   
    public float attackRange ;


}
