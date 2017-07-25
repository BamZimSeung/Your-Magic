using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player가 큐브랑 부딪히면 포탈열림.

public class WB_OpenPortal : MonoBehaviour {

    public GameObject portal;
    // Use this for initialization

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains("Cube"))
        {
            portal.SetActive(true);
        }
    }
}
