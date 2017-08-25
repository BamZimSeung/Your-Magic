using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_HandShield : MonoBehaviour {

	void Start () {
		// 소리 재생
	}

    int whathand = 0;

    public void SetWhatHand(int hand)
    {
        whathand = hand;
    }




    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("EnemyBullet"))
        {
            // 소리 재생
            WB_Audio.instance.HandPlay(WB_Audio.AudioState.shield_active, whathand);
            MAR_TouchTest.instance.NormalVibration(whathand);
        }
    }
}
