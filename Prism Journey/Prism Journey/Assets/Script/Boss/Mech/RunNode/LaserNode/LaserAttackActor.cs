using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LaserAttackActor : MonoBehaviour, IAttackActor
{
    [SerializeField] private GameObject efftect;
    [SerializeField] private GameObject[] setActiveObject;
    [SerializeField] private SphereCollider collision;
    private BossContext context;

    [SerializeField] private float destoryTimerMax;
    private float destoryTimer;

    private void Awake()
    {
        if (context == null) context = FindAnyObjectByType(typeof(BossContext)).GetComponentInParent<BossContext>();
        if (collision == null) collision = GetComponentInParent<SphereCollider>();
        destoryTimer = 0f;
    }

    public Transform Transform => Transform;

    private void Update()
    {
        destoryTimer += Time.deltaTime;
        if (destoryTimer > destoryTimerMax)
        {
            Cleanup();

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (other.gameObject.tag == "Player")
        {
            IDamageable damage = other.gameObject.GetComponent<IDamageable>();
            if (damage != null)
            {
                damage.TakeDamage(context.remoteConfig.laserAttackConfig.damageDeal);
                collision.enabled = false;
            }
        }
    }
   
    public void Cleanup()
    {
        Destroy(gameObject);
        if (context != null)
        {
            context.currentHp -= 1f;
        }
    }

    public virtual void SetScale(Vector3 scale)
    {
        transform.localScale = Vector3.one;
        if (efftect != null)
        {
            efftect.transform.localScale = Vector3.one;
            efftect.transform.localScale=scale;
        }
    }
    public void Initialize(Vector3 position, Quaternion rotation )
    {
        efftect.transform.localPosition = Vector3.zero;
        efftect.transform.localScale = Vector3.one;
        transform.position= position;
        transform.rotation= rotation;
        if(setActiveObject!=null || setActiveObject.Length > 0)
        {
            collision.enabled = false;
            SetDeactive();
        }
       
    }

   
    public void SetActive()
    {
        this.enabled = true;
        foreach (var obj in setActiveObject)
        {
             obj.SetActive(true);
        }
        collision.enabled = true;
    }
    public void SetDeactive()
    {
        this.enabled = false;
        foreach (var obj in setActiveObject)
        {
            obj.SetActive(false);
        }
    }

   
}
