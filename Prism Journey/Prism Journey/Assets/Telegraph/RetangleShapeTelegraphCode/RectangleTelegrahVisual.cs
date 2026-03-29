using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UIElements;

public class RectangleTelegrahVisual : MonoBehaviour
{
    [Header("References")]
    public Transform bgQuad;
    public Transform fillQuad;

    private float totalWidth;
    private float totalLength;
    public void Setup(float width, float length)
    {
        totalWidth = width;
        totalLength = length;

        if (bgQuad != null)
        {
            bgQuad.localPosition = Vector3.zero;
            bgQuad.localScale = new Vector3(totalWidth, totalLength, 1f);
        }

        SetFillPercent(0f);
    }

    public void SetFillPercent(float percent)
    {
        if (fillQuad == null)
            return;



        //Calculate the value(Percent) and fix it into 0 or 1 if is lower than 0 or bigger than 1 . 
        // Ex: percent = 1.5  then Clamp01(percent) return 1  and vice verse for 0>pecent value
        // no change if in between 0 and 1 , ex: percent = 0.5 then  clamp01(percent) return 0.5
        percent = Mathf.Clamp01(percent);


        // Current filled length on Y axis
        float currentLength = totalLength * percent;

        // Keep X width same, change Y length
        fillQuad.localScale = new Vector3(totalWidth, currentLength, 1f);

        // Bottom edge of the full telegraph
        float bottomEdgeY = -totalLength * 0.5f;

        // Move center of fill so it starts from bottom/end instead of center
        float centerY = bottomEdgeY + currentLength * 0.5f;


        fillQuad.localPosition = new Vector3(0f, centerY, 0.001f);

    }
    
    
    public Vector3 GetLeftEdgeOfTelegraph(Vector3 vecot3Right)
    {

        float scale = totalLength * 2.5f;
        return bgQuad.position + vecot3Right * scale;
    }

    public Vector3 GetRightEdgeOfTelegraph(Vector3 vecot3Right)
    {
      
        float scale = totalLength * 2.5f;
        return bgQuad.position - vecot3Right * scale;
    }



    
    public void MoveToward(Vector3 targetPosition ,float speed)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }

    public void SetRotation(Vector3 forwardDir)
    {
        forwardDir.y = 0f;

        if (forwardDir.sqrMagnitude <= 0.0001f)
            return;

        transform.rotation = Quaternion.LookRotation(forwardDir) * Quaternion.Euler(90f, 0f, 90f);
       
    }
}
