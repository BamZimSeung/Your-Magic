using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_EffectDestroy : MonoBehaviour {

    public float destroyTime;

	void Start () {
        Destroy(gameObject, destroyTime);
	}
}
