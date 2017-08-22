using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// 부하 스크립트
// 부하는 생성될때 일련의 에니메이션을 재생하고 네비메시를 이용 플레이어를 향해 뛰어간다
// 공격 범위안에 들어오면 공격 에니메이션 재생 -> Idle -> 다시 범위안이면 재생
// 얻어맞으면 hit
// 체력이 다 닳으면 Die

public class WB_BuhaCtnl : MonoBehaviour {
    public enum BuhaState
    {
        Ready, // 소환돼고 도발 -> idle상태
        Idle, // 일반 대기상태
        Move, // 플레이어를 향해 이동
        Attack, // 플레이어 공격
        Hit, // 맞을때
        Death, // 죽을때
    }

    public BuhaState my_buha;
    Animator anim;

    public float cur_time = 0;
    public float Readytime;
    public float AttackDelay = 3f;
    public float AttackRange = 3f;
    public float moveSpeed = 2f;
    GameObject Player;

    NavMeshAgent agent;


    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        my_buha = BuhaState.Ready;
        Player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update() {
        switch (my_buha)
        {
            case BuhaState.Ready:
                DemonReady();
                break;
            case BuhaState.Idle:
                DemonIdle();
                break;
            case BuhaState.Move:
                DemonMove();
                break;
            case BuhaState.Attack:
                DemonAttack();
                break;
            case BuhaState.Hit:
                DemonHit();
                break;
            case BuhaState.Death:
                DemonDeath();
                break;
        }
    }

    void DemonReady()
    {
        anim.SetInteger("BuhaState", 0); // 생성 -> 도발.
        cur_time += Time.deltaTime;
        if(cur_time > Readytime)
        {
            my_buha = BuhaState.Idle;
        }
    }
    void DemonIdle() // 플레이어와의 거리가 일정거리 이상이면 run, 이하면 attack 스테이트로 옮김.
    {
        anim.SetInteger("BuhaState", 1); // Idle 상태.
        if(Vector3.Distance(transform.position, Player.transform.position) >= AttackRange)
        {
            my_buha = BuhaState.Move;
        }
        else
        {
            my_buha = BuhaState.Attack;
        }
    }
    void DemonMove()
    {
        anim.SetInteger("BuhaState", 2); // Move 애니메이션 재생.
        agent.SetDestination(Player.transform.position); // 목적지를 프레이어로 설정.
        if (Vector3.Distance(transform.position, Player.transform.position) < AttackRange + 0.1f)
        {
            print("KKKK");
            my_buha = BuhaState.Idle;
        }
    }
    void DemonAttack()
    {

    }
    void DemonHit()
    {

    }
    void DemonDeath()
    {

    }

}
