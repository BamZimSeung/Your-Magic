﻿using System.Collections;
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
    private AudioClip[] bossClips = new AudioClip[8];

    public MAR_PlayerStats ps;

    public AudioSource bossAudio;
    public enum bossAudioState
    {
        wing,
        grr,
        summ,
        board,
        roar,
        hit,
        death,
        dealtime,
    }
    public bossAudioState boss_audio_state;
    private void Awake()
    {
        bossAudio.Stop();
        bossClips[0] = Resources.Load("boss_wing", typeof(AudioClip)) as AudioClip; // 날아올때
        bossClips[1] = Resources.Load("boss_grr", typeof(AudioClip)) as AudioClip; // ㄱㄹㄹ
        bossClips[2] = Resources.Load("boss_summ", typeof(AudioClip)) as AudioClip; // 소환
        bossClips[3] = Resources.Load("boss_board", typeof(AudioClip)) as AudioClip; // 장판
        bossClips[4] = Resources.Load("boss_roar", typeof(AudioClip)) as AudioClip; // 미사일
        bossClips[5] = Resources.Load("hit_boss", typeof(AudioClip)) as AudioClip; // 타격
        bossClips[6] = Resources.Load("boss_death", typeof(AudioClip)) as AudioClip; // 사망
        bossClips[7] = Resources.Load("boss_breathing", typeof(AudioClip)) as AudioClip; // 딜타임.
    }

    public void BossPlay(bossAudioState bossState)
    {
        if (!bossAudio.isPlaying)
        {
            bossAudio.clip = bossClips[(int)bossState];
            bossAudio.loop = false;
            bossAudio.playOnAwake = false;
            bossAudio.Play();
        }
    }
    public enum BossState
    {
        Move, // 지정된 위치까지 이동. Boss_State 0
        RotateMove, // 지정된 위치에서 가운데를 보면서 한바퀴 회전. Boss_State 1
        Idle, // 기본 상태. Boss_State 2
        Summon, // 부하 소환 Boss_State 3  // 2HitCombo
        MagicBoard, // 솬 + 장판 마법 시전. Boss_State 4 // 3HitCombo
        Fire, // 미사일 발사 Boss_State 5 // roar
        Damage, // 데미지를 입음. Boss_State 6
        Death, // 죽음 Boss_State 7
        DealTime, // 딜타임 Boss_state 8
    }

    public BossState my_Boss;
    public bool[] attackCheck; // 보스 공격 패턴 확인용, true->사용함, false->사용가능.
    public float attackDelay = 5f; // Idle-> 공격시간.
    public float dealTime = 15f;
    public float currentTime; // 현재시간 컨트롤
    public int attackChoice = 0; // 공격패턴설정
    public GameObject WeakPoint; // 딜타임에 활성화. 약점 이부분을 때려야함.

    Animator boss_anim;

    // 이동 관련 변수
    public float moveSpeed = 2f; // 시작지점부터 목적지까지 이동할 보스스피드.
    public float radius; // 보스가 하늘에서 회전할 반지름.
    public float setha; // 보스가 움직일 각도.
    public GameObject Player;
    public Transform startPos, desPos;

    // 소환마법 관련 변수
    public float summonPatternTime = 15f; // 소환패턴시간.
    public float summonDelay = 0.5f; // 소환딜레이
    public bool summonSwitch = true; // 소환스위치
    public int summonNumber = 5; // 소환개체수
    public int cur_summonNumber = 0;
    public GameObject buhaPrefab; // 소환 할 자식
    public Transform SummonPos; // 소환 위치지정.
    

    Vector3 originPos; // 보스위치 확인(목적지 이동 후 회전할떄 처음위치)


    // 이제 magicboard 관련 변수
    //public GameObject safeboardPrefab;
    public GameObject attackboardPrefab;
    public GameObject magicBoardPrefab;

    public float missTime = 5f;

    public Transform boardPos_1st, boardPos_2nd, boardPos_3rd;
    public float boardPatternTime = 15f; // 장판 마법 패턴 시간
    public bool boardSwitch = true;
    public GameObject portal_left, portal_mid, portal_right; // 포탈 위치
    public float portaldelay = 5f;
    public int player_pos; // 플레이어의 현재 위치 파악. (0 = left, 1 = mid, 2 = right)
    public int moveZoneTemp = 0;

    // fire 관련 변수
    public float firPatternTime = 15f;
    public GameObject bulletPrefab;
    public int bulletNumber = 15;
    public int cur_bulletNumber;
    public bool fireSwitch = true;
    public float fireDelay = 1f;
    public Transform firePos;
    public float fireDeltha = 30f;

  /*  public float bossMaxHp = 100f;
    public float cur_bossHp;*/


    void Start () {
        my_Boss = BossState.Move;
        boss_anim = GetComponent<Animator>();
        attackCheck = new bool[3]; // 3가지 공격..? / 0-> 2combo 몹소환, 1-> 3combo 몹 + 장판 , 2 -> roar.
        // cur_bossHp = bossMaxHp;
        InitCheck();
        portal_left.SetActive(false);
        portal_mid.SetActive(false);
        portal_right.SetActive(false);
        player_pos = 1; // 시작은 미드.
       
        
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
            case BossState.Summon:
                
                BossSummon();
                break;
            case BossState.MagicBoard:
                
                BossMagicBoard();
                break;
            case BossState.Fire:
                
                BossFire();
                break;
            case BossState.DealTime:
                BossDealTime();
                break;
            /*case BossState.Death:
                BossDeath();
                break;*/
        }
	}

    void BossMove()
    {
        // 시작지점부터 목적지까지 보스 이동. 일단 앞으로.
        boss_anim.SetInteger("Boss_State", 0);
        BossPlay(bossAudioState.wing);
        if (transform.position.x < desPos.position.x + radius)
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
        BossPlay(bossAudioState.wing);
        boss_anim.SetInteger("Boss_State", 1); // 스테이트 변경.
        transform.LookAt(Player.transform); // 플레이어를 노려보자.
        if (setha > 90) // 240도면 끝
        {
            transform.position = Vector3.Lerp(transform.position, desPos.position, 0.05f);
            if(Vector3.Distance(transform.position, desPos.position) < 1f) // 도착
            {
                transform.position = desPos.position; // 위치 지정해주고
                currentTime = 0f;
                bossAudio.Stop();
                my_Boss = BossState.Idle; // 보스상태 Idle.
            }
        }
        else
        {
            // 보스가 지정된 위치에 도달한 후, 반지름을 가지고 한바퀴 회전한다.(center -> Despos, 반지름 -> Radius)
            Vector3 NextPosition = desPos.position + new Vector3(radius * Mathf.Cos(setha), transform.position.y-desPos.position.y, radius * Mathf.Sin(setha));
            transform.position = Vector3.Lerp(transform.position, NextPosition, 1.5f * Time.deltaTime);
            if (Vector3.Distance(transform.position, NextPosition) < 10f) // 일정거리 도달하면
            {
                transform.position = NextPosition;
                originPos = transform.position;
                setha += 30; // 90도 추가.
            }
        }
    }

    void BossIdle() // idle에서 어택딜레이를 기다린 다음 다음 공격을 설정합니다.
    {
        BossPlay(bossAudioState.grr);
        boss_anim.SetInteger("Boss_State", 2);
        AttackDelayChoice();
        transform.LookAt(GameObject.FindWithTag("Player").transform);
    }

    void AttackDelayChoice()
    {
        currentTime += Time.deltaTime;
        if (currentTime > attackDelay)
        {
            if(checkFull())
            {
                InitCheck();
            }
            while (attackCheck[attackChoice])
            {
                attackChoice = Random.Range(0, 3);
            }
            attackCheck[attackChoice] = true;
            currentTime = 0;
            bossAudio.Stop();
            switch (attackChoice)
            {
                case 0:
                    my_Boss = BossState.Summon;
                    break; // 소환.
                case 1:
                    my_Boss = BossState.MagicBoard;
                    break; // 솬 + 장판.
                case 2:
                    my_Boss = BossState.Fire;
                    break; // 미사일.
            }
        }
    }

    void InitCheck()
    {
        for(int i=0; i<3; i++)
        {
            attackCheck[i] = false;
        }
    }

    bool checkFull()
    {
        int count = 0;
        for(int i=0; i<3; i++)
        {
            if(attackCheck[i]) // 사용했으면 count++.
            {
                count++;
            }
        }
        if (count >= 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void BossSummon()
    {
        boss_anim.SetInteger("Boss_State", 3);
        BossPlay(bossAudioState.summ);
        if (summonSwitch && cur_summonNumber <= summonNumber) // 소환스위치 켜있고 숫자가 정해진 것보다 아래일때만 생성.
        {
            summonSwitch = false; // 소환 쉬자.
            StartCoroutine("Summon");
        }

        currentTime += Time.deltaTime;
        if (currentTime > summonPatternTime) // 딜타임 시작.
        {
            currentTime = 0;
            cur_summonNumber = 0; // 현재 소환갯수 초기화.
            summonSwitch = true; // 스위치 초기화.
            bossAudio.Stop();
            my_Boss = BossState.DealTime;
        }
    }
    void BossMagicBoard() // 장판 공격 패턴 = 소환 + 장판생성.
    {
        boss_anim.SetInteger("Boss_State", 4);
        BossPlay(bossAudioState.board);
         // 꼭 가야할 지점.
        // 안전 보드 배치.
        if (boardSwitch) // 아직 소환안했으면
        {
            int safezone = Random.Range(0, 3);// 0~2번중 안전한 지역 선택.
            if(safezone == player_pos) // 꼭 가야하는 지점.
            {
                safezone = (safezone + 1) % 3;
            }
            moveZoneTemp = safezone;
            // 보드 생성.
            GameObject attackboard_1st, attackboard_2nd;
            //GameObject safeboard = Instantiate(safeboardPrefab);
            attackboard_1st = Instantiate(attackboardPrefab);
            attackboard_2nd = Instantiate(attackboardPrefab);

            switch (safezone) // 소환하고
            {

                case 0: //0자리에 안전배치
                    //safeboard.transform.position = boardPos_1st.position;
                    attackboard_1st.transform.position = boardPos_2nd.position;
                    attackboard_2nd.transform.position = boardPos_3rd.position;
                    StartCoroutine(SummonMagicBoard(attackboard_1st.transform, attackboard_2nd.transform));
                    break;
                case 1: // 1자리에 안전배치
                    //safeboard.transform.position = boardPos_2nd.position;
                    attackboard_1st.transform.position = boardPos_1st.position;
                    attackboard_2nd.transform.position = boardPos_3rd.position;
                    StartCoroutine(SummonMagicBoard(attackboard_1st.transform, attackboard_2nd.transform));
                    break;
                case 2: //2자리에 안전배치
                    //safeboard.transform.position = boardPos_3rd.position;
                    attackboard_1st.transform.position = boardPos_1st.position;
                    attackboard_2nd.transform.position = boardPos_2nd.position;
                    StartCoroutine(SummonMagicBoard(attackboard_1st.transform, attackboard_2nd.transform));
                    break;
            }
            boardSwitch = false; // 스위치 끄기.-> 더이상 장판 소환 x
        }


        if (currentTime > portaldelay) // 포탈 온액티브.
        {
            portal_left.SetActive(false);
            portal_mid.SetActive(false);
            portal_right.SetActive(false);
        }
        else
        {
            switch (moveZoneTemp)
            {
                case 0: // 왼쪽일때
                    portal_left.SetActive(true);
                    portal_left.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case 1: // 중앙일때
                    portal_mid.SetActive(true);
                    portal_mid.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case 2: // 오른쪽일때
                    portal_right.SetActive(true);
                    portal_right.transform.GetChild(0).gameObject.SetActive(true);
                    break;
            }
        }

        currentTime += Time.deltaTime;
        if(currentTime >= missTime && player_pos == moveZoneTemp)
        {
            GameObject.FindWithTag("Player").GetComponent<MAR_PlayerStats>().IsboardOver();
            
        }

        if (currentTime >= boardPatternTime) // 장판 마법 패턴이 끝나면 상태 초기화 -> 딜타임.
        {
            boardSwitch = true;
            currentTime = 0f;
            cur_summonNumber = 0;
            bossAudio.Stop();
            my_Boss = BossState.DealTime;
        }
        
    } // 장판마법패턴(소환몹 공격 + 워프해라)
    void BossFire()
    {
        BossPlay(bossAudioState.roar);
        boss_anim.SetInteger("Boss_State", 5);
        if(fireSwitch &&cur_bulletNumber <= bulletNumber) // 스위치 켜져있고 불렛넘버보다 작으면
        {
            fireSwitch = false; // 총알 생성 끄고.
            StartCoroutine("Fire");
        }

        currentTime += Time.deltaTime;
        if(currentTime > firPatternTime) // 파이어 패턴 끝나면 초기화.
        {
            currentTime = 0f;
            cur_bulletNumber = 0;
            fireSwitch = true;
            bossAudio.Stop();
            my_Boss = BossState.DealTime;
        }
        
    } // 미사일발사패턴(방어해라)

    void BossDealTime()
    {
        BossPlay(bossAudioState.dealtime);
        WeakPoint.SetActive(true); // 약점활성화
        boss_anim.SetInteger("Boss_State", 8); // 딜타임!.

        currentTime += Time.deltaTime;
        if(currentTime > dealTime) // 딜타임이 끝나면
        {
            bossAudio.Stop();
            my_Boss = BossState.Idle; // Idle.
            WeakPoint.SetActive(false); // 약점 off.
            currentTime = 0;
        }
    }

    public void BossDamaged() // 맞으면?
    {
        boss_anim.SetTrigger("Gethit");
        /*cur_bossHp -= 50;
        if (cur_bossHp <= 0) // 죽으면?
        {
            bossAudio.Stop();
        }*/
    }

    
    public void BossDeath()
    {
        MAR_PsCtrl.instance.GameClear();
        my_Boss = BossState.Death;
        BossPlay(bossAudioState.death);
        boss_anim.SetInteger("Boss_State", 7); // 보스상태 죽음으로 변경.
        Debug.Log("보스 죽어유~");
        // 몇초후 삭제.
        Destroy(this.gameObject, 3.0f);

    }

    IEnumerator Summon() // 부하 소환.
    {
        GameObject buha = Instantiate(buhaPrefab); // 생성.
        Vector3 sumPos = SummonPos.transform.position + new Vector3(0,0, Random.Range(-12, 12)); // 뿌리자.
        buha.transform.position = sumPos; // 생성위치 지정.
        Debug.Log(buha.transform.position);
        cur_summonNumber++; // 소환수 증가
        yield return new WaitForSeconds(summonDelay);
        summonSwitch = true; // 딜레이후 다시 뽑자.
    }

    IEnumerator Fire() // 총알 소환.
    {
        GameObject bullet = Instantiate(bulletPrefab); // 생성.
        // 총알 생성 위치 = xpos + 30 (여기서 랜덤 x,y 구해서 더해주자, 왼쪽 오른쪽 번갈아가면서 나오도록)
        fireDeltha = -fireDeltha; // 음수 양수 역전시켜주기.
        bullet.transform.position = firePos.position + new Vector3(0, 0, fireDeltha) + new Vector3(0, Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
        bullet.GetComponent<JS_BossRangeAttack>().startPos = bullet.transform.position;
        cur_bulletNumber++; // 총알 수 증가
        yield return new WaitForSeconds(fireDelay);
        fireSwitch = true; // 딜레이후 다시 뽑자.
    }

   

    IEnumerator SummonMagicBoard(Transform one, Transform two) // 1,2 지역을 받아서 장판공격을 합니다.
    {
        yield return new WaitForSeconds(missTime); // 회피시간가지고

        GameObject boardattack_1st, boardattack_2st;
        boardattack_1st = Instantiate(magicBoardPrefab);
        boardattack_1st.transform.position = one.position;

        boardattack_2st = Instantiate(magicBoardPrefab);
        boardattack_2st.transform.position = two.position;
        
    }

}
