using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AirMonster Spawn 스크립트는
// AirMonster -> Drone을 일정 시간간격으로 생성하는데
// Drone에게 필요한 GameObject를 달아준다.


public class WB_AirMonSpawn : MonoBehaviour {
    public float spawnDelay;
    public GameObject dronePrefab;
    public float currentTime = 0f;
    public WB_AirMonster airmonctnl;
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
    // Update is called once per frame
    void Update () {
        currentTime += Time.deltaTime;
        if(currentTime > spawnDelay)
        {
            GameObject drone = Instantiate(dronePrefab);
            drone.transform.position = transform.position;
            airmonctnl = drone.GetComponent<WB_AirMonster>();
            airmonctnl.Spawn = this.gameObject;
            for(int i=0; i< transform.childCount; i++)
            {
                airmonctnl.nodes.Add(Nodes[i]);
            }
            airmonctnl.Player = Player;
            currentTime = 0f;
        }
	}
}
