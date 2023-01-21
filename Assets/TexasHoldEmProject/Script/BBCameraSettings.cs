using UnityEngine;
using System.Collections;

namespace BLabTexasHoldEmProject {

public class BBCameraSettings : MonoBehaviour {

	public float setting_16_9 = 3.53f;
	public float setting_3_2 = 3.99f;
	public float setting_4_3 = 4.49f;

	void Start () {

		string ratio = BBStaticData.getRatio(GetComponent<Camera>());

		Debug.Log("----------------------------->> Ratio : " + ratio);

		switch(ratio) {
		case "16:9":
		 GetComponent<Camera>().orthographicSize = setting_16_9;
		break;
		case "3:2":
			GetComponent<Camera>().orthographicSize = setting_3_2;
		break;
		case "4:3":
			GetComponent<Camera>().orthographicSize = setting_4_3;
		break;
		}

	}

		
}
}