using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagicControllerTestTwo : MonoBehaviour
{
    enum Magic_State
    {
        IDLE,
        SHOW,
        CHOICE,
        GRAB
    }


    public enum DIR
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3,
        NOTHING = 5
    }

    //어떤 손과 어떤 버튼을 기준으로 할지
    public OVRInput.Controller handController;
    public OVRInput.Controller touch;
    public OVRInput.Button magicButton;

    //마법을 저장하는 배열
    public GameObject[] magicPrefab;

    //생성된 마법을 저장하는 배열
    public GameObject[] magicArray;

    //마법들을 보여줄 기본 위치
    public GameObject magicPadPrefab;

    //생성된 마법 패드
    GameObject magicPad;

    protected Vector3 magicLScale;
    public float bigScale = 2.0f;
    public float scaleSpeed = 5.0f;

    Magic_State _state = Magic_State.IDLE;

    //매직 패드에 중심과의 거리
    public float magicTerm = 0.075f;

    // Use this for initialization
    void Start()
    {
        //매직패드를 미리 생성시켜 놓아서
        //나중에 보여주기만 한다.
        magicPad = Instantiate(magicPadPrefab);
        magicArray = new GameObject[4];

        //위치 설정 후 부모 설정
        magicPad.transform.position = transform.position + Vector3.right * 0.1f;
        magicPad.transform.up = transform.right;
        magicPad.transform.parent = transform;



        //매직 패드에 들어갈 마법 설정
        for (int i = 0; i < 4; i++)
        {
            GameObject magic = Instantiate(magicPrefab[i]);
            magic.transform.parent = magicPad.transform;
            magicArray[i] = magic;
            if (i == 0)
            {
                magic.transform.localPosition = new Vector3(0, 0, magicTerm);
            }
            else if (i == 1)
            {
                magic.transform.localPosition = new Vector3(magicTerm, 0, 0);
            }
            else if (i == 2)
            {
                magic.transform.localPosition = new Vector3(0, 0, -magicTerm);
            }
            else if (i == 3)
            {
                magic.transform.localPosition = new Vector3(-magicTerm, 0, 0);
                magicLScale = magic.transform.localScale;
            }
            else
            {
                Debug.Log("MagicPadInit Error");
            }
        }

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
                MagicUnShowCheck();
                break;
        }


    }


    void MagicShowCheck()
    {
        if(transform.localRotation.eulerAngles.z>=70 && transform.localRotation.eulerAngles.z <= 110)
        {
            if(OVRInput.GetDown(magicButton, handController)||true)
            {
                magicPad.SetActive(true);
                _state = Magic_State.CHOICE;
            }
        }
    }


    void MagicUnShowCheck()
    {
        if(!(transform.localRotation.eulerAngles.z >= 70 && transform.localRotation.eulerAngles.z <= 110))
        {
            magicPad.SetActive(false);
            _state = Magic_State.IDLE;
        }
    }


    

}