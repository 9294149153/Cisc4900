using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlramDetection : MonoBehaviour
{
    
    [SerializeField] private EnemyStateManager enemy;

    private const string detectionName = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(detectionName)) return;

        enemy.playerInAlarmZone = true;
        enemy.playerTransform = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(detectionName)) return;

        enemy.playerInAlarmZone = false;
        enemy.playerTransform = null;
        
    }

}
