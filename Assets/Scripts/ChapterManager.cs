using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    // �� ��ũ��Ʈ�� ������ é�͸� ��Ÿ���ϴ�. ��, é���� ���õ� å���� ���ϴ�.


    public TextMeshProUGUI chapterUI;
    static public int ChapterBigNum = 1;  // é�� '1' - 1
	static public int ChapterSmallNum = 1;// é�� 1 - '1'

	private void Start()
	{
		chapterUI.text = "é��" + " " + ChapterBigNum + "-" + ChapterSmallNum;
	}
}
