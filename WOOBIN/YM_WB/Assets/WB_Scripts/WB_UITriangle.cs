using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트는 스케일을 줄임 -> 원상복귀.
public class WB_UITriangle : MonoBehaviour {
    Vector3 startScale;
    public float speed = 2f;
	// Use this for initialization
	void Start () {
        startScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.localScale.x > 0.5f)
        {
            transform.localScale = transform.localScale - Vector3.one * speed * Time.deltaTime;
        }
        else
        {
            transform.localScale = startScale;
        }
	}
}
