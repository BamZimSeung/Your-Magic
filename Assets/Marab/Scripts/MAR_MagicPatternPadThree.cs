using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_MagicPatternPadThree : MonoBehaviour {

    public MAR_MagicPatternBallTestThree[] balls;

    public void BallsInit()
    {
        for(int i = 0; i < 9; i++)
        {
            balls[i].Init();
        }
    }
}
