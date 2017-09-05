using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_GravityBall : MonoBehaviour {

    // 그래비티볼 생성 시
    // 영역 진입 대상 리스트에 추가
    // 리스트 내에 대상 주기적으로 중력 적용

    public float forcePower;
    public float addForceGap;
    public List<GameObject> targets;
    public ForceMode fM;

    void Start()
    {
        StartCoroutine("GiveGravityForce");
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Enemy") || col.CompareTag("Object") || col.CompareTag("Corpse"))
        {
            targets.Add(col.gameObject);
        }
    }

    IEnumerator GiveGravityForce()
    {
        WaitForSeconds wfs = new WaitForSeconds(addForceGap);

        Vector3 forceDir = Vector3.zero;

        while (true)
        {
            for(int i = 0; i < targets.Count; i++)
            {
                forceDir = transform.position - targets[i].transform.position;
                forceDir = Vector3.Normalize(forceDir) * forcePower;
                targets[i].GetComponent<Rigidbody>().AddForce(forceDir, fM);

                if (targets[i].CompareTag("Enemy"))
                {
                    targets[i].GetComponent<JS_Monster>().MonsterDamage(0, JS_Monster.MonProperty.Normal);
                }
            }
            yield return wfs;
        }
    }
}
