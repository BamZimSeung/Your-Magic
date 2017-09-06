using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JS_Monster : MonoBehaviour
{
    // 지상 몬스터인지 여부
    public bool isGroundMon = true;

    // 원거리 몬스터인지 여부
    public bool isRangeMon = false;

    // 보스인지의 여부
    public bool isBoss = false;

    // 공격 위치
    public Transform attackPos;

    // 시체 프리팹
    public GameObject corpsePrefab;

    // 원거리 공격 프리팹
    public GameObject rangeAttackPrefab;

    public enum MonsterState { Idle, Trace, Attack, Die , Damage};

    // 몬스터 상태
    public MonsterState monsterState = MonsterState.Idle;

    public WB_BossCtnl wb;

    Transform monsterTr;
    Transform playerTr;
    Rigidbody monsterRB;
    NavMeshAgent nvAgent;
    Animator animator;

    // 추적 가능 범위
    public float traceDist = 10.0f;
    // 공격 가능 범위
    public float attackDist = 2.0f;

    // 데미지 입는 시간
    public float damageDelayTime = 0.2f;

    // 공격 간의 시간
    public float attackDelay = 1f;

    bool isDie = false;
    public int hp = 90;
    float currentTime = 0f;

    public enum MonProperty { Normal, Fire, Ice, Electric};

    public MonProperty mProperty = MonProperty.Normal;

    void Awake()
    {
        if (!isBoss)
        {
            monsterTr = gameObject.GetComponent<Transform>();

            monsterRB = gameObject.GetComponent<Rigidbody>();

            nvAgent = gameObject.GetComponent<NavMeshAgent>();

            animator = gameObject.GetComponent<Animator>();
        }
    }

    void Start()
    {
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        if (isGroundMon && !isBoss)
        {
            nvAgent.enabled = true;
            StartCoroutine(CheckMonsterState());
            StartCoroutine(MonsterAction());
        }
    }

    IEnumerator CheckMonsterState()
    {
        float damageTime = 0f;

        while (!isDie)
        {
            yield return null;

            if (monsterState != MonsterState.Damage)
            {
                float dist = Vector3.Distance(playerTr.position, monsterTr.position);

                if (dist <= attackDist)
                {
                    monsterState = MonsterState.Attack;
                }
                else if (dist <= traceDist)
                {
                    monsterState = MonsterState.Trace;
                }
                else
                {
                    monsterState = MonsterState.Idle;
                }
            }
            else
            {
                damageTime += Time.deltaTime;
                if(damageTime > damageDelayTime)
                {
                    monsterState = MonsterState.Idle;
                    monsterRB.velocity = Vector3.zero;
                    damageTime = 0f;
                }
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.Idle:
                    nvAgent.ResetPath();
                    animator.SetBool("IsTrace", false);
                    animator.SetBool("IsAttack", false);
                    break;

                case MonsterState.Trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = false;

                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsTrace", true);
                    break;

                case MonsterState.Attack:
                    nvAgent.ResetPath();
                    nvAgent.isStopped = true;
                    if (delayTime())
                    {
                        animator.SetBool("IsAttack", true);
                    }
                    else
                    {
                        animator.SetBool("IsAttack", false);
                    }
                    animator.SetBool("IsTrace", false);
                    break;

                case MonsterState.Damage:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsTrace", false);
                    animator.SetBool("IsAttack", false);
                    break;
            }
            yield return null;
        }
    }

    bool delayTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime < attackDelay)
        {
            return false;
        }
        else
        {
            currentTime = 0f;
            return true;
        }
    }

    // 몬스터가 데미지를 입음
    public void MonsterDamage(int damage, MonProperty _mProperty)
    {
        if(mProperty != _mProperty)
        {
            hp -= damage/4;
        }
        else
        {
            hp -= damage;
        }

        if (!isBoss)
        {
            if (DieCheck())
            {
                MonsterDie();
            }
            else
            {
                animator.SetTrigger("Damage");
                monsterState = MonsterState.Damage;
            }
        }
        else
        {
            if (DieCheck())
            {
                MonsterDie();
            }
            else
            {
                // 보스 데미지 입음
                wb.BossDamaged();
            }
        }
    }

    bool DieCheck()
    {
        if(hp <= 0)
        {
            // 죽음
            return true;
        }
        else
        {
            return false;
        }
    }

    void MonsterDie()
    {
        StopAllCoroutines();

        if (!isDie)
        {
            isDie = true;
            monsterState = MonsterState.Die;

            if (!isBoss)
            {
                Instantiate(corpsePrefab, transform.position, Quaternion.identity);

                monsterState = MonsterState.Die;

                Instantiate(corpsePrefab, transform.position, Quaternion.identity);

                if (isGroundMon)
                {
                    JS_StageCtrl.Instance.DecreaseMonTempCount(JS_StageCtrl.MonType.Ground);
                }
                else
                {
                    JS_StageCtrl.Instance.DecreaseMonTempCount(JS_StageCtrl.MonType.Air);
                }

                Destroy(gameObject);
            }
            else
            {
                wb.BossDeath();
                JS_StageCtrl.Instance.DecreaseMonTempCount(JS_StageCtrl.MonType.Ground);
            }
        }
    }

    // 몬스터가 공격함
    public void MonsterAttack()
    {
        if (isRangeMon)
        {
            GameObject rangeAttack = Instantiate(rangeAttackPrefab, attackPos.position, Quaternion.identity) as GameObject;
            rangeAttack.GetComponent<JS_EnemyRangeAttack>().SetTarget(playerTr);
        }
        else
        {

        }
    }
}
