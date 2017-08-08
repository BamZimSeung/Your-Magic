using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_IceSpear : MonoBehaviour {

    // 1. 적에게 충돌
    // 2. 해당 적에게 데미지를 입힌다.
    // 3. 적 4명을 검출
    // 4. 순서대로 찾아가서 데미지를 입힌다.

    // 공격력
    public int power;

    // 속도
    public float speed;

    // 적 검출 범위
    public float searchRange;

    // 적을 따라가는 시간 사이
    public float followGap;

    // 적에게 맞았다고 판단되는 거리
    public float hitJudgeRange;

    // 적에게 돌아가는 정도
    [Range(0,1f)]
    public float rotateValue;
    
    // 첫번째로 맞았는지 여부
    public bool isHit = false;

    // 발사됐는지 여부
    public bool isShot = false;

    public List<Transform> enemysTrans;

	void Start () {
        isHit = false;
        isShot = false;
    }

    void Update()
    {
        if (isShot)
        {
            transform.Translate(transform.forward * speed * Time.deltaTime,Space.World);
            gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        // 적과 충돌할 시
        if (col.CompareTag("Enemy"))
        {
            if (!isHit)
            {
                isHit = true;

                // 근처 적을 검출
                Ray ray = new Ray(transform.position, transform.position);
                RaycastHit[] hitInfos = Physics.SphereCastAll(ray, searchRange, 0f, 1 << LayerMask.NameToLayer("Enemy"));

                int min = hitInfos.Length < 4 ? hitInfos.Length : 4;

                for (int i = 0; i < min; i++)
                {
                    enemysTrans.Add(hitInfos[i].transform);
                }

                StartCoroutine("FollowEnemies");
            }

            // 적에게 데미지를 준다.
            col.gameObject.GetComponent<Monster>().MonsterDamage(power);
        }
    }

    IEnumerator FollowEnemies()
    {
        WaitForSeconds wfs = new WaitForSeconds(followGap);

        int i = 0;

        while(i < enemysTrans.Count)
        {
            yield return wfs;
            // 적에게 부딪혔다고 판단되는 거리가 되지 않았다면
            while (enemysTrans[i]!=null && Vector3.Distance(transform.position, enemysTrans[i].position) > hitJudgeRange)
            {
                // 적을 향해 회전
                transform.forward = Vector3.Lerp(transform.forward, enemysTrans[i].position - transform.position, rotateValue);
                yield return wfs;
            }

            i++;
        }

        Destroy(gameObject);
    }
}
