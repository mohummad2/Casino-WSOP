using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using BLabTexasHoldEmProject;


namespace BLabProjectMultiplayer.LoginController {

public class ProductionLoginControllerAtStart : MonoBehaviour {


    public GameObject MainGamePanel;
	public GameObject LoginPanel;
	public GameObject PanelMainGuestData;
	public GameObject PanelMainFacebookData;
	public GameObject PanelMainRegister;

#if USE_FACEBOOK
	public BBFacebookController facebookController;
#endif

	public Toggle[] guestAvatarList;
    public Sprite[] guestAvatarOrigin;

    public InputField guestPlayerName; 

    public bool checkConnectionAtStart = false;


	void OnConnectedToPhoton(){
#if USE_PHOTON
#endif
	}

	void OnJoinedLobby(){
#if USE_PHOTON
#endif
	}

	public void OnDisconnectedFromPhoton() {
	}

	public void gotFacebookResult(bool _success) {
	  if(_success) {
		  saveFacebookData();
		  PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__LOGGED_TYPE,MultiplayerCommonStaticData.PlayerLoggedType.Facebook.ToString());
		  MainGamePanel.SetActive(true);
		  LoginPanel.SetActive(false);
		  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	  } else {
		  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	  }
	}

	// Use this for initialization
	void Start () {

#if UNITY_EDITOR
  gameObject.AddComponent<BBGetScreenShoot>();
#endif

#if USE_PHOTON
            PhotonNetwork.PhotonServerSettings.JoinLobby = true;
#endif

			if(PlayerPrefs.HasKey(ProductionMainMenuController.PlayerPreferKeyNames.GOT_PLAYER_INFO_DATA)) {

				MainGamePanel.SetActive(true);

		    } else {

				LoginPanel.SetActive(true);
#if UNITY_WEBGL
			GameObject.Find("ButtonRegister").GetComponent<Button>().interactable = false; 
#endif

#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
				GameObject.Find("ButtonFacebook").GetComponent<Button>().interactable = true;
#else
				GameObject.Find("ButtonFacebook").GetComponent<Button>().interactable = false;
#endif
		    }
		
	}

	public void buttonPressController(GameObject _go) {

	 switch(_go.name) {
	  case "ButtonGuest":
		PanelMainGuestData.SetActive(true);
		setGuestPlayerPictureOnToggle();
		string guestTempPlayerName = "player" + UnityEngine.Random.Range(0, 100).ToString();
		guestPlayerName.text = guestTempPlayerName;
	  break;
		case "ButtonBackToMainFromGuest":
		  savePlayerInfoData();
		  MainGamePanel.SetActive(true);
		  LoginPanel.SetActive(false);
		break;
		case "ButtonFacebook":
		 PanelMainFacebookData.SetActive(true);
#if USE_FACEBOOK
  if(facebookController == null) {
	facebookController = GameObject.Find("BBFacebook").GetComponent<BBFacebookController>();
  }
		 facebookController.executeFBLogin();
#endif
		break;
		case "ButtonBackToMainFromFacebook":
		 saveFacebookData();
		  MainGamePanel.SetActive(true);
		  LoginPanel.SetActive(false);
		break;
		case "ButtonRegister":
		  PanelMainRegister.SetActive(true);
		break;
	 }

	}

	void saveFacebookData() {
#if USE_FACEBOOK 
			Texture2D guestAvatarPicture = null;
	        string s_guestPlayerName = facebookController.ProfileName;
	        guestAvatarPicture = facebookController.playerAvatar.texture;
			PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER_SHARE_PICTURE, BBStaticVariable.getStringByteFromTexture(guestAvatarPicture));
			PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__SHARE_NAME, s_guestPlayerName);
			PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.GOT_PLAYER_INFO_DATA,"YES");
#endif
	}

	void savePlayerInfoData() {
	  Texture2D guestAvatarPicture = null;
	  string s_guestPlayerName = guestPlayerName.text;

		for(int x = 0;x < guestAvatarList.Length;x++) {
		 if(guestAvatarList[x].isOn) {
			guestAvatarPicture = guestAvatarOrigin[x].texture; //guestAvatarList[x].transform.Find("Image").GetComponent<Image>().sprite.texture;
			break;
		 }
		}

			PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER_SHARE_PICTURE, BBStaticVariable.getStringByteFromTexture(guestAvatarPicture));
			PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__SHARE_NAME, s_guestPlayerName);
			PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.GOT_PLAYER_INFO_DATA,"YES");


			Debug.Log("Player Image : " + PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER_SHARE_PICTURE));
			Debug.Log("Player Name : " + PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__SHARE_NAME));

	}

	void setGuestPlayerPictureOnToggle() {
		for(int x = 0;x < guestAvatarList.Length;x++) {
				Texture2D newTex = BBStaticVariable.ScaleTexture(guestAvatarList[x].transform.Find("Image").GetComponent<Image>().sprite.texture,64,64);
				Texture2D rTex = BBStaticVariable.CalculateTexture(64,64,32,32,32,newTex);
		        Sprite tmpSprite = Sprite.Create (rTex, new Rect(0,0,rTex.width,rTex.height), new Vector2(.5f,.5f));
				guestAvatarList[x].transform.Find("Image").GetComponent<Image>().sprite = tmpSprite;
		}
	}


}
}