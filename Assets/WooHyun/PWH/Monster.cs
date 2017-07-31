using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public GameObject corpsePrefab;

    public enum MonsterState { idle, trace, attack, die ,hit};

    public MonsterState monsterState = MonsterState.idle;

    Transform monsterTr;
    Transform playerTr;
    NavMeshAgent nvAgent;
    Animator animator;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;

    bool isDie = false;
    int hp = 90;

    void Start()
    {
        monsterTr = this.gameObject.GetComponent<Transform>();

        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();

        animator = this.gameObject.GetComponent<Animator>();


        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {
                monsterState = MonsterState.attack;
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
                
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.idle:
                    nvAgent.ResetPath();

                    animator.SetBool("IsTrace", false);
                    break;

                case MonsterState.trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.Resume();

                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsTrace", true);
                    break;

                case MonsterState.attack:

                    nvAgent.ResetPath();
                    animator.SetBool("IsAttack", true);
                    break;    
            }
            yield return null;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
            hp -= 90;
            if (hp <= 0)
            {
                // MonsterDie();
                Instantiate(corpsePrefab, transform.position, Quaternion.identity);
                Destroy(coll.gameObject);
            }
            else
            {             
                animator.SetTrigger("Hitting");
                monsterState = MonsterState.idle;            
            }
            
        }
    }

    void MonsterDie()
    {
        StopAllCoroutines();

        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.Stop();
        animator.SetTrigger("IsDie");
       
        Destroy(gameObject);
    }
}
