using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Touch를 이용해 물체를 잡고, 놓고, 던지는 동작처리

public class HandControllerTestTwo : MonoBehaviour
{

    //어떤 손인지 기억할 변수
    public OVRInput.Controller handController;
    public OVRInput.Controller touch;
    public OVRInput.Button grabButton;

    //Grab 기능 활성화 여부
    bool isGrabbing = false;

    //잡은 오브젝트
    GameObject grabbedObject;

    //Grab 범위 지정
    public float grabRange = 0.1f;

    //잡을 개체 Layer지정
    public LayerMask grabLayer;
    

    public float power = 6.0f;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        
        //Grab버튼을 누르면 Grab기능 활성화
        if (isGrabbing == false && OVRInput.GetDown(grabButton, touch))
        {
            //물체잡는 동작
            GrabObject();
        }
        //Grab기능이 활성화 되어있고, up 이벤트 발생하면?
        else if (isGrabbing == true && OVRInput.GetUp(grabButton, touch))
        {
            //물체를 떨어트리는 동작
            DropObject();
        }

        if (isGrabbing==true && grabbedObject != null)
        {
            ControllObject();
        }

    }
    //물체 놓기
    //1. 잡은 물체가 있을경우 물체를 놓는다.
    //2. 던질경우 해당 방향으로 날아가도록 한다.
    //3. 회전시켜 던질경우 ??
    void DropObject()
    {
        isGrabbing = false;

        if (grabbedObject != null)
        {
            grabbedObject = null;
        }
    }

    float moveSpeed = 5.0f;

    void ControllObject()
    {
        Vector3 euler = grabbedObject.transform.eulerAngles;
        Vector3 vel = OVRInput.GetLocalControllerVelocity(handController);
        euler.y += (vel.x + vel.z) * moveSpeed ;
        grabbedObject.transform.eulerAngles = euler;
    }


    //물체 잡기
    void GrabObject()
    {
        //1. Grab기능 활성화
        isGrabbing = true;
        //2. Grab영역 안에 물체가 있으면 Grabbable을 판단하고 잡기
        //3. 만약에, 많은 물체가 있으면 제일 가까운 물체를 우선적으로 잡는다.
        // 영역에서 범위 충돌 검사.
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, grabRange, 0.0f);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.name.Contains("MagicPadTwo"))
                {
                    grabbedObject = hits[i].transform.gameObject;
                    break;
                }
            }
        }
        else
        {
            isGrabbing = false;
        }
    }
}