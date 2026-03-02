using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class AlramDetection : MonoBehaviour
{
    
    [SerializeField] private EnemyStateManager enemy;

    private const string detectionName = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(detectionName)) return;

        enemy.PlayerInAlramZone(1);
        enemy.AssignPlayerTransform(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(detectionName)) return;

        enemy.PlayerInAlramZone(0);
        enemy.AssignPlayerTransform(null);

    }

}
*/