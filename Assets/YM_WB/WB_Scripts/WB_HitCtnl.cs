using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 일단 마우스클릭하면 맞은 걸로 치고 구현.
// 플레이어를 약간 흔들고
// 화면에 빨간 이미지를 잠시 뛰운다.
// hit bar 차오르게.


public class WB_HitCtnl : MonoBehaviour {
    public Image red_img;
    public float density; // 흔드는 강도.
    public float delayMove;
    Vector3 startPos;
    Vector3 changePos;
                          // Update is called once per frame
    private void Start()
    {
        startPos = transform.position;
        changePos = Vector3.left;
    }

    void Update () {
		if(Input.GetButton("Fire1")) // 클릭
        {
            // 플레이어 흔들기
            StartCoroutine("PlayerMove");
        }
        
        
	}

    IEnumerator PlayerMove()
    {
        transform.position = transform.position + Vector3.left * density + Vector3.back * density;
        red_img.enabled = true;
        yield return new WaitForSeconds(delayMove);
        transform.position = Vector3.Lerp(transform.position, startPos, 0.2f);
        red_img.enabled = false;
    }
}
