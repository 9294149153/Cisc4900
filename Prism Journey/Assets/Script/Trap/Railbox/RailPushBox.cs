using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
    public class RailPushBox : MonoBehaviour
    {
    [Header("Path")]
    [SerializeField] private Transform[] points;

    [Header("Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private float pushSideTolerance = 0.7f;

    [Header("Obstacle Check")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float blockCheckExtraDistance = 0.1f;

    private int currentPoint = 0;
    private bool isMoving;
    private Vector3 currentTarget;
    private int targetPointIndex;

    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        if (points != null && points.Length > 0 && points[0] != null)
        {
            transform.position = points[0].position;
            currentPoint = 0;
        }
    }

    private void Update()
    {
        if (points == null || points.Length < 2 || player == null)
            return;

        if (!isMoving && Input.GetKeyDown(KeyCode.J))
        {
            TryStartPush();
        }

        if (isMoving)
        {
            MoveTo(currentTarget);

            if (Reached(currentTarget))
            {
                transform.position = currentTarget;
                currentPoint = targetPointIndex;
                isMoving = false;
            }
        }
    }

    private void TryStartPush()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (Vector3.Distance(transform.position, player.position) > interactRange)
        {
            Debug.Log("Too far");
            return;
        }

        if (distanceToPlayer > interactRange)
        {
            Debug.Log("Too far: " + distanceToPlayer);
            return;
        }

        // Try next point
        if (currentPoint < points.Length - 1)
        {
            Vector3 moveDir = points[currentPoint + 1].position - transform.position;

            if (IsPlayerBehindMoveDirection(moveDir))
            {
                currentTarget = points[currentPoint + 1].position;
                targetPointIndex = currentPoint + 1;
                isMoving = true;
                Debug.Log("Push to next point");
                return;
            }
        }

        // Try previous point
        if (currentPoint > 0)
        {
            Vector3 moveDir = points[currentPoint - 1].position - transform.position;

            if (IsPlayerBehindMoveDirection(moveDir))
            {
                currentTarget = points[currentPoint - 1].position;
                targetPointIndex = currentPoint - 1;
                isMoving = true;
                Debug.Log("Push to previous point");
                return;
            }
        }

        Debug.Log("Wrong side");
    }

    private bool IsPlayerBehindMoveDirection(Vector3 moveDirection)
    {
        Vector3 boxToPlayer = player.position - transform.position;

        boxToPlayer.y = 0f;
        moveDirection.y = 0f;

        if (boxToPlayer.sqrMagnitude < 0.0001f || moveDirection.sqrMagnitude < 0.0001f)
            return false;

        boxToPlayer.Normalize();
        moveDirection.Normalize();

        float dot = Vector3.Dot(boxToPlayer, -moveDirection);


        return dot > pushSideTolerance;
    }

    private void MoveTo(Vector3 target)
    {
        Vector3 current = transform.position;
        Vector3 next = Vector3.MoveTowards(current, target, speed * Time.deltaTime);
        Vector3 move = next - current;

        if (move.sqrMagnitude < 0.0001f)
            return;

        Vector3 direction = move.normalized;
        float distance = move.magnitude;

        if (IsBlocked(direction, distance + blockCheckExtraDistance))
        {
            isMoving = false;
            return;
        }

        transform.position = next;
    }

    private bool IsBlocked(Vector3 direction, float distance)
    {
        if (boxCollider == null)
            return false;

        Vector3 center = boxCollider.bounds.center;

        Vector3 halfExtents =
            Vector3.Scale(boxCollider.size, transform.lossyScale) * 0.5f;

        halfExtents *= 0.95f; // slightly smaller than real box

        bool blocked = Physics.BoxCast(
            center,
            halfExtents,
            direction,
            out RaycastHit hit,
            transform.rotation,
            distance,
            obstacleLayer,
            QueryTriggerInteraction.Ignore
        );

        if (blocked)
            Debug.Log("Blocked by: " + hit.collider.name);

        return blocked;
    }

    private bool Reached(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) < 0.05f;
    }

    private void OnDrawGizmos()
    {
        if (points != null && points.Length >= 2)
        {
            Gizmos.color = Color.yellow;

            for (int i = 0; i < points.Length - 1; i++)
            {
                if (points[i] == null || points[i + 1] == null)
                    continue;

                Gizmos.DrawLine(points[i].position, points[i + 1].position);
                Gizmos.DrawSphere(points[i].position, 0.1f);
            }
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}





