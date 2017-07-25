using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShieldTest : MonoBehaviour {
    public GameObject LHand;
    public GameObject RHand;
    public Transform PlayerPos;

    public GameObject handShield;
    GameObject shield;

    public Vector3[] motion = new Vector3[2] { new Vector3(312.4f, 74.6f, 38.8f), new Vector3(312.4f, 257.4f, 334.3f) };

    private void Start()
    {
        shield = Instantiate(handShield);
        shield.transform.localScale *= 2;
        shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (shield.activeSelf == true)
        {
            shield.transform.forward = PlayerPos.forward;
            shield.transform.position = (LHand.transform.position + RHand.transform.position) / 2.0f + (LHand.transform.right*-1 + RHand.transform.right)*0.1f;
        }

        if (HandState.handState[(int)HandState.Hand.LEFT] == HandState.State.IDLE
            && HandState.handState[(int)HandState.Hand.RIGHT] == HandState.State.IDLE)
        {
            if ((LHand.transform.right.z <= -0.4f) && (RHand.transform.right.z * -1 <= -0.4f)
                && (LHand.transform.position.x > RHand.transform.position.x))
            {
                if (Mathf.Abs(LHand.transform.position.y - RHand.transform.position.y) <= 0.1f)
                {
                    Debug.Log("CHEKC");
                    shield.SetActive(true);
                    shield.transform.forward = PlayerPos.forward;
                    shield.transform.position = (LHand.transform.position + RHand.transform.position) / 2.0f + (LHand.transform.right * -1 + RHand.transform.right) * 0.1f;
                    HandState.handState[(int)HandState.Hand.LEFT] = HandState.State.SHIELD;
                    HandState.handState[(int)HandState.Hand.RIGHT] = HandState.State.SHIELD;
                }

            }
        }

        else if (HandState.handState[(int)HandState.Hand.LEFT] == HandState.State.SHIELD
            && HandState.handState[(int)HandState.Hand.RIGHT] == HandState.State.SHIELD)
        {
            if (!(Mathf.Abs(LHand.transform.position.y - RHand.transform.position.y) <= 0.1f))
            {
                shield.SetActive(false);
                HandState.handState[(int)HandState.Hand.LEFT] = HandState.State.IDLE;
                HandState.handState[(int)HandState.Hand.RIGHT] = HandState.State.IDLE;
            }
            if (!((LHand.transform.right.z <= -0.4f) && (RHand.transform.right.z * -1 <= -0.4f)
                && (LHand.transform.position.x > RHand.transform.position.x)))
            {
                shield.SetActive(false);
                HandState.handState[(int)HandState.Hand.LEFT] = HandState.State.IDLE;
                HandState.handState[(int)HandState.Hand.RIGHT] = HandState.State.IDLE;
            }
        }
    }
}
