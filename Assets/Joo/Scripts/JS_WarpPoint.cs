using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_WarpPoint : MonoBehaviour {

    public ParticleSystem[] pss;
    public Color defaultColor;
    public Color selectedColor;

    public bool isSelected = false;

    public float stateChangeTime = 0.3f;

    public int nextStageIndex = 0;

    float currentTime = 0;

	void Start () {
        for (int i = 0; i < pss.Length; i++)
        {
            pss[i].startColor = defaultColor;
        }
    }
	
	void Update () {
        if (isSelected)
        {
            currentTime += Time.deltaTime;
            if(currentTime > stateChangeTime)
            {
                currentTime = 0;
                isSelected = false;
                for(int i =0; i < pss.Length; i++)
                {
                    pss[i].startColor = defaultColor;
                }
            }
        }
	}

    // 레이에 닿았다고 전달받는 함수
    public void WarpPointSelected()
    {
        if (!isSelected)
        {
            for (int i = 0; i < pss.Length; i++)
            {
                pss[i].startColor = selectedColor;
            }
            isSelected = true;
        }
        currentTime = 0;
    }

    // 다음 스테이지로 설정
    public void SetNextStageIndex()
    {
        // 다음 스테이지 인덱스 변경
        JS_StageCtrl.Instance.stageIndex = nextStageIndex;
        // 다음 스테이지 시작
        JS_StageCtrl.Instance.SetStartTrue();
        // 워프 포인트 비활성화
        gameObject.GetComponentInParent<JS_StageInfo>().SetWarpPointsDisable();
    }
}
