using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Player;

public class ColorSwap : MonoBehaviour
{
    private PlayerColor playerColor;
    private ColorDetection playerColorDetection;

    //Refference to the VFX
    [SerializeField] private GameObject parentOfPraticleEfffect;
    public ParticleSystem praticleEffectPrefab;

    [Header("Timer For ColorSwap")]
    private bool canColorSwap;
    [SerializeField] private float swapColdown = 2f;
    
    private void Awake()
    {
        playerColor=GetComponent<PlayerColor>();
        playerColorDetection=GetComponent<ColorDetection>();
    }
    private void Start()
    {
        canColorSwap = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&& canColorSwap)
        {
            TrySwap();
            canColorSwap=false;
            StartCoroutine(CanColorSwap(swapColdown));
        }

        
    }

    public IEnumerator CanColorSwap(float coldwonTime)
    {
        yield return new WaitForSeconds(coldwonTime);
        canColorSwap=true;

    }
    public void TrySwap()
    {
        //only return true when there are an interactable instance that was in the range and with different color identity
        var target = playerColorDetection.FindClosestDifferentColor(playerColor.GetCurrentColorIdentity());
        if (target == null) return;
        StartCoroutine( PlayEffect(1));
        ColorIdentity oldColor=target.GetColorIdentity();
        target.SetColor(playerColor.GetCurrentColorIdentity());
        playerColor.SetColor(oldColor);
       
        
    }


    IEnumerator PlayEffect(float delay)
    {
        parentOfPraticleEfffect.SetActive(true);
        praticleEffectPrefab.Play();
        yield return new WaitForSeconds(delay);
        praticleEffectPrefab.Clear();
        parentOfPraticleEfffect.SetActive(false);
    }


}
