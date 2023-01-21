#if USE_PHOTON
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

namespace BLabTexasHoldEmProject {

public class NewGameControllerMultiplayer : Photon.MonoBehaviour {

	BBPlayerData activeplayer = new BBPlayerData();
	public static class CustomPropertiesKeyList {
	        public const string IsGameStarted = "IsGameStarted";
			public const string lastBet = "lastBet";
			public const string cardsDeck = "cardsDeck";
			public const string moneyOnTable = "moneyOnTable";
			public const string currentMoneyOnTable = "currentMoneyOnTable";
			public const string currentPlayerTotalMoney = "currentPlayerTotalMoney";
			public const string waitingForPlayerResponse = "waitingForPlayerResponse";
			public const string underGunPlayer = "underGunPlayer";
			public const string moneyToCoverOnAlignment = "moneyToCoverOnAlignment";
			public const string lastPhaseBeforeAlignment = "lastPhaseBeforeAlignment";
			public const string isPlayerUnderAllIn = "isPlayerUnderAllIn"; // Y or N
			public const string AllInExecutedAtPhase = "AllInExecutedAtPhase";
			public const string playerWhoRequestAllInID = "playerWhoRequestAllInID";
	}

    PhotonView _photonView;
	public GameObject playersPrefab;
	public BBGlobalDefinitions _BBGlobalDefinitions; 
	public NewMultiplayerHelper _NewMultiplayerHelper;
	public Transform[] playersContainerOnCanvas;

	  public GameObject PanelGoingToDisconnect;
	  public GameObject PanelPlayerLeftAloneWonThePot;
	  public GameObject PanelMinPlayerToStartGameHand;
	  public GameObject PanelPlayerDisconnectCoseOutOfMoney;
	  public GameObject PanelPlayerWonCoseNoOneAcceptCall;


	public List<BBPlayerData> playerDataList;   

	public GameObject UIMoveingController;
	public Text TextNewGameInSeconds;

	public List<Vector2> GlobalDeck = new List<Vector2>();
	private List<Vector2> mainDeck = new List<Vector2>();
	public Transform[] shuffledCardsList; 
	public GameObject[] playersCardPositions;
	public Transform playersCardPositionsRoot;
	Quaternion rotatedCardRotation, rotatedForShowCardOnTable;
	[HideInInspector]
	public Transform cardsDiscardPosition;
	[HideInInspector]
	public int lastGivenCardIdx = 0;
	private Vector2 currentGivenCard;

	[HideInInspector]
	public Vector2[] flopCardList = new Vector2[3];
	[HideInInspector]
	public Vector2 turnCard;
	[HideInInspector]
	public Vector2 riverCard;


	[HideInInspector]
	public NewBBGuiInterfaceMultiplayer _BBGuiInterface;
	[HideInInspector]
	public NewRPCController _RPCController;

	public GameObject ButtonStartTheGame;

	[HideInInspector]
	public bool waitingResponceFromPlayer = false;
	[HideInInspector]
	public bool gotResponceFromPlayer = false;

	private Text TextWailResponseCountDown;
	private int responseTimeOutSeconds = 12;




   void Awake() {

		Debug.Log("[NewGameControllerMultiplayer][Awake] 1 ");

	   _NewMultiplayerHelper = gameObject.AddComponent<NewMultiplayerHelper>();
			Debug.Log("[NewGameControllerMultiplayer][Awake] 2 ");
	   _BBGlobalDefinitions.gameType = BBStaticVariable.gameType;
			Debug.Log("[NewGameControllerMultiplayer][Awake] 3 ");
	   _photonView = GetComponent<PhotonView>();
			Debug.Log("[NewGameControllerMultiplayer][Awake] 4 ");
	   _BBGuiInterface = GetComponent<NewBBGuiInterfaceMultiplayer>();
			Debug.Log("[NewGameControllerMultiplayer][Awake] 5 ");
	   _RPCController = GetComponent<NewRPCController>();
			Debug.Log("[NewGameControllerMultiplayer][Awake] 6 ");
#if UNITY_EDITOR
		gameObject.AddComponent<BBGetScreenShoot>();
#endif
			Debug.Log("[NewGameControllerMultiplayer][Awake] 7 ");
   }

	IEnumerator Start () {

		Debug.Log("[NewGameControllerMultiplayer][Start] 1 ");

		//TextWailResponseCountDown = GameObject.Find("TextWailResponseCountDown").GetComponent<Text>();

			Debug.Log("[NewGameControllerMultiplayer][Start] 2 ");

		rotatedCardRotation = GameObject.Find("RotatedCardOnTable").transform.rotation;
		//rotatedForShowCardOnTable = GameObject.Find("RotatedForShowCardOnTable").transform.rotation;

			Debug.Log("[NewGameControllerMultiplayer][Start] 3 ");

		cardsDiscardPosition = GameObject.Find("RotatedCardOnTable").transform;

			Debug.Log("[NewGameControllerMultiplayer][Start] 4 ");
	      bool isGameStarted = false; 

			if(PhotonNetwork.room.CustomProperties[NewGameControllerMultiplayer.CustomPropertiesKeyList.IsGameStarted] != null) {
				string gameStarted = (string)PhotonNetwork.room.CustomProperties[NewGameControllerMultiplayer.CustomPropertiesKeyList.IsGameStarted];
				if(gameStarted.Contains("YES")) {
				    isGameStarted = true;
				}
		    }

			Debug.Log("[NewGameControllerMultiplayer][Start] Network : " + PhotonNetwork.connectedAndReady 
		                                                    + " : " + PhotonNetwork.room.Name
		                                                    + " : " + PhotonNetwork.player.NickName
		                                                    + " : " + PhotonNetwork.room.MaxPlayers
		                                                    + " : " + PhotonNetwork.room.PlayerCount
		                                                    + " gameType : " + _BBGlobalDefinitions.gameType
				                                            + " isGameStarted : " + isGameStarted
		    );



		    GameObject _Player = PhotonNetwork.Instantiate(playersPrefab.name,Vector3.zero,Quaternion.identity,0);

		    _Player.name = PhotonNetwork.player.NickName;

			 if(isGameStarted) {
			   PhotonNetwork.player.NickName = _Player.name + "[O]";
			 }

		if(PhotonNetwork.player.IsMasterClient) {
			Hashtable game_Type = new Hashtable();
			if(_BBGlobalDefinitions.gameType == BBGlobalDefinitions.GameType.NoLimit) {
					game_Type["GameType"] = "A";
			} else {
					game_Type["GameType"] = "L";
			}
			PhotonNetwork.room.SetCustomProperties(game_Type);

			yield return new WaitForEndOfFrame();

			Invoke("activateStartGameButton",5);

	    } else {
			string GT = (string)PhotonNetwork.room.CustomProperties["GameType"];
			if(GT == "A") {
			   _BBGlobalDefinitions.gameType = BBGlobalDefinitions.GameType.NoLimit;
			} else {
			   _BBGlobalDefinitions.gameType = BBGlobalDefinitions.GameType.Limited;
			  GameObject allInImage = GameObject.Find("ImageGameAllIn");
			  if(allInImage) {
			    Destroy(allInImage);
			  }
			}
	    }


	}

	void gotGenericButtonClick(GameObject _go) {

	   switch(_go.name) {
		  case "ButtonStartTheGame":
		    if(canStartTheHand()) {
		       ButtonStartTheGame.SetActive(false);
			   _photonView.RPC("RPCStartGameHand",PhotonTargets.All);
			}
		  break;
		  case "ButtonMainMenu":
		    PhotonNetwork.Disconnect();
		  break;
		  case "ButtonHidePanel":
		    GetComponent<NewResultEngine>().finalResultPanel.SetActive(false);
		  break;
		  case "ButtonShowPanel":
			GetComponent<NewResultEngine>().finalResultPanel.SetActive(true);
		  break;
		   case "ButtonTestNewGameHand":
			  StartCoroutine(StartExecuteNewGameHand());
		   break;
			case "ButtonToyWonTehTableCoseAlone":
			PanelPlayerLeftAloneWonThePot.SetActive(false);
		   break;
			case "ButtonCloseOnYouWonPanel":
				GetComponent<NewResultEngine>().PanelLocalPlayerWon.SetActive(false);
				GetComponent<NewResultEngine>().finalResultPanel.SetActive(false);
				PanelPlayerWonCoseNoOneAcceptCall.SetActive(false);
		   break;
			case "ButtonZoom":
			BBStaticData.executeCameraZoom();
		   break;
		   case "ButtonMinPlayersToStatGame":
		   PanelMinPlayerToStartGameHand.SetActive(false);
		   ButtonStartTheGame.SetActive(true);
		   break;
	   }

	}

	bool canStartTheHand() {
	  bool tmpRet = false;
			  if(PhotonNetwork.room.PlayerCount >= 3) {
			   tmpRet = true;
			  } else {
				 PanelMinPlayerToStartGameHand.SetActive(true);
			  }
      return tmpRet;
	}


	IEnumerator StartExecuteNewGameHand() {

	   yield return StartCoroutine( GetComponent<BBMoveingObjectsController>().cleanTable() );
	   GetComponent<NewResultEngine>().finalResultPanel.SetActive(false);
	   setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.IsGameStarted,"NO");
	   _BBGuiInterface.ImageGameGeneralPhase.color = Color.red;

	   List<BBPlayerData> pdWithOutMoneyList = new List<BBPlayerData>();
	   List<BBPlayerData> pdToResetList = new List<BBPlayerData>();
	   for(int x = 0; x < playerDataList.Count;x++) {if(playerDataList[x].playerGameObject != null) pdToResetList.Add(playerDataList[x]);}
	   foreach(BBPlayerData pd in pdToResetList) {
	     pd.isObserver = false;
	     pd.isOutOfGame = false;
		 pd.playerGameObject.transform.Find("TextObserver").GetComponent<Text>().text = "";
		 pd.currentMoneyOnTable = 0;
		 setPlayerCustomProperties(pd.playerGameObject.GetComponent<PhotonView>().owner,CustomPropertiesKeyList.currentMoneyOnTable,0.0f);
		 pd.AllInImage.enabled = false;
		 PhotonPlayer pp = _NewMultiplayerHelper.getPhotonPlayer(playerDataList,pd.playerPosition);
		 float ppTotalMoney = getFloatPlayerProperties(pp,CustomPropertiesKeyList.currentPlayerTotalMoney);
		 if(ppTotalMoney < 50) {
		   pdWithOutMoneyList.Add(pd);
		 }
	   }



		if(PhotonNetwork.player.IsMasterClient) {
		   foreach(BBPlayerData pd in pdWithOutMoneyList) {
				executePlayerIsOutCoseNoMoreMoney(pd.playerPosition);
				yield return new WaitForSeconds(3);
		   }

			yield return new WaitForEndOfFrame();
			_RPCController.ShareMoneyData();
			yield return new WaitForEndOfFrame();
			_RPCController.shareInfomessagePlayerRelated(""," Waiting Dealer Start Game Hand...",0,true);
			yield return new WaitForEndOfFrame();
			_RPCController.shareGamePhaseText("Waiting Dealer...");
			yield return new WaitForEndOfFrame();
	        ButtonStartTheGame.SetActive(true);

	    }

	}

	void adjustPlayersPositions() {
		List<BBPlayerData> allActivePlayers = new List<BBPlayerData>();

			for(int x = 0;x < playerDataList.Count;x++) {
		        if(playerDataList[x].playerGameObject != null) {
					allActivePlayers.Add(playerDataList[x]);
		         }
             }

			playerDataList.Clear();
			for(int x = 0;x < 10;x++) {
			  BBPlayerData pd = new BBPlayerData();
			  playerDataList.Add(pd);
			}

			for(int x = 0;x < allActivePlayers.Count;x++) {
	          playerDataList[x] = allActivePlayers[x];
			  playerDataList[x].playerGameObject.transform.SetParent(playersContainerOnCanvas[x]);
			  playerDataList[x].playerGameObject.transform.localPosition = new Vector3(0,0,0);
			  playerDataList[x].playerPosition = x;
			  PhotonPlayer pp = playerDataList[x].playerGameObject.GetComponent<PhotonView>().owner;
				 Hashtable plaPos = new Hashtable(); 
				 plaPos["playerPos"] = x;
				 pp.SetCustomProperties(plaPos);
	        }



    }


	[PunRPC]
	void RPCStartGameHand() {
	  StartCoroutine(startGameHand());
	}

    IEnumerator startGameHand() {

		 adjustPlayersPositions();

		 yield return new WaitForSeconds(1);

		 turnCard = Vector2.zero;
         riverCard = Vector2.zero;
		 flopCardList[0] = Vector2.zero;flopCardList[1] = Vector2.zero;flopCardList[2] = Vector2.zero;
		 //_BBGuiInterface.ImageGameGeneralPhase.color = Color.green;

		 setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.IsGameStarted,"YES");
		 yield return new WaitForEndOfFrame();
		 setRoomCustomProperties(CustomPropertiesKeyList.moneyOnTable,0.0F);
		 yield return new WaitForEndOfFrame();
		 setRoomCustomProperties(CustomPropertiesKeyList.lastBet,0.0f);
		 yield return new WaitForEndOfFrame();


		 //List<BBPlayerData> playersList = _NewMultiplayerHelper.getActiveInGamePlayersList(playerDataList);
		   List<BBPlayerData> playersList = _NewMultiplayerHelper.getAllInGamePlayersList(playerDataList);

		 foreach(BBPlayerData pd in playersList) {
		   pd.isOutOfGame = false;
				if(pd.playerName.Contains("[O]")) {
						int idx = pd.playerName.IndexOf("[O]");
						string newT = pd.playerName.Remove(idx,3);
						pd.playerName = newT;
						Debug.Log("-------New Player Name --> : " + pd.playerName);
						pd.T_playerName.text = newT;
			     }
           pd.underAllin = false;
           pd.currentMoneyOnTable = 0;
           pd.playerActiveImage.color = Color.red;
		   PhotonPlayer pp = pd.playerGameObject.GetComponent<PhotonView>().owner;
           setPlayerCustomProperties(pp,CustomPropertiesKeyList.currentMoneyOnTable,0.0f);
           setPlayerStringCustomProperties(pp,CustomPropertiesKeyList.isPlayerUnderAllIn,"N");
		 }

		 yield return new WaitForSeconds(2);
		  _RPCController._photonView.RPC("RPCShareMoneyData",PhotonTargets.All);


		if(PhotonNetwork.player.IsMasterClient) {

           InitializeDeck();

           yield return new WaitForEndOfFrame();
	       string tmpDeck = "";
	          for(int x = 0; x < GlobalDeck.Count;x++) {
			    tmpDeck = tmpDeck + ( GlobalDeck[x].x.ToString() + "," + GlobalDeck[x].y.ToString() + "#" ); 
	          }
	      // Debug.Log("tmpDeck : " + tmpDeck);
		   setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.cardsDeck,tmpDeck);

		   yield return new WaitForSeconds(5);
		}

           _RPCController.shareCardsDeck();

           yield return new WaitForEndOfFrame();

           _RPCController.setActiveDealer();

			yield return new WaitForEndOfFrame();

			_RPCController.setSmallAndBigBlidPlayer(_BBGlobalDefinitions.currentActivedealer);

			yield return new WaitForEndOfFrame();

			if(PhotonNetwork.player.IsMasterClient) {

			     _RPCController.executeSmallAndBigBet();

				 yield return new WaitForSeconds(5);

			     _RPCController.setGameState(BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound);

				 yield return new WaitForSeconds(5);

				 _RPCController.giveFirstCardsToPlayers();

				switch(PhotonNetwork.playerList.Length) {case 2: yield return new WaitForSeconds(3);break;case 3: yield return new WaitForSeconds(5);break;case 4: yield return new WaitForSeconds(7);break;case 5: yield return new WaitForSeconds(9);break;case 6: yield return new WaitForSeconds(11);break;case 7: yield return new WaitForSeconds(12);break;case 8: yield return new WaitForSeconds(14);break;case 9: yield return new WaitForSeconds(16);break;case 10: yield return new WaitForSeconds(16);break;}

				yield return new WaitForSeconds(5);

				StartCoroutine( starExecuteFirstBettingRound() );

			}




			if(PhotonNetwork.player.IsMasterClient) {
//				StartCoroutine( starExecuteFirstBettingRound() );
			}

	}

#region utility
	public int getFirstPlayerPosition() {
	  int tmpTet = 0;
	  if(PhotonNetwork.playerList.Length == 1) {
	    return tmpTet;
	  }
	  List<int> posL = new List<int>();
	  int[] _matrix = new int[10]; 
	  foreach(PhotonPlayer pp in PhotonNetwork.playerList) {
	     if(pp.NickName != PhotonNetwork.player.NickName) {
		    posL.Add((int)pp.CustomProperties["playerPos"]);
			//Debug.Log("ADD >>>>>>>>>>>>>>>>getFirstPlayerPosition>>>>>>>>> NOT LOCAL : " + (int)pp.CustomProperties["playerPos"] + " : " + pp.name);
		 } else {
			//Debug.Log("**NO ADD** >>>>>>>>>>>>>>>>getFirstPlayerPosition>>>>>>>>>  LOCAL : " + pp.name);
		 }
	  }
	  posL.Sort();
	  for(int x = 0; x < posL.Count;x++) {
	   _matrix[posL[x]] = 1;
	  }
	  for(int x = 0;x < _matrix.Length;x++) {
	     //Debug.Log("MATRIX : " + _matrix[x]);
	     if(_matrix[x] == 0) {
           tmpTet = x; 	       
	      break;
	     }
	  }
		return tmpTet;
	}

	public void setPlayersData(BBPlayerData pDataList, GameObject playerGO) {
			
			Text TextPlayerName = playerGO.transform.Find("TextPlayerName").GetComponent<Text>();
			pDataList.T_playerName = TextPlayerName;
			pDataList.T_playerName.text = pDataList.playerName;

			Text TextPlayerBetType = playerGO.transform.Find("TextPlayerBetType").GetComponent<Text>();
			pDataList.T_TextPlayerBetType = TextPlayerBetType;
			pDataList.T_TextPlayerBetType.text = "";

			Text TextPlayerMoneyOnTable = playerGO.transform.Find("TextPlayerMoneyOnTable").GetComponent<Text>();
			pDataList.T_PlayerMoneyOnTable = TextPlayerMoneyOnTable;
			pDataList.T_PlayerMoneyOnTable.text = "";

			Text TextPlayerMoneyTotal =playerGO.transform.Find("TextPlayerMoneyTotal").GetComponent<Text>();
			pDataList.T_PlayerMoneyTotal = TextPlayerMoneyTotal;

			switch(_BBGlobalDefinitions.gameType) {
			case BBGlobalDefinitions.GameType.Limited: 
				pDataList.T_PlayerMoneyTotal.text = BBStaticData.getMoneyValue( BBStaticVariable.gameLimitedStackValue );
				pDataList.currentPlayerTotalMoney = BBStaticVariable.gameLimitedStackValue;
			    break;
			case BBGlobalDefinitions.GameType.NoLimit: 
				//pDataList.T_PlayerMoneyTotal.text = BBStaticData.getMoneyValue( _BBGlobalDefinitions.playersCashDuringOpenGame[pDataList.playerPosition] );
				//pDataList.currentPlayerTotalMoney = _BBGlobalDefinitions.playersCashDuringOpenGame[pDataList.playerPosition]; 
				pDataList.T_PlayerMoneyTotal.text = BBStaticData.getMoneyValue( BBStaticVariable.gameLimitedStackValue );
				pDataList.currentPlayerTotalMoney = BBStaticVariable.gameLimitedStackValue;
			    break;

			}

			Image PlayerActiveImage = playerGO.transform.Find("PlayerActiveImage").GetComponent<Image>();
			pDataList.playerActiveImage = PlayerActiveImage;
			pDataList.TimerCircle = playerGO.transform.Find("TimerCircle").GetComponent<Image>();
			   if(_BBGlobalDefinitions.playersStateIsOutDuringOpenGame[pDataList.playerPosition]) {
				   pDataList.playerActiveImage.color = Color.blue;
			   } else {
			       pDataList.playerActiveImage.color = Color.red;
			   }

			        Sprite sprite = null;
			        if(pDataList.playerAvatarImageIdx.Length > 5) {
				      sprite = BBStaticVariable.getSpriteFromBytes(pDataList.playerAvatarImageIdx,true,"0");
					} else {
				      sprite = BBStaticVariable.getSpriteFromBytes(pDataList.playerAvatarImageIdx,false,pDataList.playerAvatarImageIdx);
					}
			       //var sprite = Resources.Load<Sprite>("Avatar/playerAvatar_" + pDataList.playerAvatarImageIdx);//pDataList.playerPosition.ToString());
			Image AvatarImage = playerGO.transform.Find("AvatarImage").GetComponent<Image>();
			pDataList.playerAvatarImage = AvatarImage;
			pDataList.playerAvatarImage.overrideSprite = sprite;

			Texture2D texCountry;
			string _cc = pDataList.playerCountryCode;
			if( (_cc.Length == 0) || (_cc == "XX") ) {
			   texCountry = Resources.Load("NULL") as Texture2D;
			} else {
			   texCountry = Resources.Load(_cc) as Texture2D;
			}
			playerGO.transform.Find("ImageCountry").GetComponent<RawImage>().texture = texCountry;


			GameObject PlayerDealerImage = playerGO.transform.Find("PlayerDealerImage").gameObject;
			pDataList.PlayerDealerImage = PlayerDealerImage;
			pDataList.PlayerDealerImage.SetActive(false);

			Transform checkAllInImg = playerGO.transform.Find("AllInImage");
			if(checkAllInImg != null) {
				pDataList.AllInImage = checkAllInImg.gameObject.GetComponent<Image>();
				pDataList.AllInImage.enabled = false;
	        }

	}

	int getNextPlayer(int afterIdx) {
	    int tmpRet = -1;

		//Debug.Log("getNextPlayer --> afterIdx : " + afterIdx);

		List<BBPlayerData> pdList = _NewMultiplayerHelper.getActiveInGamePlayersList(playerDataList);

		int highterPos = 0;
		foreach(BBPlayerData pd in pdList) {
		   if(pd.playerPosition > highterPos) highterPos = pd.playerPosition; 
			//Debug.Log("getNextPlayer --> : " + pd.playerName + " : " + pd.playerPosition + " : " + highterPos);
		}

		if(afterIdx > highterPos) {
		  afterIdx = highterPos;
				//Debug.Log(">>(afterIdx > highterPos)<< getNextPlayer --> afterIdx : " + afterIdx + " highterPos : " + highterPos);
		}

				 if(/*(afterIdx == pdList.Count-1) ||*/ (afterIdx == highterPos) ) {
				   BBPlayerData pd = pdList.Find(item => item.playerPosition == 0);
				   if(pd != null) {
					  tmpRet = pd.playerPosition;
				   } else {
					  //Debug.Log("NULL getNextPlayer --> tmpRet : " + tmpRet);
							for(int x = 1; x < pdList.Count;x++) {
								BBPlayerData pd2 = pdList.Find(item => item.playerPosition == x);
								  if(pd2 != null) {
									tmpRet = pd2.playerPosition;
									break;
								  } else {
								  }      					      
							}
				   }
				} else {
				    int next = (afterIdx + 1);
					//Debug.Log("getNextPlayer --> next : " + next);
				    BBPlayerData pd = pdList.Find(item => item.playerPosition == next);
				    if(pd != null) {
				       tmpRet = pd.playerPosition;
							//Debug.Log("NOT NULL getNextPlayer --> tmpRet : " + tmpRet);
				     } else {
							//Debug.Log("NULL getNextPlayer --> tmpRet : " + tmpRet);
							  for(int x = next; x < highterPos;x++) { //pdList.Count;x++) {
								next++;
								  BBPlayerData pd2 = pdList.Find(item => item.playerPosition == next);
								  if(pd2 != null) {
									tmpRet = pd2.playerPosition;
									break;
								  } else {
								  }      					      
							  }
				     }
				}
		

		if(tmpRet == -1) {
		  Debug.Log("============================================= CRITICAL ANOMALIE HAND STOP ============================== error : " + tmpRet);
		  _photonView.RPC("RPCManageAnomalieOnGameHandMustStop",PhotonTargets.All,"ERROR_ON_GETNEXT");
		}

	   setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.underGunPlayer,tmpRet);

		return tmpRet;
	}


#endregion

#region cards management

    public GameObject getCard(Vector2 vCard, bool wantRotate, bool excludeGiveAllCardOpen) {
        string type = vCard.x.ToString();
	    string card = vCard.y.ToString();
	    string val = type + "_" + card;	
	    GameObject  c = Instantiate(Resources.Load(val)) as GameObject;	
        c.name = val;
		c.GetComponent<BBCard>().v2_value = vCard;
        c.GetComponent<BBCard>().setCardImage(vCard);

        if(excludeGiveAllCardOpen) {
				if (wantRotate)
				{
					c.transform.rotation = rotatedCardRotation;
				}
        }
        return c;
    }

	public GameObject getCard(Vector2 vCard, bool wantRotate) {
     
        string type = vCard.x.ToString();
	    string card = vCard.y.ToString();
	    string val = type + "_" + card;	
        
	    GameObject  c = Instantiate(Resources.Load(val)) as GameObject;	
        c.name = val;
		c.GetComponent<BBCard>().v2_value = vCard;
        c.GetComponent<BBCard>().setCardImage(vCard);

        if(wantRotate) {
               c.transform.rotation = rotatedCardRotation;
         }

			return c;
     }


   public Transform[] getGiveCardsList(int startPos) {

		 List<BBPlayerData> pdList = new List<BBPlayerData>();
		 foreach(BBPlayerData pd in playerDataList) {if(pd.playerGameObject != null) {pdList.Add(pd);}}

	     Transform[] tmpRes = new Transform[pdList.Count * 2];

		int idx = startPos;
        int counter = 1;

		for(int x = 0;x < tmpRes.Length; x++) {
	                if(counter == 1) {
                       tmpRes[x] = playersCardPositionsRoot.Find(idx.ToString() + "_" + counter.ToString()); 
                       counter++;         	                   
		            } else {
				      tmpRes[x] = playersCardPositionsRoot.Find(idx.ToString() + "_" + counter.ToString()); 
				      counter = 1;
				       //idx++;
					   idx = getNextPlayer(idx);     
		            }
		}

		 return tmpRes;
  }


  void activateStartGameButton() {
		//_BBGuiInterface.TextGamePhase.text = "Phase : " + "Waiting hand"; 
		if(PhotonNetwork.player.IsMasterClient) {
				_RPCController.shareInfomessagePlayerRelated(""," Waiting Dealer Start Game Hand...",0,true);
                ButtonStartTheGame.SetActive(true);
       }
  }

	void InitializeDeck(){
			mainDeck.Clear();
			GlobalDeck.Clear();
			for(int deckValue = 1; deckValue <= 13; deckValue++){
				for(int deckSuit = 1; deckSuit <= 4; deckSuit++){
					mainDeck.Add(new Vector2(deckSuit, deckValue));
				}
			}
			if(BBStaticData.debugGameControllerMultiplayer) Debug.Log("Deck Initialized. Size: " + mainDeck.Count);
			DisplayMainDeck();
	}

	void DisplayMainDeck(){
		  ShuffleDeck(5);
		  GlobalDeck.AddRange(mainDeck);
		 //Debug.Log("Deck size: " + GlobalDeck.Count);
	}

	void ShuffleDeck(int times){
			
			for(int i = 0; i < times; i++){
				List<Vector2> tempDeck1 = new List<Vector2>();
				for(int j = 0; j < mainDeck.Count; j++){
					tempDeck1.Add(mainDeck[j]);
				}
				
				List<Vector2> tempDeck2 = new List<Vector2>();
				int indexCard;
				
				while(0 < tempDeck1.Count){
					int minimum = 0;
					int maximum = tempDeck1.Count;
					indexCard = UnityEngine.Random.Range(minimum, maximum);
					
					tempDeck2.Add(tempDeck1[indexCard]);
					tempDeck1.RemoveAt(indexCard);
				}
				
				mainDeck.Clear();
				for(int k = 0; k < tempDeck2.Count; k++){
					mainDeck.Add(tempDeck2[k]);
				}
				tempDeck2.Clear();
			}
		}

	IEnumerator giveOneCard(int startPosPlayerId, Transform destPos, bool covered) {

	   if(GlobalDeck.Count > 10) {

			currentGivenCard = GlobalDeck[lastGivenCardIdx];
			GameObject cartToGive = null;
			//bool wantRotate = false;
			bool wantRotate = covered;

			cartToGive = getCard(GlobalDeck[lastGivenCardIdx],wantRotate,true);
			cartToGive.transform.position = GetComponent<BBMoveingObjectsController>().playersChipStartingPoint[startPosPlayerId].position;
				if (startPosPlayerId == _BBGlobalDefinitions.currentActivedealer && destPos != cardsDiscardPosition)
					cartToGive.transform.localScale = new Vector3(2.8f, 2.8f, 2.8f);
				yield return new WaitForEndOfFrame();
			yield return StartCoroutine( GetComponent<BBMoveingObjectsController>().moveCard(cartToGive, destPos) );
			lastGivenCardIdx++;
       }

	}


#endregion

#region Action On Custom Properties

	public void setRoomCustomProperties(string keyProperties, string value) {
			Hashtable ht = new Hashtable();
			ht[keyProperties] = value;
			PhotonNetwork.room.SetCustomProperties(ht); 
	}

	public void setRoomCustomProperties(string keyProperties, float value) {
			Hashtable ht = new Hashtable();
			ht[keyProperties] = value;
			PhotonNetwork.room.SetCustomProperties(ht); 
	}

	public void setRoomCustomProperties(string keyProperties, int value) {
			Hashtable ht = new Hashtable();
			ht[keyProperties] = value;
			PhotonNetwork.room.SetCustomProperties(ht); 
	}


	public void setPlayerCustomProperties(PhotonPlayer pp, string keyProperties, float value) {
			Hashtable ht = new Hashtable();
			ht[keyProperties] = value;
			pp.SetCustomProperties(ht);
	}

	public void setPlayerStringCustomProperties(PhotonPlayer pp, string keyProperties, string value) {
			Hashtable ht = new Hashtable();
			ht[keyProperties] = value;
			pp.SetCustomProperties(ht);
	}


	 public string getStringRoomProperties(string keyProperties) {
	  string tmpRet = "";
	  if(PhotonNetwork.room.CustomProperties[keyProperties] == null) {
		Hashtable ht = new Hashtable();
		ht[keyProperties] = tmpRet;
		PhotonNetwork.room.SetCustomProperties(ht);
	  } else {
		tmpRet = (string)PhotonNetwork.room.CustomProperties[keyProperties];
	  }
	  return tmpRet;
	 }

	 public int getIntRoomProperties(string keyProperties) {
	  int tmpRet = 0;
	  if(PhotonNetwork.room.CustomProperties[keyProperties] == null) {
	    tmpRet = 0;
		Hashtable ht = new Hashtable();
		ht[keyProperties] = tmpRet;
		PhotonNetwork.room.SetCustomProperties(ht);
	  } else {
		tmpRet = (int)PhotonNetwork.room.CustomProperties[keyProperties];
	  }
	  return tmpRet;
	}


	public float getFloatRoomProperties(string keyProperties) {
	  float tmpRet = 0;
	  if(PhotonNetwork.room.CustomProperties[keyProperties] == null) {
	    tmpRet = 0;
		Hashtable ht = new Hashtable();
		ht[keyProperties] = tmpRet;
		PhotonNetwork.room.SetCustomProperties(ht);
	  } else {
		tmpRet = (float)PhotonNetwork.room.CustomProperties[keyProperties];
	  }
	  return tmpRet;
	}

	public float getFloatPlayerProperties(PhotonPlayer pp, string keyProperties) {
	  float tmpRet = 0;
	  if(pp.CustomProperties[keyProperties] == null) {
	    tmpRet = 0;
		Hashtable ht = new Hashtable();
		ht[keyProperties] = tmpRet;
		pp.SetCustomProperties(ht);
	  } else {
		tmpRet = (float)pp.CustomProperties[keyProperties];
	  }
	  return tmpRet;
	}

	public string getStringPlayerProperties(PhotonPlayer pp, string keyProperties) {
	  string tmpRet = "";
	  if(pp.CustomProperties[keyProperties] == null) {
	    tmpRet = "";
		Hashtable ht = new Hashtable();
		ht[keyProperties] = tmpRet;
		pp.SetCustomProperties(ht);
	  } else {
		tmpRet = (string)pp.CustomProperties[keyProperties];
	  }
	  return tmpRet;
	}


#endregion

#region photonEvents

	void OnDisconnectedFromPhoton() {
#if USE_UNITY_ADV
#if UNITY_ANDROID || UNITY_IOS
		BLab.GetCoinsSystem.UnityAdvertisingController.ShowAdPlacement( BLab.GetCoinsSystem.UnityAdvertisingController.zoneIdVideo);
#endif
#endif
	 SceneManager.LoadScene(0);
	}
	void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
	   Debug.Log("OnPhotonPlayerConnected>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> new newPlayer : " + newPlayer.NickName);
	}


	void OnMasterClientSwitched(PhotonPlayer newMasterClient) {
			Debug.Log("OnMasterClientSwitched>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> new master : " + newMasterClient.NickName);

			_RPCController.setActiveDealer();

			if(PhotonNetwork.room.CustomProperties[NewGameControllerMultiplayer.CustomPropertiesKeyList.IsGameStarted] != null) {
				string gameStarted = (string)PhotonNetwork.room.CustomProperties[NewGameControllerMultiplayer.CustomPropertiesKeyList.IsGameStarted];
				if(gameStarted.Contains("YES")) {
				} else {
					activateStartGameButton();
				}
		    }

	}

	void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
			Debug.Log("OnPhotonPlayerDisconnected>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> otherPlayer : " + otherPlayer.NickName + " current Players : " + PhotonNetwork.playerList.Length);

			if(PhotonNetwork.playerList.Length == 1) {

			    PanelPlayerLeftAloneWonThePot.SetActive(true);
				float potMoney = getFloatRoomProperties(CustomPropertiesKeyList.moneyOnTable);
				if(potMoney > 0) {
					PanelPlayerLeftAloneWonThePot.transform.Find("TextMoneyOnTable").GetComponent<Text>().text = BBStaticData.getMoneyValue(potMoney);
					BBPlayerData pd = new BBPlayerData();
					pd = _NewMultiplayerHelper.getMyPlayerData(playerDataList,PhotonNetwork.player.NickName);
					pd.currentPlayerTotalMoney += potMoney;
					pd.T_PlayerMoneyTotal.text = BBStaticData.getMoneyValue(pd.currentPlayerTotalMoney);
					BBStaticVariable.updatePlayerGeneralMoney(true,potMoney);
				} else {
					PanelPlayerLeftAloneWonThePot.transform.Find("TextMoneyOnTable").GetComponent<Text>().text = " No Money On Table...";
				}
				_BBGuiInterface.setActionButtonPosition(false);
				StartCoroutine(StartExecuteNewGameHand());
			} else {

	            if(PhotonNetwork.player.IsMasterClient) {
	              _RPCController.removeCards(otherPlayer.NickName);
	              string s_waitingResponseFromPlayer = getStringRoomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.waitingForPlayerResponse);
				  if(!string.IsNullOrEmpty(s_waitingResponseFromPlayer)) {
					 if(s_waitingResponseFromPlayer == otherPlayer.NickName) {
						managePlayerExitDuringWaitForResponse(s_waitingResponseFromPlayer);
					 }
	              }
	            }
	         }

	}

#endregion

#region betting

  public IEnumerator executeSmallAndBigBlindBet() {
	 //Debug.Log(_BBGlobalDefinitions.moneyOnTable +  " : <<< moneyOnTable  ***[NewGameControllerMultiplayer]***[executeSmallAndBigBlindBet]*** : " + _BBGlobalDefinitions.currentActivedealer + " : " + _BBGlobalDefinitions.smallBlindPlayerId + " : " + _BBGlobalDefinitions.bigBlindPlayerId);
	 _RPCController.shareGamePhaseText("FirstBettingRound");
	 _BBGlobalDefinitions.moneyOnTable = 0;
	 _BBGlobalDefinitions.lastBet = _BBGlobalDefinitions.bigBlindValue;

	      float valToSet = BBStaticData.cardsHandProgressive * 25; 
	      yield return StartCoroutine( _RPCController.executeChipBet(_BBGlobalDefinitions.smallBlindPlayerId,valToSet, "SmallBlind") );

	 yield return new WaitForSeconds(3);

	      float valToSet2 = BBStaticData.cardsHandProgressive * 50; 
			yield return StartCoroutine( _RPCController.executeChipBet(_BBGlobalDefinitions.bigBlindPlayerId,valToSet2, "BigBlind") );

  }

  IEnumerator executeAllInBet(float value,int playerIdx) {
	  Debug.Log(_BBGlobalDefinitions.moneyOnTable +  " : <<< moneyOnTable  ***[NewGameControllerMultiplayer]***[executeAllInBet]*** : " + _BBGlobalDefinitions.playerToTalk);
	  yield return StartCoroutine( _RPCController.executeChipBet(playerIdx,value, "AllIn") );
  }


  IEnumerator executeRaiseBet(float value,int playerIdx) {
	 // Debug.Log(_BBGlobalDefinitions.moneyOnTable +  " : <<< moneyOnTable  ***[NewGameControllerMultiplayer]***[executeRaiseBet]*** : " + _BBGlobalDefinitions.playerToTalk);
	  yield return StartCoroutine( _RPCController.executeChipBet(playerIdx,value, "Raise") );
  }


  IEnumerator executeCallBet(float value,int playerIdx) {
	  //Debug.Log(_BBGlobalDefinitions.moneyOnTable +  " : <<< moneyOnTable  ***[NewGameControllerMultiplayer]***[executeCallBet]*** : " + _BBGlobalDefinitions.playerToTalk);
	  yield return StartCoroutine( _RPCController.executeChipBet(playerIdx,value, "Call") );
  }

  IEnumerator executeFold(int playerIdx) {
			//Debug.Log(_BBGlobalDefinitions.moneyOnTable +  " : <<< moneyOnTable  ***[NewGameControllerMultiplayer]***[executeFold]*** : " + _BBGlobalDefinitions.playerToTalk);
			yield return StartCoroutine( _RPCController.executeFold(playerIdx));
	 yield break;
  }

#endregion

#region Count Down

	int NewGameCountDownCounter = BBStaticVariable.secondsWaitToStartNewGameHand;
	void executeNewGameCountDown() {
		//if(BBStaticData.debugGameControllerMultiplayer)Debug.Log("----executeNewGameCountDown------->>>>>>>>> : " + NewGameCountDownCounter + " isInvoking : " + IsInvoking("executeNewGameCountDown"));
		TextNewGameInSeconds.text = NewGameCountDownCounter.ToString();
		NewGameCountDownCounter--;
		if(NewGameCountDownCounter < 1) {
			CancelInvoke();
			StartCoroutine(StartExecuteNewGameHand());
			UIMoveingController.SetActive(false);
		}
	}

	void  executeResponseTimeOutCountDown() {
			foreach (BBPlayerData pd in playerDataList)
				if (pd.playerName != null)
					if (pd.playerActiveImage.color == Color.green && pd.T_TextPlayerBetType.text == "")
					{
						activeplayer = pd;
						break;
					}
			//TextWailResponseCountDown.text = responseTimeOutSeconds.ToString();
            activeplayer.TimerCircle.gameObject.SetActive(true);
            activeplayer.TimerCircle.fillAmount = (float)responseTimeOutSeconds / 15;
			responseTimeOutSeconds--;

			if(responseTimeOutSeconds < 7) {
				GetComponent<BBSoundController>().playClip(GetComponent<BBSoundController>().genBip);
			}

			if(responseTimeOutSeconds < 1) {
				//TextWailResponseCountDown.text = "";
				activeplayer.TimerCircle.gameObject.SetActive(false);
				responseTimeOutSeconds = BBStaticVariable.atBetRequestResponseTimeOutSeconds;

				  string _underGunPlayer = getStringRoomProperties(CustomPropertiesKeyList.waitingForPlayerResponse);
				  if(_underGunPlayer == PhotonNetwork.player.NickName) {
				      GameObject tmpGO = new GameObject("buttonGameFOLD");
		              Destroy(tmpGO,5);
		              gotGameActionButton(tmpGO);
		          }

				CancelInvoke("executeResponseTimeOutCountDown");
				responseTimeOutSeconds = BBStaticVariable.atBetRequestResponseTimeOutSeconds;
			}
	}

	public void executeInvokeCountDownForBetResponse() {
	  InvokeRepeating("executeResponseTimeOutCountDown",1,1);
	  responseTimeOutSeconds = BBStaticVariable.atBetRequestResponseTimeOutSeconds;
	}


	public void cancelInvokeCountDownForBetResponse() {
	    CancelInvoke("executeResponseTimeOutCountDown");
		responseTimeOutSeconds = BBStaticVariable.atBetRequestResponseTimeOutSeconds;
		//TextWailResponseCountDown.text = ""; 
        activeplayer.TimerCircle.gameObject.SetActive(false);
	}

#endregion

#region Phases Execution

 [PunRPC]
 void RPCStartExecuteShowDown() {
	StartCoroutine(startExecuteShowDown());
	NewGameCountDownCounter = BBStaticVariable.secondsWaitToStartNewGameHand;
	InvokeRepeating("executeNewGameCountDown",1,1);
 }

 IEnumerator startExecuteShowDown() {

         yield return new WaitForSeconds(3);

		Debug.Log("*******************************[BBGameController][startExecute*** SHOW DOWN ***Phase]************************** Phase : " + _BBGlobalDefinitions.gamePhaseDetail);

		 _RPCController.setAllPlayersAtState(BBGlobalDefinitions.GamePhaseDetail.None);
		 _BBGlobalDefinitions.gamePhaseDetail = BBGlobalDefinitions.GamePhaseDetail.ShowDown;
	     _RPCController.setGameState(BBGlobalDefinitions.GamePhaseDetail.ShowDown);

		//_BBGuiInterface.TextGamePhase.text = "Phase : ShowDown";
		//	_BBGuiInterface.TextGamePhaseInfo.text = "Starting ShowDown...";
			BBMoveingObjectsController BBMove = GetComponent<BBMoveingObjectsController>();
			GetComponent<NewResultEngine>().excecuteShowDown();
			StartCoroutine(BBMove.removeAllChips());

			/*
					if(_BBGlobalDefinitions.gameType == BBGlobalDefinitions.GameType.Limited) {
						 GetComponent<BBShowDownResultControllerMultiplayer>().executeFinalShowDown();
					} else {
						// GetComponent<BBAllInControllerMultiplayer>().executeShowDowmAllin();
							GetComponent<BBShowDownResultControllerMultiplayer>().executeFinalShowDown();
					}
			*/
			yield break;

 }

 [PunRPC]
 void RPCStartExecuteRiverRound() {
   StartCoroutine(startExecuteRiverRound());
 }

 IEnumerator startExecuteRiverRound() {
	 setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.lastBet,_BBGlobalDefinitions.bigBlindValue);
	 yield return new WaitForEndOfFrame();
	 _RPCController.setAllPlayersAtState(BBGlobalDefinitions.GamePhaseDetail.None);
			yield return new WaitForEndOfFrame();
	 _BBGlobalDefinitions.gamePhaseDetail = BBGlobalDefinitions.GamePhaseDetail.River;
	 _RPCController.setGameState(BBGlobalDefinitions.GamePhaseDetail.River);
	 yield return new WaitForEndOfFrame();
	 _RPCController.setAllImageState(Color.red);

	 yield return new WaitForEndOfFrame();
	 int underGun = 0;
	 underGun =  getNextPlayer(_BBGlobalDefinitions.currentActivedealer);

	 Debug.Log("startExecuteTurnRound --> underGun --->> : " + underGun);

	 //setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.underGunPlayer,underGun);
				yield return new WaitForSeconds(1);
				yield return StartCoroutine( giveOneCard(_BBGlobalDefinitions.currentActivedealer,cardsDiscardPosition,true) );
				yield return new WaitForSeconds(1);
				cardsDiscardPosition.position = new Vector3(cardsDiscardPosition.position.x, cardsDiscardPosition.position.y + 0.01f, cardsDiscardPosition.position.z);
			    yield return StartCoroutine( giveOneCard(_BBGlobalDefinitions.currentActivedealer,GameObject.Find("openCard_RIVER").transform,false) );
				riverCard = currentGivenCard;
				yield return new WaitForSeconds(1);

		 if(PhotonNetwork.player.IsMasterClient) {		 
			_RPCController.shareGamePhaseText("Phase : River");
	        int firstPlayerToTalk = underGun;
			if(_NewMultiplayerHelper.checkForMoneyPossibility(this,firstPlayerToTalk) == false) {
				StartCoroutine( executeActionOnPlayerResponse("button_outOfGame",firstPlayerToTalk) );
             } else {
		        _RPCController.setActiveTalker(firstPlayerToTalk,true);
		        yield return new WaitForEndOfFrame();
		        _RPCController.askPlayerToBet(firstPlayerToTalk);
		     }
	    }
     
 }


 [PunRPC]
 void RPCStartExecuteTurnRound() {
   StartCoroutine(startExecuteTurnRound());
 }

 IEnumerator startExecuteTurnRound() {
	 setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.lastBet,_BBGlobalDefinitions.bigBlindValue);
	 yield return new WaitForEndOfFrame();
	 _RPCController.setAllPlayersAtState(BBGlobalDefinitions.GamePhaseDetail.None);
	 yield return new WaitForEndOfFrame();
	 _BBGlobalDefinitions.gamePhaseDetail = BBGlobalDefinitions.GamePhaseDetail.Turn;
	 _RPCController.setGameState(BBGlobalDefinitions.GamePhaseDetail.Turn);
	 yield return new WaitForEndOfFrame();
	 _RPCController.setAllImageState(Color.red);

	 int underGun = 0;
	 underGun =  getNextPlayer(_BBGlobalDefinitions.currentActivedealer);
	 //Debug.Log("startExecuteTurnRound --> underGun --->> : " + underGun);

	// setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.underGunPlayer,underGun);
				yield return new WaitForSeconds(1);
				yield return StartCoroutine( giveOneCard(_BBGlobalDefinitions.currentActivedealer,cardsDiscardPosition,true) );
				yield return new WaitForSeconds(1);
				cardsDiscardPosition.position = new Vector3(cardsDiscardPosition.position.x, cardsDiscardPosition.position.y + 0.01f, cardsDiscardPosition.position.z);
				yield return StartCoroutine( giveOneCard(_BBGlobalDefinitions.currentActivedealer,GameObject.Find("openCard_TURN").transform,false) );
				turnCard = currentGivenCard;
				yield return new WaitForSeconds(1);

		 if(PhotonNetwork.player.IsMasterClient) {		 
			_RPCController.shareGamePhaseText("Phase : Turn");
	        int firstPlayerToTalk = underGun;
			if(_NewMultiplayerHelper.checkForMoneyPossibility(this,firstPlayerToTalk) == false) {
				StartCoroutine( executeActionOnPlayerResponse("button_outOfGame",firstPlayerToTalk) );
             } else {
	           _RPCController.setActiveTalker(firstPlayerToTalk,true);
	           yield return new WaitForEndOfFrame();
	           _RPCController.askPlayerToBet(firstPlayerToTalk);
	         }
	    }
     
 }

 [PunRPC]
 void RPCStartExecuteFlopRound() {
    StartCoroutine(startExecuteFlopRound());
 }

 IEnumerator startExecuteFlopRound() {
	 setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.lastBet,_BBGlobalDefinitions.bigBlindValue);
	 yield return new WaitForEndOfFrame();
	 _RPCController.setAllPlayersAtState(BBGlobalDefinitions.GamePhaseDetail.None);
	 yield return new WaitForEndOfFrame();
	 _BBGlobalDefinitions.gamePhaseDetail = BBGlobalDefinitions.GamePhaseDetail.Flop;
	 _RPCController.setGameState(BBGlobalDefinitions.GamePhaseDetail.Flop);
	 yield return new WaitForEndOfFrame();
	 _RPCController.setAllImageState(Color.red);

	 int underGun = 0;
	 underGun =  getNextPlayer(_BBGlobalDefinitions.currentActivedealer);
	 Debug.Log("startExecuteFlopRound --> underGun --->> : " + underGun);

	// setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.underGunPlayer,underGun);

			   yield return new WaitForSeconds(1);

				yield return StartCoroutine( giveOneCard(_BBGlobalDefinitions.currentActivedealer,cardsDiscardPosition,true) );

				yield return new WaitForSeconds(1);
				cardsDiscardPosition.position = new Vector3(cardsDiscardPosition.position.x, cardsDiscardPosition.position.y + 0.01f, cardsDiscardPosition.position.z);

				yield return StartCoroutine( giveOneCard(_BBGlobalDefinitions.currentActivedealer,GameObject.Find("openCard_1").transform,false) );
				flopCardList[0] = currentGivenCard;

				yield return new WaitForSeconds(1);

				yield return StartCoroutine( giveOneCard(_BBGlobalDefinitions.currentActivedealer,GameObject.Find("openCard_2").transform,false) );
				flopCardList[1] = currentGivenCard;

				yield return new WaitForSeconds(1);

				yield return StartCoroutine( giveOneCard(_BBGlobalDefinitions.currentActivedealer,GameObject.Find("openCard_3").transform,false) );
				flopCardList[2] = currentGivenCard;

				yield return new WaitForSeconds(2);


	 if(PhotonNetwork.player.IsMasterClient) {
		_RPCController.shareGamePhaseText("Phase : Flop");
        int firstPlayerToTalk = underGun;
		if(_NewMultiplayerHelper.checkForMoneyPossibility(this,firstPlayerToTalk) == false) {
			StartCoroutine( executeActionOnPlayerResponse("button_outOfGame",firstPlayerToTalk) );
        } else {
            _RPCController.setActiveTalker(firstPlayerToTalk,true);
            yield return new WaitForEndOfFrame();
            _RPCController.askPlayerToBet(firstPlayerToTalk);
        }
    }

  yield break;
 }

 IEnumerator starExecuteFirstBettingRound() {
    int[] toExclude = new int[2];
    toExclude[0] = _BBGlobalDefinitions.smallBlindPlayerId;
	toExclude[1] = _BBGlobalDefinitions.bigBlindPlayerId;

	_RPCController.setAllPlayersAtState(BBGlobalDefinitions.GamePhaseDetail.None,toExclude,BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound);

	 setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.lastBet,_BBGlobalDefinitions.bigBlindValue);
	 int underGun = 0;

	 List<BBPlayerData> activePlayers = _NewMultiplayerHelper.getActiveInGamePlayersList(playerDataList);

	 if(activePlayers.Count > 2) {
		underGun =  getNextPlayer(_BBGlobalDefinitions.bigBlindPlayerId);
	 } else {
		underGun = _BBGlobalDefinitions.smallBlindPlayerId;
		setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.underGunPlayer,underGun);
	 }

    if(PhotonNetwork.player.IsMasterClient) {
		_RPCController.shareGamePhaseText("FirstBettingRound");

        int firstPlayerToTalk = underGun;
		if(_NewMultiplayerHelper.checkForMoneyPossibility(this,firstPlayerToTalk) == false) {
		   StartCoroutine( executeActionOnPlayerResponse("button_outOfGame",firstPlayerToTalk) );
         } else {
            _RPCController.setActiveTalker(firstPlayerToTalk,true);
            yield return new WaitForEndOfFrame();
            _RPCController.askPlayerToBet(firstPlayerToTalk);
         }
    }

    yield break;
 }

 IEnumerator starExecuteMoneyAlignment() {

	if(PhotonNetwork.player.IsMasterClient) {

				yield return new WaitForEndOfFrame();
	           _RPCController.setAllImageState(Color.red);

	    List<BBPlayerData> activePlayers = _NewMultiplayerHelper.getActiveInGamePlayersList(playerDataList);

		//Debug.Log(">>> starExecuteMoneyAlignment >>> activePlayers : " + activePlayers.Count);

	    if(activePlayers.Count > 0) {
	                List<BBPlayerData> tmpPlayersToAsk = new List<BBPlayerData>();
					float maxPlayerBet =_NewMultiplayerHelper.getMaxPlayerBet(playerDataList);
					setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.moneyToCoverOnAlignment,maxPlayerBet);
					int gotPlayerToAsk = -1;
					for(int x = 0;x < activePlayers.Count;x++) {
						PhotonPlayer pp = _NewMultiplayerHelper.getPhotonPlayer(playerDataList,activePlayers[x].playerPosition);
						float playerBetOnTable = getFloatPlayerProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.currentMoneyOnTable);
						//Debug.Log(">>> starExecuteMoneyAlignment >>> playerBetOnTable : " + playerBetOnTable + " : " + activePlayers[x].playerPosition + " max : " + maxPlayerBet);
					    if(playerBetOnTable < maxPlayerBet) {
						     gotPlayerToAsk = activePlayers[x].playerPosition;
						     tmpPlayersToAsk.Add(activePlayers[x]);
							_RPCController.setSinglePlayerAtState(activePlayers[x].playerPosition,BBGlobalDefinitions.GamePhaseDetail.None);
					    } else {
							_RPCController.setSinglePlayerAtState(activePlayers[x].playerPosition,BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign);
					    }
					}

					//Debug.Log(">>> starExecuteMoneyAlignment >>> gotPlayerToAsk : " + gotPlayerToAsk);

                    if(gotPlayerToAsk != -1) {
                        _RPCController.setGameState(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign);
						yield return new WaitForEndOfFrame();
						_RPCController.shareGamePhaseText("MoneyAlignment");
						int underGun = tmpPlayersToAsk[0].playerPosition;
						setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.underGunPlayer,underGun);
						int firstPlayerToTalk = underGun;
						if(_NewMultiplayerHelper.checkForMoneyPossibility(this,firstPlayerToTalk) == false) {
				          StartCoroutine( executeActionOnPlayerResponse("button_outOfGame",firstPlayerToTalk) );
                        } else {
				           _RPCController.setActiveTalker(firstPlayerToTalk,true);
				           yield return new WaitForEndOfFrame();
				          _RPCController.askPlayerToBet(firstPlayerToTalk);
				        }
				    } else {
						_RPCController.setGameState(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign);
						yield return new WaitForEndOfFrame();
						StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign,""));
				    }

	    } else {

	    }

    }
  }

#endregion


    void Update() {

      if(PhotonNetwork.player.IsMasterClient) {
         if(waitingResponceFromPlayer) {
               if(gotResponceFromPlayer) {
                  waitingResponceFromPlayer = false;
                  _RPCController.shareGotResponceFromPlayer();
                  switch(_BBGlobalDefinitions.gamePhaseDetail) {
					case BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound:
					   //StartCoroutine(executeFirstBettingRoundResponse());
					break;
                  }
               }
         }
      }

    }

	public void gotGameActionButton(GameObject _go) {

	       _BBGuiInterface.ImageCardsContainerOnButtons.SetActive(false);

			List<BBPlayerData> tmpList = new List<BBPlayerData>();
	        for(int x = 0;x < playerDataList.Count;x++) {if(playerDataList[x].playerGameObject != null) {tmpList.Add(playerDataList[x]);}}
		    BBPlayerData pd = tmpList.Find(item => item.playerName == PhotonNetwork.player.NickName);

			//TODO(Useful Debug) Debug.Log("***[NewGameControllerMultiplayer]***[gotGameActionButton]*** _go name : " + _go.name +
			//          " phase + " + _BBGlobalDefinitions.gamePhaseDetail + 
			//          " talker : " + _BBGlobalDefinitions.playerToTalk); 

            _BBGuiInterface.setActionButtonPosition(false);

	        _RPCController.shareGotGameActionButton(_go.name, pd.playerPosition);

	}

	public void sharedPlayerActionButton(string buttName,int playerIdx) {

			cancelInvokeCountDownForBetResponse();

			/* TODO(Useful Debug)
			   Debug.Log("***[NewGameControllerMultiplayer]***[@@@ sharedPlayerActionButton @@@]*** _go name : " + buttName +
			          " phase : " + _BBGlobalDefinitions.gamePhaseDetail + 
			          " talker on Global : " + _BBGlobalDefinitions.playerToTalk +
			          " playerIdx from click : " + playerIdx +
			          " allIn executed at : " + (BBGlobalDefinitions.GamePhaseDetail)getIntRoomProperties(CustomPropertiesKeyList.AllInExecutedAtPhase)); 
            */
            StartCoroutine(executeActionOnPlayerResponse(buttName,playerIdx));        			        

	}


#region Execute Action On Response

	IEnumerator executeActionOnPlayerResponse(string buttName,int playerIdx) {

        if(!PhotonNetwork.player.IsMasterClient) yield break;

        switch(_BBGlobalDefinitions.gamePhaseDetail) {
		   case BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign:
				_RPCController.setSinglePlayerAtState(playerIdx,BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign);
				switch(buttName) {
				case "buttonGameCALL":
					yield return StartCoroutine(executeCallForMoneyAlignRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign,buttName));
				break;
				case "buttonGameFOLD":
					yield return StartCoroutine(executeFoldRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign,buttName));
				break;
				case "button_outOfGame":
					executePlayerIsOutCoseNoMoreMoney(playerIdx);
					yield return new WaitForSeconds(4);
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign,buttName));
				break;
               }
		   break;
           case BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound:
				_RPCController.setSinglePlayerAtState(playerIdx,BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound);
               switch(buttName) {
				case "buttonGameCALL":
					yield return StartCoroutine(executeCallRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound,buttName));
				break;
				case "buttonGameFOLD":
					yield return StartCoroutine(executeFoldRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound,buttName));
				break;
				case "buttonGameRAISE":
					yield return StartCoroutine(executeRaiseRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound,buttName));
				break;
				case "button_outOfGame":
					executePlayerIsOutCoseNoMoreMoney(playerIdx);
					yield return new WaitForSeconds(4);
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign,buttName));
				break;
               }
           break;
		   case BBGlobalDefinitions.GamePhaseDetail.Flop:
				_RPCController.setSinglePlayerAtState(playerIdx,BBGlobalDefinitions.GamePhaseDetail.Flop);
               switch(buttName) {
				case "buttonGameCALL":
					yield return StartCoroutine(executeCallRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.Flop,buttName));
				break;
				case "buttonGameFOLD":
					yield return StartCoroutine(executeFoldRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.Flop,buttName));
				break;
				case "buttonGameRAISE":
					yield return StartCoroutine(executeRaiseRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.Flop,buttName));
				break;
				case "button_outOfGame":
					executePlayerIsOutCoseNoMoreMoney(playerIdx);
					yield return new WaitForSeconds(4);
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign,buttName));
				break;
				case "buttonGameALLIN":
					yield return StartCoroutine(executeAllInRequest(playerIdx, BBGlobalDefinitions.GamePhaseDetail.Flop));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.AllIn,buttName));
				break;
               }
           break;
			case BBGlobalDefinitions.GamePhaseDetail.Turn:
				_RPCController.setSinglePlayerAtState(playerIdx,BBGlobalDefinitions.GamePhaseDetail.Turn);
               switch(buttName) {
				case "buttonGameCALL":
					yield return StartCoroutine(executeCallRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.Turn,buttName));
				break;
				case "buttonGameFOLD":
					yield return StartCoroutine(executeFoldRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.Turn,buttName));
				break;
				case "buttonGameRAISE":
					yield return StartCoroutine(executeRaiseRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.Turn,buttName));
				break;
				case "button_outOfGame":
					executePlayerIsOutCoseNoMoreMoney(playerIdx);
					yield return new WaitForSeconds(4);
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign,buttName));
				break;
				case "buttonGameALLIN":
					yield return StartCoroutine(executeAllInRequest(playerIdx, BBGlobalDefinitions.GamePhaseDetail.Turn));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.AllIn,buttName));
				break;
               }
           break;
			case BBGlobalDefinitions.GamePhaseDetail.River:
				_RPCController.setSinglePlayerAtState(playerIdx,BBGlobalDefinitions.GamePhaseDetail.River);
               switch(buttName) {
				case "buttonGameCALL":
					yield return StartCoroutine(executeCallRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.River,buttName));
				break;
				case "buttonGameFOLD":
					yield return StartCoroutine(executeFoldRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.River,buttName));
				break;
				case "buttonGameRAISE":
					yield return StartCoroutine(executeRaiseRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.River,buttName));
				break;
				case "button_outOfGame":
					executePlayerIsOutCoseNoMoreMoney(playerIdx);
					yield return new WaitForSeconds(4);
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign,buttName));
				break;
				case "buttonGameALLIN":
					yield return StartCoroutine(executeAllInRequest(playerIdx, BBGlobalDefinitions.GamePhaseDetail.River));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.AllIn,buttName));
				break;
               }
           break;
			case BBGlobalDefinitions.GamePhaseDetail.AllIn:
				_RPCController.setSinglePlayerAtState(playerIdx,BBGlobalDefinitions.GamePhaseDetail.AllIn);
				switch(buttName) {
					case "buttonGameCALL":
						yield return StartCoroutine(executeCallRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.AllIn,buttName));
					break;
					case "buttonGameFOLD":
						yield return StartCoroutine(executeFoldRequest(playerIdx));
					yield return StartCoroutine(executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail.AllIn,buttName));
					break;
				}
           break;
        }



    }


#endregion

#region Execute Call Fold Raise Requests

	IEnumerator executeFoldRequest(int playerIdx) {
			yield return StartCoroutine(executeFold(playerIdx));
			yield return new WaitForSeconds(3);
    }

	IEnumerator executeCallForMoneyAlignRequest(int playerIdx) {
	 		float toCover = getFloatRoomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.moneyToCoverOnAlignment);
			PhotonPlayer pp = _NewMultiplayerHelper.getPhotonPlayer(playerDataList,playerIdx);
			float playerOnTab = getFloatPlayerProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.currentMoneyOnTable);
			float toBet = toCover - playerOnTab;
			_RPCController.setSinglePlayerAtState(playerIdx,BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign);
			yield return StartCoroutine(executeCallBet(toBet,playerIdx));
			_RPCController.shareLastBetValue(toBet);
			yield return new WaitForSeconds(3);
    }


    IEnumerator executeCallRequest(int playerIdx) {
			float toBet = getFloatRoomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.lastBet);

             if(_BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.AllIn) {
				Debug.Log("===================== executeCallRequest GamePhaseDetail.AllIn ======================= player : " + playerIdx + " toBet : " + toBet);
				yield return StartCoroutine( executeAllInAccept(playerIdx,toBet) );
             } else {
				_RPCController.shareLastBetValue(toBet);
				yield return StartCoroutine(executeCallBet(toBet,playerIdx));
				yield return new WaitForSeconds(3);
			 }
    }

    IEnumerator executeAllInAccept(int playerID, float toBetAllIn) {
			_RPCController.shareLastBetValue(toBetAllIn);
			yield return new WaitForEndOfFrame();
			PhotonPlayer pp = _NewMultiplayerHelper.getPhotonPlayer(playerDataList,playerID);

			float playerMoneyDispo = getFloatPlayerProperties(pp,CustomPropertiesKeyList.currentPlayerTotalMoney);

			if(playerMoneyDispo >= toBetAllIn) {
				yield return StartCoroutine(executeCallBet(toBetAllIn,playerID));
				yield return new WaitForEndOfFrame();
                _RPCController.ShareMoneyData();
				yield return new WaitForSeconds(3);
			} else {
			  float _diffToBet = toBetAllIn - playerMoneyDispo;
			  //float _toGiveBack = toBetAllIn - _diffToBet;
              int IdTogiveBack = getIntRoomProperties(CustomPropertiesKeyList.playerWhoRequestAllInID);
				Debug.Log("========= NOT COVERED ============ executeAllInAccept GamePhaseDetail.AllIn ======================= player : " + 
				playerID + " toBet : " + toBetAllIn + " _diffToBet : " + _diffToBet + " IdTogiveBack : " + IdTogiveBack);
                BBPlayerData pd = _NewMultiplayerHelper.getMyPlayerData(playerDataList,IdTogiveBack);
                //pd.currentPlayerTotalMoney += _diffToBet;
                PhotonPlayer ppBack = _NewMultiplayerHelper.getPhotonPlayer(playerDataList,IdTogiveBack);
                yield return new WaitForEndOfFrame();
                float ppBackTotMoney = getFloatPlayerProperties(ppBack,CustomPropertiesKeyList.currentPlayerTotalMoney);
				yield return new WaitForEndOfFrame();
                ppBackTotMoney += _diffToBet;
				setPlayerCustomProperties(ppBack,CustomPropertiesKeyList.currentPlayerTotalMoney,ppBackTotMoney);
				yield return new WaitForEndOfFrame();
                _RPCController.ShareMoneyData();
				yield return new WaitForEndOfFrame();
                if(pd.playerName == PhotonNetwork.player.NickName) {
                   BBStaticVariable.updatePlayerGeneralMoney(true,_diffToBet);
                }

				Transform PlayersChipEndingPosition = GameObject.Find("PlayersChipEndingPosition ").transform;
				Vector3 _from = PlayersChipEndingPosition.Find(IdTogiveBack.ToString()).position;
                Vector3 _to = pd.transform_card_1.position;

				yield return StartCoroutine(_RPCController.executeChipBet(playerID,playerMoneyDispo,"Call"));
				yield return new WaitForSeconds(3);
				StartCoroutine( _RPCController.executeChipBetToPosition(playerID,_diffToBet,"Back Not Covered",_from,_to) );
			}

    }

	IEnumerator executeRaiseRequest(int playerIdx) {
			float toBet = getFloatRoomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.lastBet);
			toBet += _BBGlobalDefinitions.smallBlindValue;

			bool canRaise = _NewMultiplayerHelper.checkForMoneyPossibility(this,playerIdx,toBet);

			if(canRaise) {
			   setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.lastBet,toBet);
			   _RPCController.shareLastBetValue(toBet);
			   yield return StartCoroutine(executeRaiseBet(toBet,playerIdx));
			   yield return new WaitForSeconds(3);
			} else {
				StartCoroutine( executeActionOnPlayerResponse("button_outOfGame",playerIdx) );
			}
    }

	IEnumerator executeAllInRequest(int playerIdx, BBGlobalDefinitions.GamePhaseDetail gpd) {

	    setRoomCustomProperties(CustomPropertiesKeyList.AllInExecutedAtPhase,(int)gpd);
		yield return new WaitForEndOfFrame();
	    setRoomCustomProperties(CustomPropertiesKeyList.waitingForPlayerResponse,"Y");
		yield return new WaitForEndOfFrame();
	    setRoomCustomProperties(CustomPropertiesKeyList.playerWhoRequestAllInID,playerIdx);

	    PhotonPlayer pp = _NewMultiplayerHelper.getPhotonPlayer(playerDataList,playerIdx);
	    float toBet = getFloatPlayerProperties(pp,CustomPropertiesKeyList.currentPlayerTotalMoney);
		setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.lastBet,toBet);
		yield return new WaitForSeconds(1);
		_RPCController.shareLastBetValue(toBet);
		yield return new WaitForEndOfFrame();
		_RPCController.updateAllGamePhaseDetail(BBGlobalDefinitions.GamePhaseDetail.AllIn);
		yield return new WaitForEndOfFrame();
		setPlayerStringCustomProperties(pp,CustomPropertiesKeyList.isPlayerUnderAllIn,"Y");
		yield return new WaitForEndOfFrame();
		_RPCController.shareAllInRequest(playerIdx);

		yield return new WaitForSeconds(3);

		yield return StartCoroutine(executeAllInBet(toBet,playerIdx));
	}

#endregion

    [PunRPC]
	void RPCExecutePlayerWonCoseOthersFolds(string playerName) {
			Debug.Log("RPCExecutePlayerWonCoseOthersFolds -- > name : " + playerName);

	   BBPlayerData pd = _NewMultiplayerHelper.getMyPlayerData(playerDataList,playerName);

	   Debug.Log("RPCExecutePlayerWonCoseOthersFolds -- > pd null : " + (pd == null));


	   if(pd.playerName == PhotonNetwork.player.NickName) {
				PanelPlayerWonCoseNoOneAcceptCall.SetActive(true);
				float potMoney = getFloatRoomProperties(CustomPropertiesKeyList.moneyOnTable);
				PanelPlayerWonCoseNoOneAcceptCall.transform.Find("TextWinnerMoneyWon").GetComponent<Text>().text = BBStaticData.getMoneyValue(potMoney);
				pd.currentPlayerTotalMoney += potMoney;
				setRoomCustomProperties(CustomPropertiesKeyList.moneyOnTable,0.0f);
				pd.T_PlayerMoneyTotal.text = BBStaticData.getMoneyValue(pd.currentPlayerTotalMoney);
				BBStaticVariable.updatePlayerGeneralMoney(true,potMoney);
				_BBGuiInterface.setActionButtonPosition(false);
	   } else {

	   }

	 //  if(PhotonNetwork.player.IsMasterClient) {
		  StartCoroutine(StartExecuteNewGameHand());
	 //  }
	}

    IEnumerator executePlayerWonCoseOthersFolds(BBPlayerData leftPD) {
		_photonView.RPC("RPCExecutePlayerWonCoseOthersFolds",PhotonTargets.All,leftPD.playerName);
		yield break;
    }

#region Check For Round Done

	IEnumerator executeCheckForRoundDone(BBGlobalDefinitions.GamePhaseDetail gpd,string buttName) {

	  string isHandOnGoing = getStringRoomProperties(CustomPropertiesKeyList.IsGameStarted);
	  if(isHandOnGoing == "YES") {
	  } else {
	    yield break;
	  }

			int underGun = getIntRoomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.underGunPlayer);

			List<BBPlayerData> tmpList = new List<BBPlayerData>();
	        for(int x = 0;x < playerDataList.Count;x++) {if(playerDataList[x].playerGameObject != null) {tmpList.Add(playerDataList[x]);}}

	        int counterIdx = -1;
             	  
             for(int x = 0;x < tmpList.Count;x++) {
				/* TODO(Useful Debug)
				  Debug.Log("***[NewGameControllerMultiplayer]***[executeCheckForRoundDone]*** tmpList[x].isObserver : " + tmpList[x].isObserver +
					" tmpList[x].isOutOfGame : " + tmpList[x].isOutOfGame + " tmpList[x].gamePhaseDetail : " + tmpList[x].gamePhaseDetail + " : " + gpd);
				*/
                if(tmpList[x].isObserver == false && tmpList[x].isOutOfGame == false && tmpList[x].gamePhaseDetail != gpd) {
                  counterIdx = tmpList[x].playerPosition;
                  break;
                }
             }

			int inGameCounter = 0;
			BBPlayerData inGamePD = null;
			  for(int x = 0;x < tmpList.Count;x++) {
			     if(tmpList[x].isOutOfGame == false) {
			       inGameCounter++;
				   inGamePD = tmpList[x];
			     } else {
			     }
			  }
			/* TODO(Useful Debug)
			Debug.Log("***[NewGameControllerMultiplayer]***[executeCheckForRoundDone]*** phase : " + gpd +
				      " counterIdx : " + counterIdx + 
				      " last phase : " + (BBGlobalDefinitions.GamePhaseDetail)getIntRoomProperties(CustomPropertiesKeyList.lastPhaseBeforeAlignment) +
				      " inGameCounter :  " + inGameCounter); 
            */
		   if(inGameCounter == 1) {
				StartCoroutine(executePlayerWonCoseOthersFolds(inGamePD));
		   } else {
		            if(gpd == BBGlobalDefinitions.GamePhaseDetail.AllIn) {
					    Debug.Log("====================== BBGlobalDefinitions.GamePhaseDetail.AllIn ========================= " +
					              " playeWhoStarted AllIn : " + getIntRoomProperties(CustomPropertiesKeyList.playerWhoRequestAllInID) +
					              " underGun : " + underGun);
					     int allInAplicant = getIntRoomProperties(CustomPropertiesKeyList.playerWhoRequestAllInID);

					    if(allInAplicant != underGun) {
					        // allIn ShowDown
						     if(buttName == "buttonGameCALL") {
						         Debug.Log("===========allIn ShowDown=========== BBGlobalDefinitions.GamePhaseDetail.AllIn ==========allIn ShowDown=============== " +
					              " playeWhoStarted AllIn : " + getIntRoomProperties(CustomPropertiesKeyList.playerWhoRequestAllInID) +
					              " underGun : " + underGun);
					              PhotonPlayer pp = _NewMultiplayerHelper.getPhotonPlayer(playerDataList,underGun);
					              setPlayerStringCustomProperties(pp,CustomPropertiesKeyList.isPlayerUnderAllIn,"Y");
					              yield return new WaitForSeconds(2);
						         _photonView.RPC("RPCStartExecuteShowDown",PhotonTargets.AllViaServer);
						     } else {
							      if(PhotonNetwork.player.IsMasterClient) {
				                        int firstPlayerToTalk = getNextPlayer(underGun);
						                _RPCController.setActiveTalker(firstPlayerToTalk,true);
					                    yield return new WaitForEndOfFrame();
					                    _RPCController.askPlayerToBet(firstPlayerToTalk);
				                  }
						     }

					    } else {
					              if(PhotonNetwork.player.IsMasterClient) {
				                        int firstPlayerToTalk = getNextPlayer(underGun);
						                _RPCController.setActiveTalker(firstPlayerToTalk,true);
					                    yield return new WaitForEndOfFrame();
					                    _RPCController.askPlayerToBet(firstPlayerToTalk);
				                  }
				        }

		            } else { // start Not AllIn
				             if(counterIdx == -1) {
				                switch(gpd) {
				                 case BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound:
				                    setRoomCustomProperties(CustomPropertiesKeyList.lastPhaseBeforeAlignment,(int)BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound);
									_RPCController.shareGamePhaseText("MoneyAlignment");
									StartCoroutine( starExecuteMoneyAlignment() );
				                 break;
								case BBGlobalDefinitions.GamePhaseDetail.Flop:
				                    setRoomCustomProperties(CustomPropertiesKeyList.lastPhaseBeforeAlignment,(int)BBGlobalDefinitions.GamePhaseDetail.Flop);
				                    yield return new WaitForSeconds(2);
									_RPCController.shareGamePhaseText("MoneyAlignment");
									StartCoroutine( starExecuteMoneyAlignment() );
				                 break;
								case BBGlobalDefinitions.GamePhaseDetail.Turn:
				                    setRoomCustomProperties(CustomPropertiesKeyList.lastPhaseBeforeAlignment,(int)BBGlobalDefinitions.GamePhaseDetail.Turn);
									_RPCController.shareGamePhaseText("MoneyAlignment");
									StartCoroutine( starExecuteMoneyAlignment() );
				                 break;
								case BBGlobalDefinitions.GamePhaseDetail.River:
				                    setRoomCustomProperties(CustomPropertiesKeyList.lastPhaseBeforeAlignment,(int)BBGlobalDefinitions.GamePhaseDetail.River);
									_RPCController.shareGamePhaseText("MoneyAlignment");
									StartCoroutine( starExecuteMoneyAlignment() );
				                 break;
				                 case BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign:
									BBGlobalDefinitions.GamePhaseDetail phase = ( BBGlobalDefinitions.GamePhaseDetail)getIntRoomProperties(CustomPropertiesKeyList.lastPhaseBeforeAlignment);
									switch(phase) {
									  case BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound:
										_RPCController.shareGamePhaseText("Phase : Flop");
										_photonView.RPC("RPCStartExecuteFlopRound",PhotonTargets.AllViaServer);
									  break;
									  case BBGlobalDefinitions.GamePhaseDetail.Flop:
										_RPCController.shareGamePhaseText("Phase : Turn");
										_photonView.RPC("RPCStartExecuteTurnRound",PhotonTargets.AllViaServer);
									  break;
									  case BBGlobalDefinitions.GamePhaseDetail.Turn:
										_RPCController.shareGamePhaseText("Phase : River");
										_photonView.RPC("RPCStartExecuteRiverRound",PhotonTargets.AllViaServer);
									  break;
									  case BBGlobalDefinitions.GamePhaseDetail.River:
										_RPCController.shareGamePhaseText("Phase : ShowDown");
										_photonView.RPC("RPCStartExecuteShowDown",PhotonTargets.AllViaServer);
									  break;
									}
				                 break;
				                }
				                 
				             } else {
								if(PhotonNetwork.player.IsMasterClient) {
				                    int firstPlayerToTalk = getNextPlayer(underGun);
								    if(_NewMultiplayerHelper.checkForMoneyPossibility(this,firstPlayerToTalk) == false) {
									    StartCoroutine( executeActionOnPlayerResponse("button_outOfGame",firstPlayerToTalk) );
		                             } else {
					                    _RPCController.setActiveTalker(firstPlayerToTalk,true);
					                    yield return new WaitForEndOfFrame();
					                    _RPCController.askPlayerToBet(firstPlayerToTalk);
					                 }
				                }
				             }
				       } // end Not AllIn
		     }
    }

#endregion

	void OnApplicationQuit() {
        Debug.Log("Application ending");
        PhotonNetwork.Disconnect();
    }

    void managePlayerExitDuringWaitForResponse(string playerName) {
		StartCoroutine(executeCheckForRoundDone(_BBGlobalDefinitions.gamePhaseDetail,""));
		setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.waitingForPlayerResponse,"");
    }

    [PunRPC]
    void RPCManageAnomalieOnGameHandMustStop(string _error) {
        Debug.Log("============================================= CRITICAL ANOMALIE HAND STOP ============================== error : " + _error);
		_BBGuiInterface.TextGamePhase.text = "CRITICAL : " + _error;
		_BBGuiInterface.TextGamePhaseInfo.text = "CRITICAL : " + _error;
    }


    [PunRPC]
	void RPCexecutePlayerIsOutCoseNoMoreMoney(int playerIdx) {
			Debug.Log("*************************** RPCexecutePlayerIsOutCoseNoMoreMoney ************************ id : " + playerIdx);
			List<BBPlayerData> pdList = _NewMultiplayerHelper.getActiveInGamePlayersList(playerDataList);;
			BBPlayerData pd = pdList.Find(item => item.playerPosition == playerIdx);
			if(pd != null) {
			   pd.isOutOfGame = true;
			   pd.runOutOfMoney = true;
			   pd.playerGameObject.GetComponent<NewPlayerControllerMultiplayer>().OutOfGameImage.SetActive(true);
			   if(pd.playerName == PhotonNetwork.player.NickName) {
			     StartCoroutine(autoDisconnect());
			   }
			}
	}

    void executePlayerIsOutCoseNoMoreMoney(int playerIdx) {
	  _photonView.RPC("RPCexecutePlayerIsOutCoseNoMoreMoney",PhotonTargets.AllViaServer,playerIdx);
    }

	IEnumerator autoDisconnect() {
	  PanelPlayerDisconnectCoseOutOfMoney.SetActive(true);
	  Text sec = PanelPlayerDisconnectCoseOutOfMoney.transform.Find("TextSeconds").GetComponent<Text>();
	        yield return new WaitForSeconds(1);sec.text = "9";
			yield return new WaitForSeconds(1);sec.text = "8";
			yield return new WaitForSeconds(1);sec.text = "7";
			yield return new WaitForSeconds(1);sec.text = "6";
			yield return new WaitForSeconds(1);sec.text = "5";
			yield return new WaitForSeconds(1);sec.text = "4";
			yield return new WaitForSeconds(1);sec.text = "3";
			yield return new WaitForSeconds(1);sec.text = "2";
			yield return new WaitForSeconds(1);sec.text = "1";
			yield return new WaitForSeconds(1);sec.text = "0";

	   PhotonNetwork.Disconnect();
	}

}
}
#endif