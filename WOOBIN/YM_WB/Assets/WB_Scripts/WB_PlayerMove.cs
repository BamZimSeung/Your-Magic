using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// WASD로 간단히 플레이어 움직임을 구현한다.
public class WB_PlayerMove : MonoBehaviour {

    float mx, my;
    public float moveSpeed =2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(x, 0, y);

        transform.position = transform.position + dir * moveSpeed * Time.deltaTime;
	}
}
