using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    public GameObject Splash;
    public void Off()
    {
		StartCoroutine(DelayedOff());  // Start the coroutine 
	}

    IEnumerator DelayedOff()
    {
        yield return new WaitForSeconds(0f);
		Splash.SetActive(false);
	}
}
