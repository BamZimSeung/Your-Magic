using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MAR_MagicControllerTestTwoOther : MonoBehaviour
{
    //어떤 손과 어떤 버튼을 기준으로 할지
    public OVRInput.Controller handController;
    public OVRInput.Button magicButton;

    //마법을 저장하는 배열
    public GameObject magicPrefab;
    GameObject lastMagic;

    public Transform playerPos;
    
    //매직 패드에 중심과의 거리
    float magicTerm = 18f;

    int whatHand;

    int maxMagic = 2;

    bool isMaked = false;
    float makeDelay = 1.0f;


    // Use this for initialization
    void Start()
    {
        if (handController == OVRInput.Controller.LTouch)
        {
            whatHand = (int)MAR_HandState.Hand.LEFT;
        }
        else
        {
            whatHand = (int)MAR_HandState.Hand.RIGHT;
        }

    }



    // Update is called once per frame
    void Update()
    {
        if (lastMagic==null || lastMagic.transform.parent != null)
        {
            isMaked = false;
        }
        if (isMaked || magicPrefab==null)
        {
            return;
        }
        switch (MAR_HandState.handState[whatHand])
        {
            case MAR_HandState.State.IDLE:
                MagicShowCheck();
                break;
        }
    }

    public void SetLastMagic(GameObject magic)
    {
        magicPrefab = magic;
    }

    void MagicShowCheck()
    {
      if((transform.localRotation.eulerAngles.z>=90-magicTerm && transform.localRotation.eulerAngles.z <=90+magicTerm) && whatHand==(int)MAR_HandState.Hand.LEFT)
        {
            if ((transform.localRotation.eulerAngles.x >= 360-magicTerm || transform.localRotation.eulerAngles.x <= magicTerm))
            {
                lastMagic = Instantiate(magicPrefab);
                lastMagic.transform.position = transform.position + transform.right*0.1f;// + Vector3.up*0.1f;
                lastMagic.transform.up = Vector3.up;
                lastMagic.transform.parent = null;
                isMaked = true;
                Destroy(lastMagic, 2f);
            }
        }
        if ((transform.localRotation.eulerAngles.z >= 270 - magicTerm && transform.localRotation.eulerAngles.z <= 270 + magicTerm) && whatHand == (int)MAR_HandState.Hand.RIGHT)
        {
            if ((transform.localRotation.eulerAngles.x >= 360 - magicTerm  || transform.localRotation.eulerAngles.x <= magicTerm))
            {
                lastMagic = Instantiate(magicPrefab);
                lastMagic.transform.position = transform.position - transform.right*0.1f;// + Vector3.up*0.1f;
                lastMagic.transform.up = Vector3.up;
                lastMagic.transform.parent = null;
                isMaked = true;
                Destroy(lastMagic, 2f);
            }
        }
    }

    
}