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
    }

    void Attack(GameObject enemy)
    {
        // 적을 공격
        enemy.GetComponent<Monster>().MonsterDamage(power);
    }	
}
