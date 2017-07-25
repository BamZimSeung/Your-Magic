using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트는 요정에대한 컨트롤 스크립트입니다.
// 요정은 정해진 플레이어 주위를 랜덤하게 돌아다닙니다.
// 요정은 플레이어를 돕는역할을 합니다. -> ex 길생성, 마법사용방법 알려주기.
public class WB_Fairy_Ctrl : MonoBehaviour {
    public GameObject Player;
    public float radius = 3f;
    public float upDown = 2f;
    
    // 원래 위치에서 (rcos셑, rsin셑) 위치로 곡선이동.\

    Transform originPos;
    Transform desPos;
    Vector3 center;
    public float journeyTime = 1.0f;
    private float startTime;
    public float setha;

    void Start()
    {
        center = Player.transform.position; // 센터 위치 잡기..
        originPos.position = center + Vector3.right * radius; // 시작위치 플레이어 + 반지름.
        

        transform.position = originPos.position; // 시작위치 잡아주기.
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update () {
        setha = setha + 30f;
        desPos.position = center + new Vector3(Mathf.Cos(setha), 0, Mathf.Sin(setha));
        float fracComplete = (Time.time - startTime) / journeyTime;
        transform.position = Vector3.Slerp(originPos.position, desPos.position, fracComplete);
	}
}
