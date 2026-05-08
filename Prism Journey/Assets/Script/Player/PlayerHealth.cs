using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour,IDamageable
{

   [SerializeField] private float maxHealth = 8f;
    [Inspectable]private float currentHealth;
    
    private bool canTakeDamage;
    [SerializeField] private float damageCooldown = 0.4f;

    [Header("PlayerAnimation Refference && active Condition")]
    [SerializeField]private PlayerAnimationBrain[] playerAnim;
         private bool hasAnimation;

    [Header("List To set Deactive when player die")]
    [SerializeField] private MonoBehaviour [] playerScript;

    private PlayerColor playerColor;

    public int PlayerCurrentHealth { get => (int)currentHealth; }
    private void Awake()
    {
        if (playerColor == null) { playerColor = GetComponent<PlayerColor>(); }
    }
  
    private void OnEnable()
    {
        currentHealth = maxHealth;
        canTakeDamage = true;

        if (playerAnim == null)
        {
            Debug.LogWarning("PLayer Health Did not refference player Animation Brain");
        }
    }

    public void SetHealth(float health)
    {
        currentHealth=health;
        if (currentHealth <= 0)
        {
            SetPlayerStatusWhenDie(false);
        }
    }
    public void TakdeDamageWithColor(ColorIdentity color , float damage)
    {
        if (!canTakeDamage) return;

        if (currentHealth <= 0) return;

        if (color== playerColor.GetCurrentColorIdentity())
        {
            if (playerAnim != null)
            {
                for (int i = 0; i < playerAnim.Length; i++)
                {
                    playerAnim[i].PlayTakeDamage();
                }
                
            }
            currentHealth -= damage;
            Debug.Log("PLauer Current health" + currentHealth);
        }

        if (currentHealth <= 0)
        {

            currentHealth = 0; // set to 0 when player health get to negative          
            SetPlayerStatusWhenDie(false);
        }

        StartCoroutine(DamageCooldown(damageCooldown));

    }
    public void TakeDamage(float damage)
    {
        if (!canTakeDamage) return;

        if (currentHealth <= 0) return;

        //Call the TakeAttack Anim

        if (playerAnim != null)
        {
            for (int i = 0; i < playerAnim.Length; i++)
            {
                playerAnim[i].PlayTakeDamage();
            }
          
        }


        currentHealth -= damage;
       
        if (currentHealth <= 0) {

            currentHealth = 0; // set to 0 when player health get to negative
           
            SetPlayerStatusWhenDie(false);
        }
        Debug.Log("PLayer Take Damage"+"Current health is ="+currentHealth);
        StartCoroutine(DamageCooldown(damageCooldown)); 

     
    }

    public void OnTakeDamageWithNoColdown(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0; // set to 0 when player health get to negative
            SetPlayerStatusWhenDie(false);
        }
    }

    // provide Waiting ,system  run in backgound until this function finish before reach to next code
    //Ex: DamageCooldown() will wait "damageCooldown " second before the next code to execute 
    private IEnumerator DamageCooldown(float damageCooldown)
    {
        canTakeDamage = false;

        yield return new WaitForSeconds(damageCooldown);

        canTakeDamage = true;
    }

    public void SetPlayerStatusWhenDie(bool value)
    {
        if(playerScript.Length<0 || playerScript==null )
        {
            Debug.Log("Plz refference Such Script");
            return;
        }
        foreach (var script in playerScript)
        {
            script.enabled = value;
        }
        if (playerAnim.Length != 0)
        {
            for (int i = 0; i < playerAnim.Length; i++)
            {
                playerAnim[i].PlayDie();
            }
        }
        else
        {
            Debug.LogError($"[PlayerHealth] || did not have player Animation reference ||",this);
        }

            PlayerSoundManager.PlaySound(PlayerSoundType.Dead, 0.1f);
    }
}
