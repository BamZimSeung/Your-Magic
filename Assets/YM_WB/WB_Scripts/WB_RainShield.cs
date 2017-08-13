using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 라인 쉴드는 마법을 막고 이놈의 체력을 깍음.

    // 마법은 쉴드를 만나면 공격을 하지않고 터지는 이펙트만 표현.
    // 쉴드는 마법을 만나면 쉴드에 영향을 줬다는 작은 이펙트 표현 + 자기 체력 깍기.


public class WB_RainShield : MonoBehaviour {
    public float MaxHP = 10f; // 일단 10방.
    public float cur_HP;
    public GameObject connectPrefab;

    // Use this for initialization
    // Update is called once per frame
    private void Start()
    {
        cur_HP = MaxHP;
    }

    void Update () {
		if(cur_HP < MaxHP/2)
        {
            // 매츄리얼 바꾸기 반쯤 깨진놈으로다가.
        }
        else if(cur_HP <= 0)
        {
            // 쉴드 삭제.
            Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        // 되나? 충돌시 레이생성 -> 포인트 검출.
        Ray ray = new Ray(other.transform.position, other.transform.forward);
        RaycastHit hitInfo;

        // 마법이 존나 회전에서 날아온다면? 원형으로 ray 방출.좁은범위, 첫번째 닿는 지점에 생성ㅎ.
        if(other.CompareTag("Monster"))
        {
            cur_HP--; // 쉴드 체력 1씩달음.
            Debug.Log("ComapreTag");
            if(Physics.Raycast(ray, out hitInfo, 1 << LayerMask.NameToLayer("RainShield")))
            {
                Debug.Log("Rayin");
                GameObject connect = Instantiate(connectPrefab);
                connect.transform.position = hitInfo.point; // 생성된 충돌이미지 -> 힛 인포 포인트에
            }
            
        }
        //Destroy(other.gameObject);
    }
}
