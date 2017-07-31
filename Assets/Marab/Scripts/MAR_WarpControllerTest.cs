using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

//Warp 한테 Fire2 버튼을 누르면 그 Warp 위치로 이동
//부모는 Warp가 있는 Tower로 교체한다.
public class MAR_WarpControllerTest : MonoBehaviour
{
    public OVRInput.Controller handController;
    public OVRInput.Button warpButtonLeft;
    public OVRInput.Button warpButtonRight;

    Transform move;
    public Transform leftHand;
    public Transform rightHand;
    public float moveSpeed = 10.0f;
    bool isMove = false;

    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isMove)
        {
            transform.position = Vector3.Lerp(transform.position, move.position, 0.3f * moveSpeed * Time.deltaTime);
            if ((transform.position - move.position).magnitude <= 0.1)
            {
                transform.position = move.position;
                isMove = false;
                //   Recenter -> Camera의 회전을 현재 시점에서 0으로 초기화
                InputTracking.Recenter();

            }
            return;
        }
        //1. 입력처리
        if (OVRInput.GetUp(warpButtonLeft, handController))
        {
            //2. Warp하고만 Ray충돌 하도록 쏘기
            Ray ray = new Ray(leftHand.position, leftHand.forward);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, 100, 1 << LayerMask.NameToLayer("Warp")))
            {
                //이동 전환
                isMove = true;
                move = hitinfo.transform;
                lr.enabled = false;
            }
            lr.enabled = false;
        }
        /*
        else if (OVRInput.GetUp(warpButtonRight, handController))
        {
            //2. Warp하고만 Ray충돌 하도록 쏘기
            Ray ray = new Ray(rightHand.position, rightHand.forward);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, 100, 1 << LayerMask.NameToLayer("Warp")))
            {
                //이동 전환
                isMove = true;
                move = hitinfo.transform;

                // - 부모가 바라보는 방향으로 방향 전환
                transform.forward = transform.parent.forward;
            }
        }
        */
        if (OVRInput.Get(warpButtonLeft, handController))
        {
            //2. Warp하고만 Ray충돌 하도록 쏘기
            GetComponent<LineRenderer>().SetPosition(0,leftHand.position);
            GetComponent<LineRenderer>().SetPosition(1,leftHand.position+leftHand.forward*100f);
            lr.enabled = true;
        }
        /*
        else if (OVRInput.Get(warpButtonRight, handController))
        {
            //2. Warp하고만 Ray충돌 하도록 쏘기
            GetComponent<LineRenderer>().SetPosition(0, rightHand.position);
            GetComponent<LineRenderer>().SetPosition(1, rightHand.position + rightHand.forward * 100f);
        }
        */

    }
}

