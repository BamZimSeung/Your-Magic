using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 포탈이 활성화 되고 몇초후에 파티클 ㄱ
public class WB_PortalCtnl : MonoBehaviour {
    public float DelayTime = 1f;
    float currentTime = 0;
    // Use this for initialization
    public GameObject particle_one;
    public GameObject particle_two;
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        if(currentTime > DelayTime)
        {
            particle_one.SetActive(true);
            particle_two.SetActive(true);
        }
	}
}
