using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_WarpPoint : MonoBehaviour {

    public ParticleSystem[] pss;
    public Color defaultColor;
    public Color selectedColor;

    public bool isSelected = false;

    public float stateChangeTime = 0.3f;
    float currentTime = 0;

	void Start () {
        for (int i = 0; i < pss.Length; i++)
        {
            pss[i].startColor = defaultColor;
        }
    }
	
	void Update () {
        if (isSelected)
        {
            currentTime += Time.deltaTime;
            if(currentTime > stateChangeTime)
            {
                currentTime = 0;
                isSelected = false;
                for(int i =0; i < pss.Length; i++)
                {
                    pss[i].startColor = defaultColor;
                }
            }
        }
	}

    public void WarpPointSelected()
    {
        if (!isSelected)
        {
            for (int i = 0; i < pss.Length; i++)
            {
                pss[i].startColor = selectedColor;
            }
            isSelected = true;
        }
        currentTime = 0;
    }
}
