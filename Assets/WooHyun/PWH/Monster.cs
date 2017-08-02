using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public GameObject corpsePrefab;

    public enum MonsterState { Idle, Trace, Attack, Die , Damage};

    public MonsterState monsterState = MonsterState.Idle;

    Transform monsterTr;
    Transform playerTr;
    Rigidbody monsterRB;
    NavMeshAgent nvAgent;
    Animator animator;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;

    public float damageDelayTime = 0.2f;

    bool isDie = false;
    public int hp = 90;

    void Awake()
    {
        monsterTr = gameObject.GetComponent<Transform>();

        monsterRB = gameObject.GetComponent<Rigidbody>();

        nvAgent = gameObject.GetComponent<NavMeshAgent>();

        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        playerTr = GameObject.Find("Player").GetComponent<Transform>();

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
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
                    animator.SetBool("IsAttack", true);
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

    public void MonsterDamage(int damage)
    {
        hp -= damage;

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

        isDie = true;
        monsterState = MonsterState.Die;
        nvAgent.isStopped = true;
        Instantiate(corpsePrefab, transform.position, Quaternion.identity);
       
        Destroy(gameObject);
    }
}
