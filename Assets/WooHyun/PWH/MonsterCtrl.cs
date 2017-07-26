using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterCtrl : MonoBehaviour {

    public enum MonsterState { idle, trace, attack, die };

    public MonsterState monsterState = MonsterState.idle;

    Transform monsterTr;
    Transform playerTr;
    NavMeshAgent nvAgent;
    Animator animator;

    public float traceDist = 5.0f;
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
        while(!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if(dist <= attackDist)
            {
                nvAgent.ResetPath();
                monsterState = MonsterState.attack;
                
            }
            else if(dist <= traceDist)
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
        while(!isDie)
        {
            switch(monsterState)
            {
                case MonsterState.idle:

                    //nvAgent.Stop();
                    nvAgent.ResetPath();
                    animator.SetBool("IsTrace",false);
                    break;

                case MonsterState .trace:
                    //이동, 거리 ,움직이게
                    nvAgent.destination = playerTr.position;

                    nvAgent.Resume();

                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsTrace", true);
                    break;

                case MonsterState .attack:

                    nvAgent.ResetPath();

                    animator.SetBool("IsAttack", true);
                    break;

            }
            yield return null;
        }
    }
   

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Bullet")
        {
            hp -= 30;
            if (hp <= 0)
            {
                MonsterDie();
            }

            Destroy(coll.gameObject);
            animator.SetTrigger("IsHit");
        }
    }

    void MonsterDie()
    {
        StopAllCoroutines();

        isDie = true;

        monsterState = MonsterState.die;
        nvAgent.Stop();
        animator.SetTrigger("IsDie");
    }
}


