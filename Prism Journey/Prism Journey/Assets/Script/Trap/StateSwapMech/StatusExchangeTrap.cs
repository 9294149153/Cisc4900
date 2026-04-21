using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StatusExchangeTrap : MonoBehaviour
{
    private Collider boxCollider;


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider.isTrigger == false)
        {
            boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player=other.gameObject;
            PlayerColor color = player.GetComponent<PlayerColor>();
            ColorSwap colorSwap=player.GetComponent<ColorSwap>();

            if (player != null )
            {
                colorSwap.SwapToOppositeColor(color.GetCurrentColorIdentity().GetSwapColor());
            }
        }
    }
}
