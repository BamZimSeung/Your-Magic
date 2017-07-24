using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트는 요정에대한 컨트롤 스크립트입니다.
// 요정은 정해진 플레이어 주위를 랜덤하게 돌아다닙니다.
// 요정은 플레이어를 돕는역할을 합니다. -> ex 길생성, 마법사용방법 알려주기.
public class WB_Fairy_Ctrl : MonoBehaviour {
    public GameObject Player;
    public float moveSpeed = 2f;
    public float radius = 3f;
    public float upDown = 2f;
    Vector3 dir;
    bool isMove = true;
    private void Start()
    {
        dir = Player.transform.position + Player.transform.forward * Mathf.Sqrt(radius) + Player.transform.right * Mathf.Sqrt(radius); // 초기 위치 설정
        transform.position = dir;
    }
    // Update is called once per frame
    void Update () {
        // 상하운동 + 원운동.
        // 상하운동.
        UpDown();
	}

    void UpDown()
    {
        float temp = Random.Range(-1.0f, 1.0f);
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * temp, 0.1f);
    }
}
