using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Touch를 이용해 물체를 잡고, 놓고, 던지는 동작처리

public class HandControllerTestTwoOther : MonoBehaviour
{

    //어떤 손인지 기억할 변수
    public OVRInput.Controller handController;
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

    int whatHand;

    // Use this for initialization
    void Start()
    {
        if (handController == OVRInput.Controller.LTouch)
        {
            whatHand = (int)HandState.Hand.LEFT;
        }
        else
        {
            whatHand = (int)HandState.Hand.RIGHT;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        //Grab버튼을 누르면 Grab기능 활성화
        if (isGrabbing == false && OVRInput.GetDown(grabButton, handController) &&
            HandState.handState[whatHand] == HandState.State.MAGIC_CONTROLL_2)
        {
            //물체잡는 동작
            GrabObject();
        }
        //Grab기능이 활성화 되어있고, up 이벤트 발생하면?
        else if (isGrabbing == true && OVRInput.GetUp(grabButton, handController) &&
            HandState.handState[whatHand] == HandState.State.MAGIC_USE)
        {
            //물체를 떨어트리는 동작
            DropObject();
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
            //1. 루트 복귀
            grabbedObject.transform.parent = null;
            //2. 물리 속성 활성화
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<Rigidbody>().useGravity = true;

            //3. 던지기
            grabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(handController);

            //4. 가능하면 회전까지
            grabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(handController) * power;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = OVRInput.GetLocalControllerAngularVelocity(handController);


            //잡은 물체 초기화
            grabbedObject = null;
            HandState.handState[whatHand] = HandState.State.IDLE;
        }
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
        RaycastHit[] hits = Physics.SphereCastAll(ray, grabRange, 0.0f, grabLayer);
        if (hits.Length > 0)
        {
            int closest = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].distance <= hits[closest].distance)
                {
                    closest = i;
                }
            }

            Debug.Log(hits[closest].collider.transform.parent);
            if (hits[closest].transform.parent.name.Contains("MagicPadTwo"))
            {
                grabbedObject = Instantiate(hits[closest].transform.gameObject);
            }
            else
            {
                return;
            }
            //부모자식 관계로 만들어준다.
            grabbedObject.transform.parent = transform;
            grabbedObject.transform.position = transform.position;

            grabbedObject.transform.localScale = Vector3.one * 0.1f;



            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            HandState.handState[whatHand] = HandState.State.MAGIC_USE;

        }
        else
        {
            isGrabbing = false;
            HandState.handState[whatHand] = HandState.State.IDLE;
        }
    }
}