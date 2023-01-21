using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace BLab.GetCoinsSystem {

public class GetCoinsController : MonoBehaviour {

	public Text TextAbsoluteMoneyWon;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		gameObject.AddComponent<BLabTexasHoldEmProject.BBGetScreenShoot>();
#endif				

	}

	void Update() {
		TextAbsoluteMoneyWon.text = String.Format("{0:0,0}", PlayerPrefs.GetFloat("MPGeneralPlayerMoney")) + " $"; 
	}
	
	void OnGenericButtonClick (GameObject _go) {
		switch(_go.name) {
		 case "ButtonGetCoinsByRewardedVideo":
#if	USE_UNITY_ADV && (UNITY_ANDROID || UNITY_IOS)
		   UnityAdvertisingController.ShowAdPlacement(UnityAdvertisingController.zoneIdrewardedVideo);
#endif
		 break;
		 case "ButtonGetCoinsByInAppPurchase":
			SceneManager.LoadScene("InAppPurchaseScene");
		 break;
		 case "ButtonExit":
		  SceneManager.LoadScene(0);
		 break;
		}
	}
}
}