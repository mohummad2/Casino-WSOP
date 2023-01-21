using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


#if USE_PHOTON
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
#endif

#if USE_PHOTON
public class MultiplayerRoomsController : Photon.MonoBehaviour {
#else
public class MultiplayerRoomsController : MonoBehaviour {
#endif

	[System.Serializable]
	public class AllMaps{
		public string mapName;
		public string mapInfo;
		public string mapSceneToLoad;
		public Texture2D mapPreview;
		public Vector2 mapPreviewTexDimention;
		public bool wantBackgroundImageOnButton = true;
	}
	public List<AllMaps> allMaps;
	private int currentSelectedMap;

	#if USE_PHOTON
	private string gameMode = ""; // future use
	#endif

  public GameObject MPRoomsAccessUI;
  public GameObject ChooseMapUI;

#if USE_PHOTON

	// Use this for initialization
	void Start () {
	
	 popolateMapsButtons();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void createRoom() {
	
	}
	
	void gotItemMAPRoomClick(GameObject _go) {
	
		Debug.Log("[MultiplayerRoomsController]gotItemMAPRoomClick : " + _go.name);
		
		string mapID = _go.transform.Find("mapID").GetComponent<Text>().text;
		int i_mapID = int.Parse(mapID);
		
		string newRoomName = "[" + mapID + "]" + BBStaticVariableMultiplayer.currentMPRoomName;
		int maxPlayers = BBStaticVariableMultiplayer.currentMPmaxPlayerNumber;
		
		PhotonNetwork.player.NickName = BBStaticVariableMultiplayer.currentMPPlayerName;
		Hashtable setMapName = new Hashtable(); 
		setMapName["MapName"] = allMaps[i_mapID].mapName;
		setMapName["mapSceneToLoad"] = allMaps[i_mapID].mapSceneToLoad;;
		setMapName["GameMode"] = gameMode;
		string[] exposedProps = new string[3];
		exposedProps[0] = "MapName";
		exposedProps[1] = "mapSceneToLoad";
		exposedProps[2] = "GameMode";
		//Create new Room
		
		GameObject ChooseMapUIText = GameObject.Find("ChooseMapUIText");
		ChooseMapUIText.GetComponent<Text>().text = "Loading...";
		ChooseMapUIText.transform.Find("Image").GetComponent<Image>().enabled = true;
		
		ChooseMapUI.SetActive(false);
		//old version--> PhotonNetwork.CreateRoom(newRoomName, true, true, maxPlayers, setMapName, exposedProps);
		PhotonNetwork.CreateRoom(newRoomName,  new RoomOptions() { MaxPlayers = (byte)maxPlayers,CustomRoomProperties = setMapName} , null);

	}
	
	
	void OnJoinedRoom(){
		print ("Joined room: " + PhotonNetwork.room + " " + PhotonNetwork.masterClient.NickName + " " + PhotonNetwork.isMasterClient);
		//We joined room, load respective map
		StartCoroutine(LoadMap((string)PhotonNetwork.room.CustomProperties["mapSceneToLoad"]));
	}
	
	IEnumerator LoadMap(string sceneName){
		
		PhotonNetwork.isMessageQueueRunning = false;

		yield return new WaitForSeconds(1);
		
		PhotonNetwork.LoadLevel((string)PhotonNetwork.room.CustomProperties["mapSceneToLoad"]);
		
		Debug.Log("Loading complete");  
	}

	public GameObject MultiplayerMapButtonsRoot;

	void popolateMapsButtons() {

		if(MultiplayerMapButtonsRoot == null) {
		   MultiplayerMapButtonsRoot = GameObject.Find("PanelBUTTONS_RoomsChooseMap");
		}

		GameObject mapButtonItem = Resources.Load("MultiplayerMapButtonItem") as GameObject;
		
		for(int i = 0; i < allMaps.Count; i++){
		
			GameObject inst = (GameObject)Instantiate(mapButtonItem) as GameObject;
			
			inst.name = allMaps[i].mapSceneToLoad;
			inst.transform.Find("UILabelMAPNameOnChooseMapButton").gameObject.GetComponent<Text>().text = allMaps[i].mapName;
			inst.transform.Find("UILabelMAPInfoOnChooseMapButton").gameObject.GetComponent<Text>().text = allMaps[i].mapInfo;
			inst.transform.Find("MapImage").gameObject.GetComponent<RawImage>().texture = allMaps[i].mapPreview;
			inst.transform.Find("MapImage").gameObject.GetComponent<RectTransform>().sizeDelta = allMaps[i].mapPreviewTexDimention;
			inst.GetComponent<Image>().enabled = allMaps[i].wantBackgroundImageOnButton;
			
			inst.transform.Find("mapID").gameObject.GetComponent<Text>().text = i.ToString();
			
			inst.transform.SetParent(MultiplayerMapButtonsRoot.transform, false);
			
		}
	
	}
	
	void OnDisconnectedFromPhoton() {
		print ("OnDisconnectedFromPhoton");
	}
	
	void OnConnectedToPhoton(){
		print ("OnConnectedToPhoton : ");
	}
	
	void OnJoinedLobby(){
		print ("OnJoinedLobby");
	}

#endif	
	
}
