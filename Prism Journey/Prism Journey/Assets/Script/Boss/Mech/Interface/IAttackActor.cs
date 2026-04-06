using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IAttackActor
   
{
    // Gives access to transform.
    Transform Transform { get; }

    // Initializes spawn position and rotation.
    void Initialize(Vector3 position, Quaternion rotation);


    // Moves actor toward a target.
    void MoveToward(Vector3 targetPosition, float speed);

    // Scales actor over time.
    void SetScaleOverTime(float speed);

    // Returns true if actor reached target.
    bool HasReached(Vector3 targetPosition, float threshold);

    // Cleans up actor object.
    void Cleanup();
}
