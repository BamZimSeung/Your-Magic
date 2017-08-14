﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 공중 몹은 노드 1,2,3을 반복적으로 왔다갔다 하면서 미사일을 주기적으로 발사한다.(+ 미사일을 발사할때는 멈춘다.)
// 공중 몹은 항상 플레이어를 바라본다.


    // node갯수가 유동적이라면?
public class WB_AirMonster : MonoBehaviour {

    public GameObject Player;
    public GameObject node, node1, node2;
    public GameObject bulletPrefab;

    public float moveSpeed = 2f;
    public float fireDelay = 5f;
    public float currentTime;
    public enum moveState
    {
        first,
        second,
        third,
        fourth,
    }

    public moveState my_move;

	// Use this for initialization
	void Start () {
        transform.position = node.transform.position; // 시작위치 = 처음노드.
        currentTime = 0;
        my_move = moveState.first;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Player.transform);
        currentTime += Time.deltaTime;
        if(currentTime > fireDelay)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
            currentTime = 0;
        }

        switch(my_move)
        {
            case moveState.first:
                if (Vector3.Distance(transform.position, node1.transform.position) > 0.5f)
                {
                    Move(node, node1);
                }
                else
                {
                    my_move = moveState.second;
                }
                break;
            case moveState.second:
                if (Vector3.Distance(transform.position, node2.transform.position) > 0.5f)
                {
                    Move(node1, node2);
                }
                else
                {
                    my_move = moveState.third;
                }
                break;
            case moveState.third:
                if (Vector3.Distance(transform.position, node1.transform.position) > 0.5f)
                {
                    Move(node2, node1);
                }
                else
                {
                    my_move = moveState.fourth;
                }
                break;
            case moveState.fourth:
                if (Vector3.Distance(transform.position, node.transform.position) > 0.5f)
                {
                    Move(node1, node);
                }
                else
                {
                    my_move = moveState.first;
                }
                break;
        }
	}

    void Move(GameObject origin, GameObject next) // 움직이는 함수.
    {
        Vector3 dir = next.transform.position - origin.transform.position;
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
    }
}