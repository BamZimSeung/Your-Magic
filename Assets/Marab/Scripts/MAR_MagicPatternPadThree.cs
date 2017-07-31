using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_MagicPatternPadThree : MonoBehaviour {

    LineRenderer lr;


    private void OnEnable()
    {
        lr = GetComponent<LineRenderer>();
    }
    public MAR_MagicPatternBallTestThree[] balls;

    public void BallsInit()
    {
        lr.enabled = false;
        for(int i = 0; i < 9; i++)
        {
            balls[i].Init();
            balls[i].GetComponent<Collider>().enabled = true;
        }
    }
    
    public void SetLineRender(int[,] array,int choice)
    {
        for(int i = 0; i < 9; i++)
        {
            if (array[choice,i] == 0)
            {
                break;
            }
            lr.positionCount = i + 1;
            lr.SetPosition(i,balls[array[choice, i] - 1].transform.position);

        }
        lr.enabled = true;
    }
}
