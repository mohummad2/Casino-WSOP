using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BLabTexasHoldEmProject {

public class BBGetScreenShoot : MonoBehaviour {
	
	 string levelName = "level_1";
	 int progr = 0;

	public string suffix = "";

	// Use this for initialization
	void Start () {

#if !UNITY_EDITOR
	//	Destroy(gameObject);	
#endif
	

	  progr = 0;

		levelName = SceneManager.GetActiveScene().name;

	}
	
	// Update is called once per frame
	void Update () {


		
		if(Input.GetKeyUp(KeyCode.J)) {
			
			ScreenCapture.CaptureScreenshot("_Screenshot_" + Screen.width + "_" + Screen.height + "_" + suffix + "_" + levelName + "_"  + progr + ".png");	
			progr++;
		}
	
	}
}
}