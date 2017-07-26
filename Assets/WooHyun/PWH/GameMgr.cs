using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour {

    public Transform[] points;

    public GameObject monsterPrefab;

    public float createTime = 2.0f;

    public int maxMonster = 5;

    public bool isGameOver = false;

    public int pre ;

	void Start () {
        points = GameObject.Find("GamePoint").GetComponentsInChildren<Transform>();

        if(points.Length>0)
        {
            StartCoroutine(this.CreateMonster());
        }
	}

    IEnumerator CreateMonster()
    {
        while (!isGameOver)
        {
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("Monster").Length;
            if (monsterCount < maxMonster)
            {
                yield return new WaitForSeconds(createTime);

                
                int idx = Random.Range(1, points.Length);

                
                 if(pre == idx)
                {
                    idx = Random.Range(1, points.Length);
                    
                }
                 else 
                 {
                     pre= idx;
                     Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
                 }

                 
                

               
            }
            else
            {
                yield return null;
            }
        }
    }
       
    

}
