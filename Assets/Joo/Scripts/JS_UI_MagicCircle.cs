using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JS_UI_MagicCircle : MonoBehaviour {

    // 표시할 룬들
    public GameObject[] runes;

    // 이펙트 프리팹
    public GameObject effectPrefab;

    // 이펙트 표시 위치
    public Transform[] effectPos;

    // 사라지는 딜레이 시간
    public float dissapearDelayTime;

    void OnEnable()
    {
        SetActiveRune(1);
    }

    void OnDisable()
    {
        // 켜진 룬 초기화
        for(int i= 0; i < runes.Length; i++)
        {
            runes[i].SetActive(false);
        }
    }
    
    public void SetActiveRune(int num)
    {
        runes[num - 1].SetActive(true);

         Instantiate(effectPrefab, effectPos[(num - 1) * 2].position, Quaternion.identity).transform.parent = transform;
         Instantiate(effectPrefab, effectPos[(num - 1) * 2 + 1].position, Quaternion.identity).transform.parent = transform;

        if (num == 3)
        {
            Invoke("SetSelfInactive", dissapearDelayTime);
        }
    }

    void SetSelfInactive()
    {
        gameObject.SetActive(false);
    }
}
