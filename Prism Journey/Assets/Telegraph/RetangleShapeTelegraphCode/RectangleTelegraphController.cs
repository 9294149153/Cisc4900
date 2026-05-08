using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleTelegraphController : MonoBehaviour, ITelegraph
{

    [Header("References")]
   [SerializeField] private Transform root;
    [SerializeField] private Transform fill;

    private float width;
    private float length;

    private void Awake()
    {
        if(root == null || fill==null)
        {
            LogMessage("RetangleTelegraphController" , "root or fill did not reference");
        }
    }
    // Initializes transform position and rotation
    public void Initialize(Vector3 position, Quaternion rotation)
    {
      
        transform.position=Vector3.zero;
        transform.position = position;
        transform.rotation = rotation;
    }

    public void SetDimensions(float width, float length)
    {
        this.width = width;
        this.length = length;
        if (root != null)
        {
           root.localScale = new Vector3(width, length,1f);
        }
    }
   
    public void SetRotation(Vector3 dir)
    {

        dir.y = 0f;

        if (dir.sqrMagnitude <= 0.0001f)
            return;

        transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(90f, 0f, 90f);
    }
    public Vector3 GetLeftEdge(Vector3 vecot3Right)
    {
        float scale = length * 2.5f;
        return root.position + vecot3Right * scale;
    } 

    public Vector3 GetRightEdge(Vector3 vecot3Right)
    {
        float scale = length * 2.5f;
        return root.position - vecot3Right * scale;
    }

    

    public void MoveToward(Vector3 targetPosition, float speed)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }

   
    public void SetFill(float percent)
    {
        if (fill == null)
            return;

        //Calculate the value(Percent) and fix it into 0 or 1 if is lower than 0 or bigger than 1 . 
        // Ex: percent = 1.5  then Clamp01(percent) return 1  and vice verse for 0>pecent value
        // no change if in between 0 and 1 , ex: percent = 0.5 then  clamp01(percent) return 0.5
        percent = Mathf.Clamp01(percent);


        // Current filled length on Y axis
        float currentLength = length * percent;

        // Keep X width same, change Y length
        fill.localScale = new Vector3(width, currentLength, 1f);

        // Bottom edge of the full telegraph
        float bottomEdgeY = -length * 0.5f;

        // Move center of fill so it starts from bottom/end instead of center
        float centerY = bottomEdgeY + currentLength * 0.5f;


        fill.localPosition = new Vector3(0f, centerY, 0.001f);
    }

    public void SnapTo(Vector3 position)
    {
        transform.position = position;
    }
    public void Cleanup()
    {
        Destroy(gameObject);
    }
   


    private void LogMessage(string scriptName,string message )
    {
        Debug.LogError($"[{scriptName}]{gameObject.name} = "+"message",this);
    }

    

    public Transform Transform => gameObject.transform;
}
