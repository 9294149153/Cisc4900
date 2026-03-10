
using UnityEngine;
/*
[CreateAssetMenu(menuName = "Enemy/Attacks/Range")]
public class RangeAttack : AttackDefinition
{


    public GameObject projectilePrefab;
    public float lifeTime = 0.25f;

    public Vector3 spawnEuler = new Vector3(90f, 0f, 0f); // adjust if needed

    public override void Perform(EnemyStateManager enemy)
    {
        if (!projectilePrefab || !enemy.attackSpawnPoint) return;

        var rot = enemy.transform.rotation * Quaternion.Euler(spawnEuler);
        var obj = Instantiate(projectilePrefab, enemy.attackSpawnPoint.position, rot);
        Destroy(obj, lifeTime);
    }



}
*/