using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MAR_MagicControllerTestThreeOther : MonoBehaviour
{
<<<<<<< HEAD
    int[,] patternArray =new int[3, 9] { 
        //파이어볼
        { 1, 4, 7, 8, 9, 0, 0, 0, 0 },
        //에너지 볼트
        { 5, 6, 0, 0, 0, 0, 0, 0, 0 },
        //쉴드
        { 3, 2, 5, 8, 7, 0, 0, 0, 0 }

    };
    int patternsNum = 3;
=======
    int[,] patternArray =new int[2, 9] { { 1, 4, 7, 8, 9, 0, 0, 0, 0 }, { 5, 6, 0, 0, 0, 0, 0, 0, 0 } };
    int patternsNum = 2;
>>>>>>> origin/master

    int[] touchPattern = new int[9];
    int touchIndex = 0;

    public GameObject[] magicPrefab;
    
    //어떤 손과 어떤 버튼을 기준으로 할지
    public OVRInput.Controller handController;
    public OVRInput.Button magicButton;
    public OVRInput.Button grabButton;

    //마법 패드를 나타낼 프리팹
    public GameObject magicPatternPrefab;

    //생성된 마법 패드
    GameObject magicPattern;
<<<<<<< HEAD
    GameObject shield;
=======
>>>>>>> origin/master

    public Transform PlayerPos;
    
    int whatHand;


    bool isGrabbing = false;
    public float grabRange=0.1f;
    public LayerMask magicBallLayer;
    GameObject grabbedObject;

    LineRenderer lr;
    bool isLineRender = false;
    

    // Use this for initialization
    void Start()
    {
        //매직패드를 미리 생성시켜 놓아서
        //나중에 보여주기만 한다.
        magicPattern = Instantiate(magicPatternPrefab);
        magicPattern.GetComponent<MAR_MagicPatternPadThree>().SetParentScript(this);
        magicPattern.GetComponent<MAR_MagicPatternPadThree>().SettingPad();
        lr=GetComponent<LineRenderer>();


        //위치 설정 후 부모 설정
        magicPattern.transform.position = transform.position + Vector3.forward * 0.1f;
        magicPattern.transform.parent = transform;

        magicPattern.SetActive(false);

        shield = Instantiate(magicPrefab[2]);
        shield.transform.parent = transform;
        shield.SetActive(false);
        if (handController == OVRInput.Controller.LTouch)
        {
            whatHand = (int)MAR_HandState.Hand.LEFT;
            magicPattern.name = magicPattern.name + "Left";
<<<<<<< HEAD
            shield.transform.position = transform.position + transform.right * -0.1f;
            shield.transform.forward = transform.right;
=======
>>>>>>> origin/master
        }
        else
        {
            whatHand = (int)MAR_HandState.Hand.RIGHT;
            magicPattern.name.Insert(0, "Right");
            magicPattern.name = magicPattern.name + "Right";
<<<<<<< HEAD
            shield.transform.position = transform.position + transform.right * 0.1f;
            shield.transform.forward = -transform.right;
=======
>>>>>>> origin/master
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        switch (MAR_HandState.handState[whatHand])
        {
            case MAR_HandState.State.SHIELD:
                if (OVRInput.GetDown(magicButton, handController))
                {
                    shield.SetActive(false);
                    MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
                }
                break;
            case MAR_HandState.State.IDLE:
                MagicShowCheck();
                break;
            case MAR_HandState.State.MAGIC_CONTROLL_3:
                if (isGrabbing == false && OVRInput.GetDown(grabButton, handController))
                {
                    //물체잡는 동작
                    GrabMagicObject();
                }
                MagicUnShowCheck();
                break;
        }
        if (isLineRender)
        {
            if (lr.positionCount <= 1)
            {
                lr.enabled = false;
                return;
            }
            lr.SetPosition(lr.positionCount - 1, transform.position);
        }
    }


    public void MagicShowCheck()
    {
        if (OVRInput.GetDown(magicButton, handController))
        {
                magicPattern.SetActive(true);
                magicPattern.GetComponent<MAR_MagicPatternPadThree>().BallsInit();
                magicPattern.transform.parent = null;
                Vector3 rot = transform.transform.localRotation.eulerAngles;
                rot.x = 0;
                rot.z = 0;
                magicPattern.transform.eulerAngles = rot;
<<<<<<< HEAD
=======
            Debug.Log(transform.localRotation.eulerAngles + "," + rot);
>>>>>>> origin/master
            magicPattern.transform.position = transform.position + PlayerPos.forward * 0.1f;
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
            magicPattern.SetActive(false);
            Destroy(grabbedObject);
            magicPattern.transform.position = transform.position + Vector3.forward * 0.1f;
            magicPattern.transform.parent = transform;
            isGrabbing = false;
         }
        lr.positionCount--;
    }

    bool PatternCheck()
    {
        if (touchIndex == 1)
        {
            for (int i = 0; i < patternsNum; i++)
            {
                if (touchPattern[0] == patternArray[i, 0])
                {
                    magicPattern.GetComponent<MAR_MagicPatternPadThree>().SetLineRender(patternArray, i, lr);
                    isLineRender = true;
                }
            }
            return false;
        }
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
<<<<<<< HEAD
            CastingMagic(magicPrefab[index]);
=======
            if (magicPrefab[index].name.Contains("Bolt"))
            {
                GetComponent<MAR_MagicShoot>().SetMagic();
            }
            else
            {
                GetComponent<MAR_HandControllerTestOther>().SetGrabObject(magicPrefab[index]);
                GetComponent<MAR_MagicControllerTestTwoOther>().SetLastMagic(magicPrefab[index]);
            }
>>>>>>> origin/master
            return true;
        }
    }

    void GrabMagicObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, grabRange, 0.0f, magicBallLayer);
        touchIndex = 0;
        if (hits.Length > 0)
        {
            int closest = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                if (whatHand == 0 && hits[i].transform.parent.name.Contains("Right"))
                {
                    continue;
                }
                else if(whatHand == 1 && hits[i].transform.parent.name.Contains("Left"))
                {
                    continue;
                }
                    if (closest < 0)
                    {
                        closest = i;
                    }
                    else if (hits[i].distance <= hits[closest].distance)
                    {
                        closest = i;
                    }
            }

            if (closest < 0)
            {
                isGrabbing = false;
                return;
            }
            
            grabbedObject =Instantiate(hits[closest].transform.gameObject);
            hits[closest].transform.gameObject.SetActive(false);
            grabbedObject.transform.parent = transform;
            grabbedObject.transform.position = transform.position;
            grabbedObject.AddComponent<Rigidbody>();
            grabbedObject.GetComponent<Collider>().isTrigger = false;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            touchPattern[touchIndex]=hits[closest].transform.GetComponent<MAR_MagicPatternBallTestThree>().id;
            touchIndex++;
            PatternCheck();
            isGrabbing = true;
        }
    }



    void CastingMagic(GameObject magic)
    {
        if (magic.name.Contains("Bolt"))
        {
            GetComponent<MAR_MagicShoot>().SetMagic();
        }
        else if (magic.name.Contains("Shield"))
        {
            shield.SetActive(true);
            MAR_HandState.handState[whatHand] = MAR_HandState.State.SHIELD;
        }
        else
        {
            GetComponent<MAR_HandControllerTestOther>().SetGrabObject(magic);
            GetComponent<MAR_MagicControllerTestTwoOther>().SetLastMagic(magic);
        }

    }
    




    void GrabMagicObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, grabRange, 0.0f, magicBallLayer);
        touchIndex = 0;
        if (hits.Length > 0)
        {
            int closest = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                if (whatHand == 0 && hits[i].transform.parent.name.Contains("Right"))
                {
                    continue;
                }
                else if(whatHand == 1 && hits[i].transform.parent.name.Contains("Left"))
                {
                    continue;
                }
                    if (closest < 0)
                    {
                        closest = i;
                    }
                    else if (hits[i].distance <= hits[closest].distance)
                    {
                        closest = i;
                    }
            }

            if (closest < 0)
            {
                isGrabbing = false;
                return;
            }
            
            grabbedObject =Instantiate(hits[closest].transform.gameObject);
            hits[closest].transform.gameObject.SetActive(false);
            grabbedObject.transform.parent = transform;
            grabbedObject.transform.position = transform.position;
            grabbedObject.AddComponent<Rigidbody>();
            grabbedObject.GetComponent<Collider>().isTrigger = false;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            touchPattern[touchIndex]=hits[closest].transform.GetComponent<MAR_MagicPatternBallTestThree>().id;
            touchIndex++;
            PatternCheck();
            isGrabbing = true;
        }
    }

    void MagicUnShowCheck()
    {
        if (OVRInput.GetDown(magicButton, handController))
        {
            magicPattern.SetActive(false);
            magicPattern.transform.parent = transform;
            MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
            if (grabbedObject != null)
            {
                Destroy(grabbedObject);
            }
            isGrabbing = false;
            lr.positionCount = 0;
        }
        if(OVRInput.GetUp(grabButton,handController) && isGrabbing == true && grabbedObject != null)
        {
            magicPattern.SetActive(false);
            magicPattern.transform.parent = transform;
            Destroy(grabbedObject);
            MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
            isGrabbing = false;
            lr.positionCount = 0;
        }
    }    

}