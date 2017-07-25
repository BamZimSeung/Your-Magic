using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 마법 선택 1번째 방법

/*
1. 터치의 조이스틱을 통한 마법 선택
 - 터치를 눌렀을때 임시적으로 마법 4개 정도 보여준다.
   ㄴ 터치를 눌렀을 때, 4개가 동서남북으로 각각 뜨게한다.
      ㄴ 이때, 그 손을 부모로 가지며 같이 움직인다.
   ㄴ 조이스틱을 조작하여 그 방향으로 가면 커지고, 멀리가면 작아지게 한다.
      ㄴ 거리를 이용
 - 선택하고 손을 놓으면 선택된 마법이 바로 손에 들리게 한다.
   ㄴ 선택하고 터치를 떼면 그 마법이 그 위치에 생성되게 한다.
      ㄴ 그 마법은 집을 수 있다.
*/

public class MagicRightControllerTestOne : MagicControllerTestOne
{


    override public void MagicScaleChange(DIR dir)
    {
        OVRInput.Button thumDir;
        switch (dir)
        {
            case DIR.UP:
                thumDir = OVRInput.Button.SecondaryThumbstickUp;
                break;
            case DIR.RIGHT:
                thumDir = OVRInput.Button.SecondaryThumbstickRight;
                break;
            case DIR.DOWN:
                thumDir = OVRInput.Button.SecondaryThumbstickDown;
                break;
            case DIR.LEFT:
                thumDir = OVRInput.Button.SecondaryThumbstickLeft;
                break;
            default:
                thumDir = OVRInput.Button.SecondaryThumbstick;
                break;
        }
        
        if (OVRInput.Get(thumDir, touch))
        {
            Vector3 ls = magicArray[(int)dir].transform.localScale;
            ls = Vector3.Lerp(ls, magicLScale * bigScale, 0.3f * scaleSpeed * Time.deltaTime);
            magicArray[(int)dir].transform.localScale = ls;
        }
        else
        {
            Vector3 ls = magicArray[(int)dir].transform.localScale;
            ls = Vector3.Lerp(ls, magicLScale, 0.3f * scaleSpeed * Time.deltaTime);
            magicArray[(int)dir].transform.localScale = ls;
        }
    }

    override public DIR ChoiceWhat()
    {

        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp, touch))
        {
            return DIR.UP;
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown, touch))
        {
            return DIR.DOWN;
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft, touch))
        {
            return DIR.LEFT;
        }
        else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight, touch))
        {
            return DIR.RIGHT;
        }
        else
        {
            return DIR.NOTHING;
        }
    }
    

}
