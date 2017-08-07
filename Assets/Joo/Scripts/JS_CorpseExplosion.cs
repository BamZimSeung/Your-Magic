using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_CorpseExplosion : MonoBehaviour {

    public float power;

	void Start () {
        CorpseExplosion();
    }

    void CorpseExplosion()
    {
        Rigidbody[] rbs = gameObject.GetComponentsInChildren<Rigidbody>();

        for(int i = 0; i < rbs.Length; i++)
        {
            rbs[i].AddExplosionForce(power + Random.Range(-100f,0f), transform.position, 1f, 5f);
        }
    }	
}
