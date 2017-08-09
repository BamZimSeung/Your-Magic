using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이지 순서에 따라 stageIndex 를 참조해 stageInfos 에 접근한다.
// 스테이지의 시작은 플레이어의 이동으로 부터 트리거
// 스테이지의 끝은 생성 몬스터를 모두 처치했는지 여부

public class JS_StageCtrl : MonoBehaviour {

    public static JS_StageCtrl Instance = new JS_StageCtrl();

    // 스테이지 인덱스
    public int stageIndex = 0;

    // 스테이지 정보들
    public JS_StageInfo[] stageInfos;

    // 스테이지 시작 여부
    public bool isStart;

    // 스테이지 시작 후 웨이브 시작 전까지 대기 시간
    public float waitTime;

    // 현재 시간
    float currentTime;

    // 스테이지에서 현재 생성되어있는 몬스터 수
    int tempGroundMon = 0;
    int tempAirMon = 0;

    // 스테이지에서 총 생성된 몬스터 수
    int totalGroundMon = 0;
    int totalAirMon = 0;

    // 몬스터 생성 시간
    public float genGapTime;

    // 몬스터 생성 현재 시간
    float airMonGenTempTime;
    float groundMonGenTempTime;

    void Start () {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        InitValue();

        stageIndex = 0;
    }

    void Update()
    {
        if (isStart)
        {
            if(currentTime < waitTime)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                // 웨이브 시작
                // 몬스터 수와 제한 수 체크해가며 몬스터 젠

                // 지상 몬스터 생성
                if(stageInfos[stageIndex].groundMonCount > totalGroundMon)
                {
                    if(stageInfos[stageIndex].limitGroundMon > tempGroundMon)
                    {

                    }
                }

                // 공중 몬스터 생성
                if (stageInfos[stageIndex].groundMonCount > totalGroundMon)
                {
                    if (stageInfos[stageIndex].limitGroundMon > tempGroundMon)
                    {

                    }
                }

                if(stageInfos[stageIndex].groundMonCount > totalGroundMon && stageInfos[stageIndex].groundMonCount > totalGroundMon)
                {

                }
            }
        }
    }

    public void SetStartTrue()
    {
        if (!isStart)
        {
            isStart = true;
            currentTime = 0;
        }
    }

    public void SetStartFalse()
    {
        if (isStart)
        {
            isStart = false;

            InitValue();
        }
    }

    void InitValue()
    {
        currentTime = 0;

        airMonGenTempTime = 0;
        groundMonGenTempTime = 0;

        tempGroundMon = 0;
        tempAirMon = 0;
        totalAirMon = 0;
        totalGroundMon = 0;
    }
}
