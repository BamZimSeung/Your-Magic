using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]

public class JS_EnergyBoltHit : MonoBehaviour {

    public int power;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            Attack(col.gameObject);
        }
        else if (col.CompareTag("BossBullet"))
        {
            col.gameObject.GetComponent<JS_BossRangeAttack>().Damaged(power);
        }
    }

    void Attack(GameObject enemy)
    {
        // 적을 공격
        enemy.GetComponent<JS_Monster>().MonsterDamage(power);
    }	
}
