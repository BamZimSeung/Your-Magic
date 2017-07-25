using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 타겟이 플레이어의 왼, 오른쪽인지 판단해서 그쪽방향 45도 위에 삼각형을 그려줍니다.
// 삼각형 위치에서 타겟의 위치까지 Line을 그려줍니다.
// 타겟과 플레이어의 거리가 일정거리 미만이고, 타겟을 바라보면 타겟의 위치에 삼각형을 배치하고, Text를 붙여줍니다.

public class WB_DisplayCanvas : MonoBehaviour {
    public bool isLeft = false;
    public float xpos;
    LineRenderer lr;
    public GameObject destination;
    public GameObject player;
    public float xratio = 2f;
    public float yratio = 2f;
    public Image triangle;
    public float lineWidth = 0.5f;
    public GameObject text;
    Vector3 startScale;

    public bool isRecognize = false;
    public float recognizeLengh = 7.5f;
    // Use this for initialization
    void Start () {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        startScale = transform.localScale;
        if (destination.transform.position.x - player.transform.position.x < 0) // 왼, 오른쪽 판별.
        {
            isLeft = true;
        }
        else
        {
            isLeft = false;
        }

        if (isLeft)
        {
            transform.position = transform.position + Vector3.left * xratio + Vector3.up * yratio;
        }
        else
        {
            transform.position = transform.position + Vector3.right * xratio + Vector3.up * yratio;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isRecognize)
        {
            lr.SetPosition(0, triangle.transform.position + Vector3.up * 0.45f);
            lr.SetPosition(1, destination.transform.position);
            // 플레이어와 목적지와의 거리가 일정거리 미만이면 UI를 붙여준다.
            Ray ray = new Ray(player.transform.position, player.transform.forward);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.name.Contains("Target") && hitInfo.distance <= recognizeLengh)
                {
                    isRecognize = true;
                }
            }
        }
        else
        {
            // 라인렌더러 없애고, 삼각형을 목적지 위에 붙여줌
            lr.SetPosition(0, transform.position);
            lr.SetPosition(0, transform.position);
            lr.startWidth = 0;
            transform.position = Vector3.Lerp(triangle.transform.position, destination.transform.position + Vector3.up * 2.5f, 0.02f);
            // 움직임에 따라 크기 줄여주자.
            transform.localScale = Vector3.Lerp(startScale, startScale * Vector3.Distance(transform.position, destination.transform.position), 0.02f);
            text.SetActive(true);
        }

	}
}
