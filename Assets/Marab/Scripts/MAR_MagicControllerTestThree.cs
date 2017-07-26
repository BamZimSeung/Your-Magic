using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MAR_MagicControllerTestThree : MonoBehaviour
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

    int whatHand;
    float term;
    // Use this for initialization
    void Start()
    {
        //매직패드를 미리 생성시켜 놓아서
        //나중에 보여주기만 한다.
        magicPad = Instantiate(magicPadPrefab);

        //위치 설정 후 부모 설정
        magicPad.transform.position = transform.position + Vector3.forward * 0.1f;
        magicPad.transform.parent = transform;

        magicPad.SetActive(false);

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
                MagicShowCheck();
                break;
            case MAR_HandState.State.MAGIC_CONTROLL_3:
                MagicChoice();
                MagicUnShowCheck();
                break;
        }       

    }


    void MagicShowCheck()
    {
        if (OVRInput.GetDown(magicButton, handController))
        {
            magicPad.SetActive(true);
            magicPad.GetComponent<MAR_MagicPatternPadThree>().BallsInit();
            magicPad.transform.forward = PlayerPos.transform.forward;
            magicPad.transform.position = transform.position + Vector3.forward * 0.1f;
            magicPad.transform.parent = null;
            touchIndex = 0;
            MAR_HandState.handState[whatHand] = MAR_HandState.State.MAGIC_CONTROLL_3;
        }        
    }

    void MagicChoice()
    {
        Ray ray = new Ray(transform.position+transform.forward * -0.075f + transform.right*term, transform.forward+ transform.right * term);
        RaycastHit[] hit;
        hit = Physics.RaycastAll(ray, 0.15f);
        if(hit.Length>0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform.name.Contains("Ball"))
                {
                    if (hit[i].transform.name.Contains("Fire"))
                    {
                        return;
                    }
                    MAR_MagicPatternBallTestThree mp = hit[i].transform.gameObject.GetComponent<MAR_MagicPatternBallTestThree>();
                    if (touchIndex != 0)
                    {
                        if (touchPattern[touchIndex - 1] == mp.id)
                        {
                            return;
                        }
                    }
                    mp.TouchHand();
                    touchPattern[touchIndex] = mp.id;
                    touchIndex++;
                    if (PatternCheck())
                    {
                        magicPad.SetActive(false);
                        magicPad.transform.position = transform.position + Vector3.forward * 0.2f;
                        magicPad.transform.parent = transform;
                        MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
                    }
                }
            }
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
        }
        if((magicPad.transform.position - transform.position).magnitude > 0.3f)
        {
            magicPad.SetActive(false);
            magicPad.transform.parent = transform;
            MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
        }
    }    

}