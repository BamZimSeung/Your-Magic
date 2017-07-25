using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//앞뒤, 좌우 이동 가능한 스크립트
//중력을 갖는다.
[RequireComponent(typeof(CharacterController))]
public class Test_Move : MonoBehaviour {
    public float gravity = -20;
    float yVelocity = 0;

    public float moveSpeed = 10;

    CharacterController cc;
	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        //1. 좌우 앞뒤 입력처리
        // - 조이스틱 대응? 키보드 대응? 터치 대응?

        // - Horizontal, Vertical
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // - 카메라가 바라보는 방향으로 입력 값 변환
        Vector3 camDir = Camera.main.transform.TransformDirection(new Vector3(h, 0, v));
        camDir.Normalize();

        Vector3 dir = Camera.main.transform.right * h + Camera.main.transform.forward * v;
        dir.Normalize();

        //2. 중력 처리
        // - 전체 이동방향에서 중력 적용
        if (cc.isGrounded)
        {
            yVelocity = 0;
        }
        yVelocity += gravity * Time.deltaTime;
        camDir.y = yVelocity;

        //3. 카메라가 바라보는 방향으로 이동
        // - 최종 정해진 방향으로 이동
        cc.Move(moveSpeed*camDir*Time.deltaTime);
    }
}
