﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_MagicShoot : MonoBehaviour {

    //어떤 손과 어떤 버튼을 기준으로 할지
    public OVRInput.Controller handController;
    public OVRInput.Button magicButton;

    int whatHand;

    public int magicCount = 30;
    int count = 0;

    public GameObject energyBolt;
    GameObject showMagic;
    bool isShoot = false;

    public void SetMagic()
    {
        showMagic = Instantiate(energyBolt);
        if (whatHand == 0)
        {
            showMagic.transform.position = transform.position - transform.right * 0.1f;
            showMagic.transform.parent = transform;
        }
        else
        {
            showMagic.transform.position = transform.position + transform.right * 0.1f;
            showMagic.transform.parent = transform;
        }
        MAR_HandState.handState[whatHand] = MAR_HandState.State.MAGIC_SHOOT;
        count = magicCount;
        isShoot = true;
    }

    private void Start()
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
    void Update ()
    {
        switch (MAR_HandState.handState[whatHand])
        {
            case MAR_HandState.State.MAGIC_SHOOT:
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, handController))
                {
                    MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
                    Destroy(showMagic);
                    isShoot = false;
                }
                break;
        }
        if (isShoot&&count > 0)
        {
            if (OVRInput.GetDown(magicButton, handController))
            {
                GameObject bolt = Instantiate(energyBolt);
                bolt.transform.parent = null;
                bolt.transform.position = transform.position;
                bolt.transform.forward = transform.forward;
                bolt.transform.localScale = Vector3.one * 0.1f;
                bolt.GetComponent<Collider>().enabled = true;
                bolt.GetComponent<Rigidbody>().useGravity = true;
                bolt.GetComponent<Rigidbody>().isKinematic = false;
                bolt.GetComponent<Rigidbody>().velocity=transform.forward*10f;
                MAR_TouchTest.instance.ShootVibration(whatHand);
                count--;
            }
        }
        else if(isShoot&&count<=0)
        {
            MAR_HandState.handState[whatHand] = MAR_HandState.State.IDLE;
            Destroy(showMagic);
            isShoot = false;
        }
	}
}
