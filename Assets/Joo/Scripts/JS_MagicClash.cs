using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SphereCollider))]
public class JS_MagicClash : MonoBehaviour {

    // 히트 이펙트 프리팹
    public GameObject hitPrefab;

    // 부딪혔다면
    void OnTriggerEnter(Collider col)
    {
        // 부딪힌게 Player가 아니라면
        if (!col.CompareTag("Player")||!col.CompareTag("Bullet"))
        {
            // 터짐
            CallHitEffect();
        }
    }

    void CallHitEffect()
    {
        // 히트 이펙트 생성
        Instantiate(hitPrefab, transform.position, Quaternion.identity);
        // 현재 오브젝트 제거
        Destroy(gameObject);
    }
}
