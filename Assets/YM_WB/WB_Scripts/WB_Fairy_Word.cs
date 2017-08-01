using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WB_Fairy_Word : MonoBehaviour {
    public Text fairyText;
    public List<string> pages;
    
    public int now_number = 0;
	// Use this for initialization

	void Start () {
        pages = new List<string>();
        InitWord();
        fairyText.text = pages[0]; // 시작 텍스트.
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1")) // 추후에 스토리에 맞게 진행방법을 정해야함.
        {
            now_number++;
            if (now_number < pages.Count)
            {
                fairyText.text = Set_word(now_number);
            }
        }
	}

    string Set_word(int now_number)
    {
        return pages[now_number];
    }

    void InitWord() // 요정이 할말 적기.
    {
        // stage0
        pages.Add("Hellow");
        pages.Add("용사");
        // stage1
        pages.Add("1탄시작");
        pages.Add("뀨");
        // stage2
    }
}
