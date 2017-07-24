using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPatternBallTestThree : MonoBehaviour {
    public int id;
    Vector3 lScale;
    float scalSpeed = 10;

    private void OnEnable()
    {
        lScale = transform.localScale;

    }
    private void Start()
    {
    }

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
            Debug.Log(id + "COROUTINE");
            yield return null;
        }
        yield return null;
    }

    public void Init()
    {
        Debug.Log(id + "Init");
        StopCoroutine("TouchByHand");
        transform.localScale = lScale;
    }
}
