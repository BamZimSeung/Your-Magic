using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트는 요정에대한 컨트롤 스크립트입니다.
// 요정은 정해진 플레이어 주위를 랜덤하게 돌아다닙니다.
// 요정은 플레이어를 돕는역할을 합니다. -> ex 길생성, 마법사용방법 알려주기.
public class WB_Fairy_Ctrl : MonoBehaviour {
    public GameObject Player;
    public GameObject fairy_position;
    public GameObject Word_Ballon;
    public float radius = 3f;
    public float upDown = 2f;
    
    // 원래 위치에서 (rcos셑, rsin셑) 위치로 곡선이동.\

    public Vector3 originPos;
    public Vector3 desPos;
    public Vector3 center;
    public float journeyTime = 0.5f;
    private float startTime;
    public float setha;
    public float deltax = 3;

    public bool isMove = true;
    public bool isBack = true;

    // Fairy 상태를 알려줍니다.
    // Idle에서 기다리고
    // Move에서 돌아다닙니다.

    public enum fairyState
    {
        Idle, // 대기
        Move, // 움직임
        Talk, // 말 걸때.
    }

    public fairyState m_fairy_state;
    void Start()
    {
        m_fairy_state = fairyState.Move;
        center = Player.transform.position; // 센터 위치 잡기..
        originPos = center + Vector3.right * radius; // 시작위치 플레이어 + 반지름.
        transform.position = originPos; // 시작위치 잡아주기.
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update ()
    {
        switch (m_fairy_state)
        { 
            case fairyState.Idle:
                Fairy_Idle();
               break;
            case fairyState.Move:
                Fairy_Move();
                break;
            case fairyState.Talk:
                Fairy_Talk();
                break;
        }
    }

    void Fairy_Idle()
    {
        originPos = transform.position; // 원래위치를 현재위치로 설정합니다.
        desPos = fairy_position.transform.position;// 지정된 위치로 설정.
        transform.position = Vector3.Lerp(originPos, desPos, 0.2f); // 원래자리로 돌아감.
        if(Vector3.Distance(transform.position, desPos) < 0.1f) // 도착하면
        {
            transform.position = desPos; // 고정.
            originPos = desPos; // 원래 위치도 고정
        }
        startTime = Time.time;
        Word_Ballon.SetActive(false);
    }

    void Fairy_Move()
    {
        center = Player.transform.position; // 센터 위치 잡기..
        //desPos = new Vector3(radius * Mathf.Cos(setha), Mathf.Abs(radius / 2 * Mathf.Cos(setha)), Mathf.Abs(radius * Mathf.Sin(setha))); // 원운동 포지션구하기
        desPos = Player.transform.forward * Mathf.Abs(deltax) + Player.transform.right * deltax + new Vector3(0, Mathf.Abs(radius * Mathf.Cos(setha)), 0);
        desPos = center + desPos; // 정면주위에서 돌아다니도록 게다가 forward쪽을 바라보게하려면?
        if (isMove)
        {

            float fracComplete = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Slerp(originPos, desPos, fracComplete); // 원운동
            if (Vector3.Distance(transform.position, desPos) < 0.1f) // 도착
            {
                setha = setha + Random.Range(120, 360); // 움직일 범위 설정해줌
                setha = setha % 360;
                deltax = -deltax;
                originPos = transform.position; // 원래 위치를 현재위치로 설정.
                startTime = Time.time; // 시작시간 셋팅.
            }
        }
        Word_Ballon.SetActive(false);
    }

    void Fairy_Talk()
    {
        originPos = transform.position; // 원래위치를 현재위치로 설정합니다.
        desPos = fairy_position.transform.position;// 지정된 위치로 설정.
        transform.position = Vector3.Lerp(originPos, desPos, 0.2f); // 원래자리로 돌아감.
        if (Vector3.Distance(transform.position, desPos) < 0.1f) // 도착하면
        {
            transform.position = desPos; // 고정.
            originPos = desPos; // 원래 위치도 고정
        }
        startTime = Time.time;
        Word_Ballon.SetActive(true);
    }
}
