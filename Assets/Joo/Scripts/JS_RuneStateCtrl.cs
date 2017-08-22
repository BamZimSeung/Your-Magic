using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_RuneStateCtrl : MonoBehaviour {

    // 재질
    public Material activeMat;
    public Material inactiveMat;

    public MeshRenderer runeMR;

    public GameObject effects;

    public GameObject successEffect;
    public GameObject failEffect;

    public bool isActive = false;
	
    public void SetRuneActive()
    {
        if (!isActive)
        {
            isActive = true;
            runeMR.material = activeMat;
            effects.SetActive(true);
        }
    }

    public void SetRuneInactive()
    {
        if (isActive)
        {
            isActive = false;
            runeMR.material = inactiveMat;
            effects.SetActive(false);
        }
    }

    public void MagicSuccess()
    {
        Instantiate(successEffect, transform.position, Quaternion.identity);
    }

    public void MagicFail()
    {
        Instantiate(failEffect, transform.position, Quaternion.identity);
    }
}
