using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 이 스크립트는 보스 컨트롤 입니다.
// 보스는 시작위치에서 보스포지션까지 날아온후 1바퀴 회전하고 지정된 위치까지 내려가고 Idle상태가됩니다.
// Idle상태에서 몇초 지연시킨 후 2콤보를 재생(부하 몬스터 생성)
// 생성후 Idle상태로 복귀. ( 플레이어가 모든 부하를 제거하면 -> 딜타임) GetHit재생, 
// Idle상태로 복귀( 몇초 후) roar 애니메이션 몇초간 재생하면서 원거리 미사일 발사(입과 날개주변)
// 원거리 미사일 발사 후 몇초간 딜타임.(맞을 경우 GetHit)
// 위 상황을 2회 반복 후 마지막 패턴 시작
// 3Combo 공격 -> 부하 몬스터 소환하며 장판 시전.
// 그리고 풀 딜타임.(GetHit) -> Death 애니메이션.

public class WB_BossCtnl : MonoBehaviour {
    public enum BossState
    {
        Move, // 지정된 위치까지 이동. Boss_State 0
        RotateMove, // 지정된 위치에서 가운데를 보면서 한바퀴 회전. Boss_State 1
        Idle, // 기본 상태. Boss_State 2
        Summon, // 부하 소환 Boss_State 3
        Fire, // 미사일 발사 Boss_State 4
        MagicBoard, // 장판 마법 시전. Boss_State 5
        Damage, // 데미지를 입음. Boss_State 6
        Death, // 죽음 Boss_State 7
    }

    public BossState my_Boss;
    public float moveSpeed = 2f; // 시작지점부터 목적지까지 이동할 보스스피드.
    public float radius; // 보스가 하늘에서 회전할 반지름.
    public float setha; // 보스가 움직일 각도.
    public GameObject Player;

    Vector3 originPos; // 보스위치 확인(목적지 이동 후 회전할떄 처음위치)

    public Transform startPos, desPos;
    Animator boss_anim;
	// Use this for initialization
	void Start () {
        my_Boss = BossState.Move;
        boss_anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(my_Boss)
        {
            case BossState.Move:
                BossMove();
                break;
            case BossState.RotateMove:
                BossRotateMove();
                break;
            case BossState.Idle:
                BossIdle();
                break;
        }
	}

    void BossMove()
    {
        // 시작지점부터 목적지까지 보스 이동. 일단 앞으로.
        boss_anim.SetInteger("Boss_State", 0);
        if (transform.position.z > desPos.position.z + radius)
        {
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            originPos = transform.position;
            my_Boss = BossState.RotateMove;
        }
    }

    void BossRotateMove()
    {
        boss_anim.SetInteger("Boss_State", 1); // 스테이트 변경.
        transform.LookAt(Player.transform); // 플레이어를 노려보자.
        if (setha > 360) // 360도면 끝
        {
            transform.position = Vector3.Lerp(transform.position, desPos.position, 0.05f);
            if(Vector3.Distance(transform.position, desPos.position) < 1f) // 도착
            {
                transform.position = desPos.position; // 위치 지정해주고
                my_Boss = BossState.Idle; // 보스상태 Idle.
            }
        }
        else
        {
            // 보스가 지정된 위치에 도달한 후, 반지름을 가지고 한바퀴 회전한다.(center -> Despos, 반지름 -> Radius)
            Vector3 NextPosition = desPos.position + new Vector3(radius * Mathf.Cos(setha), transform.position.y, radius * Mathf.Sin(setha));
            transform.position = Vector3.Lerp(transform.position, NextPosition, 0.05f);
            if (Vector3.Distance(transform.position, NextPosition) < 0.5f) // 일정거리 도달하면
            {
                transform.position = NextPosition;
                setha += 30; // 90도 추가.
            }
        }
    }

    void BossIdle()
    {
        boss_anim.SetInteger("Boss_State", 2);
    }
}
