using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class CheckForSingleInstance : MonoBehaviour {

	// Use this for initialization
	void Awake () {
#if !UNITY_EDITOR

#if UNITY_IOS || UNITY_ANDROID

#else
        Process myProcess = Process.GetCurrentProcess();

        UnityEngine.Debug.Log("Process : " + myProcess.Id);

		if(PlayerPrefs.HasKey("instanceCheck")) {

			if(PlayerPrefs.GetInt("instanceCheck") == myProcess.Id) {

			} else {
				Application.Quit();
			}
 
		} else {
			PlayerPrefs.SetInt("instanceCheck",myProcess.Id);
		}

		DontDestroyOnLoad(transform.gameObject);

		CheckForSingleInstance[] csi = FindObjectsOfType(typeof(CheckForSingleInstance))  as CheckForSingleInstance[];

		if(csi.Length > 1)
         {
             Destroy(gameObject);
         }
#endif
		
#endif
	}

	void Start() {
	}

	void OnApplicationQuit()
    {
		PlayerPrefs.DeleteKey("instanceCheck");
    }
	
}
