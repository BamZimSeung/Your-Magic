using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_MagicFingerPointTestThree : MonoBehaviour {

    MAR_MagicControllerTestThreeOther handParentScript;

    public void SetHandParent(MAR_MagicControllerTestThreeOther script)
    {
        handParentScript = script;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("MagicBall"))
        {
            MAR_MagicPatternBallTestThree mp = other.transform.gameObject.GetComponent<MAR_MagicPatternBallTestThree>();
            other.GetComponent<Collider>().enabled = false;
            mp.TouchHand();
            handParentScript.MagicChoice(mp.id);
        }
    }


}
