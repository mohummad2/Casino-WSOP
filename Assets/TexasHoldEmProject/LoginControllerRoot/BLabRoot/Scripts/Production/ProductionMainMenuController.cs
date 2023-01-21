using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

using BLabTexasHoldEmProject;

namespace BLabProjectMultiplayer.LoginController {

public class ProductionMainMenuController : MonoBehaviour {

 public string mainMenuSceneToLaunch = "MainMenu";

 public bool setMusicAlwaysOff = true;
 public bool setSoundsAlwaysOff = true;


[System.Serializable]
 public class PlayerPreferKeyNames {
			public const string GOT_PLAYER_INFO_DATA = "GOT_PLAYER_INFO_DATA";
			public const string MUSIC_ON = "MUSIC_ON";
			public const string SOUNDS_ON = "SOUNDS_ON";
			public const string PLAYER_SHARE_PICTURE = "PLAYER_SHARE_PICTURE";
			public const string PLAYER__SHARE_NAME = "PLAYER__SHARE_NAME";
			public const string PLAYER__LOGGED_TYPE = "PLAYER__LOGGED_TYPE";

 }

  public bool savePlayerPicturedataToPersistentDataPath = false;

  public GameObject PanelMain;
  public GameObject PanelMainHeader;
  public GameObject PanelMainSettings;
  public GameObject PanelMainGuestData;
  public GameObject PanelMainGetStartingBetValue;
  public GameObject PanelMainGetCoins;
  public GameObject ButtonCashDonate;

  public GameObject MusicButton;
  public GameObject SoundsButton;

  //public Button LogoutButton;

  public Toggle[] guestAvatarList;
  public Sprite[] guestAvatarOrigin;

  public InputField guestPlayerName; 

  public Image imagePlayerImageOnHeader;
  public Text textPlayerName;
  public Text textPlayerTotalCoins;

		public Text TextlabelGameMinimumBet;
		public Text TextlabelGameStartAtPlayers;

		public Dropdown DDStartingBetValue;

		public Text Score;

		BBGlobalDefinitions _BBGlobalDefinitions;

 void someCleanUp() {

 }

  public void buttonsPressController(GameObject _go) {

			Debug.Log("buttonsPressController : " + _go.name);

     switch(_go.name) {
	  case "ButtonMainMenuSettings":
	     PanelMain.SetActive(false);
	     PanelMainSettings.SetActive(true);
		 setMusicButton( (PlayerPrefs.GetString(PlayerPreferKeyNames.MUSIC_ON) == "ON") );
		 setSoundsButton( (PlayerPrefs.GetString(PlayerPreferKeyNames.SOUNDS_ON) == "ON") );
      break;
	  case "ButtonBackToMainFromSettings":
		 PanelMain.SetActive(true);
	     PanelMainSettings.SetActive(false);
	  break;
	  case "ButtonBackToMainFromGuest":
		   savePlayerInfoData();

		   PlayerPrefs.SetString(PlayerPreferKeyNames.PLAYER__LOGGED_TYPE,MultiplayerCommonStaticData.PlayerLoggedType.Guest.ToString());

		   textPlayerName.text = PlayerPrefs.GetString(PlayerPreferKeyNames.PLAYER__SHARE_NAME);
		   showPlayerPicture();
		   StartCoroutine(showPlayerCoins());

		 PanelMain.SetActive(true);
	     PanelMainGuestData.SetActive(false);
	  break;
	  case "ButtonMusic":
		    if(MusicButton.GetComponent<Image>().enabled == true) {
		     setMusicButton(false);
			} else {
			 setMusicButton(true);
			}
	  break;
	  case "ButtonSound":
			if(SoundsButton.GetComponent<Image>().enabled == true) {
		     setSoundsButton(false);
			} else {
			 setSoundsButton(true);
			}
	  break;
	  case "ButtonEnterGame":

				#if USE_PHOTON				
		        BBStaticVariableMultiplayer.UseDirectFindASeatThenGoIn = false;
		        #endif

				if(PlayerPrefs.HasKey(PlayerPreferKeyNames.GOT_PLAYER_INFO_DATA)) {
					PanelMain.SetActive(false);
			        PanelMainHeader.SetActive(false);
					switch(PlayerPrefs.GetString(PlayerPreferKeyNames.PLAYER__LOGGED_TYPE)) {
					case "Guest":
						MultiplayerCommonStaticData.playerLoggedType = MultiplayerCommonStaticData.PlayerLoggedType.Guest;
					break;
					case "Facebook":
						MultiplayerCommonStaticData.playerLoggedType = MultiplayerCommonStaticData.PlayerLoggedType.Facebook;
					break;
					case "Other":
						MultiplayerCommonStaticData.playerLoggedType = MultiplayerCommonStaticData.PlayerLoggedType.Other;
					break;
					}

					SceneManager.LoadScene(mainMenuSceneToLaunch);

				} else {
					 PanelMain.SetActive(false);
					 PanelMainGuestData.SetActive(true);
					 setGuestPlayerPictureOnToggle();
					 string guestTempPlayerName = "player" + UnityEngine.Random.Range(0, 100).ToString();
					 guestPlayerName.text = guestTempPlayerName;
				}
			


	  break;
	  case "ButtonGoOnSetSatrtingBet":
			if(setMusicAlwaysOff) PlayerPrefs.SetString(PlayerPreferKeyNames.MUSIC_ON,"OFF");
			if(setSoundsAlwaysOff) PlayerPrefs.SetString(PlayerPreferKeyNames.SOUNDS_ON,"OFF");

			switch(DDStartingBetValue.value) {
			 case 0: MultiplayerCommonStaticData.startingBetValue = 10;break;
				case 1: MultiplayerCommonStaticData.startingBetValue = 20;break;
				case 2: MultiplayerCommonStaticData.startingBetValue = 40;break;
				case 3: MultiplayerCommonStaticData.startingBetValue = 50;break;
				case 4: MultiplayerCommonStaticData.startingBetValue = 100;break;
			}


	  break;
	  case "ButtonDeletePlayerPref":
	   PlayerPrefs.DeleteAll();
	   SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	  break;
	  case "ButtonInAppGet_2000":
	  break;
	  case "ButtonInAppGet_5000":
	  break;
	  case "ButtonInAppGet_10000":
	  break;
	  case "ButtonInAppGet_20000":
	  break;
	  case "ButtonMainMenuBuyCoins":
	  break;
	  case "ButtonBackToMainFromGetCoins":
		PanelMainGetCoins.SetActive(false);
		PanelMain.SetActive(true);
	  break;
				case "ButtonCashDonate":
					PanelMainGetCoins.SetActive(true);
					PanelMain.SetActive(true);
					break;
	  case "ButtonLogoutFromSettings":
		PlayerPrefs.DeleteKey(PlayerPreferKeyNames.GOT_PLAYER_INFO_DATA);
		PlayerPrefs.DeleteKey(PlayerPreferKeyNames.PLAYER__LOGGED_TYPE);
	    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	  break;
	  case "ButtonEnterGameFindMeASeat":
		#if USE_PHOTON
		BBStaticVariableMultiplayer.UseDirectFindASeatThenGoIn = true;
		SceneManager.LoadScene("demoMultiplayerMainMenu");
		#endif
	  break;
				case "DailyScore":
					_go.SetActive(false);
					PlayerPrefs.SetInt("Score", (int.Parse(Score.text) + 100));
					if (PlayerPrefs.GetInt("Score") >= 15000)
						PlayerPrefs.SetInt("Score", 15000);
					Score.text = PlayerPrefs.GetInt("Score").ToString();
				break;

					//ПОТОМ УДАЛИТЬ
				case "+SCORE":
					PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 1000);
					if (PlayerPrefs.GetInt("Score") >= 15000)
						PlayerPrefs.SetInt("Score", 15000);
					Score.text = PlayerPrefs.GetInt("Score").ToString();
					break;
     }
  }


	// Use this for initialization
	void Start () {

			if (PlayerPrefs.GetInt("Score") >= 15000)
				PlayerPrefs.SetInt("Score", 15000);
			Score.text = PlayerPrefs.GetInt("Score").ToString();

			System.DateTime Day = System.DateTime.Now;

			if (PlayerPrefs.GetInt("Day") == 0)
				PlayerPrefs.SetInt("Day", Day.Day);

			if (Day.Day != PlayerPrefs.GetInt("Day"))
				transform.Find("DailyScore").gameObject.SetActive(true);

			_BBGlobalDefinitions = Resources.Load("BBGlobalDefinitions") as BBGlobalDefinitions;

			Debug.Log("Platform : " + Application.platform.ToString() + Application.isPlaying + " UseLoginSystem : " + _BBGlobalDefinitions.UseLoginSystem);

	        someCleanUp();

			Screen.sleepTimeout = SleepTimeout.NeverSleep;

	  if(PlayerPrefs.HasKey(PlayerPreferKeyNames.MUSIC_ON)) {
	  } else {
		 PlayerPrefs.SetString(PlayerPreferKeyNames.MUSIC_ON,"ON");
	  }

	  if(PlayerPrefs.HasKey(PlayerPreferKeyNames.SOUNDS_ON)) {
	  } else {
		 PlayerPrefs.SetString(PlayerPreferKeyNames.SOUNDS_ON,"ON");
	  }

	  if(PlayerPrefs.HasKey(PlayerPreferKeyNames.GOT_PLAYER_INFO_DATA)) {

		Debug.Log("PLAYER__LOGGED_TYPE : " + PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__LOGGED_TYPE));

		 textPlayerName.text = PlayerPrefs.GetString(PlayerPreferKeyNames.PLAYER__SHARE_NAME);
		 showPlayerPicture();
	  } else {
		 /*PanelMain.SetActive(false);
		 PanelMainGuestData.SetActive(true);
		 setGuestPlayerPictureOnToggle();
		 string guestTempPlayerName = "player" + UnityEngine.Random.Range(9999,100000).ToString();
		 guestPlayerName.text = guestTempPlayerName;*/
	  }

			if(PlayerPrefs.HasKey(PlayerPreferKeyNames.GOT_PLAYER_INFO_DATA)) {
	            StartCoroutine(showPlayerCoins());
            }
            				
	}

	IEnumerator showPlayerCoins() {
	  yield return new WaitForSeconds(2);
	  //textPlayerTotalCoins.text = BBStaticData.getCashValueFromFloat(GetComponent<CoinsController>().getPlayerCoins()); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setMusicButton(bool wantOn) {
	 if(wantOn) {
			MusicButton.GetComponent<Image>().enabled = true;
			MusicButton.transform.Find("ImageOnHandle").GetComponent<Image>().enabled = true;
			MusicButton.transform.Find("ImageOff").GetComponent<Image>().enabled = false;
			MusicButton.transform.Find("ImageOffHandle").GetComponent<Image>().enabled = false;
				PlayerPrefs.SetString(PlayerPreferKeyNames.MUSIC_ON,"ON");
	 } else {
			MusicButton.GetComponent<Image>().enabled = false;
			MusicButton.transform.Find("ImageOnHandle").GetComponent<Image>().enabled = false;
			MusicButton.transform.Find("ImageOff").GetComponent<Image>().enabled = true;
			MusicButton.transform.Find("ImageOffHandle").GetComponent<Image>().enabled = true;
				PlayerPrefs.SetString(PlayerPreferKeyNames.MUSIC_ON,"OFF");
	 }
	}

	void setSoundsButton(bool wantOn) {
		if(wantOn) {
			SoundsButton.GetComponent<Image>().enabled = true;
			SoundsButton.transform.Find("ImageOnHandle").GetComponent<Image>().enabled = true;
			SoundsButton.transform.Find("ImageOff").GetComponent<Image>().enabled = false;
			SoundsButton.transform.Find("ImageOffHandle").GetComponent<Image>().enabled = false;
				PlayerPrefs.SetString(PlayerPreferKeyNames.SOUNDS_ON,"ON");
	 } else {
			SoundsButton.GetComponent<Image>().enabled = false;
			SoundsButton.transform.Find("ImageOnHandle").GetComponent<Image>().enabled = false;
			SoundsButton.transform.Find("ImageOff").GetComponent<Image>().enabled = true;
			SoundsButton.transform.Find("ImageOffHandle").GetComponent<Image>().enabled = true;
				PlayerPrefs.SetString(PlayerPreferKeyNames.SOUNDS_ON,"OFF");
	 }

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

			PlayerPrefs.SetString(PlayerPreferKeyNames.PLAYER_SHARE_PICTURE, BBStaticVariable.getStringByteFromTexture(guestAvatarPicture));
			PlayerPrefs.SetString(PlayerPreferKeyNames.PLAYER__SHARE_NAME, s_guestPlayerName);
			PlayerPrefs.SetString(PlayerPreferKeyNames.GOT_PLAYER_INFO_DATA,"YES");

			if(savePlayerPicturedataToPersistentDataPath) {
			  File.WriteAllText(Application.persistentDataPath + "/" + "playerImageData" + ".seka", PlayerPrefs.GetString(PlayerPreferKeyNames.PLAYER_SHARE_PICTURE));
			}

			Debug.Log("Player Image : " + PlayerPrefs.GetString(PlayerPreferKeyNames.PLAYER_SHARE_PICTURE));
			Debug.Log("Player Name : " + PlayerPrefs.GetString(PlayerPreferKeyNames.PLAYER__SHARE_NAME));

	}

	void showPlayerPicture() {
		 Sprite playerImg = BBStaticVariable.getSpriteFromBytes(PlayerPrefs.GetString(PlayerPreferKeyNames.PLAYER_SHARE_PICTURE));
			Texture2D newTex = BBStaticVariable.ScaleTexture(playerImg.texture,64,64);
			Texture2D rTex = BBStaticVariable.CalculateTexture(64,64,32,32,32,newTex);
		 Sprite tmpSprite = Sprite.Create (rTex, new Rect(0,0,rTex.width,rTex.height), new Vector2(.5f,.5f));
		 imagePlayerImageOnHeader.sprite = tmpSprite;

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