using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPatternPadThree : MonoBehaviour {

    public MagicPatternBallTestThree[] balls;

    public void BallsInit()
    {
        for(int i = 0; i < 9; i++)
        {
            balls[i].Init();
        }
    }
}
