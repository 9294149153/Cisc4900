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


    public virtual void SetScale(Vector3 scale)
    {
        Debug.Log("Used Base SetScale");
    }
    // Moves actor toward a target.
    virtual void MoveToward(Vector3 targetPosition, float speed)
    {

    }

    // Scales actor over time.
   virtual  void SetScaleOverTime(float speed)
    {

    }

    // Returns true if actor reached target.
     virtual bool HasReached(Vector3 targetPosition, float threshold)
    {
       return false;
    }

    // Cleans up actor object.
    void Cleanup();
}
