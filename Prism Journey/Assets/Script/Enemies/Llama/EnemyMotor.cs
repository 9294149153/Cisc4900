using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : MonoBehaviour
{
    private EnemyContext enemyContext;
    private float rotationSpeed = 540f;

    private void Awake()
    {
       if(enemyContext==null) enemyContext = GetComponent<EnemyContext>();

    }


    public void SetChaseSpeed(float speed)
    {
        enemyContext.agent.speed = speed;
    }
    public void StopAgent()
    {
        enemyContext.agent.isStopped = true;
    }
   

    public void ResumeAgent(Vector3 targetPosition)
    {
        enemyContext.agent.isStopped = false;
        enemyContext.agent.SetDestination(targetPosition);
    }

    public void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
