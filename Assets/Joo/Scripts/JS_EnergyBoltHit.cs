using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]

public class JS_EnergyBoltHit : MonoBehaviour {

    public float power;

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
        Debug.Log("적 공격! " + power + " 만큼 데미지 입힘!");
    }	
}
