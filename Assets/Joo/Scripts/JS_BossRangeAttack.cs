using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_BossRangeAttack : MonoBehaviour {

    // 체력
    public int hp;

    int tempHP;

    // 남은 분열 횟수
    public int restSplit = 2;

    // 분열된 공격 프리팹
    public GameObject rangeAttackPrefab;

    // 파괴 이펙트 프리팹
    public GameObject destroyEffectPrefab;

    // 분열 시 퍼지는 거리 범위
    public float splitRange;

    // 속도
    public float speed;

    // 시작 위치로 가는 시간 간격
    public float moveTimeGap;

    // 시작 위치로 가는 속도
    [Range(0, 1f)]
    public float moveSpeed;

    // 시작 시 이동하는 위치
    public Vector3 startPos;

    public bool isSetting = false;

    Vector3 dir;

    Transform target;

    void Start()
    {
        tempHP = hp;
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        StartCoroutine("SetStartPos");
    }

    void Update()
    {
        // 위치를 잡은 상태
        if (isSetting)
        {
            dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime);
        }
    }

    IEnumerator SetStartPos()
    {
        isSetting = false;

        WaitForSeconds wfs = new WaitForSeconds(moveTimeGap);

        while (Vector3.Distance(transform.position, startPos) > 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position, startPos, moveSpeed);
            yield return wfs;
        }

        transform.position = startPos;
        isSetting = true;
    }

    public void Damaged(int damage)
    {
        tempHP -= damage;

        if (tempHP <= 0)
        {
            Split();

            Destroy(gameObject);
        }
    }

    void Split()
    {
        if (restSplit > 0)
        {
            restSplit--;

            // 2개로 분할
            for (int i = 0; i < 2; i++)
            {
                GameObject rangeAttack = Instantiate(rangeAttackPrefab, transform.position, Quaternion.identity) as GameObject;

                // 크기 절반
                rangeAttack.transform.localScale = transform.localScale * 0.5f;

                JS_BossRangeAttack BRA = rangeAttack.GetComponent<JS_BossRangeAttack>();

                // 체력 절반
                BRA.hp = this.hp / 2;
                // 속도 2 증가
                BRA.speed = this.speed + 2;
                // 남은 횟수 입력
                BRA.restSplit = this.restSplit;
                // 시작 지점 입력
                BRA.startPos = new Vector3(transform.position.x + Random.Range(-splitRange, splitRange), transform.position.y + Random.Range(0, splitRange), transform.position.z + Random.Range(-splitRange, splitRange));
            }
        }

        GameObject des_Effct = Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity) as GameObject;
        des_Effct.transform.localScale = transform.localScale;
    }
}
