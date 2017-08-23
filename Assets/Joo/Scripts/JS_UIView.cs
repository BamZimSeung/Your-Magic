using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_UIView : MonoBehaviour {

    public GameObject crosshair;
    public float value;
    Vector3 originScale;

    void Start()
    {
        originScale = crosshair.transform.localScale;
    }

    // Update is called once per frame
    void Update () {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hitinfo;

        int layerMask = 1 << LayerMask.NameToLayer("UI");

        layerMask = ~layerMask;

        if (Physics.Raycast(ray, out hitinfo, 1000, layerMask))
        {
            //크로스헤어의 위치 = 레이가 부딪힌 곳
            crosshair.transform.position = hitinfo.point;

            //크로스헤어의 크기를 원래 크기 * 거리 * 보정값
            crosshair.transform.localScale = originScale * hitinfo.distance * value;
        }
        else
        {
            crosshair.transform.localPosition = Vector3.forward * 100;
            crosshair.transform.localScale = originScale * 100 * value;
        }
    }
}
