using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTelegraphVisual : MonoBehaviour
{
    [Header("References")]
    public Transform bgSphere;
    public Transform fillSphere;


    private float totalWidth;
    private float totalLength;


   

    private void Start()
    {
        
    }
   
    public void Setup(float width, float length)
    {
        totalWidth = width;
        totalLength = length;

        if (bgSphere != null)
        {
            bgSphere.localPosition = Vector3.zero;
            bgSphere.localScale = new Vector3(totalLength, 0f, totalWidth);
        }

        SetFillPercent(0f);
    }
    public void SetFillPercent(float percent)
    {
        if (fillSphere == null)
            return;



        //Calculate the value(Percent) and fix it into 0 or 1 if is lower than 0 or bigger than 1 . 
        // Ex: percent = 1.5  then Clamp01(percent) return 1  and vice verse for 0>pecent value
        // no change if in between 0 and 1 , ex: percent = 0.5 then  clamp01(percent) return 0.5
        percent = Mathf.Clamp01(percent);


        // Current filled length on Y axis
        float currentLength = totalLength * percent;
        float currentWidth = totalWidth * percent;
        
        //change x and y scale
        fillSphere.localScale = new Vector3(currentLength,0f,currentWidth);

        fillSphere.localPosition = new Vector3(bgSphere.localPosition.x,bgSphere.localPosition.y+0.05f,bgSphere.localPosition.z);

    }

    public Vector3 GetPosition()
    {
        return  transform.position;
    }

    public void MoveToTarget(Vector3 target , float moveSpeed)
    {
        transform.position=Vector3.MoveTowards(transform.position,target,moveSpeed*Time.deltaTime);
    }

}


