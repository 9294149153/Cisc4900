using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBossAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform rigging;

   

    private void Awake()
    {
        if(anim == null) anim = GetComponent<Animator>();
    }



   
    
}
