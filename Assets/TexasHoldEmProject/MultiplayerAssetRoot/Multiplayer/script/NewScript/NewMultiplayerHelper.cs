#if USE_PHOTON
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using Photon;

namespace BLabTexasHoldEmProject {

public class NewMultiplayerHelper : Photon.MonoBehaviour {

  public  bool checkForMoneyPossibility(NewGameControllerMultiplayer BBGC, int playerID) {
      bool tmpRet = false;
      PhotonPlayer pp = getPhotonPlayer(BBGC.playerDataList,playerID);
	  string _key = NewGameControllerMultiplayer.CustomPropertiesKeyList.currentPlayerTotalMoney;
	  float playerDisponibility =  BBGC.getFloatPlayerProperties(pp,_key);
	  float _lastBet = BBGC.getFloatRoomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.lastBet);
      if(playerDisponibility >= _lastBet) {
        tmpRet = true;
      } else {
        tmpRet = false;
      }
     return tmpRet;
 }

 public  bool checkForMoneyPossibility(NewGameControllerMultiplayer BBGC, int playerID,float toBet) {
      bool tmpRet = false;
      PhotonPlayer pp = getPhotonPlayer(BBGC.playerDataList,playerID);
	  string _key = NewGameControllerMultiplayer.CustomPropertiesKeyList.currentPlayerTotalMoney;
	  float playerDisponibility =  BBGC.getFloatPlayerProperties(pp,_key);
      if(playerDisponibility >= toBet) {
        tmpRet = true;
      } else {
        tmpRet = false;
      }
     return tmpRet;
 }


  public float getMaxPlayerBet(List<BBPlayerData> data) {
     float tmpRet = 0;
	 List<BBPlayerData> tmpList = new List<BBPlayerData>();
	 for(int x = 0;x < data.Count;x++) {
	   if(data[x].playerGameObject != null && data[x].isOutOfGame == false) {
	     tmpList.Add(data[x]);
	   }
	 }
	 foreach(BBPlayerData pd in tmpList) {
	   PhotonPlayer pp = pd.playerGameObject.GetComponent<PhotonView>().owner;
				float onTabVal = GetComponent<NewGameControllerMultiplayer>().getFloatPlayerProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.currentMoneyOnTable); 
	   if(onTabVal > tmpRet) tmpRet = onTabVal;
	 }
	 return tmpRet;
  }

  public PhotonPlayer getPhotonPlayer(List<BBPlayerData> data,int playerIdx) {
     PhotonPlayer tempRet = null;
	 List<BBPlayerData> tmpList = new List<BBPlayerData>();
	 for(int x = 0;x < data.Count;x++) {
	   if(data[x].playerGameObject != null) {
	     tmpList.Add(data[x]);
	   }
	 }
	  BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);

	  if(pd != null) tempRet = pd.playerGameObject.GetComponent<PhotonView>().owner;

	  return tempRet;
  }

  public PhotonPlayer getPhotonPlayer(List<BBPlayerData> data,int playerIdx, out string _name,out int _playerPos) {
     _name = "";
     _playerPos = 0;

     PhotonPlayer tempRet = null;
	 List<BBPlayerData> tmpList = new List<BBPlayerData>();
	 for(int x = 0;x < data.Count;x++) {
	   if(data[x].playerGameObject != null) {
	     tmpList.Add(data[x]);
	   }
	 }
	  BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);
	  tempRet = pd.playerGameObject.GetComponent<PhotonView>().owner;
	  _name = pd.playerName;
	  _playerPos = pd.playerPosition;
	  return tempRet;
  }


  public List<BBPlayerData> getActiveInGamePlayersUnderAllInList(List<BBPlayerData> data) {
	 List<BBPlayerData> tmpList = new List<BBPlayerData>();
	 for(int x = 0;x < data.Count;x++) {
	     PhotonPlayer pp = getPhotonPlayer(data,data[x].playerPosition);
	     string underAllIn = GetComponent<NewGameControllerMultiplayer>().getStringPlayerProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.isPlayerUnderAllIn);
		 if(data[x].playerGameObject != null && 
		    data[x].isOutOfGame == false && 
		    data[x].isObserver == false  &&
		    underAllIn == "Y") {
	     tmpList.Add(data[x]);
	   }
	 }
	 return tmpList;
  }

  public List<BBPlayerData> getAllInGamePlayersList(List<BBPlayerData> data) {
	 List<BBPlayerData> tmpList = new List<BBPlayerData>();
	 for(int x = 0;x < data.Count;x++) {
		 if(data[x].playerGameObject != null) {
	     tmpList.Add(data[x]);
	   }
	 }
	 return tmpList;
  }


  public List<BBPlayerData> getActiveInGamePlayersList(List<BBPlayerData> data) {
	 List<BBPlayerData> tmpList = new List<BBPlayerData>();
	 for(int x = 0;x < data.Count;x++) {
		 if(data[x].playerGameObject != null && data[x].isOutOfGame == false && data[x].isObserver == false) {
	     tmpList.Add(data[x]);
	   }
	 }
	 return tmpList;
  }

  public BBPlayerData getMyPlayerData(List<BBPlayerData> data, string _name) {
	  List<BBPlayerData> tmpList = new List<BBPlayerData>();
	  for(int x = 0;x < data.Count;x++) {
		 if(data[x].playerGameObject != null) {
	       tmpList.Add(data[x]);
	      }
	   }
	  BBPlayerData pd = tmpList.Find(item => item.playerName == _name);
	  return pd;
  }

  public BBPlayerData getMyPlayerData(List<BBPlayerData> data, int playerIdx) {
	  List<BBPlayerData> tmpList = new List<BBPlayerData>();
	  for(int x = 0;x < data.Count;x++) {
		 if(data[x].playerGameObject != null) {
	       tmpList.Add(data[x]);
	      }
	   }
	  BBPlayerData pd = tmpList.Find(item => item.playerPosition == playerIdx);
	  return pd;
  }


}
}
#endif