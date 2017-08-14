using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 이 스크립트는 하르트의 애니메이션을 컨트롤함.
// 하르트는 플레이어를 향해 Nav로 이동하고있을것임.
// 방패가 깨지고 한방 맞으면 죽게 설정.
// 방패가 있는한 무적.

public class WB_HartAnimCtnl : MonoBehaviour {

    Animator anim;
    public float deathDelay;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (transform.childCount < 4) // 방패가 깨짐.
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Grabble")) // 공격을 받음.
            {
                anim.SetTrigger("death"); // 죽음 애니메이션 ㄱ
                StartCoroutine("death");
            }
        }
    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(deathDelay); // 딜레이 후
        Destroy(this.gameObject); // 삭제
    }
}
