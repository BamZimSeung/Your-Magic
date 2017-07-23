using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagicControllerTestThree : MonoBehaviour
{
    enum Magic_State
    {
        IDLE,
        SHOW,
        CHOICE,
        GRAB
    }
    

    int[,] patternArray =new int[1, 9] { { 2, 5, 8, 0, 0, 0, 0, 0, 0 } } ;
    int patternsNum = 1;

    int[] touchPattern = new int[9];
    int touchIndex = 0;



    //어떤 손과 어떤 버튼을 기준으로 할지
    public OVRInput.Controller handController;
    public OVRInput.Controller touch;
    public OVRInput.Button magicButton;

    //마법 패드를 나타낼 프리팹
    public GameObject magicPadPrefab;

    //생성된 마법 패드
    GameObject magicPad;

    Magic_State _state = Magic_State.IDLE;

    //매직 패드에 중심과의 거리
    public float magicTerm = 0.075f;

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


    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case Magic_State.IDLE:
                MagicShowCheck();
                break;
            case Magic_State.CHOICE:
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
            magicPad.transform.parent = null;
            touchIndex = 0;
            _state = Magic_State.CHOICE;            
        }        
    }

    void MagicChoice()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1))
        {
            if (hit.transform.name.Contains("Ball"))
            {
                MagicPatternBallTestThree mp = hit.transform.gameObject.GetComponent<MagicPatternBallTestThree>();
                mp.TouchByHand();
                touchPattern[touchIndex] = mp.id;
                if (PatternCheck())
                {
                    magicPad.SetActive(false);
                    magicPad.transform.position = transform.position + Vector3.forward * 0.1f;
                    magicPad.transform.parent = transform;
                    _state = Magic_State.IDLE;
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
                isCheck = patternArray[i,j] == touchPattern[j];
                if (!isCheck)
                { break; }
            }
            if (isCheck)
            {
                index = i;
            }
        }
        Debug.Log(index);
        if (index == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    void MagicUnShowCheck()
    {
        if((magicPad.transform.position - transform.position).magnitude > 10)
        {
            magicPad.SetActive(false);
            magicPad.GetComponent<MagicPatternPadThree>().BallsInit();
            magicPad.transform.position = transform.position + Vector3.forward * 0.1f;
            magicPad.transform.parent = transform;
            _state = Magic_State.IDLE;
        }
    }    

}