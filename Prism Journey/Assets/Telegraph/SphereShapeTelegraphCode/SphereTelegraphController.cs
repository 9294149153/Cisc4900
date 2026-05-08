using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class SphereTelegraphController : MonoBehaviour, ITelegraph
{
    
    [SerializeField] private Transform visualRoot;

    [SerializeField] private Transform fillVisual;

    private float totalWidth;
    private float totalLength;
    void ITelegraph.Cleanup()
    {
        Destroy(gameObject);
    }

    Vector3 ITelegraph.GetLeftEdge(Vector3 vecot3Right)
    {
        return Vector3.zero;// sphere telegraph do not return left and right edge
    }

    Vector3 ITelegraph.GetRightEdge(Vector3 vecot3Right)
    {
        return Vector3.zero; // sphere telegraph do not return left and right edge
    }

    void ITelegraph.Initialize(Vector3 position, Quaternion rotation)
    {
        transform.position=Vector3.zero;
        transform.position= position;
        transform.rotation= rotation;


    }

    void ITelegraph.MoveToward(Vector3 targetPosition, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void ITelegraph.SetDimensions(float width, float length)
    {
        totalLength = length;
        totalWidth = width;
        transform.localScale= 
        transform.localScale= Vector3.one;
        visualRoot.localScale = new Vector3(width, 0, length);
        fillVisual.localScale = Vector3.zero;

       
    }

    void ITelegraph.SetFill(float percent)
    {
        if (fillVisual == null)
            return;


        percent = Mathf.Clamp01(percent);
        float currentLength = totalLength * percent;
        float currentWidth = totalWidth * percent;

        fillVisual.localScale = new Vector3(currentLength, 0f, currentWidth);

        fillVisual.localPosition = new Vector3(visualRoot.localPosition.x, visualRoot.localPosition.y + 0.05f, visualRoot.localPosition.z);
    }

    void ITelegraph.SetRotation(Vector3 Dir)
    {
        transform.rotation = Quaternion.LookRotation(Dir);
    }

    void ITelegraph.SnapTo(Vector3 position)
    {
        //Not use for this control
    }
    Transform ITelegraph.Transform => transform;
}
