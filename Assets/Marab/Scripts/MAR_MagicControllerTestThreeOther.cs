using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MAR_MagicControllerTestThreeOther : MonoBehaviour
{
    int[,] patternArray =new int[2, 9] { { 2, 5, 8, 0, 0, 0, 0, 0, 0 }, { 4, 5, 6, 0, 0, 0, 0, 0, 0 } };
    int patternsNum = 2;

    int[] touchPattern = new int[9];
    int touchIndex = 0;

    public GameObject[] magicPrefab;


    //어떤 손과 어떤 버튼을 기준으로 할지
    public OVRInput.Controller handController;
    public OVRInput.Button magicButton;

    //마법 패드를 나타낼 프리팹
    public GameObject magicPadPrefab;

    //생성된 마법 패드
    GameObject magicPad;

    public Transform PlayerPos;

    //매직 패드에 중심과의 거리
    public float magicTerm = 0.075f;


    GameObject finger;
    public GameObject fingerPointPrefab;
    GameObject fingerPoint;


    int whatHand;
    float term;
    // Use this for initialization
    void Start()
    {
        //매직패드를 미리 생성시켜 놓아서
        //나중에 보여주기만 한다.
        magicPad = Instantiate(magicPadPrefab);
        fingerPoint = Instantiate(fingerPointPrefab);
        fingerPoint.GetComponent<MAR_MagicFingerPointTestThree>().SetHandParent(this);

        //위치 설정 후 부모 설정
        magicPad.transform.position = transform.position + Vector3.forward * 0.1f;
        magicPad.transform.parent = transform;

        magicPad.SetActive(false);
        fingerPoint.SetActive(false);

        if (handController == OVRInput.Controller.LTouch)
        {
            whatHand = (int)MAR_HandState.Hand.LEFT;
            term = -0.04f;
        }
        else
        {
            whatHand = (int)MAR_HandState.Hand.RIGHT;
            term = 0.04f;
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        switch (MAR_HandState.handState[whatHand])
        {
            case MAR_HandState.State.IDLE:
                MagicShowCheck(null);
                break;
            case MAR_HandState.State.MAGIC_CONTROLL_3:
                MagicUnShowCheck();
                break;
        }       

    }


    public void MagicShowCheck(string name)
    {
        if (name == null)
        {
            if (OVRInput.GetDown(magicButton, handController))
            {
                if (finger == null)
                {
                    if (whatHand == (int)MAR_HandState.Hand.LEFT)
                    {
                        finger = GameObject.Find("hands:b_l_index_ignore");
                    }
                    else
                    {
                        finger = GameObject.Find("hands:b_r_index_ignore");
                    }
                    if (finger == null)
                    {
                        return;
                    }
                    fingerPoint.transform.position = finger.transform.position;
                    fingerPoint.transform.parent = finger.transform;
                }
                fingerPoint.SetActive(true);
                magicPad.SetActive(true);
                magicPad.GetComponent<MAR_MagicPatternPadThree>().BallsInit();
                magicPad.transform.forward = PlayerPos.transform.forward;
                magicPad.transform.position = transform.position + PlayerPos.forward * 0.15f;
                magicPad.transform.parent = null;
                touchIndex = 0;
                MAR_HandState.handState[whatHand] = MAR_HandState.State.MAGIC_CONTROLL_3;

            }

        }
        else if (name.Contains("FireBall"))
        {
            if (finger == null)
            {
                if (whatHand == (int)MAR_HandState.Hand.LEFT)
                {
                    finger = GameObject.Find("hands:b_l_index_ignore");
                }
                else
                {
                    finger = GameObject.Find("hands:b_r_index_ignore");
                }
                if (finger == null)
                {
                    return;
                }
                fingerPoint.transform.position = finger.transform.position;
                fingerPoint.transform.parent = finger.transform;
            }
            fingerPoint.SetActive(true);
            magicPad.SetActive(true);
            magicPad.GetComponent<MAR_MagicPatternPadThree>().BallsInit();
            magicPad.transform.forward = PlayerPos.transform.forward;
            magicPad.transform.position = transform.position + PlayerPos.forward * 0.15f;
            magicPad.transform.parent = null;
            magicPad.GetComponent<MAR_MagicPatternPadThree>().SetLineRender(patternArray, 0);
            touchIndex = 0;
            MAR_HandState.handState[whatHand] = MAR_HandState.State.MAGIC_CONTROLL_3;
        }
    }

    public void MagicChoice(int id)
    {
         touchPattern[touchIndex] = id;
         touchIndex++;
         if (PatternCheck())
         {
            magicPad.SetActive(false);
            magicPad.transform.position = transform.position + Vector3.forward * 0.1f;
            magicPad.transform.parent = transform;
            MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
            fingerPoint.SetActive(false);
        }
    }

    bool PatternCheck()
    {
        int index = -1;
        for(int i = 0; i < patternsNum; i++)
        {
            bool isCheck = false;
            for(int j = 0; j < touchIndex; j++)
            {
                if (patternArray[i,j] == touchPattern[j])
                {
                    if (patternArray[i, j + 1] == 0)
                    {
                        isCheck = true;
                    }

                }
                else
                { break; }
            }
            if (isCheck)
            {
                index = i;
            }
        }
        if (index == -1)
        {
            return false;
        }
        else
        {
            GameObject magic = Instantiate(magicPrefab[index]);
            magic.transform.position = transform.position + transform.forward * 0.1f;
            return true;
        }
    }


    void MagicUnShowCheck()
    {
        if (OVRInput.GetDown(magicButton, handController))
        {
            magicPad.SetActive(false);
            magicPad.transform.parent = transform;
            MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
            fingerPoint.SetActive(false);
        }
        if((magicPad.transform.position - transform.position).magnitude > 0.3f)
        {
            magicPad.SetActive(false);
            magicPad.transform.parent = transform;
            MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
            fingerPoint.SetActive(false);
        }
    }    

}