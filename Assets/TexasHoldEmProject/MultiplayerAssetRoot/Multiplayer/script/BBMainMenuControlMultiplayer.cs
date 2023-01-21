using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

using BLabTexasHoldEmProject;
using BLabProjectMultiplayer.LoginController;

public class BBMainMenuControlMultiplayer : MonoBehaviour {

	public GameObject ButtonEU;
	public GameObject BaseMenuWindow;
	public GameObject multiplayerWindows;
	public GameObject multiplayerWindowsDirect;
	public GameObject multiplayerConnectController;
	public CheckForMultiplayerRooms checkForMultiplayerRoomsScript; 

	public GameObject TextLoadingOnFastConnect;

	public Image defaultAvaraImage;

	public GameObject[] completeGetAvatarImagesObjects;

	public GameObject TourButt;

	BBGlobalDefinitions _BBGlobalDefinitions;
	#if USE_PHOTON
	bool gotRegionChecked = false;
	string freeSeatRoomNane = "";
	bool gotFreeSeatOnRoom = false;
	#endif

#if UNITY_STANDALONE
	public RawImage cam;
	WebCamTexture webcamTexture; 

	private bool canUseWebCamera = false;

	private string camImageBytes;
#endif

	IEnumerator LoadMap(string sceneName){
		#if USE_PHOTON
		print (">>>>>> LoadMap : " + sceneName + " : " + (string)PhotonNetwork.room.CustomProperties["mapSceneToLoad"]);
		PhotonNetwork.isMessageQueueRunning = false;
		yield return new WaitForSeconds(1);
	    PhotonNetwork.LoadLevel((string)PhotonNetwork.room.CustomProperties["mapSceneToLoad"]);
		Debug.Log("Loading complete");  
		#endif
		yield break;
	}

	void OnJoinedRoom(){
		#if USE_PHOTON
		print ("Joined room: " + PhotonNetwork.room + " " + PhotonNetwork.masterClient.NickName + " " + PhotonNetwork.isMasterClient);
		StartCoroutine(LoadMap((string)PhotonNetwork.room.CustomProperties["mapSceneToLoad"]));
		#endif
	}

	IEnumerator executeCreateRoomAndJoin() {
		#if USE_PHOTON
		       BBStaticVariableMultiplayer.currentMPPlayerName = PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__SHARE_NAME);

		        PhotonNetwork.playerName = BBStaticVariableMultiplayer.currentMPPlayerName;

				print ("executeCreateRoomAndJoin *** 1 ***");
		        int maxPlayers = BBStaticVariableMultiplayer.currentMPmaxPlayerNumber;

				//string _keySBV = PhotonCustomProperties.custProp_room_startingBetValue;
			    //float _keyValSBV = MultiplayerCommonStaticData.startingBetValue;
				//string[] LobbyOptions = new string[1];
			    //LobbyOptions[0] = _keySBV;
				ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() {
				//{ _keySBV, _keyValSBV }, 
				{ "MapName", "GameMap" },
			    { "mapSceneToLoad", "MultiplayerGameSceneLimitedNew" },
				{ "GameMode", "Default" }
			    };

				RoomOptions roomOptions = new RoomOptions();
		        roomOptions.IsVisible = true;
		        roomOptions.IsOpen = true;
		         roomOptions.MaxPlayers = (byte)maxPlayers;
		        //roomOptions.CustomRoomPropertiesForLobby = LobbyOptions;
		        roomOptions.CustomRoomProperties = customProperties;

		        int r = UnityEngine.Random.Range(1,99999);

		        string newRoomName = "[" + "0" + "]" + setPrefix() + "Room" + r.ToString();

		        BBStaticVariableMultiplayer.currentMPRoomName = newRoomName;

				PhotonNetwork.CreateRoom(newRoomName, roomOptions , TypedLobby.Default);
		#endif
				yield break;

    }

	IEnumerator findASeatThenGoinToBestServer() {

		#if USE_PHOTON

			print ("findASeatThenGoinToBestServer ========== NEW ========>>>>  findASeatThenGoin");

			//Text findingText = ShowNOSeatFound.transform.Find("TitleText").GetComponent<Text>(); 
	        //string fixecdFindText = "Connecting To Game";

			if(!PhotonNetwork.connected) {
			    PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.BestRegion;
				PhotonNetwork.ConnectUsingSettings(BBStaticVariableMultiplayer.photonConnectionVersion);
			} else {
				PhotonNetwork.Disconnect();
				SceneManager.LoadScene(0);
			}

		

			yield return new WaitUntil(() => (gotRegionChecked == true));

		print ("findASeatThenGoinToBestServer ========== 2 ========>>>>  gotRegionChecked : " + gotRegionChecked + " gotFreeSeatOnRoom : " + gotFreeSeatOnRoom);

			if(gotFreeSeatOnRoom) {
			       PhotonNetwork.playerName = BBStaticVariableMultiplayer.currentMPPlayerName;
				  print ("findASeatThenGoin freeSeatRoomNane : " + freeSeatRoomNane + " : " + PhotonNetwork.connectedAndReady);
				  PhotonNetwork.JoinRoom(freeSeatRoomNane);
			} else {
			    
				 StartCoroutine(executeCreateRoomAndJoin());
			}
	 #endif

	 yield break;
  }

	public void executeFindMeASeat() {

		GameObject TextLoadingOnFindSeatAndGo = GameObject.Find("TextLoadingOnFindSeatAndGo");
		if(TextLoadingOnFindSeatAndGo) {
			TextLoadingOnFindSeatAndGo.GetComponent<Text>().text = "loading...";
		}

				BaseMenuWindow.SetActive(false);
				multiplayerWindowsDirect.SetActive(false);
				checkForMultiplayerRoomsScript.useDirectAccess = false;

		        StartCoroutine(findASeatThenGoinToBestServer());

				//ShowNOSeatFound.SetActive(true);
				//checkForMultiplayerRoomsScript.useDirectAccesToSeePlayersInRooms = false;
			    //if(BBStaticVariableMultiplayer.connectJustToBestPhotonServer) {
				//  StartCoroutine(findASeatThenGoinToBestServer());
			    //} else {
				//  StartCoroutine(findASeatThenGoin());
				//}
	}

	IEnumerator OnReceivedRoomListUpdate() { 
		#if USE_PHOTON
		Debug.Log("OnReceivedRoomListUpdate rooms : " + PhotonNetwork.GetRoomList().Length);

		  foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList()) {
			  if(roomInfo.PlayerCount < roomInfo.MaxPlayers) {
					freeSeatRoomNane = roomInfo.Name;
					gotFreeSeatOnRoom = true;
					break;
			  }
			}

		  gotRegionChecked = true;

		  Debug.Log("OnReceivedRoomListUpdate gotRegionChecked : " + gotRegionChecked);
		  #endif
	  yield break;
	}

	// Use this for initialization
	IEnumerator Start () {

		if (PlayerPrefs.GetInt("Score") == 15000)
			TourButt.SetActive(true);
		else
			TourButt.SetActive(false);

		_BBGlobalDefinitions = Resources.Load("BBGlobalDefinitions") as BBGlobalDefinitions;

		#if USE_PHOTON

		Debug.Log("Start _BBGlobalDefinitions.UseLoginSystem : " + _BBGlobalDefinitions.UseLoginSystem + 
			" _BBGlobalDefinitions.UseDirectFindASeatThenGoIn : " + BBStaticVariableMultiplayer.UseDirectFindASeatThenGoIn +
		          " connected : " + PhotonNetwork.connected);

		if(_BBGlobalDefinitions.UseLoginSystem) {

			if(BBStaticVariableMultiplayer.UseDirectFindASeatThenGoIn) {
				gotFreeSeatOnRoom = false;
				gotRegionChecked = false;
				executeFindMeASeat();
            }

		} else {
			foreach(GameObject g in completeGetAvatarImagesObjects) {
			  g.SetActive(true);
			}
		}

		#endif

#if UNITY_EDITOR
		gameObject.AddComponent<BLabTexasHoldEmProject.BBGetScreenShoot>();
#endif
#if UNITY_WEBGL
		GameObject.Find("BUTTON_PLAY_MULTIPLAYER_BEST_SERVER").GetComponent<Button>().interactable = false;
#endif

#if UNITY_STANDALONE

   if(_BBGlobalDefinitions.UseLoginSystem == false) {
		WebCamDevice[] camList = WebCamTexture.devices;
         	
		if(camList.Length > 0) {           	      
			yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
	        if (Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone)) {
	                canUseWebCamera = true;
					webcamTexture = new WebCamTexture();
	                cam.texture = webcamTexture;
	                webcamTexture.Play();
	        } else {
	           canUseWebCamera = false;
	        }
	    } else {
		   canUseWebCamera = false;
	    }

	    if(!canUseWebCamera) {
				GameObject img = GameObject.Find("RawImageWebCam");
				if(img) img.SetActive(false);
				GameObject butt = GameObject.Find("ButtonGetCamPicture");
				if(butt) butt.SetActive(false);
	    }
   }

#else
		        GameObject img = GameObject.Find("RawImageWebCam");
				if(img) img.SetActive(false);
				GameObject butt = GameObject.Find("ButtonGetCamPicture");
				if(butt) butt.SetActive(false);
#endif


    if(_BBGlobalDefinitions.UseLoginSystem == false) {

		if(PlayerPrefs.HasKey("myAvatarImageName")) {
			if(File.Exists(Application.persistentDataPath + "/" + "myPersonalAvatar" + ".png")) {
			  GameObject.Find("ButtonAvatarChoice_20_").GetComponent<RawImage>().texture = BBStaticVariable.LoadPNG(Application.persistentDataPath + "/" + "myPersonalAvatar" + ".png");
			}

			   Transform avatarImagesRoot = GameObject.Find("PanelAvatarListRoot").transform;
			   Image[] allImg = avatarImagesRoot.GetComponentsInChildren<Image>();
			   foreach(Image i in allImg) {
				   if(i.gameObject.name.Contains("ButtonAvatarChoice")) {
					//Debug.Log("avater i.sprite.name : " + i.sprite.name);
					  if(i.sprite.name.Contains(PlayerPrefs.GetString("myAvatarImageName"))) {
		                  i.color = Color.green;
		                  string[] splitted = i.sprite.name.Split('_');
						  BBStaticVariable.myAvatarImageNameIdx = splitted[1];
						  break;
					  }
				  }
			   }

				if(PlayerPrefs.GetString("myAvatarImageName").Length > 5) {
					GameObject.Find("ButtonAvatarChoice_20_").GetComponent<RawImage>().color = Color.green;
					BBStaticVariable.myAvatarImageNameIdx = "20";
					Texture2D tex_ = BBStaticVariable.LoadPNG(Application.persistentDataPath + "/" + "myPersonalAvatar" + ".png");
					PlayerPrefs.SetString("myAvatarImageName",BBStaticVariable.getStringByteFromTexture(tex_));
				}

		} else {
		   defaultAvaraImage.color = Color.green;
		}
	 } else {
		   PlayerPrefs.SetString("myAvatarImageName",PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER_SHARE_PICTURE));
			BBStaticVariable.myAvatarImageNameIdx = "20";
	 }
		yield break;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	public void gotButtonClick() {
//		Application.LoadLevel("demoMPConnect");
//	}
	
	public void buttonsClickController(GameObject _go) {
	
		switch(_go.name) {
			case "BUTTON_PLAY_MULTIPLAYER_BEST" :
			    StartCoroutine(GetCountryCodeViaIP());
				BaseMenuWindow.SetActive(false);
				multiplayerWindows.SetActive(true);
				checkForMultiplayerRoomsScript.useDirectAccess = false;
				multiplayerConnectController.SetActive(true);
			break;
			case "BUTTON_PLAY_MULTIPLAYER_DIRECT":
			    StartCoroutine(GetCountryCodeViaIP());
				BaseMenuWindow.SetActive(false);
				//multiplayerWindowsDirect.SetActive(true);
				TextLoadingOnFastConnect.SetActive(true);
				checkForMultiplayerRoomsScript.useDirectAccess = true;
				//checkForMultiplayerRoomsScript.gotDirectConnectButton(ButtonEU);
				PhotonNetwork.ConnectToRegion(CloudRegionCode.eu, BBStaticVariableMultiplayer.photonConnectionVersion);
				multiplayerConnectController.SetActive(true);
			break;
		    case "BUTTON_PLAY_MULTIPLAYER_BEST_SERVER":
			    StartCoroutine(GetCountryCodeViaIP());
			    TextLoadingOnFastConnect.SetActive(true);
			    BaseMenuWindow.SetActive(false);
			    checkForMultiplayerRoomsScript.directConnectToBestServer = true;
			    checkForMultiplayerRoomsScript.useDirectAccess = false;
				multiplayerConnectController.SetActive(true);

		    break;
		    case "BUTTON_MainMenu":
		        SceneManager.LoadScene(0);
		    break;
			case "BUTTON_Tournament":
				PlayerPrefs.SetInt("Score", 0);
				_go.SetActive(false);
			break;

		}
	
	}
	
	public void getWebCamPicture() {

#if UNITY_STANDALONE
			if(!canUseWebCamera) return; 

			Texture2D snap = new Texture2D(webcamTexture.width, webcamTexture.height);

			snap.SetPixels(webcamTexture.GetPixels());

		//	webcamTexture.Stop();

            snap.Apply();

            Texture2D newTex = ScaleTexture(snap,64,64);

            Texture2D rTex = CalculateTexture(64,64,32,32,32,newTex);

			byte[] byteArray= rTex.EncodeToPNG();
         
            camImageBytes = Convert.ToBase64String(byteArray);

			Debug.Log(camImageBytes);


			File.WriteAllBytes(Application.persistentDataPath + "/" + "myPersonalAvatar" + ".png", rTex.EncodeToPNG());

			GameObject.Find("ButtonAvatarChoice_20_").GetComponent<RawImage>().texture = rTex;
#endif

	}

	public void gotButtonAvatarChoiceClick(GameObject _go) {
	  resetAllAvatarImages();

	  if(_go.name.Contains("20")) {
		  RawImage tmpImg = _go.GetComponentInChildren<RawImage>();
		  tmpImg.color = Color.green;
	      BBStaticVariable.myAvatarImageNameIdx = "20";
		  Texture2D tex_ = BBStaticVariable.LoadPNG(Application.persistentDataPath + "/" + "myPersonalAvatar" + ".png");
		  PlayerPrefs.SetString("myAvatarImageName",BBStaticVariable.getStringByteFromTexture(tex_));
	  } else {
		  Image tmpImg = _go.GetComponentInChildren<Image>();
		  string spriteNane = tmpImg.sprite.name;
		  tmpImg.color = Color.green;
		  string[] splitted = spriteNane.Split('_');
	      BBStaticVariable.myAvatarImageNameIdx = splitted[1];
	      PlayerPrefs.SetString("myAvatarImageName",splitted[1]);
	  }

	}
	void resetAllAvatarImages() {
		Transform avatarImagesRoot = GameObject.Find("PanelAvatarListRoot").transform;
		Image[] allImg = avatarImagesRoot.GetComponentsInChildren<Image>();
		foreach(Image i in allImg) {
				if(i.gameObject.name == "ImageBG") {
				} else {
		          i.color = Color.white;
                }
		}

			RawImage ri = avatarImagesRoot.GetComponentInChildren<RawImage>();
			ri.color = Color.white;
	}

	public static IEnumerator GetCountryCodeViaIP() {
		string countryCode = "XX";
		string url = "http://www.geoplugin.net/json.gp";

		WWW www = new WWW(url);
		float startTime = Time.time;
		
		Debug.Log("GetCountryCodeViaIP : " + url);
		
		// Wait for download to complete
		while(!www.isDone) {
			if(www.error != null || Time.time - startTime > 8.0f) break; 
			yield return new WaitForSeconds(0.2f);
		}
		
		if (www.error != null) { 
			Debug.Log(www.error); 
			countryCode = "XX";
			PlayerPrefs.SetString("countryCode", countryCode);
		} else {
			//Debug.Log(www.error); 
			
		}
		
		//Debug.Log(www.text); 
		
		if(www.isDone && www.error == null && www.text != null) { 
			
			
			countryCode = www.text.Substring(www.text.IndexOf("countryCode") + 11 + 3, 2);
			//countryCode = www.text.Substring(www.text.IndexOf("countryCode") + 17, 2);
			
			Debug.Log ("CountryCode from IP: " + countryCode);
			
			PlayerPrefs.SetString("countryCode", countryCode);
		} else { 
			countryCode = "XX";
			PlayerPrefs.SetString("countryCode", countryCode);
		}		
	}


  private Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight) {
     Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,false);
     //float incX=(1.0f / (float)targetWidth);
     //float incY=(1.0f / (float)targetHeight);
     for (int i = 0; i < result.height; ++i) {
         for (int j = 0; j < result.width; ++j) {
             Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
             result.SetPixel(j, i, newColor);
         }
     }
     result.Apply();
     return result;
  }


 Texture2D CalculateTexture(int h, int w,float r,float cx,float cy,Texture2D sourceTex){
     //Color [] c= sourceTex.GetPixels(0, 0, sourceTex.width, sourceTex.height);
     Texture2D b=new Texture2D(h,w);
     for(int i = (int)(cx-r) ; i < cx + r ; i ++)
     {
         for(int j = (int)(cy-r) ; j < cy+r ; j ++)
         {
             float dx = i - cx;
             float dy = j - cy;
             float d = Mathf.Sqrt(dx*dx + dy*dy);
             if(d <= r)
                 b.SetPixel(i-(int)(cx-r),j-(int)(cy-r),sourceTex.GetPixel(i,j));
             else
                 b.SetPixel(i-(int)(cx-r),j-(int)(cy-r),Color.clear);
         }
     }
     b.Apply ();
     return b;
 }

	string setPrefix() {
		//		if(Application.platform == RuntimePlatform.Android) return "[AND]";
		//		else if(Application.platform == RuntimePlatform.IPhonePlayer) return "[IOS]";
		//		else if(Application.platform == RuntimePlatform.WindowsEditor) return "[DEV]";
		//		else if(Application.platform == RuntimePlatform.OSXPlayer) return "[MAC]";
		//		else if(Application.platform == RuntimePlatform.WSAPlayerARM) return "[WIN8]";
		//		else if(Application.platform == RuntimePlatform.WSAPlayerX64) return "[WIN8]";
		//		else if(Application.platform == RuntimePlatform.WSAPlayerX86) return "[WIN8]";
		//		else if(Application.platform == RuntimePlatform.WindowsPlayer) return "[WIN]";
		//		else if(Application.platform == RuntimePlatform.LinuxPlayer) return "[LNX]";
		//		else if(Application.platform == RuntimePlatform.OSXEditor) return "[MAC]";
		////		else if(Application.platform == RuntimePlatform.WP8Player) return "[WP8]";
		//		else return "[???]";
		return "";
	}

}
