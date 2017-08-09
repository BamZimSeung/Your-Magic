using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_StageInfo : MonoBehaviour {

    // 몬스터 생성 위치
    public Transform[] genPoints;

    // 생성 몬스터 종류
    public GameObject[] groundMonPrefabs;
    public GameObject[] airMonPrefabs;

    // 생성 몬스터 수 (지상, 공중)
    public int groundMonCount;
    public int airMonCount;

    // 몬스터 수 제한 (지상, 공중)
    public int limitGroundMon;
    public int limitAirMon;

    // 다음 장소 인덱스
    public int nextIndex;
}
