using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectIdentity : MonoBehaviour
{


    [SerializeField] private float destroyTimerMax = 1f;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= destroyTimerMax)
        {
            Destroy(gameObject); 
        }
    }
}
