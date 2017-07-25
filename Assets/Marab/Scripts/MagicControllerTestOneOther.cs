using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagicControllerTestOneOther : MonoBehaviour
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
        RIGHT,
        LEFT,
        NOTHING 
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

    protected Vector3[] magicLScale;
    public float bigScale = 3.0f;
    public float scaleSpeed = 20.0f;

    Magic_State _state = Magic_State.IDLE;
    DIR _dir = DIR.NOTHING;

    float delayLimit = 500;
    float delay = 0;
    bool isDelay = false;
    int index = 2;
    //매직 패드에 중심과의 거리
    public float magicTerm = 0.075f;

    // Use this for initialization
    void Start()
    {
        //매직패드를 미리 생성시켜 놓아서
        //나중에 보여주기만 한다.
        magicPad = Instantiate(magicPadPrefab);
        magicArray = new GameObject[4];
        magicLScale = new Vector3[4];
        //위치 설정 후 부모 설정
        magicPad.transform.position = transform.position + Vector3.up * 0.1f;
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
            }
            else
            {
                Debug.Log("MagicPadInit Error");
            }
            magicLScale[i] = magic.transform.localScale;
        }

        magicPad.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case Magic_State.IDLE:
                if (OVRInput.GetDown(magicButton, touch))
                {
                    MagicShow();
                }
                break;
            case Magic_State.CHOICE:
                MagicChoice();
                break;
        }


    }


    void MagicShow()
    {
        magicPad.SetActive(true);
        _state = Magic_State.CHOICE;
    }


    void MagicUnShow()
    {
        magicPad.SetActive(false);
        _state = Magic_State.IDLE;
    }

    // 매직패드가 보여진 상태에서
    // 조이스틱의 방향에따라 크기가 커지고 작아지게 한다.
    void MagicChoice()
    {
        if (delay == 0)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft,touch))
            {
                isDelay = true;
                delay++;
                _dir = DIR.LEFT;
            }

            else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, touch))
            {
                isDelay = true;
                delay++;
                _dir = DIR.RIGHT;
            }
            else
            {
                _dir=DIR.NOTHING;
            }
        }
        else
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, touch))
            {
                if (_dir == DIR.RIGHT)
                {
                    delay = 0;
                    _dir = DIR.LEFT;
                }
                else if (_dir == DIR.LEFT)
                {
                    delay += 1;
                    if (delay > delayLimit)
                    {
                        delay = 1;
                        isDelay = true;
                    }
                }
            }

            else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, touch))
            {
                if (_dir == DIR.LEFT)
                {
                    delay = 0;
                    _dir = DIR.RIGHT;
                }
                else if (_dir == DIR.RIGHT)
                {
                    delay += 1;
                    if (delay > delayLimit)
                    {
                        delay = 1;
                        isDelay = true;
                    }
                }
            }
            else
            {
                delay = 0;
                _dir = DIR.NOTHING;
            }
        }

        if (isDelay)
        {
            switch (_dir)
            {
                case DIR.LEFT:
                    MagicLeftRot();
                    break;
                case DIR.RIGHT:
                    MagicRightRot();
                    break;
                case DIR.NOTHING:
                    break;
            }
            isDelay = false;
        }

        ScaleChanger();


        if (OVRInput.GetDown(magicButton, touch))
        {
                GameObject magic = Instantiate(magicPrefab[index]);
                magic.transform.position = transform.position;
                magic.transform.localScale = Vector3.one * 0.1f;
                magic.GetComponent<Collider>().enabled = true;
                MagicInit();
                MagicUnShow();
        }

    }

    void MagicLeftRot()
    {
        index--;
        if (index < 0)
        {
            index = 3;
        }
    }
    void MagicRightRot()
    {
        index++;
        if (index >3)
        {
            index = 0;
        }
    }
    void ScaleChanger()
    {
        for(int i = 0; i < 4; i++)
        {
            if (i == index)
            {
                Vector3 ls = magicArray[i].transform.localScale;
                ls = Vector3.Lerp(ls, magicLScale[i] * bigScale, 0.3f * scaleSpeed * Time.deltaTime);
                magicArray[i].transform.localScale = ls;
            }
            else
            {
                Vector3 ls = magicArray[i].transform.localScale;
                ls = Vector3.Lerp(ls, magicLScale[i], 0.3f * scaleSpeed * Time.deltaTime);
                magicArray[i].transform.localScale = ls;
            }
        }
    }

    void MagicInit()
    {
        for (int i = 0; i < 4; i++)
        {
            magicArray[i].transform.localScale = magicLScale[i];
        }
    }
    

}