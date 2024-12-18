using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
	public GameObject Ending;
    public void RestartBtn()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Ending.SetActive(false);
	}
}
