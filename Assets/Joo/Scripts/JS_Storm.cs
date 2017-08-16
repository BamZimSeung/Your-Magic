using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_Storm : MonoBehaviour {

    // 기다리는 시간
    public float delayTime;

    // 공격 시간
    public float attackTime;

    // 공격 간격
    public float attackGap;

    // 공격력
    public int power;

    // 공격 범위
    public float attackRange;

	void Start () {
        Invoke("StormAttack", delayTime);
	}
	
    void StormAttack()
    {
        StartCoroutine("Storming");
    }

    IEnumerator Storming()
    {
        // 근처 적을 검출
        Collider[] colInfos = Physics.OverlapSphere(transform.position, attackRange, 1 << LayerMask.NameToLayer("Enemy"));

        Debug.Log(colInfos.Length);

        WaitForSeconds wfs = new WaitForSeconds(attackGap);

        float time = 0;

        // 공격 간격마다 공격
        while (time < attackTime)
        {
            for(int i = 0; i < colInfos.Length; i++)
            {
                colInfos[i].transform.GetComponent<JS_Monster>().MonsterDamage(power);
            }

            yield return wfs;

            time += attackGap;
        }
    }
}
