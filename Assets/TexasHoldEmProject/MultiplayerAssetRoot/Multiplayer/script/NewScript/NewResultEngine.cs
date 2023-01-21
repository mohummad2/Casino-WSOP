#if USE_PHOTON
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BLabTexasHoldEmProject {

public class NewResultEngine : MonoBehaviour {

/*
	struct FinalResultStruct {
		public BBPlayerCardsResultValueControllerMultiplayer.CardsValues cardsResult;
		public int playerID;
		public int best_1;
		public int best_2;
		public int best_3;
		public int best_4;
		public int best_5;
	}
*/
	public enum CardsResultValues {RoyalFlush,StraightFlush,Poker,FullHouse,Flush,Straight,ThreeOfAkind,TwoPair,Pair,HighCard,Null}
	List<BBPlayerData> winnerList = new List<BBPlayerData>();
		public class CompleteResultStruct {
	 public bool wonHand = false;
	 public Vector2 playerCard_1;
	 public Vector2 playerCard_2;
     public Vector2[] flopCards = new Vector2[3];
     public Vector2 turnCard;
	 public Vector2 riverCard;
	 public int kikers = 0;
	 public int maxCardValue = 0;
	 public int playerIdx;
	 public CardsResultValues cardsResultValues; 
	 /// <summary>
	 /// The full high cards. x = tris value y = pair value
	 /// </summary>
	 public Vector2 FullHighCards = Vector2.zero;
	 /// <summary>
	 /// The flush ordered cards value. Reverse Ordered
	 /// </summary>
	 public List<int> FlushOrderedCardsValue = new List<int>();
	 public List<int> TrisOthersCards = new List<int>();
	 public Vector2 TwoPairHighCards = Vector2.zero;
	 public int TwoPairFifthCard = 0;
	 public List<int> PairOthersBestCards = new List<int>();
	 public int PairValue = 0;
	 public int HighCardValue;
	 public List<int> HighCardOthersBestCards = new List<int>();
	 public List<int> bestFive = new List<int>();
	}

	public GameObject[] playerFinalResultUIList;
	public GameObject finalResultPanel;
	public GameObject PanelLocalPlayerWon;
	public Text finaltext;

		public BBMoveingObjectsController BBMove;
	public void excecuteShowDown() {

			finalResultPanel.SetActive(true);

	foreach(GameObject g in playerFinalResultUIList) g.SetActive(false);

	    NewGameControllerMultiplayer GCM = GetComponent<NewGameControllerMultiplayer>();

		List<BBPlayerData> pdList = null;
		string GT = (string)PhotonNetwork.room.CustomProperties["GameType"];
		if(GT == "A") {
		   if(GCM._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.AllIn) {
			   pdList = GetComponent<NewMultiplayerHelper>().getActiveInGamePlayersUnderAllInList(GCM.playerDataList);
		   } else {
			   pdList = GetComponent<NewMultiplayerHelper>().getActiveInGamePlayersList(GCM.playerDataList);
		   }
		} else {
			pdList = GetComponent<NewMultiplayerHelper>().getActiveInGamePlayersList(GCM.playerDataList);
		}

		/*/ TODO(Useful Debug)
		foreach(BBPlayerData pd in pdList) {

		      PhotonPlayer pp = pd.playerGameObject.GetComponent<PhotonView>().owner;
		      string s_underAllIn = GCM.getStringPlayerProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.isPlayerUnderAllIn);

				Debug.Log("pdList -> excecuteShowDown ========================= GT : " + GT + 
				          " name : " + pd.playerName +
				          " position : " + pd.playerPosition +
				          " is observer : " + pd.isObserver +
						  " s_underAllIn : " + s_underAllIn +
				          " ===========================");
		}
		*/


		for(int x = 0;x < GCM.playerDataList.Count;x++) {
				/* TODO(Useful Debug)
				    Debug.Log("[" + x +"]excecuteShowDown ========================= GT : " + GT + 
					" name : " + GCM.playerDataList[x].playerName +
					" position : " + GCM.playerDataList[x].playerPosition +
					" is observer : " + GCM.playerDataList[x].isObserver +
					" is out : " + GCM.playerDataList[x].isOutOfGame +
					" is llocal : " + GCM.playerDataList[x].isLocalPlayer +
					" PhotonNetwork.player.name : " + PhotonNetwork.player.name +
					" gameObject pd.playerGameObject == null : " + (GCM.playerDataList[x].playerGameObject == null) +
				    " ===========================");
				 */
				if(GCM.playerDataList[x].isLocalPlayer == true) {
					if(GCM.playerDataList[x].isObserver) {
						if(GCM.playerDataList[x].playerName.Contains(PhotonNetwork.player.NickName)) {
						   return;
						}
					}
				}
		}


	  for(int x = 0;x < pdList.Count;x++) {

		//PhotonPlayer pp = GetComponent<NewMultiplayerHelper>().getPhotonPlayer(data,data[x].playerPosition);
	    // string underAllIn = GetComponent<NewGameControllerMultiplayer>().getStringPlayerProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.isPlayerUnderAllIn);


           if(pdList[x].isOutOfGame == true || pdList[x].isObserver == true) {
           } else {
				NewResultEngine.CompleteResultStruct data = new NewResultEngine.CompleteResultStruct();
				NewResultEngine.CompleteResultStruct dataOut = new NewResultEngine.CompleteResultStruct();
				data.playerCard_1 = pdList[x].card_1_Value;
				data.playerCard_2 = pdList[x].card_2_Value;
				data.flopCards[0] = GCM.flopCardList[0];
				data.flopCards[1] = GCM.flopCardList[1];
				data.flopCards[2] = GCM.flopCardList[2];

	            data.turnCard = GCM.turnCard;
				data.riverCard = GCM.riverCard;

	            data.playerIdx = pdList[x].playerPosition;

			    /*TODO(useful debug)
				Debug.Log("playerCard_1 : " + data.playerCard_1 + "  playerCard_2 : " + data.playerCard_2);
				Debug.Log("data.flopCards[0] : " + data.flopCards[0] + "  data.flopCards[1] : " + data.flopCards[1] + "  data.flopCards[2] : " + data.flopCards[2]);
				Debug.Log("data.turnCard : " + data.turnCard + "  data.riverCard : " + data.riverCard);
				Debug.Log("isPlayer Out : " + pdList[x].isOutOfGame + "  isObserver : " + pdList[x].isObserver);
				*/

				getPlayerCardsResult(pdList[x].playerPosition,data,out dataOut);
				pdList[x].completeResultStruct = dataOut;
		   }
	   }

	  int bestResult = 10;
	  for(int x = 0;x < pdList.Count;x++) {
		   if(pdList[x].isOutOfGame == true || pdList[x].isObserver == true) {
           } else {
			     //Debug.Log("++++++++++ Result idx : " + pdList[x].playerPosition);
				 //Debug.Log("++++++++++ Result result : " + pdList[x].completeResultStruct.cardsResultValues);
				 string bestFive = "";
				  foreach(int i in pdList[x].completeResultStruct.bestFive) {
				   bestFive = bestFive + i + " : ";
				  }
				 //Debug.Log(bestFive);
				 //Debug.Log("-----------------------------------------------");
				 if((int)pdList[x].completeResultStruct.cardsResultValues < bestResult) bestResult = (int)pdList[x].completeResultStruct.cardsResultValues;
          }
	  }

			//Debug.Log("bestResult : " + (NewResultEngine.CardsResultValues)bestResult);

			List<BBPlayerData> pdWinnerList = new List<BBPlayerData>();

			for(int x = 0;x < pdList.Count;x++) {
				if(pdList[x].completeResultStruct.cardsResultValues == (NewResultEngine.CardsResultValues)bestResult) {
				  pdWinnerList.Add(pdList[x]);
				}
			}

			//Debug.Log("pdWinnerList Count : " + pdWinnerList.Count);

			if(pdWinnerList.Count < 1) {
			  return;
			}

			for(int x = 0;x < pdWinnerList.Count;x++) {
				//Debug.Log("Winner : " + pdWinnerList[x].playerPosition + " result : " + pdWinnerList[x].completeResultStruct.cardsResultValues + "\n");
			}

			List<BBPlayerData> pdListComplete = null; 
			if(GT == "A") {
				if(GCM._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.AllIn) {
				   pdListComplete = GetComponent<NewMultiplayerHelper>().getActiveInGamePlayersUnderAllInList(GCM.playerDataList);
				} else {
				   pdListComplete = GetComponent<NewMultiplayerHelper>().getActiveInGamePlayersList(GCM.playerDataList);
				}
			} else {
				pdListComplete = GetComponent<NewMultiplayerHelper>().getActiveInGamePlayersList(GCM.playerDataList);
			}

			List<BBPlayerData> _res = new List<BBPlayerData>();

			if(pdWinnerList.Count > 1) {
				
				switch((NewResultEngine.CardsResultValues)bestResult) {
				 case NewResultEngine.CardsResultValues.HighCard:
				    _res = getWinnerCheck_HighCard(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 Debug.Log("FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"); 
					 BBPlayerData _pd = pdListComplete.Find(item => item.playerPosition == pd.playerPosition);
					 _pd.completeResultStruct.wonHand = true;
				   }
				 break;
				 case NewResultEngine.CardsResultValues.Pair:
				   _res = getWinnerCheck_Pair(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 Debug.Log("FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"); 
						BBPlayerData _pd = pdListComplete.Find(item => item.playerPosition == pd.playerPosition);
					    _pd.completeResultStruct.wonHand = true;
				   }
				 break;
				 case NewResultEngine.CardsResultValues.TwoPair:
				   _res = getWinnerCheck_TwoPair(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					  Debug.Log("FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"); 
						BBPlayerData _pd = pdListComplete.Find(item => item.playerPosition == pd.playerPosition);
					    _pd.completeResultStruct.wonHand = true;
				   }
				 break;
				 case NewResultEngine.CardsResultValues.ThreeOfAkind:
				   _res = getWinnerCheck_Tris(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 Debug.Log("FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"); 
						BBPlayerData _pd = pdListComplete.Find(item => item.playerPosition == pd.playerPosition);
					    _pd.completeResultStruct.wonHand = true;
				   }
				 break;
				case NewResultEngine.CardsResultValues.Straight:
				   _res = getWinnerCheck_Straight(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
						Debug.Log("FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n");
						BBPlayerData _pd = pdListComplete.Find(item => item.playerPosition == pd.playerPosition);
					    _pd.completeResultStruct.wonHand = true;
				   }
				 break;
				 case NewResultEngine.CardsResultValues.Flush:
				   _res = getWinnerCheck_Flush(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
						Debug.Log("FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n");
						BBPlayerData _pd = pdListComplete.Find(item => item.playerPosition == pd.playerPosition);
					    _pd.completeResultStruct.wonHand = true;
				   }
				 break;
				 case NewResultEngine.CardsResultValues.FullHouse:
					_res = getWinnerCheck_FullHouse(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
						Debug.Log("FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n");
						BBPlayerData _pd = pdListComplete.Find(item => item.playerPosition == pd.playerPosition);
					    _pd.completeResultStruct.wonHand = true;
				   }
				 break;
				 case NewResultEngine.CardsResultValues.Poker:
					_res = getWinnerCheck_Poker(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
						Debug.Log("FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n");
						BBPlayerData _pd = pdListComplete.Find(item => item.playerPosition == pd.playerPosition);
					    _pd.completeResultStruct.wonHand = true;
				   }
				 break;				
			   }

			} else {
			  pdWinnerList[0].completeResultStruct.wonHand = true;
			  _res = pdWinnerList;
			}


			/* TODO(Useful Debug)
			foreach(BBPlayerData pd in pdListComplete) {
				Debug.Log("pdListComplete ---------------------- start");
				Debug.Log("player idx : " + pd.playerPosition + " bestFive : " + pd.completeResultStruct.bestFive[0] + " : " + pd.completeResultStruct.bestFive[1] + " : " + pd.completeResultStruct.bestFive[2] + " : "+ pd.completeResultStruct.bestFive[3] + " : "+ pd.completeResultStruct.bestFive[4]);
				Debug.Log("player idx : " + pd.playerPosition + " cardsResultValues : " + (NewResultEngine.CardsResultValues)pd.completeResultStruct.cardsResultValues);
				Debug.Log("player idx : " + pd.playerPosition + " *** wonHand *** : " + pd.completeResultStruct.wonHand);
				Debug.Log("player idx : " + pd.playerPosition + " *** isOut *** : " + pd.isOutOfGame);
				Debug.Log("pdListComplete ----------------------end");
			}
			*/


            winnerList = _res;

            executeMoneyUpdate(winnerList);
			StartCoroutine( popolateFinalPanel(pdListComplete) );

	}

	IEnumerator popolateFinalPanel(List<BBPlayerData> data) {

			yield return StartCoroutine(rotateCoveredCards(data));
			for(int x = 0;x < data.Count;x++) {

			  if( (data[x].playerGameObject != null) && (!data[x].isOutOfGame) ) {
					playerFinalResultUIList[x].SetActive(true);

					Sprite sprite = null;
					if(data[x].playerAvatarImageIdx.Length > 5) {
						sprite = BBStaticVariable.getSpriteFromBytes(data[x].playerAvatarImageIdx,true,"0");
					} else {
						sprite = BBStaticVariable.getSpriteFromBytes(data[x].playerAvatarImageIdx,false,data[x].playerAvatarImageIdx);
					}
				//var sprite = Resources.Load<Sprite>("Avatar/playerAvatar_" + currentPlayersDataList[x].playerAvatarImageIdx); //x.ToString());

				Image AvatarImage = playerFinalResultUIList[x].transform.Find("ImageAvatar").GetComponent<Image>();
				AvatarImage.overrideSprite = sprite;
					playerFinalResultUIList[x].transform.Find("TextPlayerPosition").GetComponent<Text>().text = "("+(x+1).ToString()+")";

					if(data[x].completeResultStruct.wonHand) {sprite = Resources.Load<Sprite>("star-won");Image wonLoseImage = playerFinalResultUIList[x].transform.Find("ImageWonLose").GetComponent<Image>();wonLoseImage.overrideSprite = sprite;}
					else if(!data[x].completeResultStruct.wonHand) {  sprite = Resources.Load<Sprite>("star-lose");Image wonLoseImage = playerFinalResultUIList[x].transform.Find("ImageWonLose").GetComponent<Image>();wonLoseImage.overrideSprite = sprite;}

					Texture2D texCountry;
					string _cc = data[x].playerCountryCode;
					if( (_cc.Length == 0) || (_cc == "XX") ) {
					   texCountry = Resources.Load("NULL") as Texture2D;
					} else {
					   texCountry = Resources.Load(_cc) as Texture2D;
					}
					RawImage wonLoseImage2 = playerFinalResultUIList[x].transform.Find("ImageHumanCpu").GetComponent<RawImage>();
			      wonLoseImage2.texture = texCountry;

					playerFinalResultUIList[x].transform.Find("TextPlayerName").GetComponent<Text>().text = data[x].T_playerName.text;

					//string bestFive =  data[x].completeResultStruct.bestFive[0].ToString() + " : " + data[x].completeResultStruct.bestFive[1].ToString() + " : " + data[x].completeResultStruct.bestFive[2].ToString() + " : " + data[x].completeResultStruct.bestFive[3].ToString() + " : " + data[x].completeResultStruct.bestFive[4].ToString();
					playerFinalResultUIList[x].transform.Find("TextResult").GetComponent<Text>().text = getFormattedBestFive(data[x].completeResultStruct.bestFive); //bestFive;
					playerFinalResultUIList[x].transform.Find("TextBestFive").GetComponent<Text>().text = data[x].completeResultStruct.cardsResultValues.ToString();

			       if(data[x].completeResultStruct.wonHand) {
						playerFinalResultUIList[x].transform.Find("TextMoneyResult").GetComponent<Text>().text = BBStaticData.getMoneyValue(data[x].currentMoneyOnTable) + "/" + BBStaticData.getMoneyValue(data[x].currentPlayerTotalMoney);
						playerFinalResultUIList[x].transform.Find("TextMoneyResult").GetComponent<Text>().color = Color.green;
						finaltext.text = ("Dealer: " + data[x].T_playerName.text + " wins " + BBStaticData.getMoneyValue(data[x].currentMoneyOnTable) + " with a " + data[x].completeResultStruct.cardsResultValues.ToString());

				   } else {
						playerFinalResultUIList[x].transform.Find("TextMoneyResult").GetComponent<Text>().text = BBStaticData.getMoneyValue(data[x].currentMoneyOnTable) + "/" + BBStaticData.getMoneyValue(data[x].currentPlayerTotalMoney);
						playerFinalResultUIList[x].transform.Find("TextMoneyResult").GetComponent<Text>().color = Color.red;
			       }

			       if(data[x].completeResultStruct.kikers != 0) {
						playerFinalResultUIList[x].transform.Find("TextBestKicker").GetComponent<Text>().text = getFormattedValue(data[x].completeResultStruct.kikers);
			       }
			}
		}


	}

	void executeMoneyUpdate(List<BBPlayerData> winner) {

	   NewGameControllerMultiplayer BBGC = GetComponent<NewGameControllerMultiplayer>();

	   //BBGC.UIMoveingController.SetActive(true);

	   float totalPot =  BBGC.getFloatRoomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.moneyOnTable); //BBGC._BBGlobalDefinitions.moneyOnTable;
	   float splittedValue = 0;

	   if(winner.Count > 1) {
	     splittedValue = totalPot / winner.Count;
	        for(int x = 0;x < winner.Count;x++) {
				    winner[x].currentPlayerTotalMoney = winner[x].currentPlayerTotalMoney + splittedValue;
					if(winner[x].T_PlayerMoneyTotal != null)  winner[x].T_PlayerMoneyTotal.text = BBStaticData.getMoneyValue(winner[x].currentPlayerTotalMoney);
				    BBStaticVariable.updatePlayerGeneralMoney(true,splittedValue);
					PhotonPlayer pp = GetComponent<NewMultiplayerHelper>().getPhotonPlayer(BBGC.playerDataList,winner[x].playerPosition);
					BBGC.setPlayerCustomProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.currentPlayerTotalMoney,winner[x].currentPlayerTotalMoney);
				    BBGC.setPlayerCustomProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.currentMoneyOnTable,0);
					if (winner[x].isLocalPlayer)
						PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 70);
	        }
	   } else {
				winner[0].currentPlayerTotalMoney = winner[0].currentPlayerTotalMoney + totalPot;
				PhotonPlayer pp = GetComponent<NewMultiplayerHelper>().getPhotonPlayer(BBGC.playerDataList,winner[0].playerPosition);
				BBGC.setPlayerCustomProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.currentPlayerTotalMoney,winner[0].currentPlayerTotalMoney);
				BBGC.setPlayerCustomProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.currentMoneyOnTable,0);

				if(winner[0].T_PlayerMoneyTotal != null) winner[0].T_PlayerMoneyTotal.text = BBStaticData.getMoneyValue(winner[0].currentPlayerTotalMoney);
			    BBStaticVariable.updatePlayerGeneralMoney(true,splittedValue);
	   }


	       BBGC.setRoomCustomProperties(NewGameControllerMultiplayer.CustomPropertiesKeyList.moneyOnTable,0.0f);
	       BBGC._BBGuiInterface.TextGamePOT.text = "0";

			/* TODO(Useful Debug)
			Debug.Log("executeRoundEnd--------> winner.Count : " + winner.Count + 
			          " player pos : " +  winner[0].playerPosition +
				      " won : " +  winner[0].completeResultStruct.wonHand +
				      " pot : " + totalPot +
				      " player tot money : " + winner[0].currentPlayerTotalMoney);
           */
	}

	public void getPlayerCardsResult(int playerId,CompleteResultStruct completeResultStructIN ,out CompleteResultStruct completeResultStructOUT) {
	  
		//Debug.Log("getPlayerCardsResult -> chech_RoyalFlush result : " + check_RoyalFlush(completeResultStructIN));



			completeResultStructOUT = null;
       
        
			if(check_RoyalFlush(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.RoyalFlush;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}
			if(check_StraightFlush(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.StraightFlush;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}
			if(check_Poker(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.Poker;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}
			if(check_Full(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.FullHouse;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}
			if(check_flush(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.Flush;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}
			if(check_straight(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.Straight;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}
			if(check_tris(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.ThreeOfAkind;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}
			if(check_TwoPair(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.TwoPair;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}
			if(check_Pair(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.Pair;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}
			if(check_HighCard(completeResultStructIN)) {completeResultStructIN.cardsResultValues = CardsResultValues.HighCard;completeResultStructIN.playerIdx = playerId;completeResultStructOUT = completeResultStructIN;return;}

	 }


	 bool check_RoyalFlush(CompleteResultStruct data) {
	   bool tmpRet = false;
	   int seme = 0;
	     if(justCheck_Flush(data,out seme) == false) {
	      return tmpRet;
	     }

		 //Debug.Log("chech_RoyalFlush semeValue : " + seme);

	     List<Vector2> allCards = getAllCardsData(data);
	     List<Vector2> flushCards = new List<Vector2>();

	     foreach(Vector2 v in allCards) if(v.x == seme) flushCards.Add(v);

		 int toCheckPoint = 10;
		 Vector2 res = Vector2.zero;
	     for(int x = 0;x < flushCards.Count;x++) {
	       switch(x) {
				case 0: res = flushCards.Find(item => item.y == toCheckPoint);if(res.y == 0) return tmpRet;break;
				case 1:toCheckPoint = 11; res = flushCards.Find(item => item.y == toCheckPoint);if(res.y == 0) return tmpRet;break;
				case 2:toCheckPoint = 12; res = flushCards.Find(item => item.y == toCheckPoint);if(res.y == 0) return tmpRet;break;
				case 3:toCheckPoint = 13; res = flushCards.Find(item => item.y == toCheckPoint);if(res.y == 0) return tmpRet;break;
				case 4:toCheckPoint = 1; res = flushCards.Find(item => item.y == toCheckPoint);if(res.y == 0) return tmpRet;break;
	       }
	     }
	        tmpRet = true;
			data.bestFive.Add(10);data.bestFive.Add(11);data.bestFive.Add(12);data.bestFive.Add(13);data.bestFive.Add(1);
	        data.kikers = getKiker(flushCards,data.playerCard_1,data.playerCard_2);
			//Debug.Log("chech_RoyalFlush toCheckPoint : " + toCheckPoint + " : " + res);
	   return tmpRet;
	 }

	 bool check_StraightFlush(CompleteResultStruct data) {
			bool tmpRet = false;
	        int seme = 0;
	          if(justCheck_Flush(data,out seme) == false) {
				//Debug.Log("check_StraightFlush justCheck_Flush == FALSE ");
	            return tmpRet;
	          }
			//Debug.Log("check_StraightFlush justCheck_Flush == TRUE ");

			 List<Vector2> allCards = getAllCardsData(data);
	         List<Vector2> flushCards = new List<Vector2>();
			 foreach(Vector2 v in allCards) if(v.x == seme) flushCards.Add(v);

			//Debug.Log("check_StraightFlush flushCards count " + flushCards.Count);


			 List<int> flushCardsVal = new List<int>();
			 foreach(Vector2 v in flushCards) flushCardsVal.Add((int)v.y);

			flushCardsVal.Sort();

			Vector2 _res = Vector2.zero;

			if(flushCardsVal.Count > 5) {

				for(int x = 0;x < flushCardsVal.Count;x++) {
				   if((x+1) < flushCardsVal.Count) {
				     if(flushCardsVal[x] == flushCardsVal[x+1]) {
				       flushCardsVal.Remove(flushCardsVal[x]);
				     }
				   }
			    }

				for(int x = 0;x < flushCardsVal.Count;x++) {
					//Debug.Log("------------>> : " + x + " : " + flushCardsVal[x] + " : " + flushCardsVal.Count);
				}

			    if(flushCardsVal.Count > 4) {
					 tmpRet = true;
					 data.bestFive.Add(flushCardsVal[flushCardsVal.Count-5]);
					 data.bestFive.Add(flushCardsVal[flushCardsVal.Count-4]);
					 data.bestFive.Add(flushCardsVal[flushCardsVal.Count-3]);
					 data.bestFive.Add(flushCardsVal[flushCardsVal.Count-2]);
					 data.bestFive.Add(flushCardsVal[flushCardsVal.Count-1]);
					 data.maxCardValue = flushCardsVal[flushCardsVal.Count-1];
			    }

				for(int x = 0;x < data.bestFive.Count;x++) {
					//Debug.Log("======== data.bestFive : " + data.bestFive[x]);
				}
                //    Debug.Log("======== counter : " + counter + " _max : " + _max + " start : " + _startFind);

			} else {
				_res = BBStaticData.newGetSequenceForNumer(flushCardsVal[0],flushCardsVal);
				if(_res.x > 4) {
				     tmpRet = true;
					 data.maxCardValue = flushCardsVal[4];
					 data.bestFive.Add(flushCardsVal[0]);data.bestFive.Add(flushCardsVal[1]);data.bestFive.Add(flushCardsVal[2]);data.bestFive.Add(flushCardsVal[3]);data.bestFive.Add(flushCardsVal[4]);
				}
			}

/*
			 Vector2 _res = Vector2.zero;
			 flushCardsVal.Sort();
			     if(flushCardsVal.Count == 6) {
				           Debug.Log("check_StraightFlush flushCardsVal.Count == 6 ");
				           _res = BBStaticData.newGetSequenceForNumer(flushCardsVal[0],flushCardsVal);
				            Debug.Log("check_StraightFlush _res *** 1 ***: " + _res);
				         if(_res.x > 4) {
					       data.bestFive.Add(flushCardsVal[0]);data.bestFive.Add(flushCardsVal[1]);data.bestFive.Add(flushCardsVal[2]);data.bestFive.Add(flushCardsVal[3]);data.bestFive.Add(flushCardsVal[4]);
				         } else {
					       _res = BBStaticData.newGetSequenceForNumer(flushCardsVal[1],flushCardsVal);
					        if(_res.x > 4) data.bestFive.Add(flushCardsVal[1]);data.bestFive.Add(flushCardsVal[2]);data.bestFive.Add(flushCardsVal[3]);data.bestFive.Add(flushCardsVal[4]);data.bestFive.Add(flushCardsVal[5]);
					        Debug.Log("check_StraightFlush _res *** 2 ***: " + _res);
				         }
			     } else if(flushCardsVal.Count == 7) {
				           Debug.Log("check_StraightFlush flushCardsVal.Count == 7 ");
				           _res = BBStaticData.newGetSequenceForNumer(flushCardsVal[0],flushCardsVal);
				            Debug.Log("check_StraightFlush _res *** 1 ***: " + _res);
				         if(_res.x > 4) {
					       data.bestFive.Add(flushCardsVal[0]);data.bestFive.Add(flushCardsVal[1]);data.bestFive.Add(flushCardsVal[2]);data.bestFive.Add(flushCardsVal[3]);data.bestFive.Add(flushCardsVal[4]);
				         } else {
					        _res = BBStaticData.newGetSequenceForNumer(flushCardsVal[1],flushCardsVal);
					        Debug.Log("check_StraightFlush _res *** 2 ***: " + _res);
					        if(_res.x > 4) {
						      data.bestFive.Add(flushCardsVal[1]);data.bestFive.Add(flushCardsVal[2]);data.bestFive.Add(flushCardsVal[3]);data.bestFive.Add(flushCardsVal[4]);data.bestFive.Add(flushCardsVal[5]);
					        } else {
						      _res = BBStaticData.newGetSequenceForNumer(flushCardsVal[2],flushCardsVal);
					          Debug.Log("check_StraightFlush _res *** 3 ***: " + _res);
						      if(_res.x > 4) data.bestFive.Add(flushCardsVal[2]);data.bestFive.Add(flushCardsVal[3]);data.bestFive.Add(flushCardsVal[4]);data.bestFive.Add(flushCardsVal[5]);data.bestFive.Add(flushCardsVal[6]);
					        }
				         }

			     } else {
			       foreach(int i in flushCardsVal)  Debug.Log("check_StraightFlush flushCardsVal : " + i);
			       _res = BBStaticData.newGetSequenceForNumer(flushCardsVal[0],flushCardsVal);
			       Debug.Log("check_StraightFlush _res : " + _res);
				   if(_res.x > 4) data.bestFive.Add(flushCardsVal[0]);data.bestFive.Add(flushCardsVal[1]);data.bestFive.Add(flushCardsVal[2]);data.bestFive.Add(flushCardsVal[3]);data.bestFive.Add(flushCardsVal[4]);
			     }


			 if(_res.x > 4) {
			    tmpRet = true;
				data.maxCardValue = flushCardsVal[flushCardsVal.Count-1];
                data.kikers = getKiker(flushCards,data.playerCard_1,data.playerCard_2);
			 }
*/
			  
		return tmpRet;

	 }

	 bool check_Poker(CompleteResultStruct data) {
     bool tmpret = false;

			List<Vector2> allCards = getAllCardsData(data);

			for(int i = 0; i < allCards.Count;i++) {
				if(allCards[i].y == 1) {
				   allCards[i] = new Vector2(allCards[i].x,14);
				}
			}

			int counter = 0;
	        bool gotPoker = false;
	        int maxCardValue = 0;
	        List<Vector2> pokerList = new List<Vector2>();

			for(int x = 0;x < allCards.Count;x++) {
			    pokerList.Clear();
				  for(int y = 0; y < allCards.Count;y++) {
	               if(allCards[x].y == allCards[y].y) {
	                  counter++;
						pokerList.Add(allCards[y]);
	                    if(counter == 4) {
	                       gotPoker = true;
	                       maxCardValue = (int)allCards[y].y;
	                       break;
	                    }
	               }         
	          }
	          if(gotPoker) {
	            break;
	          }
	          counter = 0;
			}

          List<int> othersCard = new List<int>();
          foreach(Vector2 v in allCards) {
            if(v.y != (float)maxCardValue) {
              othersCard.Add((int)v.y);
            }
          }

          othersCard.Sort();
          othersCard.Reverse();

          if(gotPoker) {
             tmpret = true;
             data.maxCardValue = maxCardValue;
             data.kikers = getKiker(pokerList,data.playerCard_1,data.playerCard_2);
             //Debug.Log("+++++++++++++++++++poker : " + maxCardValue + " : " + othersCard[0]);

				data.bestFive.Add(maxCardValue);data.bestFive.Add(maxCardValue);data.bestFive.Add(maxCardValue);data.bestFive.Add(maxCardValue);data.bestFive.Add(othersCard[0]);
          }

          return tmpret;
	 }

	 bool check_Full(CompleteResultStruct data) {
			List<Vector2> allCards = getAllCardsData(data);
			List<Vector2> allCardsToRemove = allCards;
			bool tmpRet = false;

			int counter = 0;
	        bool gotTris = false;
			bool gotPair = false;
			int maxCardValueTris = 0;
			int maxCardValuePair = 0;

			for(int x = 0;x < allCards.Count;x++) {
				  for(int y = 0; y < allCards.Count;y++) {
	               if(allCards[x].y == allCards[y].y) {
	                  counter++;
	                    if(counter == 3) {
	                        gotTris = true;
							maxCardValueTris = (int)allCards[y].y;
	                        break;
	                    }
	               }         
	          }
	          if(gotTris) {
	            break;
	          }
	          counter = 0;
			}

			if(gotTris) {
	                for(int x = 0; x < allCardsToRemove.Count;x++) {
					   if(allCardsToRemove[x].y == maxCardValueTris) {
						allCardsToRemove.Remove(allCardsToRemove[x]);
	                   }
	                }

				      for(int x = 0;x < allCardsToRemove.Count;x++) {
					       for(int y = 0; y < allCardsToRemove.Count;y++) {
						     if(allCardsToRemove[x].y == allCardsToRemove[y].y) {
			                   counter++;
			                    if(counter == 2) {
			                       gotPair = true;
								   maxCardValuePair = (int)allCardsToRemove[y].y;
			                       break;
			                    }
			                  }         
			               }
			               if(gotPair) {
			                break;
			               }
			             counter = 0;
					  }                

					  if(gotPair) {
					     tmpRet = true;
					     data.FullHighCards.x = maxCardValueTris;
					     data.FullHighCards.y = maxCardValuePair;
					     data.bestFive.Add(maxCardValueTris);data.bestFive.Add(maxCardValueTris);data.bestFive.Add(maxCardValueTris);data.bestFive.Add(maxCardValuePair);data.bestFive.Add(maxCardValuePair);
					  }

			} else {
			  tmpRet = false;
			}

			return tmpRet;

	 }

	 bool check_flush(CompleteResultStruct data) {
			bool tmpRet = false;
	        int seme;
	        if(justCheck_Flush(data,out seme)) {
				List<Vector2> allCards = getAllCardsData(data);
				List<int> flushCardsInt = new List<int>();
				foreach(Vector2 v in allCards) {
				  if(v.x == seme) {
				    flushCardsInt.Add((int)v.y);
				  }
				}

				for(int x = 0;x < flushCardsInt.Count;x++) {
				  if(flushCardsInt[x] == 1) flushCardsInt[x] = 14; 
				}

				flushCardsInt.Sort();
				flushCardsInt.Reverse();
				data.maxCardValue = flushCardsInt[0];
				data.FlushOrderedCardsValue = flushCardsInt;
				tmpRet = true;
				data.bestFive.Clear();
				for(int x = 0;x < 5;x++) {
				  data.bestFive.Add(flushCardsInt[x]);
				}
	        } 
	        return tmpRet;
	 }

	 bool check_straight(CompleteResultStruct data) {
			bool tmpRet = false;
			List<Vector2> allCards = getAllCardsData(data);
			List<int> allCardsInt = new List<int>();
			foreach(Vector2 v in allCards) {allCardsInt.Add((int)v.y);}
			Vector2 _res = Vector2.zero;
			List<int> tmpListOfFiveSequence = new List<int>();
			foreach(int i in allCardsInt) {
				_res = BBStaticData.newGetSequenceForNumer(i,allCardsInt);
				//Debug.Log("check_flush -> : " + _res);
			  if(_res.x >= 5) {
			    tmpListOfFiveSequence.Add((int)_res.y);
			  }
			}

			//foreach(int v in tmpListOfFiveSequence) Debug.Log("check_flush tmpListOfFiveSequence -> : " + v);

			if(tmpListOfFiveSequence.Count > 0) {
			   tmpListOfFiveSequence.Sort();
			   tmpListOfFiveSequence.Reverse();
			   data.maxCardValue = tmpListOfFiveSequence[0];
			   data.bestFive.Add(tmpListOfFiveSequence[0]);data.bestFive.Add(tmpListOfFiveSequence[0]-1);data.bestFive.Add(tmpListOfFiveSequence[0]-2);data.bestFive.Add(tmpListOfFiveSequence[0]-3);data.bestFive.Add(tmpListOfFiveSequence[0]-4);
			   tmpRet = true;
			}

			return tmpRet;
	 }

	 bool check_tris(CompleteResultStruct data) {
			bool tmpRet = false;
			List<Vector2> allCards = getAllCardsData(data);

			for(int i = 0; i < allCards.Count;i++) {
				if(allCards[i].y == 1) {
				   allCards[i] = new Vector2(allCards[i].x,14);
				}
			}

			int counter = 0;
	       // bool gotTris = false;
			//int maxCardValueTris = 0;
			List<int> tmpListOfTris = new List<int>();

			 for(int x = 0;x < allCards.Count;x++) {
				  for(int y = 0; y < allCards.Count;y++) {
	               if(allCards[x].y == allCards[y].y) {
	                  counter++;
	                    if(counter == 3) {
						  tmpListOfTris.Add((int)allCards[y].y);
						  counter = 0;
						  continue;	                        
 	                    }
	               }         
	          }
	          counter = 0;
			}

			if(tmpListOfTris.Count > 0) {
			  tmpListOfTris.Sort();
			  tmpListOfTris.Reverse();
			  data.maxCardValue = tmpListOfTris[0];

			    List<int> otherList = new List<int>();
				for(int x = 0;x < allCards.Count;x++) {
					if((int)allCards[x].y != tmpListOfTris[0]) {
						otherList.Add((int)allCards[x].y);
					}
				}
			 otherList.Sort();
             otherList.Reverse();
             data.TrisOthersCards = otherList;
             tmpRet = true;
			 data.bestFive.Add(tmpListOfTris[0]);data.bestFive.Add(tmpListOfTris[1]);data.bestFive.Add(tmpListOfTris[2]);data.bestFive.Add(otherList[0]);data.bestFive.Add(otherList[1]);
			}

			return tmpRet;


	 }

	 bool check_TwoPair(CompleteResultStruct data) {
			bool tmpRet = false;
			List<Vector2> allCards = getAllCardsData(data);

			for(int i = 0; i < allCards.Count;i++) {
				if(allCards[i].y == 1) {
				   allCards[i] = new Vector2(allCards[i].x,14);
				}
			}

			int counter = 0;
			List<Vector2> tmpListOfPair = new List<Vector2>();

			for(int x = 0;x < allCards.Count;x++) {
				  for(int y = 0; y < allCards.Count;y++) {
	               if(allCards[x].y == allCards[y].y) {
	                  counter++;
	                    if(counter == 2) {
							Vector2 _res = tmpListOfPair.Find(item => item == allCards[y]);
						    if(_res == Vector2.zero) tmpListOfPair.Add(allCards[y]);
						  counter = 0;
						  continue;	                        
 	                    }
	               }         
	          }
	          counter = 0;
			}

			  //foreach(Vector2 v in tmpListOfPair)
			  //Debug.Log(data.playerIdx + " : ***** 1 *** TwoPairHighCards -> tmpListOfPair : " + v);

			 if(tmpListOfPair.Count > 1) {
				List<int> bestTwoPair = new List<int>();
				for(int x = 0;x < tmpListOfPair.Count;x++) {
					//Debug.Log("tmpListOfPair -> : " + tmpListOfPair[x]);
					bestTwoPair.Add((int)tmpListOfPair[x].y);
				}
				bestTwoPair.Sort();
				bestTwoPair.Reverse();
				data.TwoPairHighCards = new Vector2((float)bestTwoPair[0],(float)bestTwoPair[1]);
				//Debug.Log("******** TwoPairHighCards -> : " + data.TwoPairHighCards);
				data.maxCardValue = bestTwoPair[0];
				List<Vector2> listForKiker = new List<Vector2>();
				for(int x = 0;x < allCards.Count;x++) {
					if( allCards[x].y == (float)bestTwoPair[0] || allCards[x].y == (float)bestTwoPair[1] ) {
					  listForKiker.Add(allCards[x]);
					}
				}
				data.kikers = getKiker(listForKiker,data.playerCard_1,data.playerCard_2);
				List<int> otherCardList = new List<int>();
				for(int x = 0;x < allCards.Count;x++) {
					if( allCards[x].y == (float)bestTwoPair[0] || allCards[x].y == (float)bestTwoPair[1] ) {
					} else {
						otherCardList.Add((int)allCards[x].y);
					}
				}
				otherCardList.Sort();
				otherCardList.Reverse();
				data.TwoPairFifthCard = otherCardList[0];
				data.bestFive.Add((int)data.TwoPairHighCards.x);data.bestFive.Add((int)data.TwoPairHighCards.x);data.bestFive.Add((int)data.TwoPairHighCards.y);data.bestFive.Add((int)data.TwoPairHighCards.y);data.bestFive.Add(otherCardList[0]);
				tmpRet = true;
		    }

         return tmpRet;
	 }

	 bool check_Pair(CompleteResultStruct data) {

			bool tmpRet = false;
			List<Vector2> allCards = getAllCardsData(data);

			for(int i = 0; i < allCards.Count;i++) {
				if(allCards[i].y == 1) {
				   allCards[i] = new Vector2(allCards[i].x,14);
				}
			}

			int counter = 0;
			List<int> otherCards = new List<int>();
			List<Vector2> forKiker = new List<Vector2>();

			int pairCard = 0;

			for(int x = 0;x < allCards.Count;x++) {
				  for(int y = 0; y < allCards.Count;y++) {
	               if(allCards[x].y == allCards[y].y) {
	                  counter++;
	                    if(counter == 2) {
						  counter = 0;
						  pairCard = (int)allCards[y].y;
						  break;
 	                    }
	               }         
	          }
	          counter = 0;
			}

			if(pairCard != 0) {
				for(int x = 0;x < allCards.Count;x++) {
				  if(allCards[x].y != (float)pairCard) {
						int _res = otherCards.Find(item => item == (int)allCards[x].y);
						//Debug.Log("otherCards _res : " + _res + " allCards[x] : " + allCards[x]);
						if(_res == 0) {
				           otherCards.Add((int)allCards[x].y);
				        }
				  } else {
				    forKiker.Add(allCards[x]);
				  }
				}

				data.PairValue = pairCard;
				data.kikers = getKiker(forKiker,data.playerCard_1,data.playerCard_2);
				otherCards.Sort();
                otherCards.Reverse();
                data.PairOthersBestCards = otherCards;
				data.bestFive.Add(pairCard);data.bestFive.Add(pairCard);data.bestFive.Add(otherCards[0]);data.bestFive.Add(otherCards[1]);data.bestFive.Add(otherCards[2]);
                tmpRet = true;
			}



			return tmpRet;
	 }

	 bool check_HighCard(CompleteResultStruct data) {
			List<Vector2> allCards = getAllCardsData(data);
			List<int> allCardsInt = new List<int>();
			List<Vector2> forKiker = new List<Vector2>();
			for(int x = 0;x < allCards.Count;x++) {
			   allCardsInt.Add((int)allCards[x].y);
			}

			for(int i = 0; i < allCardsInt.Count;i++) {
			  if(allCardsInt[i] == 1) allCardsInt[i] = 14;
			}

            allCardsInt.Sort();
            allCardsInt.Reverse();

			for(int i = 0; i < allCardsInt.Count;i++) {
				//Debug.Log("check_HighCard---------------->> : " + allCardsInt[i]);
			}

            data.HighCardValue = allCardsInt[0];
            data.HighCardOthersBestCards = allCardsInt;

			Vector2 _res = allCards.Find(item => item.y == (float)data.HighCardValue);
            forKiker.Add(_res);

            data.kikers = getKiker(forKiker,data.playerCard_1,data.playerCard_2); 
			data.bestFive.Add(allCardsInt[0]);data.bestFive.Add(allCardsInt[1]);data.bestFive.Add(allCardsInt[2]);data.bestFive.Add(allCardsInt[3]);data.bestFive.Add(allCardsInt[4]);
            return true;

	 }

	 bool justCheck_Flush(CompleteResultStruct data, out int seme) {
	     seme = 0;
	     bool tmpRet = false;

	     List<Vector2> allCards = getAllCardsData(data);
	     int semeCounter = 0;
	     int semeValue = 0;
	      for(int s = 1;s < 5;s++) {
		     for(int x = 0;x < allCards.Count;x++) {
					//Debug.Log("chech_RoyalFlush : " + allCards[x] + " : " + s);
		        if(allCards[x].x == s) {
		           semeCounter++;
		             if(semeCounter == 5) {
		                semeValue = s;
		                break;
		             }
		        }    
		     }
		     semeCounter = 0;
		  }

		  //Debug.Log("check_Flush : " + semeValue);

		  if(semeValue != 0) {
		     seme = semeValue;
		    // data.kikers = getKiker(
		     tmpRet = true;
		  }

		  return tmpRet;
	 }

	 List<Vector2> getAllCardsData(CompleteResultStruct data) {
		 List<Vector2> allCards = new List<Vector2>();
	     foreach(Vector2 c in data.flopCards) {allCards.Add(c);}
	     if(data.turnCard != Vector2.zero) allCards.Add(data.turnCard);
		 if(data.riverCard != Vector2.zero) allCards.Add(data.riverCard);
         allCards.Add(data.playerCard_1);
		 allCards.Add(data.playerCard_2);
		 return allCards;
	}

	 int getKiker(List<Vector2> bestFive,Vector2 playerCard_1,Vector2 playerCard_2) {

	 // foreach(Vector2 i in bestFive)  Debug.Log("getKiker bestFive : " + i);
	 // Debug.Log("getKiker playerCard_1 : " + playerCard_1);
	 // Debug.Log("getKiker playerCard_2 : " + playerCard_2);

	  List<Vector2> listWithOutPlayerCards = new List<Vector2>();
      int tmpRet = -1;

			Vector2 _res =  bestFive.Find(item => item == playerCard_1);
			if(_res == Vector2.zero) listWithOutPlayerCards.Add(playerCard_1);
			_res =  bestFive.Find(item => item == playerCard_2);
			if(_res == Vector2.zero) listWithOutPlayerCards.Add(playerCard_2);

			 if(listWithOutPlayerCards.Count > 0) {
			    List<int> _l = new List<int>();
			      foreach(Vector2 v in listWithOutPlayerCards) {
			        _l.Add((int)v.y);
			      }
                _l.Sort();
			    _l.Reverse();
				tmpRet = _l[0];
			 } else {

			 }

        return tmpRet;
	}


	public List<BBPlayerData> getWinnerCheck_HighCard(List<BBPlayerData> pdList) {
        List<BBPlayerData> tmpRet;
		List<int> tmpRetToRemove = new List<int>();

			int highCard = 0; // check list 0
			 for(int x = 0;x < pdList.Count;x++) {
					//Debug.Log("getWinnerCheck_HighCard start -> : " + x + " : " + "pdList[x].completeResultStruct.HighCardOthersBestCards[0] : " + pdList[x].completeResultStruct.HighCardOthersBestCards[0]);
					if(pdList[x].completeResultStruct.HighCardOthersBestCards[0] > highCard) {
					  highCard = pdList[x].completeResultStruct.HighCardOthersBestCards[0];
					}
             }

			//Debug.Log("getWinnerCheck_HighCard highCard : " + highCard); 

			for(int x = 0;x < pdList.Count;x++) {
				if(pdList[x].completeResultStruct.HighCardOthersBestCards[0] < highCard) {
					//Debug.Log("getWinnerCheck_HighCard REMOVE : " + pdList[x].playerPosition + " : " + pdList[x].completeResultStruct.HighCardOthersBestCards[0]); 
					tmpRetToRemove.Add(pdList[x].playerPosition);
				}
			}

			for(int x = 0;x < tmpRetToRemove.Count;x++) {
				BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);
				pdList.Remove(pd);
			}

			highCard = 0; // check list 1
			if(pdList.Count > 1) {
			  tmpRetToRemove.Clear();
			  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.HighCardOthersBestCards[1] > highCard) {highCard = pdList[x].completeResultStruct.HighCardOthersBestCards[1];}}
			  //Debug.Log("getWinnerCheck_HighCard highCard [1] : " + highCard); 
			  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.HighCardOthersBestCards[1] < highCard) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
			  for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}
			}

			highCard = 0; // check list 2
			if(pdList.Count > 1) {
			  tmpRetToRemove.Clear();
			  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.HighCardOthersBestCards[2] > highCard) {highCard = pdList[x].completeResultStruct.HighCardOthersBestCards[2];}}
			  //Debug.Log("getWinnerCheck_HighCard highCard [2] : " + highCard); 
			  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.HighCardOthersBestCards[2] < highCard) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
			  for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}
			}

			highCard = 0; // check list 3
			if(pdList.Count > 1) {
			  tmpRetToRemove.Clear();
			  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.HighCardOthersBestCards[3] > highCard) {highCard = pdList[x].completeResultStruct.HighCardOthersBestCards[3];}}
			  //Debug.Log("getWinnerCheck_HighCard highCard [3] : " + highCard); 
			  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.HighCardOthersBestCards[3] < highCard) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
			  for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}
			}

			highCard = 0; // check list 4
			if(pdList.Count > 1) {
			  tmpRetToRemove.Clear();
			  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.HighCardOthersBestCards[4] > highCard) {highCard = pdList[x].completeResultStruct.HighCardOthersBestCards[4];}}
			  //Debug.Log("getWinnerCheck_HighCard highCard [3] : " + highCard); 
			  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.HighCardOthersBestCards[4] < highCard) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
			  for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}
			}


			//Debug.Log("getWinnerCheck_HighCard tmpRetToRemove count : " + pdList.Count); 

        tmpRet = pdList;
        return tmpRet;

	}

	public List<BBPlayerData> getWinnerCheck_Pair(List<BBPlayerData> pdList) {
	   List<int> tmpRetToRemove = new List<int>();

	    int maxPairValue = 0;
		  for(int x = 0;x < pdList.Count;x++) {
				if(pdList[x].completeResultStruct.PairValue > maxPairValue) { maxPairValue = pdList[x].completeResultStruct.PairValue;}
		  }
		  //Debug.Log("getWinnerCheck_Pair start -> *** maxPairValue *** : " + maxPairValue);
		  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.PairValue < maxPairValue) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
		  for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}

			if(pdList.Count > 1) { 

				  tmpRetToRemove.Clear();
			      int maxOthersCardValue = 0;
				  for(int x = 0;x < pdList.Count;x++) {

						//Debug.Log("--------------->>>getWinnerCheck_Pair start -> pdList PairOthersBestCards : " + pdList[x].completeResultStruct.PairOthersBestCards[x]);
						 
					if(pdList[x].completeResultStruct.PairOthersBestCards[0] > maxOthersCardValue) { maxOthersCardValue = pdList[x].completeResultStruct.PairOthersBestCards[0];}
		          }
					//Debug.Log("getWinnerCheck_Pair start -> maxPairValue [0]: " + maxOthersCardValue);
					for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.PairOthersBestCards[0] < maxOthersCardValue) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
					for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}

					maxOthersCardValue = 0; // check 1
					if(pdList.Count > 1) { 
						tmpRetToRemove.Clear();
						for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.PairOthersBestCards[1] > maxOthersCardValue) { maxOthersCardValue = pdList[x].completeResultStruct.PairOthersBestCards[1];}}
						//Debug.Log("getWinnerCheck_Pair start -> maxPairValue [1]: " + maxOthersCardValue);
						for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.PairOthersBestCards[1] < maxOthersCardValue) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
						for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}
					}

					maxOthersCardValue = 0; // check 2 last
					if(pdList.Count > 1) { 
						tmpRetToRemove.Clear();
						for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.PairOthersBestCards[2] > maxOthersCardValue) { maxOthersCardValue = pdList[x].completeResultStruct.PairOthersBestCards[2];}}
						//Debug.Log("getWinnerCheck_Pair start -> maxPairValue [2]: " + maxOthersCardValue);
						for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.PairOthersBestCards[2] < maxOthersCardValue) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
						for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}
					}
		   }


       return pdList;
	}
		
    public List<BBPlayerData> getWinnerCheck_TwoPair(List<BBPlayerData> pdList) {
	   List<int> tmpRetToRemove = new List<int>();

	   float highCardPair_x = 0;
	   //int kiker = 0;
		  for(int x = 0;x < pdList.Count;x++) {
				//Debug.Log("kiker : " + pdList[x].playerPosition + " kiker : " + pdList[x].completeResultStruct.kikers);
			    if(pdList[x].completeResultStruct.TwoPairHighCards.x > highCardPair_x) { highCardPair_x = pdList[x].completeResultStruct.TwoPairHighCards.x;}
          }
			//Debug.Log("getWinnerCheck_TwoPair start -> highCardPair_x : " + highCardPair_x);

		  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.TwoPairHighCards.x < highCardPair_x) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
		  for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}

		  if(pdList.Count > 1) {
		        // going to check second pair
				tmpRetToRemove.Clear();
				//Debug.Log("getWinnerCheck_TwoPair start -> check 1 pdList.Count : " + pdList.Count);
				 float highCardPair_y = 0;
				 for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.TwoPairHighCards.y > highCardPair_y) { highCardPair_y = pdList[x].completeResultStruct.TwoPairHighCards.y;}}
			     //Debug.Log("getWinnerCheck_TwoPair start -> highCardPair_Y : " + highCardPair_y);
				 for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.TwoPairHighCards.y < highCardPair_y) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
		         for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}

				if(pdList.Count > 1) {
					//Debug.Log("getWinnerCheck_TwoPair start -> check 2 Y > 1 pdList.Count : " + pdList.Count);
					// check fifth card and kiker
					tmpRetToRemove.Clear();
					float fifithCard = 0;
					for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.TwoPairFifthCard > fifithCard) { fifithCard = pdList[x].completeResultStruct.TwoPairFifthCard;}}
					//Debug.Log("getWinnerCheck_TwoPair *** fifithCard *** : " + fifithCard);
					for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.TwoPairFifthCard < fifithCard) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
					for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}

					   if(pdList.Count > 1) {
						   //Debug.Log("getWinnerCheck_TwoPair start -> check ***fifithCard*** pdList.Count : " + pdList.Count);
					   } else {

					   }
				} else {
					//Debug.Log("getWinnerCheck_TwoPair start -> check 2 Y == 1 pdList.Count : " + pdList.Count);
				}
		  } else {
				//Debug.Log("getWinnerCheck_TwoPair start -> check 1 pdList.Count : " + pdList.Count);
		  }


      return pdList;
  }

	public List<BBPlayerData> getWinnerCheck_Tris(List<BBPlayerData> pdList) {
		    List<int> tmpRetToRemove = new List<int>();
			float highCardTris = 0; //completeResultStruct.maxCardValue
	        //int kiker = 0;

		for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.maxCardValue > highCardTris) { highCardTris = pdList[x].completeResultStruct.maxCardValue;}}
			//Debug.Log("****************getWinnerCheck_Tris start -> highCardTris : " + highCardTris);

		    for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.maxCardValue < highCardTris) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
		    for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}
			//Debug.Log("****************getWinnerCheck_Tris start -> pdList.Count : " + pdList.Count);

			if(pdList.Count > 1) { // check fourth card
			  int bestOthers = 0;
			  tmpRetToRemove.Clear();
				for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.TrisOthersCards[0] > bestOthers) { bestOthers = pdList[x].completeResultStruct.TrisOthersCards[0];}}
				//Debug.Log("check fourth card ****************getWinnerCheck_Tris start -> bestOthers : " + bestOthers);
				for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.TrisOthersCards[0] < bestOthers) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
				for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);} 

				   if(pdList.Count > 1) { // check fifth card
					  bestOthers = 0;
					  tmpRetToRemove.Clear();
					  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.TrisOthersCards[1] > bestOthers) { bestOthers = pdList[x].completeResultStruct.TrisOthersCards[1];}}
					  //Debug.Log("check fifth card ****************getWinnerCheck_Tris start -> bestOthers : " + bestOthers);
					  for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.TrisOthersCards[1] < bestOthers) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
				      for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);} 
				   }

			}

       return pdList;
	}	

	public List<BBPlayerData> getWinnerCheck_Straight(List<BBPlayerData> pdList) {	
			List<int> tmpRetToRemove = new List<int>();

			float highCard = 0;

			for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.bestFive[0] > highCard) { highCard = pdList[x].completeResultStruct.bestFive[0];}}
			//Debug.Log("****************getWinnerCheck_straight start -> highCard : " + highCard);


			for(int y = 0;y < pdList.Count-1;y++) {
				highCard = 0;
			    for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.bestFive[y] > highCard) { highCard = pdList[x].completeResultStruct.bestFive[y];}}

				for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.bestFive[y] < highCard) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
				for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}
				//Debug.Log("****************getWinnerCheck_Straight ******  -> pdList.Count : [" + y + "] : " + pdList.Count);
            }
			return pdList;
	}

	public List<BBPlayerData> getWinnerCheck_Flush(List<BBPlayerData> pdList) {	
			List<int> tmpRetToRemove = new List<int>();

			for(int x = 0;x < pdList.Count;x++) {
				if(pdList[x].completeResultStruct.FlushOrderedCardsValue[x] == 1) {
					pdList[x].completeResultStruct.FlushOrderedCardsValue[x] = 14;
			    }
			 }

			float highCard = 0;
			for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.FlushOrderedCardsValue[0] > highCard) { highCard = pdList[x].completeResultStruct.FlushOrderedCardsValue[0];}}
			//Debug.Log("****************getWinnerCheck_Flush start -> highCard : " + highCard);

            if(pdList.Count > 1) {
				for(int y = 0;y < pdList.Count-1;y++) {
					highCard = 0;
					for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.bestFive[y] > highCard) { highCard = pdList[x].completeResultStruct.bestFive[y];}}

					for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.bestFive[y] < highCard) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
					for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}
					//Debug.Log("****************getWinnerCheck_Flush ******  -> pdList.Count : [" + y + "] : " + pdList.Count);
	            }
	        }

       return pdList;
	}

	public List<BBPlayerData> getWinnerCheck_FullHouse(List<BBPlayerData> pdList) {
			List<int> tmpRetToRemove = new List<int>();

			for(int x = 0;x < pdList.Count;x++) {
				if(pdList[x].completeResultStruct.FullHighCards.x == 1) pdList[x].completeResultStruct.FullHighCards.x = 14;
				if(pdList[x].completeResultStruct.FullHighCards.y == 1) pdList[x].completeResultStruct.FullHighCards.y = 14;
			 }

			float highCard_x_tris = 0;
			float highCard_y_tris = 0;

			for(int y = 0;y < pdList.Count;y++) {
				highCard_x_tris = 0;
				tmpRetToRemove.Clear();

				if(pdList[y].completeResultStruct.FullHighCards.x > highCard_x_tris) { highCard_x_tris = pdList[y].completeResultStruct.FullHighCards.x;}

			    for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.FullHighCards.x < highCard_x_tris) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
			    for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}

			}

		    if(pdList.Count > 1) {

				for(int y = 0;y < pdList.Count;y++) {
					highCard_y_tris = 0;
					tmpRetToRemove.Clear();
					if(pdList[y].completeResultStruct.FullHighCards.y > highCard_y_tris) { highCard_y_tris = pdList[y].completeResultStruct.FullHighCards.y;}

					for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.FullHighCards.y < highCard_y_tris) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
			        for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}

				}

		    }
         return pdList;
	}

	public List<BBPlayerData> getWinnerCheck_Poker(List<BBPlayerData> pdList) {

			List<int> tmpRetToRemove = new List<int>();
			//int fifthCard = 0;
			int maxCard = 0;

			for(int x = 0;x < pdList.Count;x++) {
				if(pdList[x].completeResultStruct.maxCardValue > maxCard) { maxCard = pdList[x].completeResultStruct.maxCardValue;}
			}
			for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.maxCardValue < maxCard) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
			for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}


       return pdList;
	}

	public List<BBPlayerData> getWinner_StraightFlush(List<BBPlayerData> pdList) {

			List<int> tmpRetToRemove = new List<int>();
			int maxCard = 0;

			for(int x = 0;x < pdList.Count;x++) {
				//Debug.Log("getWinner_StraightFlush --1->> maxCard : " + pdList[x].completeResultStruct.maxCardValue);
				if(pdList[x].completeResultStruct.maxCardValue > maxCard) { maxCard = pdList[x].completeResultStruct.maxCardValue;}
			}

			for(int x = 0;x < pdList.Count;x++) {if(pdList[x].completeResultStruct.maxCardValue < maxCard) {tmpRetToRemove.Add(pdList[x].playerPosition);}}
			for(int x = 0;x < tmpRetToRemove.Count;x++) {BBPlayerData pd = pdList.Find(item => item.playerPosition == tmpRetToRemove[x]);pdList.Remove(pd);}


			//Debug.Log("getWinner_StraightFlush --->> maxCard : " + maxCard);

			return pdList;
	}

	public IEnumerator rotateCoveredCards(List<BBPlayerData> data) {
			Quaternion rotForShow = GameObject.Find("RotatedForShowCardOnTable").transform.rotation;
		for(int x = 0; x < data.Count; x++) {
				if(data[x].isOutOfGame == false) {
					if(data[x].playerGameObject != null) {
						data[x].transform_card_1.rotation = rotForShow;
				    GetComponent<BBSoundController>().playClip(GetComponent<BBSoundController>().rotateCard);
	                yield return new WaitForSeconds(0.5f);
						data[x].transform_card_2.rotation = rotForShow;
				    GetComponent<BBSoundController>().playClip(GetComponent<BBSoundController>().rotateCard);
	                yield return new WaitForSeconds(0.5f);
	             }
			  }
		}
      yield break;
	}

	int i = 0, x = 0;
	public void CheckforChips(int allChips)
		{ 
			x++;
			if (i == 0)
            {
				BBMove.removeChipPosition = winnerList[0].transform_card_1;
				i++;
				Debug.Log(winnerList[0].playerName);
			}
			else if (x > allChips / winnerList.Count)
            {
				BBMove.removeChipPosition = winnerList[i].transform_card_1;
				i++;
				x = 0;
			}
        }

	public string getFormattedValue(int val) {
	 string tmpRes = "";
		switch(val) {
		     case 1: case 14: tmpRes = "A";break;
			 case 11: tmpRes = "J";break;
			 case 12: tmpRes = "Q";break;
			 case 13: tmpRes = "K";break;
			 default:
			  tmpRes = val.ToString();
			 break;
		   }
		   return tmpRes;
	}

	public string getFormattedBestFive(List<int> bFive) {
	 string[] tmpRes = new string[5];
		for(int x = 0;x < bFive.Count;x++) {
		   switch(bFive[x]) {
		     case 1: case 14: tmpRes[x] = "A";break;
			 case 11: tmpRes[x] = "J";break;
			 case 12: tmpRes[x] = "Q";break;
			 case 13: tmpRes[x] = "K";break;
			 default:
			   if(bFive[x] > 15) {
				 tmpRes[x] = "X";
			   } else {
			     tmpRes[x] = bFive[x].ToString();
			   }
			 break;
		   }
		}
		string res = tmpRes[0] + " - " + tmpRes[1] + " - " + tmpRes[2] + " - " + tmpRes[3] + " - " + tmpRes[4];
        return res;
	}
}
}
#endif