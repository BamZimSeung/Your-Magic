using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_MagicShieldTest : MonoBehaviour {
    public GameObject LHand;
    public GameObject RHand;
    public Transform PlayerPos;

    public GameObject handShield;
    GameObject shield;
    
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

        if (MAR_HandState.handState[(int)MAR_HandState.Hand.LEFT] == MAR_HandState.State.IDLE
            && MAR_HandState.handState[(int)MAR_HandState.Hand.RIGHT] == MAR_HandState.State.IDLE)
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
                    MAR_HandState.handState[(int)MAR_HandState.Hand.LEFT] = MAR_HandState.State.SHIELD;
                    MAR_HandState.handState[(int)MAR_HandState.Hand.RIGHT] = MAR_HandState.State.SHIELD;
                }

            }
        }

        else if (MAR_HandState.handState[(int)MAR_HandState.Hand.LEFT] == MAR_HandState.State.SHIELD
            && MAR_HandState.handState[(int)MAR_HandState.Hand.RIGHT] == MAR_HandState.State.SHIELD)
        {
            if (!(Mathf.Abs(LHand.transform.position.y - RHand.transform.position.y) <= 0.1f))
            {
                shield.SetActive(false);
                MAR_HandState.handState[(int)MAR_HandState.Hand.LEFT] = MAR_HandState.State.IDLE;
                MAR_HandState.handState[(int)MAR_HandState.Hand.RIGHT] = MAR_HandState.State.IDLE;
            }
            if (!((LHand.transform.right.z <= -0.4f) && (RHand.transform.right.z * -1 <= -0.4f)
                && (LHand.transform.position.x > RHand.transform.position.x)))
            {
                shield.SetActive(false);
                MAR_HandState.handState[(int)MAR_HandState.Hand.LEFT] = MAR_HandState.State.IDLE;
                MAR_HandState.handState[(int)MAR_HandState.Hand.RIGHT] = MAR_HandState.State.IDLE;
            }
        }
    }
}
