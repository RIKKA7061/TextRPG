using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnMakerManager : MonoBehaviour
{
    

    public void BtnThemeChecker()
    {
        switch (ChapterManager.ChapterBigNum)
        {
            // é�� 1
            case 1:
                switch (ChapterManager.ChapterSmallNum)
                {
                    // é�� 1-1
                    case 1:
                        Debug.Log("yes no ��ư �����");
						

						break;
                }
                break;
        }
    }
}
