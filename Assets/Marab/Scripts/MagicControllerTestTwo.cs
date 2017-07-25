﻿using System.Collections;
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
    public OVRInput.Button magicButton;

    //마법을 저장하는 배열
    public GameObject[] magicPrefab;

    //생성된 마법을 저장하는 배열
    public GameObject[] magicArray;

    //마법들을 보여줄 기본 위치
    public GameObject magicPadPrefab;

    //생성된 마법 패드
    GameObject magicPad;

    public float bigScale = 2.0f;
    public float scaleSpeed = 5.0f;

    Magic_State _state = Magic_State.IDLE;

    //매직 패드에 중심과의 거리
    public float magicTerm = 0.075f;

    int whatHand;

    // Use this for initialization
    void Start()
    {
        //매직패드를 미리 생성시켜 놓아서
        //나중에 보여주기만 한다.
        magicPad = Instantiate(magicPadPrefab);
        magicArray = new GameObject[4];
        //위치 설정 후 부모 설정

        //매직 패드에 들어갈 마법 설정
        for (int i = 0; i < 4; i++)
        {
            GameObject magic = Instantiate(magicPrefab[i]);
            magic.transform.parent = magicPad.transform;
            magic.transform.up = magicPad.transform.up;
            magic.transform.localScale = Vector3.one * 0.03f;
            if (i == 0)
            {
                magic.transform.localPosition = new Vector3(0, 0, magicTerm);
            }
            else if (i == 1)
            {
                magic.transform.localPosition = new Vector3(magicTerm, 0, 0);
                magic.transform.localScale = Vector3.one;
            }
            else if (i == 2)
            {
                magic.transform.localPosition = new Vector3(0, 0, -magicTerm);
            }
            else if (i == 3)
            {
                magic.transform.localPosition = new Vector3(-magicTerm, 0, 0);
            }
            else
            {
                Debug.Log("MagicPadInit Error");
            }
            magicArray[i] = magic;
        }

        magicPad.SetActive(false);

        if (handController == OVRInput.Controller.LTouch)
        {
            whatHand = (int)HandState.Hand.LEFT;
            magicPad.transform.position = transform.position + Vector3.right * 0.1f;
            magicPad.transform.up = transform.right;
        }
        else
        {
            whatHand = (int)HandState.Hand.RIGHT;
            magicPad.transform.position = transform.position - Vector3.right * 0.1f;
            magicPad.transform.up = transform.right * -1;
        }
        magicPad.transform.parent = transform;

    }



    // Update is called once per frame
    void Update()
    {
        switch (HandState.handState[whatHand])
        {
            case HandState.State.IDLE:
                MagicShowCheck();
                break;
            case HandState.State.MAGIC_CONTROLL_2:
                MagicUnShowCheck();
                break;
        }
    }


    void MagicShowCheck()
    {
        if((transform.localRotation.eulerAngles.z>=60 && transform.localRotation.eulerAngles.z <= 120) && whatHand==(int)HandState.Hand.LEFT)
        {
            if(OVRInput.GetDown(magicButton, handController)||true)
            {
                magicPad.SetActive(true);
                HandState.handState[whatHand] = HandState.State.MAGIC_CONTROLL_2;
            }
        }
        if ((transform.localRotation.eulerAngles.z <= 300 && transform.localRotation.eulerAngles.z >= 240) && whatHand == (int)HandState.Hand.RIGHT)
        {
            if (OVRInput.GetDown(magicButton, handController) || true)
            {
                magicPad.SetActive(true);
                HandState.handState[whatHand] = HandState.State.MAGIC_CONTROLL_2;
            }
        }
    }


    void MagicUnShowCheck()
    {
        if (!(transform.localRotation.eulerAngles.z >= 40 && transform.localRotation.eulerAngles.z <= 140) && whatHand == (int)HandState.Hand.LEFT)
        {
            if (OVRInput.GetDown(magicButton, handController) || true)
            {
                magicPad.SetActive(false);
                HandState.handState[whatHand] = HandState.State.IDLE;
            }
        }
        if (!(transform.localRotation.eulerAngles.z <= 320 && transform.localRotation.eulerAngles.z >= 220) && whatHand == (int)HandState.Hand.RIGHT)
        {
            if (OVRInput.GetDown(magicButton, handController) || true)
            {
                magicPad.SetActive(false);
                HandState.handState[whatHand] = HandState.State.IDLE;
            }
        }
    }


    

}