using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FanTrapState
{
    None,
    ClockWise,
    CounterClockWise
}
public class FanTrapController : MonoBehaviour
{
    [SerializeField] private FanTrapState currentState;
    [SerializeField] protected GameObject rotationHead;

    [SerializeField] private float rotationSpeed = 10f;

    private void Update()
    {
        if(rotationHead == null)
        {
            Debug.LogError($"[FanTrapController] the head reference are missing so can not rotate the blade +|| [roataionhead]={rotationHead}",this);
            return;
        }

        BladeRotationDir(currentState);
    }


    void BladeRotationDir(FanTrapState state)
    {
        Vector3 rot = rotationHead.transform.eulerAngles;
        if (rotationHead.transform.rotation.y>=360f || rotationHead.transform.rotation.y <= -360f)
        {
            if (rot.y >= 360f)
            {
                rot.y = 0f;
                rotationHead.transform.eulerAngles = rot;
            }
        }
        switch (state)
        {
            case FanTrapState.None:
                break;
            case FanTrapState.ClockWise:
                rotationHead.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
                break;
            case FanTrapState.CounterClockWise:
                rotationHead.transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
                break;

        }
    }

}
