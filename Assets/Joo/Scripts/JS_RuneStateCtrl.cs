using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_RuneStateCtrl : MonoBehaviour {

    public Material activeMat;
    public Material inactiveMat;

    public MeshRenderer runeMR;

    public GameObject effects;

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
            runeMR.material = activeMat;
            effects.SetActive(false);
        }
    }
}
