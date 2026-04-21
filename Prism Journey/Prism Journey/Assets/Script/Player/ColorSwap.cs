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



    [Header("Detection")]
    [SerializeField] private Transform detectionPoint;
    [SerializeField] private float radius = 4f;
    [SerializeField] private float distance = 0.1f;
    [SerializeField] private LayerMask colorInteractableLayer;

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

        if (playerColor == null) return;


        // if user click the left mouse button then read the hit object on the screen position 
        if (Input.GetMouseButtonDown(0) && canColorSwap)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, colorInteractableLayer))
            {
                Vector3 worldPos = hit.point;
                var var = hit.collider.gameObject.GetComponent<ColorObject>();
                if (var != null)
                {
                    TrySwapWithCursor(hit.transform);
                }
            }
            canColorSwap = false;
            StartCoroutine(CanColorSwap(swapColdown));
        }

    }

    public IEnumerator CanColorSwap(float coldwonTime)
    {
        yield return new WaitForSeconds(coldwonTime);
        canColorSwap=true;

    }

    //Try to swap color with player with the key press control 
    public void TrySwap()
    {
        //only return true when there are an interactable instance that was in the range and with different color identity
        //var target = playerColorDetection.FindClosestDifferentColor(playerColor.GetCurrentColorIdentity());
        var target = FindClosestDifferentColor(playerColor.GetCurrentColorIdentity(),detectionPoint);
        if (target == null) return;
        StartCoroutine( PlayEffect(1));
        ColorIdentity oldColor=target.GetColorIdentity();
        target.SetColor(playerColor.GetCurrentColorIdentity());
        playerColor.SetColor(oldColor);
       
        
    }

    //try to swap color with player with the mouse click control
    public void TrySwapWithCursor(Transform targeting)
    {
        var target = FindClosestDifferentColor(playerColor.GetCurrentColorIdentity(), targeting);
        if (target == null) return;
        StartCoroutine(PlayEffect(1));
        ColorIdentity oldColor = target.GetColorIdentity();
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


    public void SwapToOppositeColor(ColorIdentity color )
    {
        playerColor.SetColor(color);
    }





    public IColorInteractable FindClosestDifferentColor(ColorIdentity playerCurrentColor,Transform detectionPoint)
    {
        //Detect all ColorInterableObject if in range
        RaycastHit[] colorInteractableHits = Physics.SphereCastAll(detectionPoint.position, radius, detectionPoint.forward, distance, colorInteractableLayer);

        if (colorInteractableHits.Length == 0) return null;

        Array.Sort(colorInteractableHits, (a, b) => a.distance.CompareTo(b.distance));


        foreach (RaycastHit hit in colorInteractableHits)
        {
            IColorInteractable target = hit.collider.GetComponentInParent<IColorInteractable>();
            if (target == null) continue;

            //Hit object has different color than player 
            if (target.GetColorIdentity() != playerCurrentColor)
                return target;
        }

        return null;
    }
}
