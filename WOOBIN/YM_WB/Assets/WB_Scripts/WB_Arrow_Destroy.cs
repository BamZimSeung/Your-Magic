using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 생성된 화살표는 일정시간후(도착시간을 고려해서) 지워집니다.
// 도착시간 - 0.5초 정도면 충분할 것같음.
public class WB_Arrow_Destroy : MonoBehaviour {

    public WB_Arrow_Ctrl ac;
    public float destroyDelay = 0.5f;
    float currentTime = 0f;
    // Use this for initialization
    // Update is called once per frame
    void Start()
    {
        ac = GameObject.Find("Find_Destination").GetComponent<WB_Arrow_Ctrl>();
    }
    void Update () {
        currentTime += Time.deltaTime;
        if(currentTime >  ac.des_time - destroyDelay)
        {
            Destroy(this.gameObject);
        }
		
	}
}
