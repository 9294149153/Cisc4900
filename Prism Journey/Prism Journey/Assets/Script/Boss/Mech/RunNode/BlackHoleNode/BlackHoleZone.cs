using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlackHoleStage
{
     Suck,Explosion , Finish
}
public class BlackHoleZone : MonoBehaviour ,IAttackActor
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float pullSpeed = 5f;
    [SerializeField] private float stopDistance = 0.2f;
    [SerializeField] private Transform firstHole;
    [SerializeField] private Transform secondHole;
    [SerializeField] private GameObject vfxEffect;
    [SerializeField] private ParticleSystem ps;
        private Transform playerInside;

    private BlackHoleStage currentStage;

    public BlackHoleStage CurrentStage ()=>currentStage;
    public Transform Transform => transform;

    private void Start()
    {
        currentStage=BlackHoleStage.Suck;
       if(vfxEffect != null)
        {
            vfxEffect.SetActive(false);
        }
    }
    private void Update()
    {

        switch (currentStage)
        {
            case BlackHoleStage.Suck:
               transform.localScale += Vector3.one * Time.deltaTime * 0.3f;
                SuckPlayer();
                MoveTowardTarget();
                BlackHoleSoundManager.PlaySound(SoundType.Suck, 0.5f);
                break;
            case BlackHoleStage.Explosion:
                   
                if (vfxEffect.activeInHierarchy == false)
                {
                    vfxEffect.SetActive (true);
                    firstHole.gameObject.SetActive (false );
                    secondHole.gameObject.SetActive(false);

                }
                BlackHoleSoundManager.PlaySound(SoundType.Explosion,0.5f);
                VFXAlive();
                break;
             case BlackHoleStage.Finish:
                Cleanup();
                break;
        }
       
       
    }
    public virtual void SetDimension(Vector3 scale)
    {
        transform.localScale = scale;
    }
    public void Initialize(Vector3 position, Quaternion rotation)
    {
        transform.localPosition = Vector3.one;
        transform.localPosition=position;
        transform.localRotation = rotation;
    }

    public void Cleanup()
    {
        Destroy(gameObject);
    }
    private void SuckPlayer()
    {
        if (playerInside == null)
            return;

        Vector3 toCenter = centerPoint.position - playerInside.position;
        toCenter.y = 0f; // ignore height if your game is on ground

        float distance = toCenter.magnitude;
        if (distance <= stopDistance)
            return;

        Vector3 move = toCenter.normalized * pullSpeed * Time.deltaTime;

        // prevent overshoot
        if (move.magnitude > distance)
            move = toCenter;

        playerInside.position += move;
    }
    
    private void MoveTowardTarget()
    {
        if (secondHole != null)
        {
            if ((Vector3.Distance(firstHole.transform.position, secondHole.position) > 0.05f))
            {
                secondHole.position = Vector3.MoveTowards(secondHole.position, firstHole.transform.position, 1f * Time.deltaTime*3f);
                return;
            }
            else
            {

                StartCoroutine(WaitTime(0.75f));
               
            }
        }
    }

    private void VFXAlive()
    {
        if (ps == null)
            return;

        if (!ps.IsAlive())
        {
            if (playerInside != null)
            {
                Debug.Log("hit Player");
                var player= playerInside.GetComponent<PlayerHealth>();
                if(player != null) { player.TakeDamage(20f); }
               
            }
            else
            {
                Debug.Log("did not hit Player");
            }
                currentStage = BlackHoleStage.Finish;
        }
    }

    private IEnumerator WaitTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        currentStage = BlackHoleStage.Explosion;
    }
    //Grap the Transform of the player
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.transform == playerInside)
            playerInside = null;
    }

    
}
