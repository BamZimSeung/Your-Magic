using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AirMonster Spawn 스크립트는
// AirMonster -> Drone을 일정 시간간격으로 생성하는데
// Drone에게 필요한 GameObject를 달아준다.


public class WB_AirMonSpawn : MonoBehaviour {
    public GameObject Player;
    public GameObject[] Nodes;

    void Start()
    {
        Nodes = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Nodes[i] = transform.GetChild(i).gameObject;
        }
    }
}
