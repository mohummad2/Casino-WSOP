using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using BLabProjectMultiplayer.LoginController;
using BLabTexasHoldEmProject;

#if USE_PHOTON
public class CheckForMultiplayerRooms : Photon.MonoBehaviour {

	public PhotonLogLevel _logLevel;
#else
public class CheckForMultiplayerRooms : MonoBehaviour {
#endif

	private enum BBRegionList {eu,@in,none};
   private BBRegionList Current_BBRegion; 
   public int[] pingList;
   public int[] roomsList;
   
   public bool canClick = false;
	
	public GameObject baseMainMenuContainer;
	public GameObject CompleteCreateRoomUGUIMultiplayer;
	public GameObject ChooseMapUI;
	public GameObject ContainerChooseMPServer;
	public GameObject ContainerChooseMPServerDirect;
	public GameObject MultiplayerRoomsControllerSCRIPT;

	public GameObject InputFieldPlayernameGO;

	public GameObject BBMaxPlayerNumRoot;

	InputField InputFieldPlayername;
	InputField InputFieldRoomName;
	Text UILabelInfoMessageOnMPConnecting;

	#if USE_PHOTON
	private bool creatingRoomState = false;
	private int maxPlayerNumber = 2;
    #endif

	public bool useDirectAccess = false;
	public bool directConnectToBestServer = false;

	BBGlobalDefinitions _BBGlobalDefinitions;

#if USE_PHOTON

	void gotItemRoomClick(GameObject _go) {

	  
	
		UILabelInfoMessageOnMPConnecting.text = "Loading game...";
		
		if(_BBGlobalDefinitions.UseLoginSystem) {
			BBStaticVariableMultiplayer.currentMPPlayerName = setPrefix() + PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__SHARE_NAME);//InputFieldPlayername.text;
		} else {
		   BBStaticVariableMultiplayer.currentMPPlayerName = setPrefix() + InputFieldPlayername.text;
		}

		PhotonNetwork.playerName = BBStaticVariableMultiplayer.currentMPPlayerName;
		string mapToJoin = _go.transform.Find("mapNameToJoin").GetComponent<Text>().text;
		
		Debug.Log("connected : " + PhotonNetwork.connectedAndReady + " insideLobby : " + PhotonNetwork.insideLobby + " map : " + mapToJoin);
		
		PhotonNetwork.JoinRoom(mapToJoin);
		
	}
	
	
	
	
	public void gotMaxPlayerRoomSelectionUGUI (Toggle _selected) {
		
		if(_selected.name.Contains("Player_2")) maxPlayerNumber = 2;
		else if(_selected.name.Contains("Player_3")) maxPlayerNumber = 3;
		else if(_selected.name.Contains("Player_4")) maxPlayerNumber = 4;
		//else if(_selected.name.Contains("Player_6")) maxPlayerNumber = 6;

		BBStaticVariableMultiplayer.currentMPmaxPlayerNumber = maxPlayerNumber;

		Debug.Log("--------------------->>>>>> gotMaxPlayerRoomSelectionUGUI : " + _selected.name + "  " + maxPlayerNumber);
	}
	
	public void gotBackMainMenuButton() {
	   PhotonNetwork.Disconnect();
		ContainerChooseMPServer.SetActive(false);
		CompleteCreateRoomUGUIMultiplayer.SetActive(false);
		baseMainMenuContainer.SetActive(true);
		MultiplayerRoomsControllerSCRIPT.SetActive(false);
		
	}

	int getRoomMaxPlayerNumber() {
	  int tmRet = 10;
/*
		Toggle[] maxPlayersToggle = BBMaxPlayerNumRoot.GetComponentsInChildren<Toggle>();
        foreach(Toggle t in maxPlayersToggle) {
          if(t.isOn) {
            switch(t.gameObject.name) {
				case "ToggleMaxPlayer_2": tmRet = 2; break;
				case "ToggleMaxPlayer_3": tmRet = 3; break;
				case "ToggleMaxPlayer_4": tmRet = 4; break;
            }
          }
        }
*/
	  return tmRet;
	}
	
	public void gotCreateRoom() {

		if(_BBGlobalDefinitions.UseLoginSystem) {

		} else {

		    if(!InputFieldPlayername) InputFieldPlayername = GameObject.Find("InputFieldPlayername").GetComponent<InputField>();
			if(!InputFieldRoomName) InputFieldRoomName = GameObject.Find("InputFieldRoomName").GetComponent<InputField>();

			if(InputFieldPlayername.text.Length < 5) {
			   int r = UnityEngine.Random.Range(1,99999);
			   InputFieldPlayername.text = "Player_" + r.ToString();
			}
	    }

		    if(InputFieldRoomName.text.Length < 5) {
			   int r = UnityEngine.Random.Range(1,99999);
			   InputFieldRoomName.text = "Room_" + r.ToString();
			}
	
	   creatingRoomState = true;

		Debug.Log("[CheckForMultiplayerRooms] gotCreateRoom");
		
		
		BBStaticVariableMultiplayer.currentMPmaxPlayerNumber = getRoomMaxPlayerNumber();

		BBStaticVariableMultiplayer.currentMPRoomName = setPrefix() + InputFieldRoomName.text;

		if(_BBGlobalDefinitions.UseLoginSystem) {
		  BBStaticVariableMultiplayer.currentMPPlayerName = setPrefix() + PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__SHARE_NAME);;
		} else {
		  BBStaticVariableMultiplayer.currentMPPlayerName = setPrefix() + InputFieldPlayername.text;
		}

		Debug.Log("***************BBStaticVariable.currentMPmaxPlayerNumber : " + BBStaticVariableMultiplayer.currentMPmaxPlayerNumber);
		Debug.Log("***************BBStaticVariable.currentMPRoomName : " + BBStaticVariableMultiplayer.currentMPRoomName);
		Debug.Log("***************BBStaticVariable.currentMPPlayerName : " + BBStaticVariableMultiplayer.currentMPPlayerName);
		
		CompleteCreateRoomUGUIMultiplayer.SetActive(false);
		ChooseMapUI.SetActive(true);
		MultiplayerRoomsControllerSCRIPT.SetActive(true);
		
		
	}


	void directConnect() {
			ContainerChooseMPServer.SetActive(false);
			ContainerChooseMPServerDirect.SetActive(false);
		    CompleteCreateRoomUGUIMultiplayer.SetActive(true);

		if(_BBGlobalDefinitions.UseLoginSystem) {

		} else {
		  if(!InputFieldPlayername) InputFieldPlayername = GameObject.Find("InputFieldPlayername").GetComponent<InputField>();
			int rPname = UnityEngine.Random.Range(1,99999);
			InputFieldPlayername.text = "Player_" + rPname.ToString();
		}

			if(!InputFieldRoomName) InputFieldRoomName = GameObject.Find("InputFieldRoomName").GetComponent<InputField>();
			if(!UILabelInfoMessageOnMPConnecting) UILabelInfoMessageOnMPConnecting = GameObject.Find("UILabelInfoMessageOnMPConnecting").GetComponent<Text>();
			

		    int r = UnityEngine.Random.Range(1,99999);
			InputFieldRoomName.text = "Room_" + r.ToString();
			
			UILabelInfoMessageOnMPConnecting.text = "Done...";
			
			waitingForRealRoomsList = true;

			StartCoroutine( setJoinRoomButtons() );
			
	}

	//public void gotDirectConnectButton(GameObject _go) {

	//    _go.GetComponent<Button>().interactable = false;
	

	//		switch(_go.name) {
				
	//		case "ButtonEU":
	//		    PhotonNetwork.ConnectToRegion(CloudRegionCode.eu,BBStaticVariableMultiplayer.photonConnectionVersion);
	//			break;
	//		case "ButtonGOTO_MENU":
	//			break;
	//		}
	//}

	
	public void gotConnectButton(GameObject _go) {
		
		//if( GameObject.Find("CheckForMultiplayerRooms").GetComponent<CheckForMultiplayerRooms>().canClick ) {
		if(canClick) {
			
			switch(_go.name) {
				
			
			case "ButtonEU":
				BBStaticVariableMultiplayer.selectedRegionCode = CloudRegionCode.eu;
				break;

			case "ButtonGOTO_MENU":
				break;
				
			}
			
		
		    ContainerChooseMPServer.SetActive(false);
		    CompleteCreateRoomUGUIMultiplayer.SetActive(true);

		    if(_BBGlobalDefinitions.UseLoginSystem) {

		    } else {
			if(!InputFieldPlayername) InputFieldPlayername = GameObject.Find("InputFieldPlayername").GetComponent<InputField>();
				int rPName = UnityEngine.Random.Range(1,999);
			    InputFieldPlayername.text = "Player_" + rPName.ToString();
			}

			if(!InputFieldRoomName) InputFieldRoomName = GameObject.Find("InputFieldRoomName").GetComponent<InputField>();
			if(!UILabelInfoMessageOnMPConnecting) UILabelInfoMessageOnMPConnecting = GameObject.Find("UILabelInfoMessageOnMPConnecting").GetComponent<Text>();
			
			int r = UnityEngine.Random.Range(1,999);

			InputFieldRoomName.text = "Room_" + r.ToString();
			
			UILabelInfoMessageOnMPConnecting.text = "Connecting to server...";
			
			waitingForRealRoomsList = true;
			
			if(!PhotonNetwork.connectedAndReady) {
				PhotonNetwork.OverrideBestCloudServer(BBStaticVariableMultiplayer.selectedRegionCode);
				PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
			}
	  }

	}

		
	void OnDisable () {

	  
 
		if(!waitingForRealRoomsList) {
		
		      if(PhotonNetwork.connectedAndReady) {
			     PhotonNetwork.Disconnect();
			   }
			   //this.enabled = false;
	    }

	  
	}
	
	// Use this for initialization
	void OnEnable() {

		_BBGlobalDefinitions = Resources.Load("BBGlobalDefinitions") as BBGlobalDefinitions;

		if(_BBGlobalDefinitions.UseLoginSystem) {
			InputFieldPlayernameGO.SetActive(false);
		} 

		Debug.Log("======= OnEnable =============== > start < ======================= : " + System.DateTime.Now +
		   " PhotonNetwork.connected : " + PhotonNetwork.connected + 
			" directConnectToBestServer : " + directConnectToBestServer +
			" useDirectAccess : " + useDirectAccess +
			" _BBGlobalDefinitions UseLoginSystem : " + _BBGlobalDefinitions.UseLoginSystem);

	   waitingForRealRoomsList = false;
	   canClick = false;

	   pingList = new int[10];
	   roomsList = new int[10];
	
	   PhotonNetwork.logLevel = _logLevel;
		
	   PhotonNetwork.automaticallySyncScene = true;

		if(useDirectAccess) {
		  return;
	    } 

	    if(directConnectToBestServer) {
			if(!PhotonNetwork.connected) {
				PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.BestRegion;
				//PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
				PhotonNetwork.ConnectUsingSettings(BBStaticVariableMultiplayer.photonConnectionVersion);
			} else {
				PhotonNetwork.Disconnect();
				SceneManager.LoadScene(0);
			}
	     return;
	    }

		    setAllButtoDisabled();
			PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.asia);
		    PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
	        Current_BBRegion = BBRegionList.eu;
		
		

	
	}
	
	// Update is called once per frame
	void gotSearchEnds () {
	
		Debug.Log("====================== > gotSearchEnds < ======================= : " + System.DateTime.Now);
		
		canClick = true;
		GameObject labLoading = GameObject.Find("LabelLOADING");
		if(labLoading) {
			labLoading.GetComponent<Text>().text = "Done!";
			labLoading.GetComponent<Text>().color = Color.green;
			labLoading.transform.Find("BBLoading").gameObject.SetActive(false);
		}
	}
	
	IEnumerator getRoomInRegionAfterDisconnected() {
	
		Debug.Log("====================== > getRoomInRegionAfterDisconnected : " + Current_BBRegion);
		
		yield return new WaitForEndOfFrame();
		
		switch(Current_BBRegion) {
		
		 case BBRegionList.eu:
			PhotonNetwork.OverrideBestCloudServer(CloudRegionCode.eu);
			PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
			break;

		case BBRegionList.none:
			gotSearchEnds();
			break;
			
		}
		
		
	}
	
	IEnumerator getRoomInRegionAfterConnected() {
		
		Debug.Log("====================== > getRoomInRegionAfterConnected : " + Current_BBRegion);
		
		yield return new WaitForEndOfFrame();
		
		
	}
	
				
	IEnumerator getRoomInRegionAfterGotRoomsNumber(int roomNumber) {
	    
		int i_ping = PhotonNetwork.GetPing();
	    
		Debug.Log("====================== > getRoomInRegion : " + Current_BBRegion + " number : " + roomNumber + " ping : " + i_ping);
	
	   yield return new WaitForEndOfFrame();
	   
	   switch(Current_BBRegion) {
	       
		  
			case BBRegionList.eu:
				pingList[2] = i_ping;
			    roomsList[2] = roomNumber;
			    setResultData("LabelEU_PING","LabelEU_ROOMS","ButtonEU",pingList[2],roomsList[2]);
			    Current_BBRegion = BBRegionList.none;
				PhotonNetwork.Disconnect();
				break;
			
	   }
	   
	   
	   	
	}
	
	void OnFailedToConnectToPhoton(DisconnectCause cause) { 
		
		print("Failed To Connect To Server : " + cause.ToString());
		
		GameObject.Find("LabelLOADING").GetComponent<Text>().text = "NO CONNECTION TO SERVER TRY AGAING LATER....";
		GameObject.Find("ButtonGOTO_MENU").GetComponent<Button>().enabled = true;
		canClick = true;

	}
	
	void OnDisconnectedFromPhoton() {
		print ("OnDisconnectedFromPhoton useDirectAccess : " + useDirectAccess + " directConnectToBestServer : " + directConnectToBestServer);

		if(useDirectAccess) {
			useDirectAccess = false;
			gameObject.SetActive(false);
		  return;
		} 

		if(directConnectToBestServer) {
		    directConnectToBestServer = false;
			gameObject.SetActive(false);
		  return;
		} 

			if(waitingForRealRoomsList) {
			   if(PhotonNetwork.connected) {
				  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			   } else {

			   }
			} else {
			   StartCoroutine(getRoomInRegionAfterDisconnected());
			}


		 
	}
	
	void OnConnectedToPhoton(){
		print ("OnConnectedToPhoton : ");
		if(UILabelInfoMessageOnMPConnecting) UILabelInfoMessageOnMPConnecting.text = "Connected";
	}

	void OnJoinedLobby(){
		print ("OnJoinedLobby creatingRoomState : " + creatingRoomState);
		
		if(creatingRoomState) {
			
		} else {
		     PhotonNetwork.GetRoomList();
		}
		
		
	}

	IEnumerator OnPhotonCreateRoomFailed() { 
		print ("Failed OnPhotonCreateRoomFailed");
		UILabelInfoMessageOnMPConnecting.text = "On Create Room Failed!!!";
		
		yield return new WaitForSeconds(5);
		
		waitingForRealRoomsList = true;
		PhotonNetwork.Disconnect();
		
	}
	
	IEnumerator OnPhotonJoinRoomFailed(){
		print ("Failed on connecting to room");
		UILabelInfoMessageOnMPConnecting.text = "On Join Room Failed";
		
		yield return new WaitForSeconds(5);
		
		waitingForRealRoomsList = true;
		PhotonNetwork.Disconnect();
	}
	
			
	bool waitingForRealRoomsList = false;
	
	IEnumerator OnReceivedRoomListUpdate() { 

		 GameObject TextLoadingOnFastConnect = GameObject.Find("TextLoadingOnFastConnect");
		 if(TextLoadingOnFastConnect != null) {
			TextLoadingOnFastConnect.SetActive(false);
		 } 
	
		print ("OnReceivedRoomListUpdate beginning waitingForRealRoomsList : " + waitingForRealRoomsList); 

		if(useDirectAccess) {
		   directConnect();
		   yield break;
	    }

	    if(directConnectToBestServer) {
			BBStaticVariableMultiplayer.selectedRegionCode = CloudRegionCode.eu;
			ContainerChooseMPServer.SetActive(false);
			CompleteCreateRoomUGUIMultiplayer.SetActive(true);
			
			//if(!InputFieldPlayername) InputFieldPlayername = GameObject.Find("InputFieldPlayername").GetComponent<InputField>();
			if(!InputFieldRoomName) InputFieldRoomName = GameObject.Find("InputFieldRoomName").GetComponent<InputField>();
			if(!UILabelInfoMessageOnMPConnecting) UILabelInfoMessageOnMPConnecting = GameObject.Find("UILabelInfoMessageOnMPConnecting").GetComponent<Text>();
			
			int r = UnityEngine.Random.Range(1,999);
			//InputFieldPlayername.text = "Player_" + r.ToString();
			InputFieldRoomName.text = "Room_" + r.ToString();
			
			UILabelInfoMessageOnMPConnecting.text = "Connected...";
			
			waitingForRealRoomsList = true;

			StartCoroutine( setJoinRoomButtons() );

			if(!PhotonNetwork.connectedAndReady) {
				PhotonNetwork.OverrideBestCloudServer(BBStaticVariableMultiplayer.selectedRegionCode);
				PhotonNetwork.ConnectToBestCloudServer(BBStaticVariableMultiplayer.photonConnectionVersion);
			}

	     yield break;
	    }

				if(waitingForRealRoomsList) {
				    StartCoroutine( setJoinRoomButtons() );
			        if(UILabelInfoMessageOnMPConnecting) UILabelInfoMessageOnMPConnecting.text = "Connected...";

				} else {
						yield return new WaitForEndOfFrame();
						
						int roomNumber = PhotonNetwork.GetRoomList().Length;
						
						print ("OnReceivedRoomListUpdate : " + roomNumber);
						
						
						//foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList()) {
						//	Debug.Log("RName -> " + roomInfo.name);
						//}
						
						StartCoroutine(getRoomInRegionAfterGotRoomsNumber(roomNumber));
						
				}

	}
	
	Color getPingColor(int val) {
	    Debug.Log("color : " + val + "  _  " + Current_BBRegion);
		
		if(val >= 250) return Color.red;
		else if( (val <= 249) && (val >= 150) ) return Color.yellow;
		else if( val <= 149 ) return Color.green;
		else return Color.gray;
		
	}
	
	void setAllButtoDisabled() {
	
					   //StartCoroutine( setButtonDisabled("ButtonASIA") );
					   //StartCoroutine( setButtonDisabled("ButtonAU") );
		               StartCoroutine( setButtonDisabled("ButtonEU") );
		               //StartCoroutine( setButtonDisabled("ButtonJAPAN") );
		               //StartCoroutine( setButtonDisabled("ButtonUSA") );
						//StartCoroutine( setButtonDisabled("ButtonUSAWC") );
						//StartCoroutine( setButtonDisabled("ButtonCANADA") );
						//StartCoroutine( setButtonDisabled("ButtonKOREA") );
						//StartCoroutine( setButtonDisabled("ButtonINDIA") );
						//StartCoroutine( setButtonDisabled("ButtonSAMERICA") );

		
		
		
	}
	
	IEnumerator setButtonDisabled(string lab) {
	    yield return new WaitForEndOfFrame();
		GameObject g = GameObject.Find(lab);
		
		g.GetComponent<Button>().enabled = false;
		g.transform.Find("Label").GetComponent<Text>().color = Color.grey;
	}
	
	
	void setResultData(string pingLab, string roomsLab, string buttLab, int pingValue, int roomsValue) {
	
		GameObject.Find(pingLab).GetComponent<Text>().text = "Пинг сервера : " + pingValue;
		GameObject.Find(pingLab).GetComponent<Text>().color = getPingColor(pingValue);
		if(getPingColor(pingValue) == Color.red) {
			GameObject g = GameObject.Find(buttLab);
			g.GetComponent<Button>().enabled = false;
			g.transform.Find("Label").GetComponent<Text>().color = Color.grey;
		} else {
			GameObject g = GameObject.Find(buttLab);
			g.GetComponent<Button>().enabled = true;
			g.transform.Find("Label").GetComponent<Text>().color = Color.green;
		}
		GameObject.Find(roomsLab).GetComponent<Text>().text = "Игр : " + roomsValue;
	
	}
	
		public bool isTestingForRoomsbutt = false;
	
	IEnumerator setJoinRoomButtons() {
	yield return new WaitForEndOfFrame();

		Debug.Log("***********setJoinRoomButtons***************");
		
		GameObject[] _items = GameObject.FindGameObjectsWithTag("roomButton");		
		foreach (GameObject itGO in _items) { Destroy(itGO);}
		yield return new WaitForEndOfFrame();
		

		
		GameObject YESRoomsItem = Resources.Load("MultiplayerItemRoomUUI") as GameObject;
		GameObject NORoomsItem = Resources.Load("MultiplayerItemRoomNoRoomsUUI") as GameObject;
		
		GameObject PanelScrollRoot = GameObject.Find("PanelBUTTONS_Rooms");
		
		if(PanelScrollRoot) Debug.Log("***********setJoinRoomButtons*************** " + PanelScrollRoot.name);
		
		
		if(isTestingForRoomsbutt) {
			
			for(int i = 0; i < 20; i++) {
				GameObject inst = (GameObject)Instantiate(YESRoomsItem);
				
				inst.transform.SetParent(PanelScrollRoot.transform, false);
				
				inst.transform.Find("UILabelPlayersNum").GetComponent<Text>().text = "6 / 6";
				inst.transform.Find("UILabelRoomName").GetComponent<Text>().text = "text Room ciao";
				inst.transform.Find("UILabelButtJoin").GetComponent<Text>().text = i.ToString();
				
			}
			
		} else {
			
			int _rNum = PhotonNetwork.GetRoomList().Length;
			Debug.Log("------------->> numOfRooms : " + _rNum);
			//UILabelInfoMessageOnMPConnecting.text = "Opened Games : " + _rNum;
			if(_rNum > 0) {			
				foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList()) {
					
					//Debug.Log("RName -> " + roomInfo.name);
					
					GameObject goTMP = GameObject.Find(roomInfo.Name); 
					
					if(goTMP == null) {
						GameObject inst = (GameObject)Instantiate(YESRoomsItem);
						inst.transform.SetParent(PanelScrollRoot.transform, false);
						//inst.name = roomInfo.name;
						
						inst.transform.Find("UILabelPlayersNum").GetComponent<Text>().text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
						string thirdElement = roomInfo.Name.Substring(2,1);
						string mapID = "0";
						  if(thirdElement == "]") { //mapID 0..9
						    mapID = roomInfo.Name.Substring(1,1);
							inst.name = roomInfo.Name.Substring(3);
							inst.transform.Find("UILabelRoomName").GetComponent<Text>().text = roomInfo.Name.Substring(3);
						  } else { //mapID 10....->
							mapID = roomInfo.Name.Substring(1,2);
							inst.name = roomInfo.Name.Substring(4);
							inst.transform.Find("UILabelRoomName").GetComponent<Text>().text = roomInfo.Name.Substring(4);
						  }
						  int i_mapID = int.Parse(mapID);
						  Debug.Log("thirdElement : " + thirdElement + " mapID : " + mapID);
						  inst.transform.Find("RawImage").GetComponent<RawImage>().texture =  MultiplayerRoomsControllerSCRIPT.GetComponent<MultiplayerRoomsController>().allMaps[i_mapID].mapPreview;
						  inst.transform.Find("mapNameToJoin").GetComponent<Text>().text = roomInfo.Name;
					}			
					
				}
			} else {
				
				GameObject goTMP = GameObject.Find("noRoomsItem"); 
				
				if(goTMP == null) {
					GameObject inst = (GameObject)Instantiate(NORoomsItem);
					inst.transform.SetParent(PanelScrollRoot.transform, false);
					inst.name = "noRoomsItem";
				}
			}
		}
		
		
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
	
#endif
	
}
