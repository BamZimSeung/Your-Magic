using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_MagicPatternPadThree : MonoBehaviour {
    
    [HideInInspector]
    public MAR_MagicControllerTestThreeOther parentScript;
    public MAR_MagicPatternBallTestThree[] balls;
    Material[] ballMats=new Material[9];
    
    public void SettingPad()
    {
        for(int i = 0; i < 9; i++)
        {
            ballMats[i]=balls[i].GetComponent<MeshRenderer>().material;
        }
    }
    

    public void SetParentScript(MAR_MagicControllerTestThreeOther script)
    {
        parentScript = script;
    }


    public void BallsInit()
    {
        for (int i = 0; i < 9; i++)
        {
            balls[i].GetComponent<MeshRenderer>().material=ballMats[i];
            balls[i].GetComponent<Collider>().enabled = true;
            balls[i].gameObject.SetActive(true);
        }
    }
    
    public void SetLineRender(int[,] array,int choice, LineRenderer lr)
    {
        lr.positionCount = 0;
        for(int i = 8; i >= 0; i--)
        {
            if (array[choice,i] == 0)
            {
                continue;
            }
            lr.positionCount = lr.positionCount+1;
            lr.SetPosition(lr.positionCount-1,balls[array[choice, i]-1].transform.position);
        }
        lr.enabled = true;
    }
}
