using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WB_Fairy_Word : MonoBehaviour {
    public Text fairyText;
    public List<string> pages;
    
    public int now_number = 0;
    // Use this for initialization
    private void OnEnable()
    {
        pages = new List<string>();
        InitWord();
        fairyText.text = pages[0]; // 시작 텍스트.
    }
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Set_word(int now_number)
    {
        fairyText.text = pages[now_number]; 
    }

    void InitWord() // 요정이 할말 적기.
    {
        // stage0
        pages.Add("검지로 컨트롤러의 트리거 버튼을 누르고 있으면 패드가 나옵니다. 떼지마세요.");
        pages.Add("컨트롤러를 움직여서 쓰고 싶은 마법에 가져다 대면 그 마법으로 바뀝니다. 아직 떼지마세요.");
        pages.Add("이제 검지로 누른 트리거 버튼을 떼면 마법을 사용할 수 있습니다. 던지거나 조준해보세요.");
        pages.Add("다양한 마법을 써보세요.");
        pages.Add("마법이 아니라 돌이 잡히면 마나가 부족한 상태입니다.");
        //5
        pages.Add("두 손을 가운데로 모으고 컨트롤러가 앞을 향하도록 해보세요.");
        pages.Add("마나가 다차면 자동으로 상태가 풀립니다.");
        pages.Add("모션을 이용한 마법이 두 가지 있습니다.");
        pages.Add("먼저 왼손을 위로 움직이고 오른손 아래로 움직인 뒤 왼손을 내리고 오른손을 올리면서 원을 그려보세요.");
        pages.Add("이 모션을 사용하면 체력이 회복됩니다.");

        //10
        pages.Add("다음은 두 손을 머리위로 올리시고 잠시 기다려보세요.");
        pages.Add("전면에 있는 적들에게 전부 데미지를 주는 광역마법입니다.");
        pages.Add("이제 앞에 있는 균열을 잡으면 원래 장소로 이동합니다..");
    }
}
