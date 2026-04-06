using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITelegraph
{
  
    Transform Transform { get; }

    // Initializes telegraph position and rotation.
    void Initialize(Vector3 position, Quaternion rotation);

    // Sets telegraph dimensions.
    void SetDimensions(float width, float length);

    // Sets fill percent from 0 to 1.

    void SetRotation(Vector3 Dir);
    void SetFill(float percent);

    // Moves telegraph toward a target position.
    void MoveToward(Vector3 targetPosition, float speed);

    // Snaps telegraph instantly to a position.
    void SnapTo(Vector3 position);

    // Returns left edge position.
    Vector3 GetLeftEdge(Vector3 vecot3Right);

    // Returns right edge position.
    Vector3 GetRightEdge(Vector3 vecot3Right);

    // Cleans up telegraph.
    void Cleanup();
}
