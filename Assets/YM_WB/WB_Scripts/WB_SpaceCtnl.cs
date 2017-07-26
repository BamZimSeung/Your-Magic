using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WB_SpaceCtnl : MonoBehaviour {
    public float delayScene = 5;
	// Use this for initialization
	void Start () {
        StartCoroutine("DelayScene");
	}
	
	IEnumerator DelayScene()
    {
        yield return new WaitForSeconds(delayScene);
        SceneManager.LoadScene("WB_Scene1");
    }
}
