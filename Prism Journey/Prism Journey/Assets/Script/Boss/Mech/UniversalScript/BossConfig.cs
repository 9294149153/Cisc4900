using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss Config")]
public class BossConfig : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHP = 100f;
    public float minHP = 80f;

    public float p2MaxHP = 79.9f;
    public float p2MinHP = 50f;

}

