using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour,IDamageable
{

    private float maxHealth = 100f;
    private float currentHealth;

    private bool canTakeDamage;
    [SerializeField] private float damageCooldown = 1f;

    [Header("PlayerAnimation Refference && active Condition")]
    [SerializeField]private PlayerAnimationBrain playerAnim;
         private bool hasAnimation;

    [Header("List To set Deactive when player die")]
    [SerializeField] private MonoBehaviour [] playerScript;
    private void Start()
    {
        currentHealth = maxHealth;
        canTakeDamage = true;

        if (playerAnim == null)
        {
            Debug.LogWarning("PLayer Health Did not refference player Animation Brain");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(50f);
        }
       
    }

    public void TakeDamage(float damage)
    {
        if (!canTakeDamage) return;

        if (currentHealth <= 0) return;

        //Call the TakeAttack Anim
        if (playerAnim != null)
        {
            playerAnim.PlayTakeDamage();
            Debug.Log("Exectuted TakeAttackAnimation");
        }        
        

        currentHealth -= damage;

        if (currentHealth <= 0) {

            currentHealth = 0; // set to 0 when player health get to negative
            playerAnim.PlayDie();
            SetPlayerStatusWhenDie(false);
        }

        Debug.Log(currentHealth);

        StartCoroutine(DamageCooldown(damageCooldown));

     
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

    }
}
