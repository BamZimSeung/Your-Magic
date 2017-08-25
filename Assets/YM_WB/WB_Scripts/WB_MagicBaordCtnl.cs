using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 보드에 스크립트 몇초후 삭제 
public class WB_MagicBaordCtnl : MonoBehaviour {

    public float destroyDelay = 5f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, destroyDelay);
	}
}
