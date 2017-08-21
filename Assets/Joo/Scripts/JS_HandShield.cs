using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_HandShield : MonoBehaviour {

	void Start () {
		// 소리 재생
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("EnemyBullet"))
        {
            // 소리 재생
        }
    }
}
