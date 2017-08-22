using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_WeakPoint : MonoBehaviour {

    // 유지 시간
    public float maintainTime;

    // 깜빡임 주기
    public float twinklingTime;

    public Color originColor;
    public Color changedColor;

    Material WP_material;

    float currentTime;

    void Awake()
    {
        WP_material = GetComponent<MeshRenderer>().material;
    }

	void Start () {
        ResetValue();
    }

    void OnEnable()
    {
        ResetValue();
        StartCoroutine("Twinkiling");
    }

    void OnDisable()
    {
        StopCoroutine("Twinkiling");
    }
	
	void Update () {
        currentTime += Time.deltaTime;

        if (currentTime > maintainTime)
        {
            gameObject.SetActive(false);
        }
	}

    void ResetValue()
    {
        currentTime = 0;
        WP_material.color = originColor;
    }

    IEnumerator Twinkiling()
    {
        WaitForSeconds wfs = new WaitForSeconds(twinklingTime);

        bool isOriginColor = true;

        while (true)
        {
            if (isOriginColor)
            {
                WP_material.color = Color.Lerp(WP_material.color, changedColor, 0.4f);

                if (WP_material.color.g > changedColor.g - 0.05f)
                {
                    isOriginColor = false;
                }
            }
            else
            {
                WP_material.color = Color.Lerp(WP_material.color, originColor, 0.4f);

                if (WP_material.color.g < originColor.g + 0.05f)
                {
                    isOriginColor = true;
                }
            }

            yield return wfs;

            //Debug.Log(WP_material.color.g);
           // Debug.Log(isOriginColor);
        }
    }
}
