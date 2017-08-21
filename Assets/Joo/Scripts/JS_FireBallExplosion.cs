using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_FireBallExplosion : MonoBehaviour {

    public float damageRange;

    public int power;

    public float rigidPower;

    public float upperPower;

    private void OnEnable()
    {
        MAR_MagicSound.instance.FireBallExplosionPlay(transform.position);
    }
    void Start () {
        Explosion();
    }

    void Explosion()
    {
        Ray ray = new Ray(transform.position, transform.position);
        RaycastHit[] hitInfos = Physics.SphereCastAll(ray, damageRange);

        // 검출 됐을 시
        if (hitInfos != null)
        {
            for (int i = 0; i < hitInfos.Length; i++)
            {
                // Enemy 나 Object 일 경우
                if (hitInfos[i].transform.CompareTag("Enemy") || hitInfos[i].transform.CompareTag("Object")|| hitInfos[i].transform.CompareTag("Corpse"))
                {
                    Rigidbody rb = hitInfos[i].transform.GetComponent<Rigidbody>();
                    Vector3 dist = transform.position - hitInfos[i].transform.position;

                    // 폭발적인 힘을 가함
                    rb.AddExplosionForce(rigidPower, transform.position, dist.magnitude, upperPower);
                }
                if (hitInfos[i].transform.CompareTag("Enemy"))
                {
                    // 데미지 주기
                    hitInfos[i].transform.gameObject.GetComponent<JS_Monster>().MonsterDamage(power);
                }
                if (hitInfos[i].transform.CompareTag("BossBullet"))
                {
                    hitInfos[i].transform.GetComponent<JS_BossRangeAttack>().Damaged(power);
                }
            }
        }
    }
}
