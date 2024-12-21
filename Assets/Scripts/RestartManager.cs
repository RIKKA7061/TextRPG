using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
	public GameObject Ending;
	private StackTextManager stackTextManager;

	private void Start()
	{
		stackTextManager = FindAnyObjectByType<StackTextManager>();
	}

	public void RestartBtnMaker()
	{
		stackTextManager.AddButtonBelowLastText(1, "�ٽ��ϱ�"); //BtnText
	}

	public void RestartBtnAction()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Ending.SetActive(false);
	}
}
