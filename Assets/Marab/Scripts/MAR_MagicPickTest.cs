using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_MagicPickTest : MonoBehaviour {
    public OVRInput.Controller handController;
    int whatHand;

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
    private void Update()
    {
        Debug.Log(MAR_HandState.handState[0]);
    }
    public void PickMagic(string name)
    {
        if (name.Contains("FireBall"))
        {
            gameObject.GetComponent<MAR_MagicControllerTestThreeOther>().MagicShowCheck("FireBall");
        }
        else if (name.Contains("Energy"))
        {
            MAR_HandState.handState[whatHand] = MAR_HandState.State.MAGIC_SHOOT;
            gameObject.GetComponent<MAR_MagicShoot>().SetMagic();
        }

    }
}
