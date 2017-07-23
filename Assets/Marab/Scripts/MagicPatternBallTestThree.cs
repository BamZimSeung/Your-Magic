using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPatternBallTestThree : MonoBehaviour {
    public int id;
    Vector3 lScale;
    public float scaleSpeed = 5;

    private void Start()
    {
        lScale = transform.localScale;
    }

    public void TouchByHand()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.5f * scaleSpeed * Time.deltaTime);
    }

    public void Init()
    {
        transform.localScale = lScale;
    }
}
