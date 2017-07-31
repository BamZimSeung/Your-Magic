using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 이 스크립트는 게임에서 스테이지 이동 가능할때 유저가 헤매지 않도록 돕는 화살표를 만듭니다.
// 플레이어의 지점에서 맵 이동 포인트까지 화살표를 움직여 주고 플레이어가 이동할 때까지 반복 합니다.
// 시작위치를 기억하고 도착위치까지 이동합니다.
// 도착위치에 도달하면 다시 시작위치부터 재생합니다.
// 이동하면서 중간중간 화살표를 자신의 위치에 뿌려줍니다. 일정 딜레이를 가지고.


// 나중에는 nav 를 이용해야할듯 장애물 만나면 이상해질 것 이라고 생각됌. + 화살표에 이펙트(빛나고 offset 움직임 넣으면 좋을거같음).

public class WB_Arrow_Ctrl : MonoBehaviour {
    
    public float moveSpeed = 2f; // 이동 속도
    Vector3 startPos;
    Vector3 dir;
    Vector3 startScale;
    public GameObject ArrowPrefab;
    public Transform destination; // 목적지 ( 두 갈래 가능 )
    bool isCheckStart = true;
    public float createDelay = 1f;
    float currentTime;

    public float des_time; // 도착시간.
    public int arrow_number; // 만들 화살표갯수 = 거리에 비례해서 늘리면 좋을듯함.
    public float create_ratio;

    public bool isPlayerMoving = true;
    public bool stageSelect = false;
    NavMeshAgent navi; // 네비게이션.

    public bool isArrived = false; // 도착햇냐?

    // 시작위치 설정이 필요하다.

    private void Start()
    {
        navi = GetComponent<NavMeshAgent>();
        navi.speed = moveSpeed;
    }
    // Use this for initialization
    // Update is called once per frame
    // Use this for initialization
    // Update is called once per frame




        /// <summary>
        /// need -> 스테이지 선택해야 할 때 stageSelect = true, isPlayerMoving = true, destination에 도착지점 설정해주기, isArrived = false.
        /// need -> 아직 스테이지 선택중이고 플레이어가 이동하면 isPlayerMoving = true
        /// need -> 스테이지 선택 종료시 stageSelect = false
        /// </summary>


    void Update () {
        if (destination) // 목적지가 있다면 -> 갈 곳이 1개 일 경우 상상
        {
            if (stageSelect) // 스테이지 선택 상황이 되었을때.
            {
                if (isArrived == false) // 다시 시작.
                {
                    navi.SetDestination(destination.transform.position);
                    if (isPlayerMoving) // 만약 플레이어가  움직이면. 위치 초기화
                    {
                        FindStartPos();
                        InitPos();
                        isPlayerMoving = !isPlayerMoving;
                    }
                    else // 이동가능할때(즉 플레이어는 멈춰있거나 이동완료한 상황이고 스테이지 선택 상황이 계속될때)
                    {
                        if (isCheckStart) // 시작지점을 체크.
                        {
                            FindStartPos();
                            currentTime = createDelay;// 시작시 하나뿌리고
                            des_time = Vector3.Distance(startPos, destination.position) / (moveSpeed); // 도착시간 계산
                            arrow_number = (int)Mathf.Round(Vector3.Distance(startPos, destination.position) * create_ratio); // 화살표 갯수 계산.
                        }
                        // 방향을 찾고
                        dir = destination.position - transform.position;
                        /*
                        // 이동시킨다.
                        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime);
                        */
                        //transform 버전
                        currentTime += Time.deltaTime; // 딜레이체크

                        if (currentTime > des_time / arrow_number) // 딜레이를 넘으면 화살표 생산 및 방향 맞추기.
                        {
                            GameObject arrow = Instantiate(ArrowPrefab);
                            arrow.transform.position = transform.position;
                            //arrow.transform.forward = dir.normalized; 
                            arrow.transform.forward = transform.forward;
                            currentTime = 0f;
                        }
                        // 만약 도착지점에 도착하면 다시 처음상태로 돌려준다.
                        if (Vector3.Distance(destination.position, transform.position) < 2f)
                        {
                            FindStartPos();
                            InitPos();
                            isArrived = true;
                        }
                    }
                }
                else // 도착이욤. 지연 + 다시시작.
                {
                    StartCoroutine("DelayReStart");
                }
            }
            else
            {
                InitPos();
            }
        }
	}

    void FindStartPos()
    {
        isCheckStart = false;
        startPos = transform.parent.position + transform.parent.forward * 2f; 
        startScale = transform.localScale;
    }

    void InitPos()
    {
        transform.position = startPos;
        transform.localScale = startScale;
        transform.forward = transform.parent.forward; // forward 플레이어와 일치.
        isCheckStart = true;
    }

    IEnumerator DelayReStart()
    {
        InitPos();
        yield return new WaitForSeconds(0.5f);
        isArrived = false;
    }
}
