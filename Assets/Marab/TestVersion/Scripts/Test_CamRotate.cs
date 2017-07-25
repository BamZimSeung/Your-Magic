using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//마우스의 상하좌우 입력에다라 회전하는 기능을 만든다.
//상하회전은 -90~ 90이내로 제한한다. 
//1.사용자의 마우스 입력처리
//2.회전처리
//3. 회전속도
public class Test_CamRotate : MonoBehaviour {

    public float rotSpeed = 200;
    float mx;
    float my;


	// Use this for initialization
	void Start () {
        mx = transform.eulerAngles.y;
        my = transform.eulerAngles.z;

	}
	
	// Update is called once per frame
	void Update () {
        //사용자 입력처리 
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        //움직였다가 값이 돌아오기 때문에 값을 누적해야 함.

        mx += rotSpeed * h * Time.deltaTime;
        my += rotSpeed * v * Time.deltaTime;

        //-90 ~ 90 도 까지 볼 수 있게
    
        my = Mathf.Clamp(my, -90, 90);
        
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
    
}
