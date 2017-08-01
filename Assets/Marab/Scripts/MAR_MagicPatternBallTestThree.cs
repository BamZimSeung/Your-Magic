using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_MagicPatternBallTestThree : MonoBehaviour {
    public int id;    

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Collider>().isTrigger==false)
        {
            return;
        }
        if (other.name.Contains("MagicBall"))
        {
            if((transform.parent.name.Contains("Left") && other.transform.parent.name.Contains("left")))
            {
                GetComponent<MeshRenderer>().material = other.GetComponent<MeshRenderer>().material;
                GetComponent<Collider>().enabled = false;
                transform.parent.GetComponent<MAR_MagicPatternPadThree>().parentScript.MagicChoice(id);
            }
            else if(transform.parent.name.Contains("Right") && other.transform.parent.name.Contains("right"))
            {
                GetComponent<MeshRenderer>().material = other.GetComponent<MeshRenderer>().material;
                GetComponent<Collider>().enabled = false;
                transform.parent.GetComponent<MAR_MagicPatternPadThree>().parentScript.MagicChoice(id);
            }
        }
    }
}
