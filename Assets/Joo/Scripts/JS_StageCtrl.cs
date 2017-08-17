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
    float airMonGenGap;
    float groundMonGenGap;

    // 몬스터 생성 현재 시간
    float airMonGencurrentTime;
    float groundMonGencurrentTime;

    public enum MonType{
        Air,
        Ground
    };

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
        // 스테이지 시작되었다면
        if (isStart)
        {
            // 대기 시간동안 기다린 후
            if(currentTime < waitTime)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                // 웨이브 시작
                // 몬스터 수와 제한 수 체크해가며 몬스터 젠
                GenMonster();
            }
        }
    }

    void GenMonster()
    {
        int ranMonIndex = 0;
        int ranPointIndex = 0;

        // 지상 몬스터 생성
        // 총 몬스터 생성 수와 비교
        if (stageInfos[stageIndex].groundMonCount > totalGroundMon)
        {
            groundMonGencurrentTime += Time.deltaTime;

            // 몬스터 생성 제한 수와 비교
            if (stageInfos[stageIndex].limitGroundMon > tempGroundMon)
            {
                if (groundMonGencurrentTime > groundMonGenGap)
                {
                    // 소환할 몬스터 인덱스 결정
                    ranMonIndex = Random.Range(0, stageInfos[stageIndex].groundMonPrefabs.Length);
                    // 소환 위치 인덱스 결정
                    ranPointIndex = Random.Range(0, stageInfos[stageIndex].groundMonGenPoints.Length);

                    // 몬스터 소환
                    Instantiate(stageInfos[stageIndex].groundMonPrefabs[ranMonIndex], stageInfos[stageIndex].groundMonGenPoints[ranPointIndex].position, Quaternion.identity);

                    // 현재 소환된 몬스터 수 값 증가
                    tempGroundMon++;
                    totalGroundMon++;

                    // 시간 초기화
                    groundMonGenGap = Random.Range(genGapTime - 0.5f, genGapTime + 0.5f);
                    groundMonGencurrentTime = 0;
                }
            }
        }

        // 공중 몬스터 생성
        // 총 몬스터 생성 수와 비교
        if (stageInfos[stageIndex].airMonCount > totalAirMon)
        {
            airMonGencurrentTime += Time.deltaTime;

            // 몬스터 생성 제한 수와 비교
            if (stageInfos[stageIndex].limitAirMon > tempAirMon)
            {
                if (airMonGencurrentTime > airMonGenGap)
                {
                    // 소환할 몬스터 인덱스 결정
                    ranMonIndex = Random.Range(0, stageInfos[stageIndex].airMonPrefabs.Length);
                    // 소환 위치 인덱스 결정
                    ranPointIndex = Random.Range(0, stageInfos[stageIndex].airMonGenPoints.Length);

                    // 몬스터 소환
                    GameObject Mon = Instantiate(stageInfos[stageIndex].airMonPrefabs[ranMonIndex], stageInfos[stageIndex].airMonGenPoints[ranPointIndex].position, Quaternion.identity) as GameObject;

                    WB_AirMonster airMon = Mon.GetComponent<WB_AirMonster>();
                    airMon.Spawn = stageInfos[stageIndex].airMonGenPoints[ranPointIndex].gameObject;
                    for (int i = 0; i < stageInfos[stageIndex].airMonGenPoints[ranPointIndex].gameObject.GetComponent<WB_AirMonSpawn>().Nodes.Length; i++)
                    {
                        Debug.Log(stageInfos[stageIndex].airMonGenPoints[ranPointIndex].gameObject.GetComponent<WB_AirMonSpawn>().Nodes[i]);
                        airMon.nodes.Add(stageInfos[stageIndex].airMonGenPoints[ranPointIndex].gameObject.GetComponent<WB_AirMonSpawn>().Nodes[i]);
                    }
                    airMon.Player = GameObject.FindWithTag("Player");

                    // 현재 소환된 몬스터 수 값 증가
                    tempAirMon++;
                    totalAirMon++;

                    // 시간 초기화
                    airMonGenGap = Random.Range(genGapTime - 0.5f, genGapTime + 0.5f);
                    airMonGencurrentTime = 0;
                }
            }
        }

        // 설정한 몬스터 수를 모두 소환했고
        // 현재 소환된 몬스터가 없다면 웨이브 종료
        if (stageInfos[stageIndex].groundMonCount == totalGroundMon && stageInfos[stageIndex].airMonCount == totalAirMon && ((tempAirMon + tempGroundMon) == 0))
        {
            SetStartFalse();
        }
    }

    // 웨이브 시작
    public void SetStartTrue()
    {
        if (!isStart)
        {
            isStart = true;
        }
    }

    // 웨이브 종료
    public void SetStartFalse()
    {
        if (isStart)
        {
            isStart = false;

            stageInfos[stageIndex].SetWarpPointsAble();

            InitValue();
        }
    }

    // 현재 생성된 몬스터 수 감소
    public void DecreaseMonTempCount(MonType monType)
    {
        switch (monType)
        {
            case MonType.Ground:
                tempGroundMon--;
                break;
            case MonType.Air:
                tempAirMon--;
                break;
            default:
                break;
        }
    }

    // 수치 초기화
    void InitValue()
    {
        currentTime = 0;

        airMonGenGap = Random.Range(genGapTime - 0.5f, genGapTime + 0.5f);
        groundMonGenGap = Random.Range(genGapTime - 0.5f, genGapTime + 0.5f);

        airMonGencurrentTime = 0;
        groundMonGencurrentTime = 0;

        tempGroundMon = 0;
        tempAirMon = 0;

        totalAirMon = 0;
        totalGroundMon = 0;
    }
}
