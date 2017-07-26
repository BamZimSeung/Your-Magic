using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//fire1 버튼을 누르면 총알을 발사한다. 
//1, 어느위치?
//2. 어느방향
//3. 총알이 필요(총알공장주소)

public class Test_PlayFire : MonoBehaviour {

    public GameObject bulletFactory;

    public Transform firePosition;
    // * public GameObject cosshair;
    public Vector3[] power = new Vector3[13] {
   new Vector3( 0f, 1.8f, 6.3f ),
   new Vector3( 0f, 3.5f, 6.8f ),
new Vector3( 0, 1.1f, 12.9f ),
new Vector3( 0, 4.3f, 12.1f),
new Vector3( 0, 7.6f, 9.2f ),
new Vector3( 0, 7.0f, 9.6f ),
new Vector3( 0, 2.8f, 3.7f),
new Vector3( 0, 0.9f, 13.5f ),
new Vector3( 0, 5.8f, 10.0f),
new Vector3( 0, 5.3f,11.5f ),
new Vector3( 0, 1.2f, 11.5f ),
new Vector3( 0, 4.2f, 6.8f ),
new Vector3( 0, 2.4f, 8.6f )
       };

    Vector3 originSize;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update() {

        //CrosshairPosition.rotation.z = ;

       // * cosshair.transform.localScale = CrosshairPosition.localScale * 0.03f ;


        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.position = firePosition.position+firePosition.forward;
            bullet.transform.forward = firePosition.forward;
            bullet.GetComponent<Rigidbody>().velocity = power[Random.Range(0, 13)];
            bullet.GetComponent<Rigidbody>().useGravity = true;
            bullet.GetComponent<Rigidbody>().isKinematic = false;
            bullet.GetComponent<Collider>().enabled = true;
        }
    }
}
