using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BLabTexasHoldEmProject {

public class BBMoneyController : MonoBehaviour {


	BBGameController _BBGameController; 

	// Use this for initialization
	void Awake () {

				_BBGameController = GetComponent<BBGameController>();

				  if(_BBGameController._BBGlobalDefinitions.isAnOpenGame) {
				     for(int x = 0; x < _BBGameController.playerDataList.Count;x++) {
				       _BBGameController.playerDataList[x].currentPlayerTotalMoney = _BBGameController._BBGlobalDefinitions.playersCashDuringOpenGame[x];
				       _BBGameController.playerDataList[x].runOutOfMoney = _BBGameController._BBGlobalDefinitions.playersStateIsOutDuringOpenGame[x];
				     }
							_BBGameController._BBGlobalDefinitions.smallBlindValue = _BBGameController._BBGlobalDefinitions.limitedLow * BBStaticData.cardsHandProgressive;
							_BBGameController._BBGlobalDefinitions.bigBlindValue = _BBGameController._BBGlobalDefinitions.limitedHight * BBStaticData.cardsHandProgressive;
				  } else {
				     _BBGameController._BBGlobalDefinitions.resetInitialPlayerIsOutState();
				     _BBGameController._BBGlobalDefinitions.loadInitialPlayersCashDuringGame();
					 _BBGameController._BBGlobalDefinitions.isAnOpenGame = true;
					 if(_BBGameController._BBGlobalDefinitions.useSimulateMoneyValueForLocalPlayer) {
							_BBGameController._BBGlobalDefinitions.playersCashDuringOpenGame[_BBGameController._BBGlobalDefinitions.localPlayer] = _BBGameController._BBGlobalDefinitions.localPlayerMoneyValueSimulation;
					 }
						_BBGameController._BBGlobalDefinitions.smallBlindValue = _BBGameController._BBGlobalDefinitions.limitedLow;
						_BBGameController._BBGlobalDefinitions.bigBlindValue = _BBGameController._BBGlobalDefinitions.limitedHight;
				  }
				  /*
							Debug.Log("[BBMoneyController] =============================== START =================================================>> isOpenGame : " + 
							_BBGameController._BBGlobalDefinitions.isAnOpenGame +
							" smallBlindValue : " + _BBGameController._BBGlobalDefinitions.smallBlindValue +
							" bigBlindValue : " + _BBGameController._BBGlobalDefinitions.bigBlindValue +
							" cardsHandProgressive : " + BBStaticData.cardsHandProgressive
							);
				  */
	}

	public bool checkForBetPossibility (int playerID, float valueToBet) {

/*		Debug.Log("[BBMoneyController] =============================== checkForBetPossibility =================================================>> player : " 
		+ playerID 
		+  " value : " + valueToBet + 
			" playerMoney : " + _BBGameController.playerDataList[playerID].currentPlayerTotalMoney
		);
*/

		 if(SceneManager.GetActiveScene().name.Contains("Multiplayer")) {
		 } else {
				if(_BBGameController.allInInAction) {
					return true;
				} else {
					if(_BBGameController.playerDataList[playerID].currentPlayerTotalMoney >= valueToBet) {
					  return true;
					}
				}
		}
       return false;
	}

	public float getBetValue (BBGlobalDefinitions.GamePhaseDetail gamePhase) {

		float raiseBetVal = 0;

	   switch(gamePhase) {
	      case BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound: 
			       
			       switch(_BBGameController._BBGlobalDefinitions.roundRaiseCounter) {
				     case 1: raiseBetVal = _BBGameController._BBGlobalDefinitions.smallBlindValue * 2;
				      break; 
			         case 2: raiseBetVal = _BBGameController._BBGlobalDefinitions.smallBlindValue * 3;
				     break; 
			         case 3: raiseBetVal = _BBGameController._BBGlobalDefinitions.smallBlindValue * 4;
				     break;
				     default:
				       raiseBetVal = _BBGameController._BBGlobalDefinitions.smallBlindValue;
				     break;
				    }

	      break;
	   }


	   return raiseBetVal;

	}

	public void setPlayerOutForNoMoreMoney(int playerID, float valueBeingBet) {

			if(SceneManager.GetActiveScene().name.Contains("Multiplayer")) {
			} else {
				 if(_BBGameController.allInInAction) {

				 } else {
					GetComponent<BBGuiInterface>().setPlayerOUTGame(playerID);
					_BBGameController.playerDataList[playerID].isOutOfGame = true;

					_BBGameController.playerDataList[playerID].currentPlayerTotalMoney += valueBeingBet;

					if(playerID == _BBGameController._BBGlobalDefinitions.localPlayer) {
						_BBGameController.UIMoveingController.SetActive(false);
						_BBGameController.PanelLocalPlayerLose.SetActive(true);
					} else {
						_BBGameController._BBGlobalDefinitions.playersStateIsOutDuringOpenGame[playerID] = true;
						_BBGameController.playerDataList[playerID].runOutOfMoney = true;
					}
			   }
		   }
	}

}
}