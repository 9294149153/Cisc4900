using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    [SerializeField] private EnemyAIBase enemy;
    [SerializeField] private SphereCollider collider; 
    private const string detectionName = "Player";

    private void Awake()
    {
        if (!collider) collider = GetComponent<SphereCollider>();
        collider.radius = enemy.AttackZoneRadius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(detectionName)) return;
        enemy.SetAttackRange(true);

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(detectionName)) return;

        enemy.SetAttackRange(false);

    }

}
