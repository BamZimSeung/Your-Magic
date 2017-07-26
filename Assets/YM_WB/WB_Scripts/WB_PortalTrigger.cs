using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WB_PortalTrigger : MonoBehaviour {

    public GameObject Player;
    public Transform Space;
    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("WB_Space");
    }
}
