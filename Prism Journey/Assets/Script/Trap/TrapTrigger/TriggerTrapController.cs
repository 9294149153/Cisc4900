using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class TriggerTrapController : MonoBehaviour
{
    public enum FromDifferentTouch
    {
        Player,
       PushBox
    }
    [SerializeField] private GameObject vfx;
    [SerializeField] private Transform targetFinalPosition;
    [SerializeField] private GameObject target;
    [SerializeField] private Collider collider;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private float speed;

    private bool hasActive;

    [SerializeField] private FromDifferentTouch fromDifferentTouch;

    private void Awake()
    {
       collider = GetComponent<Collider>();
        if(collider != null)
        {
            collider.isTrigger=true;
        }

        //The the vfx effect off at start of the game and will active as player trigger 
        if(vfx.activeInHierarchy)
        {
            vfx.SetActive(false);
        }
        source = GetComponent<AudioSource>();

       
    }
    private void Start()
    {
        hasActive = false;
        source.playOnAwake = false;
    }

    private void Update()
    {

        if (hasActive == true)
        {

            if (Vector3.Distance(target.transform.position, targetFinalPosition.position) > 0.05f)
            {
                target.transform.position = Vector3.MoveTowards(
                    target.transform.position,
                    targetFinalPosition.position,
                    speed * Time.deltaTime
                );
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (fromDifferentTouch)
        {
            case FromDifferentTouch.Player:
                if (other.tag == "Player" && hasActive == false)
                {
                    hasActive = true;
                    vfx.SetActive(true);

                    if (clip != null)
                    {
                        source.PlayOneShot(clip, 1);

                    }

                }
                break;
               case FromDifferentTouch.PushBox:
                if(other.CompareTag("PushBox") && hasActive == false)
                {
                    hasActive = true;
                    vfx.SetActive(true);

                    if (clip != null)
                    {
                        source.PlayOneShot(clip, 1);

                    }
                }
                break;
            
        }

        Debug.Log(other.name);
        
    }
}
