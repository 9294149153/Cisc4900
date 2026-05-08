using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerVisualFromMatieral : MonoBehaviour
{
    [SerializeField] private Material colorRed;
    [SerializeField] private Material colorBlue;
    [SerializeField] private SkinnedMeshRenderer[] renderers;

    [SerializeField] private ColorIdentity identityColorBlue;
    [SerializeField] private ColorIdentity identityColorRed;

    [SerializeField] private PlayerColor playerColor; // Code refferenced


    private void Awake()
    {
        if(playerColor==null) playerColor=GetComponentInParent<PlayerColor>();
    }
    private void Start()
    {
        if (playerColor == null) {
            Debug.Log("playerColor has not refference");
            return;
        }

        if (identityColorBlue == null) { Debug.Log("ColorIdentityBlue Did  set "); }
        if (identityColorRed == null) { Debug.Log("ColorIdentityRed Did  set ");  }
   

    }
  
    public Material GetColorMaterial(ColorIdentity color)
    {
        if (color == identityColorBlue)
        {
            return colorBlue;
        }
        if ((color == identityColorRed)){
            return colorRed;
        }

        return null;
    }

    public void SetPlayerMatiral(Material value)
    {
        foreach (var r in renderers)
        {
            if (r != null)
            {
                r.material = value;
            }
        }
    }

}
