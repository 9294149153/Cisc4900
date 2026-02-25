using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{

    [SerializeField] private EnemyStateManager enemy;

    private const string detectionName = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(detectionName)) return;
        enemy.playerInAttackZone = true;
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(detectionName)) return;

        enemy.playerInAttackZone = false;

    }
}
