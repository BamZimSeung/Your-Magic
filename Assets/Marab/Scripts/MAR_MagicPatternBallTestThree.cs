using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_MagicPatternBallTestThree : MonoBehaviour {
    public int id;
    Vector3 lScale=new Vector3(0.025f,0.025f,0.025f);
    float scalSpeed = 10;
        

    public void TouchHand()
    {
        StartCoroutine("TouchByHand");
    }



    IEnumerator TouchByHand()
    {
        while (true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.5f * scalSpeed * Time.deltaTime);
            if((transform.localScale-Vector3.zero).magnitude <= 0.01f)
            {
                break;
            }
            yield return null;
        }
        yield return null;
    }

    public void Init()
    {
        StopCoroutine("TouchByHand");
        transform.localScale = lScale;
    }
}
