using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    // 이 스크립트는 현재의 챕터를 나타냅니다. 즉, 챕터의 관련된 책임을 집니다.


    public TextMeshProUGUI chapterUI;
    static public int ChapterBigNum = 1;  // 챕터 '1' - 1
	static public int ChapterSmallNum = 1;// 챕터 1 - '1'

	private void Start()
	{
		chapterUI.text = "챕터" + " " + ChapterBigNum + "-" + ChapterSmallNum;
	}
}
