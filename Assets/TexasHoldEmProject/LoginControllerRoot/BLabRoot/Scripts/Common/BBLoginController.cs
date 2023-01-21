using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using BLabTexasHoldEmProject;

namespace BLabProjectMultiplayer.LoginController {

public class BBLoginController : MonoBehaviour {

    public enum LoginAccessResult {None,RegisterSuccess,RegisterFail,LoginSuccess,LoginFail}
	LoginAccessResult loginAccessResult;

    	public Text TextMessage;
	

	public GameObject mainAccessController;
   

	
#if USE_PHOTON

	bool gotIfLoggedInresult = false;

	void Awake() {
	  if(PhotonNetwork.connected) {
	    PhotonNetwork.Disconnect();
	  }
	}

	// Use this for initialization
	void Start () {


		if(BBStaticVariableMultiplayer.gotPlayerLoggedIn) {
			mainAccessController.SetActive(true);
		} else {
		}



		  GameObject simButt = new GameObject("ButtonGuest");
		  gotButtonAction(simButt);


	}


	public void gotButtonAction(GameObject _go) {

	   switch(_go.name) {
		  case "ButtonLogin": case "ButtonGuest":

			    Texture2D simAvatarImage = null;
			    string tmpPlayerName = "";
				string tmpPlayereMail = "";

        
			 simAvatarImage = BBStaticVariable.getSpriteFromBytes(PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER_SHARE_PICTURE)).texture;
			 tmpPlayerName = PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__SHARE_NAME);
			 tmpPlayereMail = "notUsed@ciao.com";
		
				GetComponent<ProductionLoginController>().setLocalPlayerData(tmpPlayerName
					                                                        ,tmpPlayereMail,
					                                                        simAvatarImage);

			BBStaticVariableMultiplayer.gotPlayerLoggedIn = true;
			mainAccessController.SetActive(true);


			Destroy(gameObject);

		 break;
		 case "ButtonExitGoMain":
		  SceneManager.LoadScene(0);
		 break;
	   }
	 
	}






#endif			
}
}