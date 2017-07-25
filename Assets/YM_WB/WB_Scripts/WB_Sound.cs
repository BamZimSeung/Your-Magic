using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WB_Sound : MonoBehaviour {
    public AudioClip ac;
    public Transform pos;
	// Use this for initialization
	void Start () {
        AudioSource.PlayClipAtPoint(ac, pos.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
