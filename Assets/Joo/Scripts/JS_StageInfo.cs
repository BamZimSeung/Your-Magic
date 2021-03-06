﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_StageInfo : MonoBehaviour {

    // 몬스터 생성 위치
    public Transform[] airMonGenPoints;
    public Transform[] groundMonGenPoints;

    // 생성 몬스터 종류
    public GameObject[] groundMonPrefabs;
    public GameObject[] airMonPrefabs;

    // 생성 몬스터 수 (지상, 공중)
    public int groundMonCount;
    public int airMonCount;

    // 몬스터 수 제한 (지상, 공중)
    public int limitGroundMon;
    public int limitAirMon;

    // 다음 스테이지 워프포인트
    public GameObject[] nextWarpPoints;

    void Start()
    {
        SetWarpPointsDisable();
    }

    public void SetWarpPointsDisable()
    {
        for (int i = 0; i < nextWarpPoints.Length; i++)
        {
            nextWarpPoints[i].SetActive(false);
        }
    }

    public void SetWarpPointsAble()
    {
        for (int i = 0; i < nextWarpPoints.Length; i++)
        {
            nextWarpPoints[i].SetActive(true);
        }
    }
}
