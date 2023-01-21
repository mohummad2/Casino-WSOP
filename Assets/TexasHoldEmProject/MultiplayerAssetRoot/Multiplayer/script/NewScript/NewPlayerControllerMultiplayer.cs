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

  public class NewPlayerControllerMultiplayer : Photon.MonoBehaviour {

		[HideInInspector]
	    public PhotonView _photonView;

	    public NewGameControllerMultiplayer _gameController;

	    public GameObject OutOfGameImage;

	void Awake() {
			_photonView = GetComponent<PhotonView>();
			_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<NewGameControllerMultiplayer>();
	}

	IEnumerator Start () {
			bool isTheGameRoundStarted = false;

			if(PhotonNetwork.room.CustomProperties["IsGameStarted"] != null) {
				string gameStarted = (string)PhotonNetwork.room.CustomProperties["IsGameStarted"];
				if(gameStarted.Contains("YES")) {
				 isTheGameRoundStarted = true;
				}
			}

			Debug.Log("NewPlayerControllerMultiplayer : " + photonView.isMine + " : " + photonView.owner.NickName + " isTheGameRoundStarted :" + isTheGameRoundStarted);

		   if(photonView.isMine) {
				Hashtable  setPlayerCountryCode = new Hashtable() {{"CountryCode", PlayerPrefs.GetString("countryCode")}};
			    PhotonNetwork.player.SetCustomProperties(setPlayerCountryCode);

				Hashtable setPlayerAvatarCode;
				 
				if(BBStaticVariable.myAvatarImageNameIdx == "20") {
					setPlayerAvatarCode = new Hashtable() {{"AvatarCode", PlayerPrefs.GetString("myAvatarImageName")}};
				} else {
					setPlayerAvatarCode = new Hashtable() {{"AvatarCode", BBStaticVariable.myAvatarImageNameIdx}};
				}

				PhotonNetwork.player.SetCustomProperties(setPlayerAvatarCode);

				_gameController.setPlayerCustomProperties(PhotonNetwork.player,NewGameControllerMultiplayer.CustomPropertiesKeyList.currentPlayerTotalMoney,BBStaticVariable.gameLimitedStackValue);


				int playerPosition = _gameController.getFirstPlayerPosition();

				gameObject.transform.SetParent(_gameController.playersContainerOnCanvas[playerPosition]);
		        gameObject.transform.localPosition = new Vector3(0,0,0);
				BBPlayerData pd = new BBPlayerData();
				pd.playerName = photonView.owner.NickName;
				pd.playerPosition = playerPosition;

				if(BBStaticVariable.myAvatarImageNameIdx == "20")
				     pd.playerAvatarImageIdx = PlayerPrefs.GetString("myAvatarImageName"); //BBStaticVariable.myAvatarImageNameIdx;
                else
					pd.playerAvatarImageIdx = BBStaticVariable.myAvatarImageNameIdx;

				pd.playerCountryCode = PlayerPrefs.GetString("countryCode"); 

				gameObject.name = photonView.owner.NickName;
				pd.playerGameObject = gameObject;
				pd.isLocalPlayer = true;
				   if(isTheGameRoundStarted) {
				      pd.isOutOfGame = true;
				      pd.isObserver = true;
					    if(pd.playerName.Contains("[O]")) {
					       gameObject.transform.Find("TextObserver").GetComponent<Text>().text = "OBSERVER";
					    }				  
				    }

				 _gameController.playerDataList[playerPosition] = pd;  
				 Hashtable plaPos = new Hashtable(); 
				 plaPos["playerPos"] = playerPosition;
				 PhotonNetwork.player.SetCustomProperties(plaPos);

				_gameController.setPlayersData(pd,pd.playerGameObject);

		   } else { // NOT Mine

				yield return new WaitForSeconds(1);

				yield return new WaitUntil(() => photonView.owner.CustomProperties["playerPos"] != null);

					   int _plaPos = (int)photonView.owner.CustomProperties["playerPos"];

						gameObject.transform.SetParent(_gameController.playersContainerOnCanvas[_plaPos]);
				        gameObject.transform.localPosition = new Vector3(0,0,0);
						BBPlayerData pd = new BBPlayerData();
						pd.playerPosition = _plaPos;
				        gameObject.name = photonView.owner.NickName;
				        pd.playerName = photonView.owner.NickName;
						pd.playerGameObject = gameObject;
				        pd.playerAvatarImageIdx = (string)photonView.owner.CustomProperties["AvatarCode"];
				        pd.playerCountryCode = (string)photonView.owner.CustomProperties["CountryCode"];

				          if(isTheGameRoundStarted) {
						      //pd.isOutOfGame = true;
						      //pd.isObserver = true;
					          if(pd.playerName.Contains("[O]")) {
					            gameObject.transform.Find("TextObserver").GetComponent<Text>().text = "OBSERVER";
						        pd.isOutOfGame = true;
						        pd.isObserver = true;
					          }
						  }

				        _gameController.playerDataList[_plaPos] = pd;
						_gameController.setPlayersData(pd,pd.playerGameObject);
				        gameObject.transform.Find("YouImage").gameObject.SetActive(false);

		   }

			if(!photonView.owner.IsMasterClient) {
				   //yield return new WaitForSeconds(1);
				   //yield return StartCoroutine(tryToGetCards());
             }

		   yield break;
	}



}
}
#endif