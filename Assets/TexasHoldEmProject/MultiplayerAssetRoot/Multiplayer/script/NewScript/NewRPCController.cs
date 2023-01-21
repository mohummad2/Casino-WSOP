#if USE_PHOTON
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

namespace BLabTexasHoldEmProject {

public class NewRPCController : Photon.MonoBehaviour {

  [HideInInspector]
  public PhotonView _photonView;

  NewBBGuiInterfaceMultiplayer _BBGuiInterface; 
  NewGameControllerMultiplayer _GameController;

	void Awake () {
	  _photonView = GetComponent<PhotonView>();
	  _BBGuiInterface = GetComponent<NewBBGuiInterfaceMultiplayer>();
	  _GameController = GetComponent<NewGameControllerMultiplayer>();
	}

	 [PunRPC]
	 void RPCShareAllInRequest(int playerId) {
	    BBPlayerData pd = _GameController._NewMultiplayerHelper.getMyPlayerData(_GameController.playerDataList,playerId);
	    pd.AllInImage.enabled = true;

		     GameObject allInImage = GameObject.Find("ImageGameAllIn");
		      if(allInImage) {
		       Image img = allInImage.GetComponent<Image>();
		        Color c = img.color;
		        c.a = 1;
		        img.color = c;
	          }

	 }

	 public void shareAllInRequest(int playerID) {
		_photonView.RPC("RPCShareAllInRequest",PhotonTargets.All,playerID);
	 }

	 [PunRPC]
	 void RPCUpdateAllGamePhaseDetail(BBGlobalDefinitions.GamePhaseDetail state) {
		_GameController._BBGlobalDefinitions.gamePhaseDetail = state;
	 }

	 public void updateAllGamePhaseDetail(BBGlobalDefinitions.GamePhaseDetail state) {
		_photonView.RPC("RPCUpdateAllGamePhaseDetail",PhotonTargets.All,state);
	 }

	public void removeCards(string playerName) {_photonView.RPC("RPCRemoveCards",PhotonTargets.AllViaServer,playerName);}

	[PunRPC]
	void RPCRemoveCards(string playerName) {StartCoroutine(executeRemoveCards(playerName));}

	IEnumerator executeRemoveCards(string playerName) {
       BBPlayerData gotPd = new BBPlayerData();
       for(int x = 0; x < _GameController.playerDataList.Count;x++) {
         if(_GameController.playerDataList[x].playerName == playerName) {
           gotPd = _GameController.playerDataList[x];
         }
       }
		   if( gotPd.transform_card_1 == null) yield break;
			yield return StartCoroutine( GetComponent<BBMoveingObjectsController>().moveCard(gotPd.transform_card_1.gameObject, _GameController.cardsDiscardPosition) );
			_GameController.cardsDiscardPosition.position = new Vector3(_GameController.cardsDiscardPosition.position.x, _GameController.cardsDiscardPosition.position.y + 0.01f, _GameController.cardsDiscardPosition.position.z);
			if ( gotPd.transform_card_2 == null) yield break;
			yield return StartCoroutine( GetComponent<BBMoveingObjectsController>().moveCard(gotPd.transform_card_2.gameObject, _GameController.cardsDiscardPosition) );
			_GameController.cardsDiscardPosition.position = new Vector3(_GameController.cardsDiscardPosition.position.x, _GameController.cardsDiscardPosition.position.y + 0.01f, _GameController.cardsDiscardPosition.position.z);
		}

	[PunRPC]
	 void RPCSetAllPlayersAtState(BBGlobalDefinitions.GamePhaseDetail gpd) {
	   foreach(BBPlayerData _pd in _GameController.playerDataList) {
         if(_pd.playerGameObject != null) {
           _pd.gamePhaseDetail = gpd;
		   //_pd.playerGameObject.transform.Find("TextGameState").GetComponent<Text>().text = _pd.gamePhaseDetail.ToString();
         }
	   }
	 }

	 [PunRPC]
	  void RPCSetAllPlayersAtStateDiffState(BBGlobalDefinitions.GamePhaseDetail gpd,int[] toDifferentSet,BBGlobalDefinitions.GamePhaseDetail diffGpd) {
	   foreach(BBPlayerData _pd in _GameController.playerDataList) {
         if(_pd.playerGameObject != null) {
				if(toDifferentSet.Contains(_pd.playerPosition)) {
				  _pd.gamePhaseDetail = diffGpd;
				} else {
                  _pd.gamePhaseDetail = gpd;
		        }
			 // _pd.playerGameObject.transform.Find("TextGameState").GetComponent<Text>().text = _pd.gamePhaseDetail.ToString();
         }
	   }
	 }

	 public void setAllPlayersAtState(BBGlobalDefinitions.GamePhaseDetail gpd) {
		_photonView.RPC("RPCSetAllPlayersAtState",PhotonTargets.AllViaServer,gpd);
	 }

	 public void setAllPlayersAtState(BBGlobalDefinitions.GamePhaseDetail gpd, int[] toDifferentSet,BBGlobalDefinitions.GamePhaseDetail diffGpd) {
		_photonView.RPC("RPCSetAllPlayersAtStateDiffState",PhotonTargets.AllViaServer,gpd, toDifferentSet, diffGpd);
	 }


	 [PunRPC]
	 public void RPGSetSinglePlayerAtState(int playerIdx,BBGlobalDefinitions.GamePhaseDetail gpd) {
		List<BBPlayerData> tmpList = new List<BBPlayerData>();
	    for(int x = 0;x < _GameController.playerDataList.Count;x++) {if(_GameController.playerDataList[x].playerGameObject != null) {tmpList.Add(_GameController.playerDataList[x]);}}
		BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);
		if(pd != null) pd.gamePhaseDetail = gpd;
	 }

	 public void setSinglePlayerAtState(int playerIdx,BBGlobalDefinitions.GamePhaseDetail gpd) {
	    
		_photonView.RPC("RPGSetSinglePlayerAtState",PhotonTargets.All,playerIdx,gpd);
	 }

	 [PunRPC]
     void RPCShareInfomessagePlayerRelated(string preMessage, string postMessage, int playerFocused,bool ShowOnlyInfo) {
		   if(ShowOnlyInfo) {
				//_BBGuiInterface.TextGamePhaseInfo.text = postMessage;
		   } else {
				//_BBGuiInterface.TextGamePhaseInfo.text = "Player : " + _GameController.playerDataList[playerFocused].playerName + " Has To Talk"; 
				_GameController.playerDataList[playerFocused].playerActiveImage.color = Color.green;
		   }
     }

     public void shareInfomessagePlayerRelated(string preMessage, string postMessage, int playerFocused,bool ShowOnlyInfo) {
			_photonView.RPC("RPCShareInfomessagePlayerRelated",PhotonTargets.All,preMessage,postMessage,playerFocused,ShowOnlyInfo);
     }

     [PunRPC]
	 public void RPCShareGamePhaseText(string Message) {
	   //_BBGuiInterface.TextGamePhase.text = Message;
	 }

	 public void shareGamePhaseText(string Message) {
		_photonView.RPC("RPCShareGamePhaseText",PhotonTargets.All,Message);
     }


     [PunRPC]
	  void RPCshareCardsDeck() {
			if(PhotonNetwork.room.CustomProperties["cardsDeck"] != null) {
				string _cards = (string)PhotonNetwork.room.CustomProperties["cardsDeck"];
		        //Debug.Log("_cards----------------------------->> : " + _cards);
		        _GameController.GlobalDeck = BBStaticData.getCardsDeckList(_cards);
			}
	  }

     public void shareCardsDeck() {
		_photonView.RPC("RPCshareCardsDeck",PhotonTargets.Others);
     }

	[PunRPC]
	 void RPCsetActiveDealer(int activeDealerIdx) {
	     _GameController._BBGlobalDefinitions.currentActivedealer = activeDealerIdx;
         _GameController.playerDataList[activeDealerIdx].PlayerDealerImage.SetActive(true);
	 }

     public void setActiveDealer() {

        for(int x = 0; x < _GameController.playerDataList.Count;x++) {
			if(_GameController.playerDataList[x].playerGameObject != null) {
				if(_GameController.playerDataList[x].playerGameObject.GetComponent<PhotonView>().owner.IsMasterClient) {
				   _GameController._BBGlobalDefinitions.currentActivedealer = x;
				   _photonView.RPC("RPCsetActiveDealer",PhotonTargets.AllViaServer,x);
				  break;
				}
			}
        }

     }

     [PunRPC]
	 void RPCSetActiveTalker(int playerIdx, bool waitingForResponce) {
       _GameController._BBGlobalDefinitions.playerToTalk = playerIdx;
       _GameController.waitingResponceFromPlayer = waitingForResponce;
       if(waitingForResponce) {
        _GameController.gotResponceFromPlayer = false;
       } else {
       }
     }

	 public void setActiveTalker(int playerIdx, bool waitingForResponce) {
		_photonView.RPC("RPCSetActiveTalker",PhotonTargets.AllViaServer,playerIdx,waitingForResponce);
     }

     [PunRPC]
	 public void RPCShareGotResponceFromPlayer() {
	   _GameController.gotResponceFromPlayer = true;
     }

     public void shareGotResponceFromPlayer() {
		_photonView.RPC("RPCShareGotResponceFromPlayer",PhotonTargets.All);
     }

     [PunRPC]
	 void RPCsetSmallAndBigBlidPlayer(int activeDealer) {
	    List<BBPlayerData> pdList = new List<BBPlayerData>();

	    foreach(BBPlayerData pd in _GameController.playerDataList) {
	      if(pd.playerGameObject != null) {
	        pdList.Add(pd);
	      }
	    }

/*
	    int tmpActiveDealer = -1;
	    int firstGood = -1;
		int secondGood = -1;
		for(int x = 0;x < pdList.Count;x++) {
		   if(tmpActiveDealer != -1) {
		       if(firstGood != -1) {
		         firstGood = pdList[x].playerPosition;
		       }
		   } else {
		      if(pdList[x].playerPosition == activeDealer) {
		        tmpActiveDealer = activeDealer;
		      }
		   }
		}
*/

        if(pdList.Count > 2) {
           if(activeDealer == 9) {
			      _GameController._BBGlobalDefinitions.smallBlindPlayerId = 0;
			      _GameController._BBGlobalDefinitions.bigBlindPlayerId = 1;
				} else if(activeDealer == 8) {
				  _GameController._BBGlobalDefinitions.smallBlindPlayerId = 9;
			      _GameController._BBGlobalDefinitions.bigBlindPlayerId = 0;
                } else {
					_GameController._BBGlobalDefinitions.smallBlindPlayerId = (activeDealer + 1);
					_GameController._BBGlobalDefinitions.bigBlindPlayerId = (activeDealer + 2);
                }
        } else {
			if(activeDealer == 9) {
			   _GameController._BBGlobalDefinitions.smallBlindPlayerId = 9;
			   _GameController._BBGlobalDefinitions.bigBlindPlayerId = 0;
			} else if(activeDealer == 8) {
			   _GameController._BBGlobalDefinitions.smallBlindPlayerId = 8;
			   _GameController._BBGlobalDefinitions.bigBlindPlayerId = 9;
			} else {
			   _GameController._BBGlobalDefinitions.smallBlindPlayerId = activeDealer;
			   _GameController._BBGlobalDefinitions.bigBlindPlayerId = (activeDealer + 1);
			}
        }

		Debug.Log("***[NewRPCController]***[RPCsetSmallAndBigBlidPlayer]*** small : " + _GameController._BBGlobalDefinitions.smallBlindPlayerId + " Big : " + _GameController._BBGlobalDefinitions.bigBlindPlayerId);

	 }

     public void setSmallAndBigBlidPlayer(int activeDealerIdx) {
		_photonView.RPC("RPCsetSmallAndBigBlidPlayer",PhotonTargets.AllViaServer,activeDealerIdx);
     }

     [PunRPC]
	 void RPCexecuteSmallAndBigBet() {
	   StartCoroutine( _GameController.executeSmallAndBigBlindBet() );
	 }

     public void executeSmallAndBigBet() {
		_photonView.RPC("RPCexecuteSmallAndBigBet",PhotonTargets.AllViaServer);
     }

     [PunRPC]
	 void RPCShareMoneyData(){
	   StartCoroutine(ShareMoneyData());
     }

	 public IEnumerator ShareMoneyData() {
	    yield return new WaitForSeconds(3);
		List<BBPlayerData> tmpList = new List<BBPlayerData>();
	    for(int x = 0;x < _GameController.playerDataList.Count;x++) {if(_GameController.playerDataList[x].playerGameObject != null) {tmpList.Add(_GameController.playerDataList[x]);}}

		for(int x = 0; x < tmpList.Count;x++) {
			PhotonPlayer pp = tmpList[x].playerGameObject.GetComponent<PhotonView>().owner;
			    float tmpMoneyOnTable = _GameController.getFloatPlayerProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.currentMoneyOnTable); 
				float tmpMoneyTotal = _GameController.getFloatPlayerProperties(pp, NewGameControllerMultiplayer.CustomPropertiesKeyList.currentPlayerTotalMoney);
				tmpList[x].T_PlayerMoneyOnTable.text = BBStaticData.getMoneyValue(tmpMoneyOnTable);
				tmpList[x].T_PlayerMoneyTotal.text = BBStaticData.getMoneyValue(tmpMoneyTotal);
				  if(!tmpList[x].playerGameObject.GetComponent<PhotonView>().isMine) {
				     tmpList[x].currentMoneyOnTable = tmpMoneyOnTable;
				     tmpList[x].currentPlayerTotalMoney = tmpMoneyTotal;
				  }

		}
			_BBGuiInterface.TextGamePOT.text = BBStaticData.getMoneyValue( _GameController.getFloatRoomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.moneyOnTable/*"moneyOnTable"*/));

	 }

     [PunRPC]
	 void RPCsetPlayerMoneyValueAndPot(float val, int playerIdx) {
		  List<BBPlayerData> tmpList = new List<BBPlayerData>();
	     for(int x = 0;x < _GameController.playerDataList.Count;x++) {if(_GameController.playerDataList[x].playerGameObject != null) {tmpList.Add(_GameController.playerDataList[x]);}}
		 BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);

			float onTableVal = 0;
			float totalVal = 0;

		  if(pd.playerName == PhotonNetwork.player.NickName) {
				onTableVal = _GameController.playerDataList[playerIdx].currentMoneyOnTable += val;
		        totalVal = _GameController.playerDataList[playerIdx].currentPlayerTotalMoney -= val;
				float potVal = _GameController.getFloatRoomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.moneyOnTable/*"moneyOnTable"*/);
				potVal += val;
				//_GameController._BBGlobalDefinitions.moneyOnTable += val;
				_GameController.setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.moneyOnTable/*"moneyOnTable"*/,potVal);
				_GameController.setPlayerCustomProperties(pd.playerGameObject.GetComponent<PhotonView>().owner, NewGameControllerMultiplayer.CustomPropertiesKeyList.currentMoneyOnTable /*"currentMoneyOnTable"*/,onTableVal);
				_GameController.setPlayerCustomProperties(pd.playerGameObject.GetComponent<PhotonView>().owner, NewGameControllerMultiplayer.CustomPropertiesKeyList.currentPlayerTotalMoney/*"currentPlayerTotalMoney"*/,totalVal);
			//	Debug.Log("***[NewRPCController]***[RPCsetPlayerMoneyValueAndPot]*** playerIdx : " + playerIdx + " : " + val + " : " + _GameController._BBGlobalDefinitions.moneyOnTable);
				_photonView.RPC("RPCShareMoneyData",PhotonTargets.AllViaServer);
		  }
			//Debug.Log(pd.playerName +  " : <<< local +++++ALL+++ ***[NewRPCController]***[RPCsetPlayerMoneyValueAndPot]*** playerIdx : " + playerIdx + " : " + val + " : " + _GameController._BBGlobalDefinitions.moneyOnTable);
	 }

	 public void setPlayerMoneyValueAndPot(float val, int playerIdx) {

		  List<BBPlayerData> tmpList = new List<BBPlayerData>();
	     for(int x = 0;x < _GameController.playerDataList.Count;x++) {if(_GameController.playerDataList[x].playerGameObject != null) {tmpList.Add(_GameController.playerDataList[x]);}}
		 BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);
	    
		 _photonView.RPC("RPCsetPlayerMoneyValueAndPot",pd.playerGameObject.GetComponent<PhotonView>().owner,val,playerIdx);

	 }

	 [PunRPC]
	 void RPCexecuteChipBet(int playerIdx, float value,string betType) {
		StartCoroutine( GetComponent<BBMoveingObjectsController>().moveChip(value,playerIdx,GetComponent<BBMoveingObjectsController>().playersChipEndingPoint[playerIdx]) );
		if(_GameController.playerDataList[playerIdx].T_TextPlayerBetType != null) _GameController.playerDataList[playerIdx].T_TextPlayerBetType.text = betType;
	 }

	 public IEnumerator executeChipBet(int playerIdx, float value,string betType) {
	     if(PhotonNetwork.player.IsMasterClient) {
		   _photonView.RPC("RPCexecuteChipBet",PhotonTargets.AllViaServer,playerIdx,value,betType);

		   yield return new WaitForSeconds(3);

				List<BBPlayerData> tmpList = new List<BBPlayerData>();
	            for(int x = 0;x < _GameController.playerDataList.Count;x++) {if(_GameController.playerDataList[x].playerGameObject != null) {tmpList.Add(_GameController.playerDataList[x]);}}
		        BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);
	    
				_photonView.RPC("RPCsetPlayerMoneyValueAndPot",pd.playerGameObject.GetComponent<PhotonView>().owner,value,playerIdx);
	     }
	 }

	 [PunRPC]
	 void RPCexecuteChipBetToPosition(int playerIdx, float value,string betType,Vector3 _from, Vector3 _to) {

		 Debug.Log("*****************RPCexecuteChipBetToPosition**************************** RPCexecuteChipBetToPosition ID : " + playerIdx);

		 StartCoroutine( GetComponent<BBMoveingObjectsController>().moveChipFromTo(value,_from,_to) );

		 _GameController.playerDataList[playerIdx].T_TextPlayerBetType.text = betType;
	 }

	 public IEnumerator executeChipBetToPosition(int playerIdx, float value,string betType,Vector3 _from, Vector3 _to) {
	     if(PhotonNetwork.player.IsMasterClient) {
		 
		   _photonView.RPC("RPCexecuteChipBetToPosition",PhotonTargets.AllViaServer,playerIdx,value,betType,_from,_to);
	     }
	     yield break;
	 }

	 public IEnumerator executeFold(int playerIdx) {
		  if(PhotonNetwork.player.IsMasterClient) {
			 List<BBPlayerData> tmpList = new List<BBPlayerData>();
	         for(int x = 0;x < _GameController.playerDataList.Count;x++) {if(_GameController.playerDataList[x].playerGameObject != null) {tmpList.Add(_GameController.playerDataList[x]);}}
		     BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);
			 removeCards(pd.playerName);
			 yield return new WaitForSeconds(2);
			 pd.playerActiveImage.color = Color.black;
			 pd.T_TextPlayerBetType.text = "Fold";
			 pd.isOutOfGame = true;
		     _photonView.RPC("RPCshareOutGameState",PhotonTargets.AllViaServer,playerIdx);
          }
	 }

	 [PunRPC]
	 void RPCshareOutGameState(int playerIdx) {
			List<BBPlayerData> tmpList = new List<BBPlayerData>();
	        for(int x = 0;x < _GameController.playerDataList.Count;x++) {if(_GameController.playerDataList[x].playerGameObject != null) {tmpList.Add(_GameController.playerDataList[x]);}}
		    BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);
		    pd.isOutOfGame = true;
		    pd.T_TextPlayerBetType.text = "Fold";
		    pd.playerActiveImage.color = Color.black;
	 }

	 [PunRPC]
	  void RPCgiveFirstCardsToPlayers() {
		StartCoroutine(executeGiveFirstCardsToPlayers());
	  }


     [PunRPC]
	 void RPCSetAllImageState(string s_col) {
	    List<BBPlayerData> pdList = GetComponent<NewMultiplayerHelper>().getActiveInGamePlayersList(_GameController.playerDataList);
	    for(int x = 0;x < pdList.Count;x++) {
	        pdList[x].T_TextPlayerBetType.text = "";
		    switch(s_col) {
				case "R": pdList[x].playerActiveImage.color = Color.red;break;
				case "G": pdList[x].playerActiveImage.color = Color.green;break;
				default : pdList[x].playerActiveImage.color = Color.black;break;
		    }
		}
	 }

     public void setAllImageState(Color col) {
       string _col = "R";
        if(col == Color.red) {
		  _col = "R";
        } else if(col == Color.green){
          _col = "G";
        } else {
		  _col = "B";
        }

		photonView.RPC("RPCSetAllImageState",PhotonTargets.All,_col);
     }

	 IEnumerator executeGiveFirstCardsToPlayers() {

			List<BBPlayerData> pdList = new List<BBPlayerData>();
			foreach(BBPlayerData pd in _GameController.playerDataList) {if(pd.playerGameObject != null) {pdList.Add(pd);}}

			int startingPos = _GameController._BBGlobalDefinitions.currentActivedealer;

			GameObject cartToGive = null;
		    _GameController.shuffledCardsList = _GameController.getGiveCardsList(startingPos);

			//foreach(Transform t in _GameController.shuffledCardsList) Debug.Log("shuffledCardsList --> : " + t.name);

			int tmpCounter = 0;
			for(int x = 0;x < (pdList.Count * 2);x++) {
				     string[] data = _GameController.shuffledCardsList[x].transform.name.Split(new char[] { '_' }); 
			         int posVal = (int.Parse(data[0]));
			          bool wantRotate = true;
				      BBPlayerData pd = pdList.Find(item => item.playerPosition == posVal);
			          if(pd.isLocalPlayer) wantRotate = false;
				      cartToGive = _GameController.getCard(_GameController.GlobalDeck[x],wantRotate);
				   tmpCounter++;

				if(tmpCounter == 1) {
					_GameController.playerDataList[posVal].card_1_Value = _GameController.GlobalDeck[x];
					_GameController.playerDataList[posVal].transform_card_1 = cartToGive.transform;
			       }
			       if(tmpCounter == 2) {
					_GameController.playerDataList[posVal].card_2_Value = _GameController.GlobalDeck[x];
					_GameController.playerDataList[posVal].transform_card_2 = cartToGive.transform;
				      tmpCounter = 0;
			       }

				cartToGive.transform.position = GetComponent<BBMoveingObjectsController>().playersChipStartingPoint[_GameController._BBGlobalDefinitions.currentActivedealer].position;
				GetComponent<BBSoundController>().playClip(GetComponent<BBSoundController>().pickCard);
				yield return new WaitForEndOfFrame();
				yield return StartCoroutine( GetComponent<BBMoveingObjectsController>().moveCard(cartToGive, _GameController.shuffledCardsList[x].transform,posVal) );

			    yield return new WaitForSeconds(0.2f);
				_GameController.lastGivenCardIdx = x;

	        }

	 }

	 public void giveFirstCardsToPlayers() {
		_photonView.RPC("RPCgiveFirstCardsToPlayers",PhotonTargets.AllViaServer);
	 }

	 [PunRPC]
	 void RPCSetGameState(BBGlobalDefinitions.GamePhaseDetail gds ) {
	   _GameController._BBGlobalDefinitions.gamePhaseDetail = gds;
	 }

	 public void setGameState(BBGlobalDefinitions.GamePhaseDetail gds ) {
		 photonView.RPC("RPCSetGameState", PhotonTargets.AllViaServer,gds);
	 }

	 [PunRPC]
	 void RPCAskPlayerToBet(int playerIdx) {

		_GameController.executeInvokeCountDownForBetResponse();

	     List<BBPlayerData> tmpList = new List<BBPlayerData>();
	     for(int x = 0;x < _GameController.playerDataList.Count;x++) {if(_GameController.playerDataList[x].playerGameObject != null) {tmpList.Add(_GameController.playerDataList[x]);}}
		 BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);

		 if(pd != null) {
			 if(pd.playerName == PhotonNetwork.player.NickName) {
			  _BBGuiInterface.activatePlayerToTalk(pd.playerPosition);
			  _GameController.setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.waitingForPlayerResponse/*"waitingForPlayerResponse"*/,pd.playerName);
			 }

			 //_BBGuiInterface.TextGamePhaseInfo.text = "Player : " + pd.playerName + " Has To Talk"; 

			 pd.playerActiveImage.color = Color.green;
	 
			 //Debug.Log("***[NewRPCController]***[RPCAskPlayerToBet]*** playerIdx : " + playerIdx);
		 }
	 }

	 public void askPlayerToBet(int playerIdx) {
		_photonView.RPC("RPCAskPlayerToBet",PhotonTargets.AllViaServer,playerIdx);
	 }

	 [PunRPC]
	 public void RPCShareGotGameActionButton(string buttName, int playerIdx) {
	    _GameController.sharedPlayerActionButton(buttName,playerIdx);
	 }

	 public void shareGotGameActionButton(string buttName, int playerIdx) {
	   _photonView.RPC("RPCShareGotGameActionButton",PhotonTargets.AllViaServer,buttName,playerIdx);
	 }

	 [PunRPC]
	 public void RPCShareLastBetValue(float value) {
	  // _BBGuiInterface.TextGameLastBetvalue.text = "Last Bet : " +  BBStaticData.getMoneyValue(value);
	 }

	 public void shareLastBetValue(float value) {
		_photonView.RPC("RPCShareLastBetValue",PhotonTargets.AllViaServer,value);
	 }

}
}
#endif