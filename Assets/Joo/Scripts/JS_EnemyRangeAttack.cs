using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_EnemyRangeAttack : MonoBehaviour {

    // 목표물
    public Transform target;

    // 처음 목표물 위치
    Vector3 firstTargetPos;

    // 히트 이펙트
    public GameObject hitEffectPrefab;

    // 속도
    public float speed;

    // 포물선을 그리는지 여부
    public bool isParabola;

    // 포물선 최고점 높이
    public float parabolaHeight;

    // 방향
    Vector3 dir;

    // 처음 거리
    float firstDist;

    // 현재 거리
    float tempDist;

	void Start () {
        firstTargetPos = target.position;
        firstDist = Vector3.Distance(firstTargetPos.x*Vector3.right + firstTargetPos.z*Vector3.forward, transform.position.x* Vector3.right + transform.position.z*Vector3.forward);
	}
	
	void Update () {
        SetPos();
	}

    void SetPos()
    {
        float upPos = 0;

        if (isParabola)
        {
            // y축이 0인 평면상의 목표물과의 거리
            tempDist = Vector3.Distance(firstTargetPos.x * Vector3.right + firstTargetPos.z * Vector3.forward, transform.position.x * Vector3.right + transform.position.z * Vector3.forward);
            // y = -4*H*(x - 0.5)^2 + H
            upPos = -4 * parabolaHeight *(tempDist / firstDist - 0.5f) * (tempDist / firstDist - 0.5f) + parabolaHeight;

            dir = (((firstTargetPos - firstTargetPos.y * Vector3.up) - (transform.position - transform.position.y * Vector3.up)).normalized * speed) * Time.deltaTime;
        }
        else
        {
            dir = (target.position - transform.position).normalized * speed * Time.deltaTime;
        }

        transform.position += dir;

        if (isParabola)
        {
            transform.position = new Vector3(transform.position.x, firstTargetPos.y + upPos, transform.position.z);
        }
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            // 플레이어에게 데미지 주기

            Destroy(gameObject);
        }
        else if(!col.CompareTag("Enemy")||!col.CompareTag("EnemyBullet"))
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
