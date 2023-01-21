using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BLabTexasHoldEmProject {

public class BBShowDownResultController : MonoBehaviour {

	public bool logResult = true;

	public GameObject[] playerFinalResultUIList;
	public GameObject finalResultPanel;
	public GameObject PanelLocalPlayerWon;

	struct ListPairCheckVal {
	    public int pairVal;
		public int H_Val_1;
		public int H_Val_2;
		public int H_Val_3;
		public int PId;
    }

	struct FinalResultStruct {
		public BBPlayerCardsResultValueController.CardsValues cardsResult;
		public int playerID;
		public int best_1;
		public int best_2;
		public int best_3;
		public int best_4;
		public int best_5;
	}

	[System.Serializable]
	public class SimulateCardValuerForPlayerID {
		public Vector2[] simulateAllCardsValue = {new Vector2(1,2),new Vector2(2,4),new Vector2(1,4),new Vector2(4,6),new Vector2(3,2),new Vector2(1,1),new Vector2(1,9)};
	}

	public SimulateCardValuerForPlayerID[] simulateCardValuerForPlayerID = new SimulateCardValuerForPlayerID[10];


 
    public bool[] simulateTwoPair = new bool[10];
	public bool[] simulateThreeOfAkind = new bool[10];
	public bool[] simulateStraight = new bool[10];
	public bool[] simulateFlush = new bool[10];
	public bool[] simulateFullHouse = new bool[10];
	public bool[] simulatePoker = new bool[10];
	public bool[] simulateStraightFlush = new bool[10];
  [Header("Only One Possibility")]
	public bool[] simulateRoyalFlush = new bool[10];



  public bool simulatePokerOnTable = false;

  public Vector2[] simulateAllCardsValueForPoker = {new Vector2(1,8),new Vector2(2,4),new Vector2(2,8),new Vector2(4,8),new Vector2(3,8)};
 // public Vector2[] simulateAllCardsValue = {new Vector2(1,2),new Vector2(2,4),new Vector2(1,4),new Vector2(4,6),new Vector2(3,2),new Vector2(1,1),new Vector2(1,9)};

  [System.Serializable]
  public class PlayersCValue {
        public int playerPosID = -1;
		public BBPlayerCardsResultValueController.CardsValues CValue = BBPlayerCardsResultValueController.CardsValues.Null;
		public Vector2 card_1 = Vector2.zero;
		public Vector2 card_2 = Vector2.zero;
		public int maxCVal = 0;
		public bool isRoundOut = true;
		public int[] bestFive = new int[5];
		public float splittedValue = 0;
		public bool won = false;
		public int[] kickerOnPlayerCard = {0,0};  
  }

 // public PlayersCValue[] playersCVal;
	public List<PlayersCValue> playersCVal = new List<BBShowDownResultController.PlayersCValue>();
 
	BBGameController BBGC;
	Vector2[] openCardsList;

	BBPlayerCardsResultValueController.CardsValues onlyOpenCardsValue;


	void Awake() {
			BBGC = GetComponent<BBGameController>();
	}

     void setOpenCardsData() {

	        openCardsList = new Vector2[5];
			openCardsList[0] = BBGC.flopCardList[0];
			openCardsList[1] = BBGC.flopCardList[1];
			openCardsList[2] = BBGC.flopCardList[2];
			openCardsList[3] = BBGC.turnCard;
			openCardsList[4] = BBGC.riverCard;
     }

     void popolateAllResults() {
        // playersCVal = new PlayersCValue[BBGC.playerDataList.Count];

		         for(int x = 0; x < BBGC.playerDataList.Count;x++) {
		            int maxRes = 0;
		                 if(BBGC.playerDataList[x].isOutOfGame) {
						    PlayersCValue tmpV = new PlayersCValue();
						   // playersCVal[x] = tmpV;
						     playersCVal.Add(tmpV);
		                 } else {
							      BBPlayerCardsResultValueController.CardsValues res = GetComponent<BBPlayerCardsResultValueController>().getCardsResult(x,out maxRes);

						                    if(simulateTwoPair[x]) res = BBPlayerCardsResultValueController.CardsValues.TwoPair; 
						               else if(simulateThreeOfAkind[x]) res = BBPlayerCardsResultValueController.CardsValues.ThreeOfAkind; 
						               else if(simulateStraight[x]) res = BBPlayerCardsResultValueController.CardsValues.Straight; 
						               else if(simulateFlush[x]) res = BBPlayerCardsResultValueController.CardsValues.Flush; 
						               else if(simulateFullHouse[x]) res = BBPlayerCardsResultValueController.CardsValues.FullHouse; 
						               else if(simulatePoker[x]) res = BBPlayerCardsResultValueController.CardsValues.Poker; 
						               else if(simulateStraightFlush[x]) res = BBPlayerCardsResultValueController.CardsValues.StraightFlush; 
						               else if(simulateRoyalFlush[x]) res = BBPlayerCardsResultValueController.CardsValues.RoyalFlush; 

		                     //Debug.Log("$$$$$$$$$$$$$$$$$$$$$$ res : " + res + " PId : " + x + " listCount : " + playersCVal.Count);

						    PlayersCValue tmpV = new PlayersCValue();
						     tmpV.playerPosID = x;
						     tmpV.CValue = res;
						     tmpV.card_1 = BBGC.playerDataList[x].card_1_Value;
						     tmpV.card_2 = BBGC.playerDataList[x].card_2_Value;
						     tmpV.maxCVal = maxRes;
						     tmpV.isRoundOut = false;
						     tmpV.bestFive = getBestFive(res,x,tmpV.card_1,tmpV.card_2);
						     //playersCVal[x] = tmpV;
						     playersCVal.Add(tmpV);
						}
					if(logResult) {
					    Debug.Log("<-------------->-->BBShowDownResultController--->>popolateAllResults--> ID : " + x + " resVal : " + playersCVal[x].CValue + " C1 : " + playersCVal[x].card_1 + " C2 : " + playersCVal[x].card_2 + " maxV : " + playersCVal[x].maxCVal); 
						BBGC.playerDataList[x].T_PlayerMoneyTotal.text = playersCVal[x].CValue.ToString();
					}
		          } 
 
     }

	int[] getBestFive(BBPlayerCardsResultValueController.CardsValues cardval,int playerID,Vector2 card_1, Vector2 card_2) {

	    int[] tmpRes = new int[5];
	    Vector2[] allCards = new Vector2[7];

				         if(simulateTwoPair[playerID]) { allCards = simulateCardValuerForPlayerID[playerID].simulateAllCardsValue;cardval = BBPlayerCardsResultValueController.CardsValues.TwoPair;} 
					else if(simulateThreeOfAkind[playerID]) {allCards = simulateCardValuerForPlayerID[playerID].simulateAllCardsValue;;cardval = BBPlayerCardsResultValueController.CardsValues.ThreeOfAkind;}
					else if(simulateStraight[playerID]) {allCards = simulateCardValuerForPlayerID[playerID].simulateAllCardsValue;cardval = BBPlayerCardsResultValueController.CardsValues.Straight;}
					else if(simulateFlush[playerID]) {allCards = simulateCardValuerForPlayerID[playerID].simulateAllCardsValue;cardval = BBPlayerCardsResultValueController.CardsValues.Flush;}
					else if(simulateFullHouse[playerID]) {allCards = simulateCardValuerForPlayerID[playerID].simulateAllCardsValue;cardval = BBPlayerCardsResultValueController.CardsValues.FullHouse;}
					else if(simulatePoker[playerID]) {allCards = simulateCardValuerForPlayerID[playerID].simulateAllCardsValue;cardval = BBPlayerCardsResultValueController.CardsValues.Poker;}
					else if(simulateStraightFlush[playerID]) {allCards = simulateCardValuerForPlayerID[playerID].simulateAllCardsValue;cardval = BBPlayerCardsResultValueController.CardsValues.StraightFlush;}
					else if(simulateRoyalFlush[playerID]) {allCards = simulateCardValuerForPlayerID[playerID].simulateAllCardsValue;cardval = BBPlayerCardsResultValueController.CardsValues.RoyalFlush;}
					else if(simulatePokerOnTable) {
						//allCards = simulateAllCardsValue;
						    BBGC.flopCardList[0] = new Vector2(1,8);BBGC.flopCardList[1] = new Vector2(2,8); BBGC.flopCardList[2] = new Vector2(3,8);BBGC.turnCard = new Vector2(4,8);BBGC.riverCard = new Vector2(2,9);
						    allCards[0] = BBGC.flopCardList[0];
							allCards[1] = BBGC.flopCardList[1];
							allCards[2] = BBGC.flopCardList[2];
							allCards[3] = BBGC.turnCard;
							allCards[4] = BBGC.riverCard;
						    allCards[5] = new Vector2(2,11);
						    allCards[6] = new Vector2(3,13);
						cardval = BBPlayerCardsResultValueController.CardsValues.Poker;
				    }
				    else {
						    allCards[0] = BBGC.flopCardList[0];
							allCards[1] = BBGC.flopCardList[1];
							allCards[2] = BBGC.flopCardList[2];
							allCards[3] = BBGC.turnCard;
							allCards[4] = BBGC.riverCard;
							allCards[5] = card_1;
							allCards[6] = card_2;
					}

				   switch(cardval) {
					case BBPlayerCardsResultValueController.CardsValues.HighCard:
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getCompleteBestAfterHighCard playerID : " + playerID);
						tmpRes = getCompleteBestAfterHighCard(allCards,playerID);
					break;
				    case BBPlayerCardsResultValueController.CardsValues.Pair:
				        int tmpPairCardVal = getPairValue(allCards);
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getPairValue tmpPairCardVal : " + tmpPairCardVal + " PlayerID : " + playerID);
							tmpRes =  getCompleteBestAfterPair(tmpPairCardVal,allCards,playerID);
				    break;
					case BBPlayerCardsResultValueController.CardsValues.TwoPair:
				        int[] _tmpPairCardVal = getTwoPairValue(allCards);
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getTwoPairValue _tmpPairCardVal : " + _tmpPairCardVal[0] + " : " + _tmpPairCardVal[1] + " PlayerID : " + playerID);
							tmpRes =  getCompleteBestAfterTwoPair(_tmpPairCardVal,allCards,playerID);
				    break;
					case BBPlayerCardsResultValueController.CardsValues.ThreeOfAkind:
				        int tmpPairCardValT = getThreeOfAkindValue(allCards);
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getThreeOfAkindValue tmpPairCardValT : " + tmpPairCardValT + " PlayerID : " + playerID);
							tmpRes =  getCompleteBestAfterThreeOfAkind(tmpPairCardValT,allCards,playerID);
				    break;
					case BBPlayerCardsResultValueController.CardsValues.Straight:
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getCompleteBestAfterStraight playerID : " + playerID);
							tmpRes =  getCompleteBestAfterStraight(allCards);
					break;
					case BBPlayerCardsResultValueController.CardsValues.Flush:
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getCompleteBestAfterFlush playerID : " + playerID);
							tmpRes =  getCompleteBestAfterFlush(allCards,playerID);
					break;
					case BBPlayerCardsResultValueController.CardsValues.FullHouse:
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getCompleteBestAfterFullHouse playerID : " + playerID);
							tmpRes =  getCompleteBestAfterFullHouse(allCards,playerID);
					break;
					case BBPlayerCardsResultValueController.CardsValues.Poker:
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getCompleteBestAfterPoker playerID : " + playerID);
							tmpRes =  getCompleteBestAfterPoker(allCards,playerID);
					break;
					case BBPlayerCardsResultValueController.CardsValues.StraightFlush:
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getCompleteBestAfterStraightFlush playerID : " + playerID);
							tmpRes =  getCompleteBestAfterStraightFlush(allCards,playerID);
					break;
					case BBPlayerCardsResultValueController.CardsValues.RoyalFlush:
						if(logResult)  Debug.Log("<-------------->-->BBShowDownResultController--->>getBestFive-->getCompleteBestAfterRoyalFlush playerID : " + playerID);
							tmpRes =  getCompleteBestAfterRoyalFlush();
					break;
				   }

      return tmpRes;

    }

    void setPlayersOut() {
			for(int x = 0; x < BBGC.playerDataList.Count;x++) {
			    if(BBGC._BBGlobalDefinitions.playersStateIsOutDuringOpenGame[x]) {
			      BBGC.playerDataList[x].isOutOfGame = true;
			    }
			}
    }

   	public void executeFinalShowDown() {

   	   setPlayersOut();

	   setOpenCardsData();


	   int maxCardVal = 0;

		   onlyOpenCardsValue = GetComponent<BBPlayerCardsResultValueController>().getOpenCardsResult(BBGC.flopCardList,BBGC.turnCard,BBGC.riverCard,out maxCardVal);
		Debug.Log("[BBShowDownResultController][executeFinalShowDown] onlyOpenCardsValue =============>> : " + onlyOpenCardsValue + " : " + maxCardVal);
/*
		switch(onlyOpenCardsValue) {
		case BBPlayerCardsResultValueController.CardsValues.RoyalFlush:
		return;
		break;
		default:
			popolateAllResults();
		break;
		}
*/

		popolateAllResults();


		executeWinnerCheck();

		GetComponent<BBGameController>().UIMoveingController.SetActive(true);

	}

	void executeWinnerCheck() {

	  List<int[]> bestFive = new List<int[]>();

		for(int x = 0; x < playersCVal.Count;x++) {
	       bestFive.Add(playersCVal[x].bestFive);
	     }

		Debug.Log("-------executeWinnerCheck>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*** bestFive ***>>>>>>> : " + bestFive.Count);

		for(int i = 0; i < bestFive.Count;i++)  {
		    string res = "";
            for(int x = 0; x < 5;x++) {
              res = res + ("[" + bestFive[i][x] + "]");
            }
		   Debug.Log("-------executeWinnerCheck>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*** bestFive ***>>>>>>> : " + "[" + i + "]" + " : " + res);
		   res = "";
		}

		List<PlayersCValue> tmpWinnerList = new List<PlayersCValue>();

		bool testPair = false;
		bool testTwoPair = false;
		bool testForPairHighCard = false;
		bool testThreeOfAkind = false;

		if(testPair) goto TestPair;
		if(testTwoPair) goto TestTwoPair;
		if(testForPairHighCard) goto TestPairHighCard;
		if(testThreeOfAkind) goto TestThreeOfAkind;
		 
		BBPlayerCardsResultValueController.CardsValues toCheck = BBPlayerCardsResultValueController.CardsValues.RoyalFlush;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking RoyalFlush : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 
        //-----------------------------
		toCheck = BBPlayerCardsResultValueController.CardsValues.StraightFlush;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking StraightFlush : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 
		//-----------------------------
		toCheck = BBPlayerCardsResultValueController.CardsValues.Poker;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking Poker : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 
		//-----------------------------
		toCheck = BBPlayerCardsResultValueController.CardsValues.FullHouse;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking FullHouse : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 
		//-----------------------------
		toCheck = BBPlayerCardsResultValueController.CardsValues.Flush;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking Flush : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 
		//-----------------------------
		toCheck = BBPlayerCardsResultValueController.CardsValues.Straight;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking Straight : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 
		//-----------------------------
TestThreeOfAkind:
		toCheck = BBPlayerCardsResultValueController.CardsValues.ThreeOfAkind;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking ThreeOfAkind : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 
		//-----------------------------
TestTwoPair:
		toCheck = BBPlayerCardsResultValueController.CardsValues.TwoPair;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking TwoPair : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 
		//-----------------------------
TestPair:
		toCheck = BBPlayerCardsResultValueController.CardsValues.Pair;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking Pair : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 
		//-----------------------------
TestPairHighCard:
		toCheck = BBPlayerCardsResultValueController.CardsValues.HighCard;

		for(int x = 0;x < playersCVal.Count;x++) {if(playersCVal[x].CValue == toCheck) { tmpWinnerList.Add(playersCVal[x]);}}

		Debug.Log(">>>>-------executeWinnerCheck>>>>>>>>checking HighCard : " + tmpWinnerList.Count);

		if(tmpWinnerList.Count > 0) { payWinner(toCheck, tmpWinnerList); return; } 



	}

	void executeRoundEnd(List<FinalResultStruct> winner) {


	   float totalPot = BBGC._BBGlobalDefinitions.moneyOnTable;
	   float splittedValue = 0;

//		Debug.Log("executeRoundEnd--------> winner.Count : " + winner.Count);

	   if(winner.Count > 1) {
	     splittedValue = totalPot / winner.Count;
	        for(int x = 0;x < winner.Count;x++) {
	           playersCVal[winner[x].playerID].won = true;
			   playersCVal[winner[x].playerID].splittedValue = splittedValue;
				BBGC.playerDataList[winner[x].playerID].currentPlayerTotalMoney = BBGC.playerDataList[winner[x].playerID].currentPlayerTotalMoney + splittedValue;
	        }
	   } else {
			   playersCVal[winner[0].playerID].won = true;
			   playersCVal[winner[0].playerID].splittedValue = totalPot;
			   BBGC.playerDataList[winner[0].playerID].currentPlayerTotalMoney = BBGC.playerDataList[winner[0].playerID].currentPlayerTotalMoney + totalPot;
	   }

	   StartCoroutine( showFinalPlayersResult() );
	}

	public IEnumerator rotateCoveredCards() {

		Quaternion rotForShow = GameObject.Find("RotatedForShowCardOnTable").transform.rotation;

		for(int x = 0; x < BBGC.playerDataList.Count; x++) {

			  if(!BBGC.playerDataList[x].isOutOfGame) {
	                BBGC.playerDataList[x].transform_card_1.rotation = rotForShow;
				    GetComponent<BBSoundController>().playClip(GetComponent<BBSoundController>().rotateCard);
	                yield return new WaitForSeconds(0.5f);
					BBGC.playerDataList[x].transform_card_2.rotation = rotForShow;
				    GetComponent<BBSoundController>().playClip(GetComponent<BBSoundController>().rotateCard);
	                yield return new WaitForSeconds(0.5f);
			  }

		}


      yield break;

	}

	IEnumerator showFinalPlayersResult() {

  
		yield return StartCoroutine(rotateCoveredCards());	  

		int wonCounter = 0;
//		float localPlayerWonValue = 0;
		int outCounter = 0;
		bool localPlayerWon = false;


		for(int x = 0;x < playersCVal.Count;x++) {

				var sprite = Resources.Load<Sprite>("Avatar/playerAvatar_" + x.ToString());Image AvatarImage = playerFinalResultUIList[x].transform.Find("ImageAvatar").GetComponent<Image>();AvatarImage.overrideSprite = sprite;
			    playerFinalResultUIList[x].transform.Find("TextPlayerPosition").GetComponent<Text>().text = "("+(x+1).ToString()+")";

			      if(playersCVal[x].isRoundOut) {
				      sprite = Resources.Load<Sprite>("star-fold");Image wonLoseImage = playerFinalResultUIList[x].transform.Find("ImageWonLose").GetComponent<Image>();wonLoseImage.overrideSprite = sprite;
			      } else {
		                   if(playersCVal[x].won) {sprite = Resources.Load<Sprite>("star-won");Image wonLoseImage = playerFinalResultUIList[x].transform.Find("ImageWonLose").GetComponent<Image>();wonLoseImage.overrideSprite = sprite;}
			          else if(!playersCVal[x].won) {  sprite = Resources.Load<Sprite>("star-lose");Image wonLoseImage = playerFinalResultUIList[x].transform.Find("ImageWonLose").GetComponent<Image>();wonLoseImage.overrideSprite = sprite;}
			      }

			      string humanIcon = ""; if(BBGC._BBGlobalDefinitions.localPlayer == playersCVal[x].playerPosID) humanIcon = "Human-icon"; else humanIcon = "CPU-icon";
			      sprite = Resources.Load<Sprite>(humanIcon);Image wonLoseImage2 = playerFinalResultUIList[x].transform.Find("ImageHumanCpu").GetComponent<Image>();wonLoseImage2.overrideSprite = sprite;

			      playerFinalResultUIList[x].transform.Find("TextPlayerName").GetComponent<Text>().text = BBGC.playerDataList[x].T_playerName.text;

			      if(playersCVal[x].isRoundOut) {
				    outCounter++;
				    playerFinalResultUIList[x].transform.Find("TextResult").GetComponent<Text>().text = "?";
				    playerFinalResultUIList[x].transform.Find("TextBestFive").GetComponent<Text>().text = "?";
			      } else { 
			        playerFinalResultUIList[x].transform.Find("TextResult").GetComponent<Text>().text = getFormattedBestFive(playersCVal[x].bestFive);
				    playerFinalResultUIList[x].transform.Find("TextBestFive").GetComponent<Text>().text = playersCVal[x].CValue.ToString();
			      }

			       if(playersCVal[x].won) {
			         if(playersCVal[x].playerPosID == BBGC._BBGlobalDefinitions.localPlayer) {
			           localPlayerWon = true;
//					   localPlayerWonValue = playersCVal[x].splittedValue;
			         }
			         wonCounter++;
				     playerFinalResultUIList[x].transform.Find("TextMoneyResult").GetComponent<Text>().text = BBStaticData.getMoneyValue(playersCVal[x].splittedValue) + "/" + BBStaticData.getMoneyValue(BBGC.playerDataList[x].currentPlayerTotalMoney);
				     playerFinalResultUIList[x].transform.Find("TextMoneyResult").GetComponent<Text>().color = Color.green;
			       } else {
				     playerFinalResultUIList[x].transform.Find("TextMoneyResult").GetComponent<Text>().text = BBStaticData.getMoneyValue(BBGC.playerDataList[x].currentMoneyOnTable) + "/" + BBStaticData.getMoneyValue(BBGC.playerDataList[x].currentPlayerTotalMoney);
				     playerFinalResultUIList[x].transform.Find("TextMoneyResult").GetComponent<Text>().color = Color.red;
			       }

			       if(playersCVal[x].CValue == BBPlayerCardsResultValueController.CardsValues.TwoPair) {
				     playerFinalResultUIList[x].transform.Find("TextBestKicker").GetComponent<Text>().text = getFormattedValue((int)BBGC.playerDataList[x].TwoPairData.z);
			       }
			       if(playersCVal[x].kickerOnPlayerCard[0] != 0) {
				     playerFinalResultUIList[x].transform.Find("TextBestKicker").GetComponent<Text>().text = getFormattedValue(playersCVal[x].kickerOnPlayerCard[0]);
			       }
		}


		  if(localPlayerWon) {
		      int runOut = 0;
		         foreach(BBPlayerData bbpd in BBGC.playerDataList) {
		           if(bbpd.runOutOfMoney) runOut++;
		         }
		            if(runOut > 8) {
				         BBGC.executeLocalPlayerWon(BBGC.playerDataList[BBGC._BBGlobalDefinitions.localPlayer].currentPlayerTotalMoney + BBGC._BBGlobalDefinitions.moneyOnTable,true);
				         yield break;
				     } else {
				        finalResultPanel.SetActive(true);
				    }
		  } else {
		       finalResultPanel.SetActive(true);
          }

		Debug.Log("||||||||||||||||||||||||||| showFinalPlayersResult outCounter : " + outCounter);

		yield break;
	}


	void payWinner(BBPlayerCardsResultValueController.CardsValues wonKind, List<PlayersCValue> winners) {

//	int[] theWinner = new int[winners.Count];

	 foreach(PlayersCValue pcv in winners) {
		Debug.Log(">>>>---payWinner---->>>>>>>>>>>>>> : " + wonKind + " : " + winners.Count + " : bestFive : " + 
				pcv.bestFive[0] + "-" + pcv.bestFive[1] + "-" + pcv.bestFive[2] + "-" + pcv.bestFive[3] + "-" + pcv.bestFive[4] +    
		        " ID : " + pcv.playerPosID);
	  }

	  switch(wonKind) {
		  case BBPlayerCardsResultValueController.CardsValues.HighCard:
			    List<int[]> tmpResHighCards = getBestResultFromHighCard(winners); 
			    List<int[]> tmpResHighCardsList = getFinalResultForHighCard(tmpResHighCards);
			    List<FinalResultStruct> finResHighCard = new List<FinalResultStruct>();
			   foreach(int[] v in tmpResHighCardsList) {
				  FinalResultStruct lastRes = new FinalResultStruct();
				  lastRes.cardsResult = BBPlayerCardsResultValueController.CardsValues.HighCard;
				  lastRes.best_1 = v[0];
				  lastRes.best_2 = v[1];
				  lastRes.best_3 = v[2];
				  lastRes.best_4 = v[3];
				  lastRes.best_5 = v[4];
				  lastRes.playerID = v[5];
				  finResHighCard.Add(lastRes);
				Debug.Log(">>>>---payWinner---getFinalResultForHighCard ->>>>>>>>>>>>>> tmpResList : " + v[0] + ":" + v[1] + ":" + v[2] + ":" + v[3] + ":" + v[4] + ":" + v[5]);
			    }
			      executeRoundEnd(finResHighCard);
		  break;
		  case BBPlayerCardsResultValueController.CardsValues.Pair:
			    List<int[]> tmpRes = getBestResultFromPair(winners); 
			    List<int[]> tmpResList = getFinalResultForPair(tmpRes);
			    List<FinalResultStruct> finResPair = new List<FinalResultStruct>();
			    foreach(int[] v in tmpResList) {
				  FinalResultStruct lastRes = new FinalResultStruct();
				  lastRes.cardsResult = BBPlayerCardsResultValueController.CardsValues.Pair;
				  lastRes.best_1 = v[0];
				  lastRes.best_2 = v[1];
				  lastRes.best_3 = v[2];
				  lastRes.best_4 = v[3];
				  lastRes.best_5 = -1;
				  lastRes.playerID = v[4];
				  finResPair.Add(lastRes);
			      Debug.Log(">>>>---payWinner---getFinalResultForPair ->>>>>>>>>>>>>> tmpResList : " + v[0] + ":" + v[1] + ":" + v[2] + ":" + v[3] + ":" + v[4]);
			     }
			      executeRoundEnd(finResPair);
		  break;
		  case BBPlayerCardsResultValueController.CardsValues.TwoPair:
				Vector4[] tmpResTwoPair;
				List<Vector4> tmpResListTwoPair = new List<Vector4>();
			    tmpResTwoPair = getBestResultFromTwoPair(winners); // pair 2 higher
			    tmpResListTwoPair = getFinalResultForTwoPair(tmpResTwoPair);

			    List<FinalResultStruct> finResTwoPair = new List<FinalResultStruct>();
			    foreach(Vector4 v in tmpResListTwoPair) {
				  FinalResultStruct lastRes = new FinalResultStruct();
				  lastRes.cardsResult = BBPlayerCardsResultValueController.CardsValues.TwoPair;
				  lastRes.best_1 = (int)v.x;
				  lastRes.best_2 = (int)v.y;
				  lastRes.best_3 = (int)v.z;
				  lastRes.best_4 = -1;
				  lastRes.best_5 = -1;
				  lastRes.playerID = (int)v.w;
				  finResTwoPair.Add(lastRes);
				  Debug.Log(">>>>---payWinner---getFinalResultForTwoPair ->>>>>>>>>>>>>> tmpResList : " + v.y + ":" + v.x + ":" + v.z + ":" + "-1" + ":" + v.w);
			    }
			      executeRoundEnd(finResTwoPair);
		  break;
		  case BBPlayerCardsResultValueController.CardsValues.ThreeOfAkind:
			     List<int[]> tmpResThreeOfAkind = getBestResultFromThreeOfAkind(winners); 
			     List<int[]> tmpResThreeOfAkindList = getFinalResultForThreeOfAkind(tmpResThreeOfAkind);
			     List<FinalResultStruct> finResThreeOfAkind = new List<FinalResultStruct>();
			     foreach(int[] v in tmpResThreeOfAkindList) {
				  FinalResultStruct lastRes = new FinalResultStruct();
				  lastRes.cardsResult = BBPlayerCardsResultValueController.CardsValues.ThreeOfAkind;
				  lastRes.best_1 = v[0];
				  lastRes.best_2 = v[1];
				  lastRes.best_3 = v[2];
				  lastRes.best_4 = -1;
				  lastRes.best_5 = -1;
				  lastRes.playerID = v[3];
				  finResThreeOfAkind.Add(lastRes);
				  Debug.Log(">>>>---payWinner---getFinalResultForThreeOfAkind ->>>>>>>>>>>>>> tmpResList : " + v[0] + ":" + v[1] + ":" + v[2] + ":" + v[3]);
			     }
			    executeRoundEnd(finResThreeOfAkind);
		  break;
		  case BBPlayerCardsResultValueController.CardsValues.Straight:
			   List<int[]> tmpResStraightList = getFinalResultForFromStraight(winners);
			   List<FinalResultStruct> finResStraight = new List<FinalResultStruct>();
			      foreach(int[] v in tmpResStraightList) {
				     FinalResultStruct lastRes = new FinalResultStruct();
					  lastRes.cardsResult = BBPlayerCardsResultValueController.CardsValues.Straight;
					  lastRes.best_1 = v[0];
					  lastRes.best_2 = -1;
					  lastRes.best_3 = -1;
					  lastRes.best_4 = -1;
					  lastRes.best_5 = -1;
					  lastRes.playerID = v[1];
				      finResStraight.Add(lastRes);
					  Debug.Log(">>>>---payWinner---getFinalResultForThreeOfAkind ->>>>>>>>>>>>>> tmpResList : " + v[0] + ":" + v[1]);
			     }
			     executeRoundEnd(finResStraight);
		  break;
		case BBPlayerCardsResultValueController.CardsValues.Flush:
			   List<int[]> tmpResFlushList = getFinalResultForFromFlush(winners);
			   List<FinalResultStruct> finResFlush = new List<FinalResultStruct>();
			       foreach(int[] v in tmpResFlushList) {
				     FinalResultStruct lastRes = new FinalResultStruct();
				      lastRes.cardsResult = BBPlayerCardsResultValueController.CardsValues.Flush;
					  lastRes.best_1 = v[0];
					  lastRes.best_2 = -1;
					  lastRes.best_3 = -1;
					  lastRes.best_4 = -1;
					  lastRes.best_5 = -1;
					  lastRes.playerID = v[1];
				      finResFlush.Add(lastRes);
				      Debug.Log(">>>>---payWinner---getFinalResultForFromFlush ->>>>>>>>>>>>>> tmpResList : " + v[0] + ":" + v[1]);
				   }
			       executeRoundEnd(finResFlush);
		break;
		case BBPlayerCardsResultValueController.CardsValues.FullHouse:
			List<int[]> tmpResFullHouseList = getFinalResultForFromFullHouse(winners);
			List<FinalResultStruct> finResFullHouse = new List<FinalResultStruct>();
			  foreach(int[] v in tmpResFullHouseList) {
				     FinalResultStruct lastRes = new FinalResultStruct();
				       lastRes.cardsResult = BBPlayerCardsResultValueController.CardsValues.FullHouse;
					   lastRes.best_1 = v[0];
				       lastRes.best_2 = v[1];
					   lastRes.best_3 = -1;
					   lastRes.best_4 = -1;
					   lastRes.best_5 = -1;
					   lastRes.playerID = v[2];
				       finResFullHouse.Add(lastRes);
				       Debug.Log(">>>>---payWinner---getFinalResultForFromFullHouse ->>>>>>>>>>>>>> tmpResList : " + v[0] + ":" + v[1] + ":" + v[2]);
			  }
			     executeRoundEnd(finResFullHouse);
		break;
		case BBPlayerCardsResultValueController.CardsValues.Poker:
			List<int[]> tmpResPokerList = getFinalResultForFromPoker(winners);
			List<FinalResultStruct> finResPoker = new List<FinalResultStruct>();
			  foreach(int[] v in tmpResPokerList) {
				     FinalResultStruct lastRes = new FinalResultStruct();
				       lastRes.cardsResult = BBPlayerCardsResultValueController.CardsValues.Poker;
					   lastRes.best_1 = v[0];
				       lastRes.best_2 = -1;
					   lastRes.best_3 = -1;
					   lastRes.best_4 = -1;
					   lastRes.best_5 = -1;
					   lastRes.playerID = v[1];
				       finResPoker.Add(lastRes);
				       Debug.Log(">>>>---payWinner---getFinalResultForFromPoker ->>>>>>>>>>>>>> tmpResList : " + v[0] + ":" + v[1]);
		      }
			    executeRoundEnd(finResPoker);
		break;
		case BBPlayerCardsResultValueController.CardsValues.StraightFlush:
		   List<int[]> tmpSFList = new List<int[]>();
		       foreach(PlayersCValue pcv in winners) { int[] res = new int[2]; res[0] = BBGC.playerDataList[pcv.playerPosID].StraightFlushMaxCardDataValue; res[1] = pcv.playerPosID; tmpSFList.Add(res);}
               int maxVal = 0;
               for(int x = 0; x < tmpSFList.Count;x++) {
				if(tmpSFList[x][0] > maxVal) maxVal = tmpSFList[x][0]; 
               }
			  Debug.Log(">>>>---payWinner---StraightFlush ->>>>>>>>>>>>>> maxVal : " + maxVal);
			  List<int[]> lastList = new List<int[]>();
			  foreach(int[] r in tmpSFList) {if(r[0] == maxVal) {lastList.Add(r);}}

			  List<FinalResultStruct> finResStraightFlush = new List<FinalResultStruct>();
			  foreach(int[] v in lastList) {
				     FinalResultStruct lastRes = new FinalResultStruct();
				       lastRes.cardsResult = BBPlayerCardsResultValueController.CardsValues.StraightFlush;
					   lastRes.best_1 = v[0];
				       lastRes.best_2 = -1;
					   lastRes.best_3 = -1;
					   lastRes.best_4 = -1;
					   lastRes.best_5 = -1;
					   lastRes.playerID = v[1];
				       finResStraightFlush.Add(lastRes);
				       Debug.Log(">>>>---payWinner---StraightFlush ->>>>>>>>>>>>>> tmpResList : " + v[0] + ":" + v[1]);
			   }
			   executeRoundEnd(finResStraightFlush);
		break;
		case BBPlayerCardsResultValueController.CardsValues.RoyalFlush:
			           List<FinalResultStruct> finResRoyalFlush = new List<FinalResultStruct>();

			        foreach(PlayersCValue v in winners) {
			           FinalResultStruct lastResRoyalFlush = new FinalResultStruct();
			             lastResRoyalFlush.cardsResult = BBPlayerCardsResultValueController.CardsValues.RoyalFlush;
						 lastResRoyalFlush.best_1 = 1;
						 lastResRoyalFlush.best_2 = -1;
						 lastResRoyalFlush.best_3 = -1;
						 lastResRoyalFlush.best_4 = -1;
						 lastResRoyalFlush.best_5 = -1;
			             lastResRoyalFlush.playerID = v.playerPosID;
			             finResRoyalFlush.Add(lastResRoyalFlush);
			             Debug.Log(">>>>---payWinner---RoyalFlush ->>>>>>>>>>>>>> tmpResList : " + lastResRoyalFlush.best_1 + ":" + lastResRoyalFlush.playerID);
			       }      
			executeRoundEnd(finResRoyalFlush);    
		break;
	  }


/*
			Vector4[] tmpRes = { 
		                     new Vector4(8,11,11,2),
			                 new Vector4(8,10,8 ,4),
			                 new Vector4(4,6,12,1),
			                 new Vector4(3,1,11,6),
			                 new Vector4(3,1,11,8)
		                   };
*/

    }
/*
	List<int[]> getFinalResultForFromRoyalFlush(List<PlayersCValue> winners) {
		List<int[]> resList = new List<int[]>();
		int[] maxValList = new int[winners.Count];

		for(int x = 0;x < winners.Count;x++) {
		   int[] best5 = new int[5];
		    best5 = winners[x].bestFive;
		    int tmpMax = Mathf.Max(best5);
		    if() 
		       maxValList[x] = winners[x].maxCVal

		}



	}

*/
	 List<int[]> getFinalResultForFromPoker(List<PlayersCValue> winners) {

		List<int[]> resList = new List<int[]>();

		for(int x = 0;x < winners.Count;x++) {
			int[] tmp = new int[2];
			    if(BBGC.playerDataList[winners[x].playerPosID].PokerCardDataValue == 1) {
				 tmp[0] = 14;
			    } else {
				 tmp[0] = BBGC.playerDataList[winners[x].playerPosID].PokerCardDataValue;
			    }
           tmp[1] = winners[x].playerPosID;
           resList.Add(tmp);
		}

        int maxVal = 0; 

		foreach(int[] r in resList) { if(r[0] > maxVal) maxVal = r[0];}

		Debug.Log("getFinalResultForFromPoker --> maxVal : " + maxVal);

		List<int[]> returnList = new List<int[]>();

		foreach(int[] r in resList) { 
		  if(r[0] == maxVal) {
		   returnList.Add(r);
		  }
		}

		return returnList;

	}

	 List<int[]> getFinalResultForFromFullHouse(List<PlayersCValue> winners) {

		List<int[]> resList = new List<int[]>();

		for(int x = 0;x < winners.Count;x++) {
			int[] tmp = new int[3];
			    if(BBGC.playerDataList[winners[x].playerPosID].FullHouseCardData[0] == 1) {
				 tmp[0] = 14;
			    } else {
				 tmp[0] = BBGC.playerDataList[winners[x].playerPosID].FullHouseCardData[0];
			    }
			    if(BBGC.playerDataList[winners[x].playerPosID].FullHouseCardData[1] == 1) {
				 tmp[1] = 14;
			    } else {
				 tmp[1] = BBGC.playerDataList[winners[x].playerPosID].FullHouseCardData[1];
			    }
           tmp[2] = winners[x].playerPosID;
           resList.Add(tmp);
		}

        int maxValFirst = 0; 
		int maxValSecond = 0; 

        foreach(int[] r in resList) { if(r[0] > maxValFirst) maxValFirst = r[0];}

		Debug.Log("getFinalResultForFromFullHouse --> maxValFirst : " + maxValFirst);

		List<int[]> firstCheckList = new List<int[]>();
		List<int[]> secondCheckList = new List<int[]>();

		foreach(int[] r in resList) { if(r[0] == maxValFirst) firstCheckList.Add(r);}

		if(firstCheckList.Count > 1) {
			foreach(int[] r in firstCheckList) { if(r[1] > maxValSecond) maxValSecond = r[1];}
			Debug.Log("getFinalResultForFromFullHouse --> maxValSecond : " + maxValSecond);
			foreach(int[] r in firstCheckList) { if(r[1] == maxValSecond) secondCheckList.Add(r);}
			foreach(int[] r in secondCheckList) Debug.Log("getFinalResultForFromFullHouse maxValSecond --> res : " + r[0] + ":" + r[1] + ":" + r[2]);
			return secondCheckList;
		} else {
			foreach(int[] r in firstCheckList) Debug.Log("getFinalResultForFromFullHouse maxValFirst --> res : " + r[0] + ":" + r[1] + ":" + r[2]);
			return firstCheckList;
		}

	}

	 List<int[]> getFinalResultForFromFlush(List<PlayersCValue> winners) {

		List<int[]> resList = new List<int[]>();

		  for(int x = 0;x < winners.Count;x++) {
			 int[] tmp = new int[2];
			    if(BBGC.playerDataList[winners[x].playerPosID].FlushMaxCardDataValue == 1) {
				 tmp[0] = 14;
			    } else {
				 tmp[0] = BBGC.playerDataList[winners[x].playerPosID].FlushMaxCardDataValue;
			    }
			 tmp[1] = winners[x].playerPosID; 
			 resList.Add(tmp);
		  }

        int maxVal = 0;
		foreach(int[] r in resList) {
		    if(r[0] > maxVal) maxVal = r[0];
        }

		foreach(int[] r in resList) Debug.Log("-------------------resList-------1----------------------------->>getBestResultFromFlush --> maxVal : " + maxVal + " : " + r[0] + " : " + r[1] + " resListCount : " + resList.Count + "  winnerCount : " + winners.Count + " kicker : " + BBGC.playerDataList[r[1]].FlushMaxCardDataValueKicker);

		openCardsList = new Vector2[5];openCardsList[0] = BBGC.flopCardList[0];openCardsList[1] = BBGC.flopCardList[1];openCardsList[2] = BBGC.flopCardList[2];openCardsList[3] = BBGC.turnCard;openCardsList[4] = BBGC.riverCard;
		int seed = BBGC.playerDataList[winners[0].playerPosID].FlushCardDataSeed;

		for(int x = 0;x < openCardsList.Length;x++) {if(openCardsList[x].y == 1) openCardsList[x].y = 14;}

		int[] ValOnPlayerscards = new int[winners.Count];
		for(int x = 0;x < winners.Count;x++) {	
			ValOnPlayerscards[x] = BBGC.playerDataList[winners[x].playerPosID].FlushMaxCardDataValueKicker;
		}
		int maxValOnPlayersCards = Mathf.Max(ValOnPlayerscards);

       bool gotCardOnOpen = false;
       foreach(Vector2 v in openCardsList) {
           if(v.x == seed) {
              if(v.y == maxVal) {
                    gotCardOnOpen = true;
					Debug.Log("---------------------2A-------------------openCardsList------gotCardOnOpen--------->>getBestResultFromFlush --> maxVal : " + v + " gotCardOnOpen: " + gotCardOnOpen);
              }
           }
			Debug.Log("---------------------2-------------------openCardsList--------------->>getBestResultFromFlush --> maxVal : " + v);
       }

	  List<int[]> returnList = new List<int[]>();

      if(gotCardOnOpen) {
			for(int x = 0;x < winners.Count;x++) {	
				Debug.Log("---------------------3-------------------openCardsList-------gotCardOnOpen-------->>getBestResultFromFlush --> maxVal : " + maxVal + " : " + x + " : " + BBGC.playerDataList[winners[x].playerPosID].FlushMaxCardDataValueKicker + " : " + winners[x].playerPosID);
				if(maxValOnPlayersCards == BBGC.playerDataList[winners[x].playerPosID].FlushMaxCardDataValueKicker) {
					Debug.Log("---------------------3AA-------------------openCardsList-------gotCardOnOpen-------->>getBestResultFromFlush --> maxVal : " + maxVal + " : " + x);
			        int[] r = new int[2];
			         r[0] = maxVal;
					 r[1] = winners[x].playerPosID;
					 returnList.Add(r);
					 playersCVal[winners[x].playerPosID].kickerOnPlayerCard[0] = maxValOnPlayersCards;
			    }
			}
      } else {
			foreach(int[] r in resList) {
		       if(r[0] == maxVal) returnList.Add(r);
				Debug.Log("---------------------4-------------------returnList--------------->>getBestResultFromFlush --> maxVal : " + r);
		    }
      }



		

		if(returnList.Count > 1)
		   foreach(int[] r in returnList) Debug.Log("getBestResultFromStraight --> res : " + r[0] + ":" + r[1]);

		return returnList;
	}

	 List<int[]> getFinalResultForFromStraight(List<PlayersCValue> winners) {

		List<int[]> resList = new List<int[]>();

		  for(int x = 0;x < winners.Count;x++) {
			int[] tmp = new int[2];
			 int _max = Mathf.Max( winners[x].bestFive );
			 int _min = Mathf.Min( winners[x].bestFive );
			 if(_min == 1) _max = 14;
			 tmp[0] = _max;
			 tmp[1] = winners[x].playerPosID; 
			 resList.Add(tmp);
		  }

        int maxVal = 0;
		foreach(int[] r in resList) {
		    if(r[0] > maxVal) maxVal = r[0];
        }

		Debug.Log("getBestResultFromStraight --> maxVal : " + maxVal);

		List<int[]> returnList = new List<int[]>();

		foreach(int[] r in resList) {
		  if(r[0] == maxVal) returnList.Add(r);
		}

		foreach(int[] r in returnList) Debug.Log("getBestResultFromStraight --> res : " + r[0] + ":" + r[1]);

		return returnList;
	}

	 List<int[]> getFinalResultForHighCardGetRank(List<int[]> winners, int checkValCode) {
		// 0 = 1° 1 = 2°  2 = 3° 3 = 4° 4 = 5° 5 = plyerID

		foreach(int[] r in winners) {
				Debug.Log(">>>>>>><<<<<--> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3] + ":" + r[4] + ":" + r[5]);
        }


		float maxVal = 0;

		for(int x = 0; x < winners.Count;x++) {
			for(int j = 0; j < winners.Count;j++) {
					       Debug.Log("--0-->>> >>>>>>>>>>>>>>>>>>>>>>>>>>>>>> : " + winners[x][checkValCode] + ":" + winners[j][checkValCode]); // + " result : " + result[x]);
					       float m = Mathf.Max(winners[x][checkValCode],winners[j][checkValCode]); if(m >= maxVal) { maxVal = m; }
					       Debug.Log("max : " + maxVal + " m : " + m);
			}
        }

        List<int[]> retList = new List<int[]>();

        foreach(int[] r in winners) {
			if(r[checkValCode] == maxVal) {
				Debug.Log("<<<<<--> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3] + ":" + r[4] + ":" + r[5]);
			    retList.Add(r);
			}
        }
        return retList;
	}

	 List<int[]> getFinalResultForHighCard(List<int[]> winners) {

		List<int[]> finalResult = new List<int[]>();

		finalResult = getFinalResultForHighCardGetRank(winners,0);

		foreach(int[] r in finalResult) { Debug.Log("finalResult count : " + finalResult.Count + " **************************************************** check 0 --> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3] + ":" + r[4] + ":" + r[5]);}

		List<int[]> finalResult_2 = new List<int[]>();
		if(finalResult.Count > 1) {
			finalResult_2 = getFinalResultForHighCardGetRank(finalResult,1);
			foreach(int[] r in finalResult_2) { Debug.Log("*********************finalResult_2******************************* check 1 --> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3] + ":" + r[4] + ":" + r[5]);}
		} else {
			return finalResult;
		}

		List<int[]> finalResult_3 = new List<int[]>();
		if(finalResult_2.Count > 1) {
			finalResult_3 = getFinalResultForHighCardGetRank(finalResult,2);
			foreach(int[] r in finalResult_3) { Debug.Log("*********************finalResult_3******************************* check 2 --> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3] + ":" + r[4] + ":" + r[5]);}
		} else {
			return finalResult_2;
		}

		List<int[]> finalResult_4 = new List<int[]>();
		if(finalResult_3.Count > 1) {
			finalResult_4 = getFinalResultForHighCardGetRank(finalResult,3);
			foreach(int[] r in finalResult_4) { Debug.Log("*********************finalResult_4******************************* check 3 --> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3] + ":" + r[4] + ":" + r[5]);}
		} else {
			return finalResult_3;
		}

		List<int[]> finalResult_5 = new List<int[]>();
		if(finalResult_4.Count > 1) {
			finalResult_5 = getFinalResultForHighCardGetRank(finalResult,4);
			foreach(int[] r in finalResult_5) { Debug.Log("*********************finalResult_5******************************* check 4 --> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3] + ":" + r[4] + ":" + r[5]);}
			return finalResult_5;
		} else {
			return finalResult_4;
		}



	}

	 List<int[]> getBestResultFromHighCard(List<PlayersCValue> winners) {

		List<int[]> resList = new List<int[]>();

		for(int x = 0; x < winners.Count;x++) {
          int[] tmp = new int[6];
		    if(BBGC.playerDataList[winners[x].playerPosID].HighCardData[0] == 1) tmp[0] = 14; else tmp[0] = BBGC.playerDataList[winners[x].playerPosID].HighCardData[0];
			if(BBGC.playerDataList[winners[x].playerPosID].HighCardData[1] == 1) tmp[1] = 14; else tmp[1] = BBGC.playerDataList[winners[x].playerPosID].HighCardData[1];
			if(BBGC.playerDataList[winners[x].playerPosID].HighCardData[2] == 1) tmp[2] = 14; else tmp[2] = BBGC.playerDataList[winners[x].playerPosID].HighCardData[2];
			if(BBGC.playerDataList[winners[x].playerPosID].HighCardData[3] == 1) tmp[3] = 14; else tmp[3] = BBGC.playerDataList[winners[x].playerPosID].HighCardData[3];
			if(BBGC.playerDataList[winners[x].playerPosID].HighCardData[4] == 1) tmp[4] = 14; else tmp[4] = BBGC.playerDataList[winners[x].playerPosID].HighCardData[4];
			tmp[5] = BBGC.playerDataList[winners[x].playerPosID].playerPosition;
			resList.Add(tmp);
        }

		return resList;
	}

     List<int[]> getFinalResultForPair(List<int[]> winners) {

		List<ListPairCheckVal> finalList = new List<ListPairCheckVal>();
		List<int[]> returnList = new List<int[]>();

		List<ListPairCheckVal> BBRes = FindBestSinglePair(winners,0);

		Debug.Log("===============1=================>>> BBRes : " + BBRes.Count);
		foreach(ListPairCheckVal l in BBRes) Debug.Log(l.pairVal + ":" + l.H_Val_1 + ":" + l.H_Val_2 + ":" + l.H_Val_3 + ":" + l.PId);
		Debug.Log("===============2=================>>> BBRes : " + BBRes.Count);
		if(BBRes.Count > 1) {
		   List<int[]> check2 = new List<int[]>();
		     foreach(ListPairCheckVal lpc in BBRes) {
				int[] newVal = new int[5];
				newVal[0] = lpc.pairVal; newVal[1] = lpc.H_Val_1; newVal[2] = lpc.H_Val_2; newVal[3] = lpc.H_Val_3; newVal[4] = lpc.PId;
				check2.Add(newVal);
		     }  
		       List<ListPairCheckVal> BBRes2 = FindBestSinglePair(check2,1);
			   Debug.Log("================================>>> BBRes2 : " + BBRes2.Count);
			   foreach(ListPairCheckVal l in BBRes2) Debug.Log(l.pairVal + ":" + l.H_Val_1 + ":" + l.H_Val_2 + ":" + l.H_Val_3 + ":" + l.PId);

			     if(BBRes2.Count > 1) {
				    List<int[]> check3 = new List<int[]>();
				       foreach(ListPairCheckVal lpc in BBRes2) {
				         int[] newVal = new int[5];
				         newVal[0] = lpc.pairVal; newVal[1] = lpc.H_Val_1; newVal[2] = lpc.H_Val_2; newVal[3] = lpc.H_Val_3; newVal[4] = lpc.PId;
				         check3.Add(newVal);
		               }
				        Debug.Log("================================>>> BBRes3 check3 : " + check3.Count);
						List<ListPairCheckVal> BBRes3 = FindBestSinglePair(check3,2);
					    Debug.Log("================================>>> BBRes3 : " + BBRes3.Count);
					    foreach(ListPairCheckVal l in BBRes3) Debug.Log(l.pairVal + ":" + l.H_Val_1 + ":" + l.H_Val_2 + ":" + l.H_Val_3 + ":" + l.PId);

					              if(BBRes3.Count > 1) {
									List<int[]> check4 = new List<int[]>();
								       foreach(ListPairCheckVal lpc in BBRes3) {
								         int[] newVal = new int[5];
								         newVal[0] = lpc.pairVal; newVal[1] = lpc.H_Val_1; newVal[2] = lpc.H_Val_2; newVal[3] = lpc.H_Val_3; newVal[4] = lpc.PId;
								         check4.Add(newVal);
						               }
										    Debug.Log("================================>>>  check : " + check4.Count);
											List<ListPairCheckVal> BBRes4 = FindBestSinglePair(check4,3);
										    Debug.Log("================================>>> BBRes4 : " + BBRes4.Count);
										    foreach(ListPairCheckVal l in BBRes4) Debug.Log(l.pairVal + ":" + l.H_Val_1 + ":" + l.H_Val_2 + ":" + l.H_Val_3 + ":" + l.PId);
										       if(BBRes4.Count > 1) {
										         // no one winner split
										         finalList = BBRes4;
										       } else {
						                         finalList = BBRes4;
										       }

					              } else {
					                finalList = BBRes3;
					              }

			     } else {
				    finalList = BBRes2;
			     }

		} else {
			finalList = BBRes;   
		}

		foreach(ListPairCheckVal r in finalList) {
		   int[] tmpR = new int[5];
		    tmpR[0] = r.pairVal;
			tmpR[1] = r.H_Val_1;
			tmpR[2] = r.H_Val_2;
			tmpR[3] = r.H_Val_3;
			tmpR[4] = r.PId;

			returnList.Add(tmpR);
		 }

		 return returnList;

	}

	 List<int[]> getBestResultFromPair(List<PlayersCValue> winners) {

      List<int[]> resList = new List<int[]>();

         for(int x = 0; x < winners.Count;x++) {
           int[] tmp = new int[5];
			  if(BBGC.playerDataList[winners[x].playerPosID].PairData[0] == 1) tmp[0] = 14; else tmp[0] = BBGC.playerDataList[winners[x].playerPosID].PairData[0];
			  if(BBGC.playerDataList[winners[x].playerPosID].PairData[1] == 1) tmp[1] = 14; else tmp[1] = BBGC.playerDataList[winners[x].playerPosID].PairData[1];
			  if(BBGC.playerDataList[winners[x].playerPosID].PairData[2] == 1) tmp[2] = 14; else tmp[2] = BBGC.playerDataList[winners[x].playerPosID].PairData[2];
			  if(BBGC.playerDataList[winners[x].playerPosID].PairData[3] == 1) tmp[3] = 14; else tmp[3] = BBGC.playerDataList[winners[x].playerPosID].PairData[3];

			  tmp[4] = BBGC.playerDataList[winners[x].playerPosID].playerPosition;

			resList.Add(tmp);
         }

         return resList;

	}

	 List<Vector4> getFinalResultForTwoPair(Vector4[] tmpRes) {

		List<Vector4> v4List = new List<Vector4>();

        // setFor ACE
		for(int x = 0; x < tmpRes.Length;x++) {
		    if(tmpRes[x].x == 1) tmpRes[x].x = 14;
			if(tmpRes[x].y == 1) tmpRes[x].y = 14;
			if(tmpRes[x].z == 1) tmpRes[x].z = 14;
		}

			for(int x = 0; x < tmpRes.Length;x++) {
				v4List.Add(tmpRes[x]);
			}

		List<Vector4> BBresBest = new List<Vector4>();

		BBresBest = FindMaxBestpair(v4List);

		if(BBresBest.Count > 1) {
			List<Vector4> BBresSecond = new List<Vector4>();
			BBresSecond = FindMaxSecondPair(BBresBest);
			foreach(Vector4 v in BBresSecond)  Debug.Log(v);

                  if(BBresSecond.Count > 1) {

						List<Vector4> BBresHighCard = new List<Vector4>();
				        BBresHighCard = FindMaxHighCard(BBresSecond);
				        foreach(Vector4 v in BBresHighCard)  Debug.Log(v);
				        if(BBresHighCard.Count > 1) {
					        return BBresHighCard;
				        } else {
						  //res for HighCard
					        return BBresHighCard;
				        }

                  } else {
					//res for best second
				     return BBresSecond;
                  } 
 
		} else {
		   //res for best
			return BBresBest;
		}


    }

	List<int[]> getFinalResultForThreeOfAkindGetRank(List<int[]> winners, int checkValCode) {

		// 0 = trisVal 1 = 1°  2 = 2° 3 = plyerID

		float maxVal = 0;

		for(int x = 0; x < winners.Count;x++) {
			for(int j = 0; j < winners.Count;j++) {
				           Debug.Log("getFinalResultForThreeOfAkindGetRank---->>> >>>>>>>>>>>>>>>>>>>>>>>>>>>>>> : " + winners[x][checkValCode] + ":" + winners[j][checkValCode]); // + " result : " + result[x]);
					       float m = Mathf.Max(winners[x][checkValCode],winners[j][checkValCode]); if(m >= maxVal) { maxVal = m; }
					       Debug.Log("max : " + maxVal + " m : " + m);
			}
        }

        List<int[]> retList = new List<int[]>();

        foreach(int[] r in winners) {
			if(r[checkValCode] == maxVal) {
				Debug.Log("getFinalResultForThreeOfAkindGetRank <<<<<--> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3]);
			    retList.Add(r);
			}
        }
        return retList;

	}

	 List<int[]> getFinalResultForThreeOfAkind(List<int[]> winners) {

		List<int[]> finalResult = new List<int[]>();

		finalResult = getFinalResultForThreeOfAkindGetRank(winners,0);

		foreach(int[] r in finalResult) { Debug.Log("finalResult count : " + finalResult.Count + " **************************************************** check 0 --> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3]);}

		List<int[]> finalResult_2 = new List<int[]>();
		if(finalResult.Count > 1) {
			finalResult_2 = getFinalResultForThreeOfAkindGetRank(finalResult,1);
			foreach(int[] r in finalResult_2) { Debug.Log("*********************finalResult_2******************************* check 1 --> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3]);}
		} else {
			return finalResult;
		}

		List<int[]> finalResult_3 = new List<int[]>();
		if(finalResult_2.Count > 1) {
			finalResult_3 = getFinalResultForThreeOfAkindGetRank(finalResult,2);
			foreach(int[] r in finalResult_3) { Debug.Log("*********************finalResult_3******************************* check 2 --> : " + r[0] + ":" + r[1] + ":" + r[2] + ":" + r[3]);}
			return finalResult_3;
		} else {
			return finalResult_2;
		}


	}

	 List<int[]> getBestResultFromThreeOfAkind(List<PlayersCValue> winners) {
		List<int[]> resList = new List<int[]>();

         for(int x = 0; x < winners.Count;x++) {
			int[] tmp = new int[4];

			if(BBGC.playerDataList[winners[x].playerPosID].ThreeOfAkindCardData[0] == 1) tmp[0] = 14; else tmp[0] = BBGC.playerDataList[winners[x].playerPosID].ThreeOfAkindCardData[0];
			if(BBGC.playerDataList[winners[x].playerPosID].ThreeOfAkindCardData[1] == 1) tmp[1] = 14; else tmp[1] = BBGC.playerDataList[winners[x].playerPosID].ThreeOfAkindCardData[1];
			if(BBGC.playerDataList[winners[x].playerPosID].ThreeOfAkindCardData[2] == 1) tmp[2] = 14; else tmp[2] = BBGC.playerDataList[winners[x].playerPosID].ThreeOfAkindCardData[2];
			tmp[3] = BBGC.playerDataList[winners[x].playerPosID].playerPosition;

            resList.Add(tmp);
         }
         return resList;
	}

	 Vector4[] getBestResultFromTwoPair(List<PlayersCValue> winners) {
		 
      Vector4[] tmpRes = new Vector4[winners.Count];

      for(int x = 0; x < winners.Count;x++) {
            tmpRes[x].x = BBGC.playerDataList[winners[x].playerPosID].TwoPairData.x;
			tmpRes[x].y = BBGC.playerDataList[winners[x].playerPosID].TwoPairData.y;
			tmpRes[x].z = BBGC.playerDataList[winners[x].playerPosID].TwoPairData.z;
			tmpRes[x].w = BBGC.playerDataList[winners[x].playerPosID].TwoPairData.w;
      } 

	    return tmpRes;
	}

	int[] getCompleteBestAfterStraight(Vector2[] allCards) {

		int[] tmpRes = new int[5]; 
		List<int> valList = new List<int>();

		foreach(Vector2 i in allCards) if(logResult)  Debug.Log("------->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterStraight : " + i.x + " : " + i.y);

		for(int x = 0;x < allCards.Length;x++) { valList.Add((int)allCards[x].y); }

		valList.Sort();


		int result = 0;
        int counter = 0;
        int toCheckStarting = 1; 

        Vector2[] counterRes = new Vector2[11]; 

		for(int x = 0;x < allCards.Length;x++) {result = valList.Find(item => item == toCheckStarting);if(result != 0) {toCheckStarting++; counter++;} else {break;}}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 1 : " + toCheckStarting);
		counterRes[1] = new Vector2(counter,1);

		toCheckStarting = 2;
		counter = 0;
		for(int x = 0;x < allCards.Length;x++) {result = valList.Find(item => item == toCheckStarting);if(result != 0) {toCheckStarting++; counter++;} else {break;}}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 2 : " + toCheckStarting);
		counterRes[2] = new Vector2(counter,2);
        		      
		toCheckStarting = 3;
		counter = 0;
		for(int x = 0;x < allCards.Length;x++) {result = valList.Find(item => item == toCheckStarting);if(result != 0) {toCheckStarting++; counter++;} else {break;}}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 3 : " + toCheckStarting);
		counterRes[3] = new Vector2(counter,3);

		toCheckStarting = 4;
		counter = 0;
		for(int x = 0;x < allCards.Length;x++) {result = valList.Find(item => item == toCheckStarting);if(result != 0) {toCheckStarting++; counter++;} else {break;}}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 4 : " + toCheckStarting);
		counterRes[4] = new Vector2(counter,4);

		toCheckStarting = 5;
		counter = 0;
		for(int x = 0;x < allCards.Length;x++) {result = valList.Find(item => item == toCheckStarting);if(result != 0) {toCheckStarting++; counter++;} else {break;}}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 5 : " + toCheckStarting);
		counterRes[5] = new Vector2(counter,5);

		toCheckStarting = 6;
		counter = 0;
		for(int x = 0;x < allCards.Length;x++) {result = valList.Find(item => item == toCheckStarting);if(result != 0) {toCheckStarting++; counter++;} else {break;}}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 6 : " + toCheckStarting);
		counterRes[6] = new Vector2(counter,6);

		toCheckStarting = 7;
		counter = 0;
		for(int x = 0;x < allCards.Length;x++) {result = valList.Find(item => item == toCheckStarting);if(result != 0) {toCheckStarting++; counter++;} else {break;}}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 7 : " + toCheckStarting);
		counterRes[7] = new Vector2(counter,7);

		toCheckStarting = 8;
		counter = 0;
		for(int x = 0;x < allCards.Length;x++) {result = valList.Find(item => item == toCheckStarting);if(result != 0) {toCheckStarting++; counter++;} else {break;}}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 8 : " + toCheckStarting);
		counterRes[8] = new Vector2(counter,8);

		toCheckStarting = 9;
		counter = 0;
		for(int x = 0;x < allCards.Length;x++) {result = valList.Find(item => item == toCheckStarting);if(result != 0) {toCheckStarting++; counter++;} else {break;}}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 9 : " + toCheckStarting);
		counterRes[9] = new Vector2(counter,9);

		toCheckStarting = 10;
		counter = 0;
		int[] checkTEN = {10,11,12,13,1};
		for(int x = 0;x < checkTEN.Length;x++) {
		      result = valList.Find(item => item == checkTEN[x]);
		      if(result != 0) counter++;

			if(logResult)  Debug.Log("TEST----> : " + " counter : " + counter + " : " + result); 
		}
		if(logResult)  Debug.Log("RESULT----> : " + counter + " checking 10 : " + toCheckStarting);
		counterRes[10] = new Vector2(counter,10);

		foreach(Vector2 i in counterRes) if(logResult)  Debug.Log("--counterRes---->> : " + i);

		List<int> finalList = new List<int>();

		for(int x = 0; x < counterRes.Length;x++) {
		   if(counterRes[x].x > 4) {
		     finalList.Add((int)counterRes[x].y);
		   }
		}

		foreach(int i in finalList) if(logResult)  Debug.Log("--finalList---->> : " + i);

		int[] i_final = new int[finalList.Count];
		for(int x = 0; x < i_final.Length;x++) {
		 i_final[x] = finalList[x];
		}
		int bestVal = Mathf.Max(i_final);

		tmpRes[0] = bestVal;
		tmpRes[1] = bestVal+1;
		tmpRes[2] = bestVal+2;
		tmpRes[3] = bestVal+3;

		if((bestVal+4 == 14)) {
			tmpRes[4] = 1;
		} else {
			tmpRes[4] = bestVal+4;
		}

		foreach(int i in tmpRes) if(logResult)  Debug.Log("--tmpRes---->> : " + i);

        return tmpRes;
	}

	int[] getCompleteBestAfterFlush(Vector2[] allCards,int playerID) {

		int[] tmpRes = new int[5]; 
		List<int> valListSeed = new List<int>();
		int seed = 0;

		foreach(Vector2 i in allCards) if(logResult)  Debug.Log("------->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterFlush : " + i.x + " : " + i.y);
		int counter = 0;
		for(int x = 1;x < 5;x++) {

                  for(int j = 0; j < allCards.Length;j++) {
                      if(allCards[j].x == x) {
                        counter++;
                        if(counter > 4) {
                           seed = x;
                           break;
                        }
                      }
                  }
                   if(counter > 4) {
                    break;
                   }
                   counter = 0;
		}

		Debug.Log("---2---->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterFlush seed : " + seed);

		List<int> onPlayerHandcard = new List<int>();

		for(int x = 0; x < allCards.Length;x++) {
		   if(allCards[x].x == seed) {
		      valListSeed.Add((int)allCards[x].y);
				if( (allCards[x].y == BBGC.playerDataList[playerID].card_1_Value.y) || (allCards[x].y == BBGC.playerDataList[playerID].card_2_Value.y) ) {
					onPlayerHandcard.Add((int)allCards[x].y);
				}
		   }
		}

		Debug.Log("---3---->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterFlush onPlayerHandcard : " + onPlayerHandcard.Count);


       if(onPlayerHandcard.Count > 1) {
           int[] test = new int[2];
			test[0] = onPlayerHandcard[0];
			test[1] = onPlayerHandcard[1];
			int max = Mathf.Max(test);
			BBGC.playerDataList[playerID].FlushMaxCardDataValueKicker = max;
		} else if(onPlayerHandcard.Count > 0) {
			BBGC.playerDataList[playerID].FlushMaxCardDataValueKicker = onPlayerHandcard[0];
        }

		Debug.Log("---4---->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterFlush : ");

       valListSeed.Sort();

       if(valListSeed[0] == 1) {
			tmpRes[0] = valListSeed[valListSeed.Count-4];
		    tmpRes[1] = valListSeed[valListSeed.Count-3];
		    tmpRes[2] = valListSeed[valListSeed.Count-2];
		    tmpRes[3] = valListSeed[valListSeed.Count-1];
		    tmpRes[4] = valListSeed[0];
       } else {
			tmpRes[0] = valListSeed[valListSeed.Count-5];
		    tmpRes[1] = valListSeed[valListSeed.Count-4];
		    tmpRes[2] = valListSeed[valListSeed.Count-3];
		    tmpRes[3] = valListSeed[valListSeed.Count-2];
		    tmpRes[4] = valListSeed[valListSeed.Count-1];
       }



		foreach(int i in tmpRes) if(logResult)  Debug.Log("RESULT------->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterFlush : " + i);

		BBGC.playerDataList[playerID].FlushCardDataSeed = seed;
		BBGC.playerDataList[playerID].FlushMaxCardDataValue = tmpRes[4];

		return tmpRes;
	}

	int[] getCompleteBestAfterFullHouse(Vector2[] allCards, int playerID) {

		int[] tmpRes = new int[5]; 
		int trisVal = 0;
        int pairVal = 0;

		foreach(Vector2 i in allCards) if(logResult)  Debug.Log("---1---->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterFullHouse : " + i.x + " : " + i.y);
		int counter = 0;
		for(int x = 0; x < allCards.Length;x++) {
			for(int y = 0; y < allCards.Length;y++) {
			    if(allCards[x].y == allCards[y].y) {
			      counter++;
			      if(counter == 3) {
			        trisVal = (int)allCards[x].y;
			        break;
			      }
			    }
			}

			    if(counter == 3) {
			     break;
			    }
			    counter = 0;

		}

		counter = 0;
		for(int x = 0; x < allCards.Length;x++) {
			for(int y = 0; y < allCards.Length;y++) {
				if( (allCards[x].y == allCards[y].y) && ((int)allCards[x].y != trisVal) )  {
			      counter++;
			      if(counter == 2) {
			        pairVal = (int)allCards[x].y;
			        break;
			      }
			    }
			}
			if(counter == 2) {
			     break;
			    }
			    counter = 0;
		}

		tmpRes[0] = trisVal;
		tmpRes[1] = trisVal;
		tmpRes[2] = trisVal;
		tmpRes[3] = pairVal;
		tmpRes[4] = pairVal;

		foreach(int i in tmpRes) if(logResult)  Debug.Log("RESULT------->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterFullHouse : " + i);
		int[] fullData = new int[2];
		fullData[0] = trisVal;
		fullData[1] = pairVal;

		BBGC.playerDataList[playerID].FullHouseCardData = fullData;

		return tmpRes;

	}

	int[] getCompleteBestAfterStraightFlush(Vector2[] allCards, int playerID) {

		int[] tmpRes = new int[5]; 
		List<Vector2> valListAll = new List<Vector2>();
		List<int> valListSeed = new List<int>();

		foreach(Vector2 i in allCards) if(logResult)  Debug.Log("---1---->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterStraightFlush : " + i.x + " : " + i.y);

		for(int x = 0; x < allCards.Length;x++) { valListAll.Add(allCards[x]); }
		float currentSeed = 1; for(int x = 0; x < valListAll.Count;x++) { if(valListAll[x].x == currentSeed) { valListSeed.Add((int)valListAll[x].y);}}
		if(valListSeed.Count > 4) { goto Found; }

		valListSeed.Clear();
		currentSeed = 2; for(int x = 0; x < valListAll.Count;x++) { if(valListAll[x].x == currentSeed) { valListSeed.Add((int)valListAll[x].y);}}
		if(valListSeed.Count > 4) { goto Found; }

		valListSeed.Clear();
		currentSeed = 3; for(int x = 0; x < valListAll.Count;x++) { if(valListAll[x].x == currentSeed) { valListSeed.Add((int)valListAll[x].y);}}
		if(valListSeed.Count > 4) { goto Found; }

		valListSeed.Clear();
		currentSeed = 4; for(int x = 0; x < valListAll.Count;x++) { if(valListAll[x].x == currentSeed) { valListSeed.Add((int)valListAll[x].y);}}
		if(valListSeed.Count > 4) { goto Found; }

Found:


		if(logResult)  Debug.Log("---2---->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterStraightFlush valListSeed.Count : " + valListSeed.Count);

		               int maxVal = 0;
					  // bool gotResultCheck_1 = false;
					  // bool gotResultCheck_2 = false;
					  // bool gotResultCheck_3 = false;

					if(valListSeed.Count > 4) {
					   valListSeed.Sort();
					   int startingCounter = valListSeed[0];
					   int counter = 0;

						   for(int y = 0; y < valListSeed.Count;y++) {
						        if(valListSeed[y] == startingCounter) {
						           counter++;
						           startingCounter++;
						           if(counter == 5) {
						              maxVal = startingCounter;
									 // gotResultCheck_1 = true;
						           }  
						        }
						   }

					       counter = 0;
						   startingCounter = valListSeed[1];  
						   for(int y = 0; y < valListSeed.Count;y++) {
						        if(valListSeed[y] == startingCounter) {
						           counter++;
						           startingCounter++;
						           if(counter == 5) {
						              maxVal = startingCounter;
									  //gotResultCheck_2 = true;
						           }  
						        }
						   }
					      
						   counter = 0;
						   startingCounter = valListSeed[2];  
						   for(int y = 0; y < valListSeed.Count;y++) {
						        if(valListSeed[y] == startingCounter) {
						           counter++;
						           startingCounter++;
						           if(counter == 5) {
						              maxVal = startingCounter;
									 // gotResultCheck_3 = true;
						           }  
						        }
						   }
					} 

        

        tmpRes[0] = maxVal-5;
		tmpRes[1] = maxVal-4;
		tmpRes[2] = maxVal-3;
		tmpRes[3] = maxVal-2;
		tmpRes[4] = maxVal-1;


		foreach(int i in tmpRes) if(logResult)  Debug.Log("RESULT---3---->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterStraightFlush : " + i);

		BBGC.playerDataList[playerID].StraightFlushMaxCardDataValue = tmpRes[4];

		return tmpRes;
    }

	int[] getCompleteBestAfterRoyalFlush() {

		int[] tmpRes = new int[5]; 

		tmpRes[0] = 10;
		tmpRes[1] = 11;
		tmpRes[2] = 12;
		tmpRes[3] = 13;
		tmpRes[4] = 1;

		return tmpRes;
    }

	int[] getCompleteBestAfterHighCard(Vector2[] allCards, int playerID) {

		int[] tmpRes = new int[5]; 
	    List<int> valList = new List<int>();

		foreach(Vector2 i in allCards) if(logResult)  Debug.Log("---1---->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterHighCard : " + i.y);

		for(int x = 0; x < allCards.Length;x++) {
			valList.Add((int)allCards[x].y);
		}

		valList.Sort();

		if(valList[0] == 1) {
			tmpRes[0] = 1;
			tmpRes[1] = valList[6];
		    tmpRes[2] = valList[5];
		    tmpRes[3] = valList[4];
		    tmpRes[4] = valList[3];
		} else {
			tmpRes[0] = valList[6];
			tmpRes[1] = valList[5];
		    tmpRes[2] = valList[4];
		    tmpRes[3] = valList[3];
		    tmpRes[4] = valList[2];
		}

		foreach(int i in tmpRes) if(logResult)  Debug.Log("---2---->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterHighCard : " + i);

		BBGC.playerDataList[playerID].HighCardData = tmpRes;

		return tmpRes;

	}

	int[] getCompleteBestAfterPoker(Vector2[] allCards, int playerID) {

        int pokID = 0;
        int counter = 0;

		for(int x = 0; x < allCards.Length;x++) {
			for(int j = 0; j < allCards.Length;j++) {
		      if(allCards[x].y == allCards[j].y) {
		          counter++;
		      }	    
               if(counter == 4) {
                 pokID = (int)allCards[x].y;
                 break;
               }
			}
        }

		if(logResult)  Debug.Log("************2*********************** getCompleteBestAfterPoker******************************* pokID : " + pokID);   

		int[] tmpRes = new int[5]; 

		tmpRes[0] = pokID;
		tmpRes[1] = pokID;
		tmpRes[2] = pokID;
		tmpRes[3] = pokID;
		tmpRes[4] = 0;

		BBGC.playerDataList[playerID].PokerCardDataValue = pokID;

		return tmpRes;
   
	}

	int getThreeOfAkindValue(Vector2[] cardList) {
	 int tmpRes = 0;
	 int couter = 0;

	   for(int x = 0; x < cardList.Length;x++) {

	       for(int j = 0; j < cardList.Length;j++) {
	         if(cardList[x].y == cardList[j].y) {
	           couter++;
	           if(couter == 3) {
	             tmpRes = (int)cardList[x].y;
	             break;
	           }
	         }
	       }
	       if(couter == 3) {
	         break;
	       }
	       couter = 0;
	   }

	   return tmpRes;

	}

	int[] getCompleteBestAfterThreeOfAkind(int ThreeOfAkindVal,Vector2[] cardList,int playerID ) {

	   int[] tmpRes = new int[5]; 
	   List<int> valList = new List<int>();

		foreach(Vector2 i in cardList) if(logResult)  Debug.Log("------->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterThreeOfAkind : " + i.y);

		for(int x = 0; x < cardList.Length;x++) {
			if( !((int)cardList[x].y == ThreeOfAkindVal) ) {
				valList.Add((int)cardList[x].y);
		    }
		}
		 valList.Sort();

		   if(valList[0] == 1) {
			tmpRes[0] = ThreeOfAkindVal;
		    tmpRes[1] = ThreeOfAkindVal;
		    tmpRes[2] = ThreeOfAkindVal;
			tmpRes[3] = 1;
			tmpRes[4] = valList[3];
		   } else {
			tmpRes[0] = ThreeOfAkindVal;
		    tmpRes[1] = ThreeOfAkindVal;
		    tmpRes[2] = ThreeOfAkindVal;
			tmpRes[3] = valList[3];
			tmpRes[4] = valList[2];
		   }

		int[] toSave = new int[3];
		toSave[0] = ThreeOfAkindVal;
		toSave[1] = tmpRes[3];
		toSave[2] = tmpRes[4];

		  BBGC.playerDataList[playerID].ThreeOfAkindCardData = toSave;

		foreach(int i in tmpRes) if(logResult)  Debug.Log("RESULT--->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterThreeOfAkind : " + i);

		return tmpRes;

    }

	int[] getTwoPairValue(Vector2[] cardList) {
	 int[] temRes = new int[2];

	    int partVal_1 = 0;
		int partVal_2 = 0;
		int partVal_3 = 0;

	   int couter = 0;

	   for(int x = 0; x < cardList.Length;x++) {
	       for(int j = 0; j < cardList.Length;j++) {
	         if(cardList[x].y == cardList[j].y) {
	           couter++;
	           if(couter == 2) {
	             partVal_1 = (int)cardList[x].y;
	             break;
	           }
	         }
	       }
	       if(couter == 2) {
	         break;
	       }
	       couter = 0;
	   }

	   couter = 0;

		for(int x = 0; x < cardList.Length;x++) {
	        for(int j = 0; j < cardList.Length;j++) {
				if( (cardList[x].y == cardList[j].y) && ((int)cardList[x].y != partVal_1) ) {
	              couter++;
	              if(couter == 2) {
	                partVal_2 = (int)cardList[x].y;
	               break;
	              }
	         }
	       }
	       if(couter == 2) {
	         break;
	       }
	       couter = 0;
	   }

		couter = 0;

		for(int x = 0; x < cardList.Length;x++) {
	        for(int j = 0; j < cardList.Length;j++) {
				if( (cardList[x].y == cardList[j].y) && ( ((int)cardList[x].y != partVal_1) && ((int)cardList[x].y != partVal_2)  ) ) {
	              couter++;
	              if(couter == 2) {
	                partVal_3 = (int)cardList[x].y;
	               break;
	              }
	         }
	       }
	       if(couter == 2) {
	         break;
	       }
	       couter = 0;
	   }

	   List<int> valList = new List<int>();
	   valList.Add(partVal_1);
	   valList.Add(partVal_2);
	   valList.Add(partVal_3);

	   valList.Sort();

	   if(valList[0] == 1) {
			temRes[0] = 1;
	   } else {
			temRes[0] = valList[2];
	   }

		temRes[1] = valList[1];

	   return temRes;
	}

	int getPairValue(Vector2[] cardList) {
	 int tmpRes = 0;
	 int couter = 0;

	   for(int x = 0; x < cardList.Length;x++) {

	       for(int j = 0; j < cardList.Length;j++) {
	         if(cardList[x].y == cardList[j].y) {
	           couter++;
	           if(couter == 2) {
	             tmpRes = (int)cardList[x].y;
	             break;
	           }
	         }
	       }
	       if(couter == 2) {
	         break;
	       }
	       couter = 0;
	   }

	   return tmpRes;

	}

	int[] getCompleteBestAfterTwoPair(int[] pairVal,Vector2[] cardList,int playerID ) {
		int[] tmpRes = new int[5]; 
	   List<int> valList = new List<int>();


		for(int x = 0; x < cardList.Length;x++) {
			int result = -1;
			  result = valList.Find(item => item == (int)cardList[x].y);
//			  Debug.Log("^^^^^^^^^^^^^^^^^^^pairVal^^^^^^^^2^^^^^^^^^^^^^^^^^^^^^^^^^^^ " + cardList[x].y + " result : " + result);
			if(result == 0) valList.Add((int)cardList[x].y);
		}

		valList.Sort();

		if ((valList[0] == 1) && ( (pairVal[0] == 1) || (pairVal[1] == 1) ) ) {
			tmpRes[0] = pairVal[0];
		    tmpRes[1] = pairVal[0];
		    tmpRes[2] = pairVal[1];
		    tmpRes[3] = pairVal[1];
			tmpRes[4] = valList[2];
		} 
		else if(valList[0] == 1) {
			tmpRes[0] = pairVal[0];
		    tmpRes[1] = pairVal[0];
		    tmpRes[2] = pairVal[1];
		    tmpRes[3] = pairVal[1];
			tmpRes[4] = 1;
		}
	     else {
			tmpRes[0] = pairVal[0];
		    tmpRes[1] = pairVal[0];
		    tmpRes[2] = pairVal[1];
		    tmpRes[3] = pairVal[1];
			tmpRes[4] = valList[2];
		   }

//		foreach(int i in tmpRes)  Debug.Log("RESULT--->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterTwoPair : " + i + " playerID : " + playerID);

//		Debug.Log("--card_1_Value----->> : " + BBGC.playerDataList[playerID].card_1_Value.y);
//		Debug.Log("--card_2_Value----->> : " + BBGC.playerDataList[playerID].card_2_Value.y);

/*
        int fifthCard = 0;
		if(BBGC.playerDataList[playerID].card_1_Value.y >= BBGC.playerDataList[playerID].card_2_Value.y) 
			fifthCard = (int)BBGC.playerDataList[playerID].card_1_Value.y;
		else 
		    fifthCard = (int)BBGC.playerDataList[playerID].card_2_Value.y;

		Debug.Log("--fifthCard----->> : " + fifthCard);
 */      

		Vector4 twoPairValues = new Vector4( (float)pairVal[0],(float)pairVal[1],(float)tmpRes[4],(float)playerID );
		//Vector4 twoPairValues = new Vector4( (float)pairVal[0],(float)pairVal[1],(float)fifthCard,(float)playerID );

//		Debug.Log(playerID + ") =====================================================================>>> twoPairValues : " + twoPairValues);

		BBGC.playerDataList[playerID].TwoPairData = twoPairValues;
		 
		return tmpRes;

	}

	int[] getCompleteBestAfterPair(int pairVal,Vector2[] cardList, int playerID ) {

	   int[] tmpRes = new int[5]; 
	   List<int> valList = new List<int>();

		foreach(Vector2 i in cardList) if(logResult)  Debug.Log("------->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterPair : " + i.y);

		for(int x = 0; x < cardList.Length;x++) {
		   if( !((int)cardList[x].y == pairVal) ) {
				valList.Add((int)cardList[x].y);
		   }
		}
		 valList.Sort();

         tmpRes[0] = pairVal;
		 tmpRes[1] = pairVal;

		   if(valList[0] == 1) {
			tmpRes[2] = 1;
			tmpRes[3] = valList[4];
		    tmpRes[4] = valList[3];
		   } else {
			tmpRes[2] = valList[4];
			tmpRes[3] = valList[3];
		    tmpRes[4] = valList[2];

		   }

		foreach(int i in tmpRes) if(logResult)  Debug.Log("RESULT--->>%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%getCompleteBestAfterPair : " + i);

		int[] toSave = new int[5];
		toSave[0] = pairVal;
		toSave[1] = tmpRes[2];
		toSave[2] = tmpRes[3];
		toSave[3] = tmpRes[4];
		toSave[4] = playerID;

		BBGC.playerDataList[playerID].PairData = toSave;

		return tmpRes;

    }

	public List<Vector4> FindMaxHighCard(List<Vector4> list) {

	List<Vector4> tmpReturn = new List<Vector4>();

		Debug.Log("FindMaxHighCard count : " + list.Count);
		float maxVal = 0;

		for(int x = 0; x < list.Count;x++) {

			for(int j = 0; j < list.Count;j++) {

				if( list[x].z > maxVal ) maxVal = list[x].z;

			}

		}

		int counter = 0;
		foreach(Vector4 v in list) {
		  if(v.z == maxVal) {
		    counter++;
			tmpReturn.Add(v);
		  }
		}

	   


      Debug.Log("********************************** result counter : " + counter);

	
	    return tmpReturn;
	}

	public List<Vector4> FindMaxSecondPair(List<Vector4> list) {

	List<Vector4> tmpReturn = new List<Vector4>();

		Debug.Log("FindMaxSecondPair count : " + list.Count);

		float maxVal = 0;

		for(int x = 0; x < list.Count;x++) {

			for(int j = 0; j < list.Count;j++) {

				if( list[x].x > maxVal ) maxVal = list[x].x;

			}

		}

		int counter = 0;
		foreach(Vector4 v in list) {
		  if(v.x == maxVal) {
		    counter++;
		    tmpReturn.Add(v);
		  }
		}



		Debug.Log("***************FindMaxSecondPair******************* result counter : " + counter);



	    return tmpReturn;
	}

	public List<Vector4> FindMaxBestpair(List<Vector4> list) {

	List<Vector4> tmpReturn = new List<Vector4>();

		Debug.Log("FindMaxBestpair count : " + list.Count);
		float maxVal = 0;

		for(int x = 0; x < list.Count;x++) {

			for(int j = 0; j < list.Count;j++) {

				if( list[x].y > maxVal ) maxVal = list[x].y;

			}

		}

		int counter = 0;
		foreach(Vector4 v in list) {
		  if(v.y == maxVal) {
		    counter++;
		    tmpReturn.Add(v);
		  }
		}



		Debug.Log("***************FindMaxBestpair******************* result counter : " + counter);



		return tmpReturn;
	   
	}

	List<ListPairCheckVal> FindBestSinglePair(List<int[]> list, int listPairCheckValCode ) {
		// 0 = pairNum 1 = firstHigh 2 = secondHigh 3 = thirdHigh 4 = plyerID

		foreach(int[] l in list) Debug.Log("list : " + l[0] + ":" + l[1]);

    List<ListPairCheckVal> checkList = new List<ListPairCheckVal>();

    foreach(int[] l in list) {
      ListPairCheckVal lpcv = new ListPairCheckVal();
            lpcv.pairVal = l[0];
			lpcv.H_Val_1 = l[1];
			lpcv.H_Val_2 = l[2];
			lpcv.H_Val_3 = l[3];
			lpcv.PId = l[4];
            checkList.Add(lpcv);
    }

		foreach(ListPairCheckVal r in checkList) Debug.Log("checkList : " + r.pairVal + ":" + r.H_Val_1 + ":" + r.H_Val_2 + ":" + r.H_Val_3 + ":" + r.PId);

		Debug.Log("FindBestSinglePair count : " + list.Count + " listPairCheckValCode : " + listPairCheckValCode);
		float maxVal = 0;

		for(int x = 0; x < list.Count;x++) {
			for(int j = 0; j < list.Count;j++) {
				      ListPairCheckVal result;
				      switch(listPairCheckValCode) {
				          case 0:  result =  checkList.Find(item => checkList[x].pairVal > checkList[j].pairVal); 
					               Debug.Log("--0-->>> >>>>>>>>>>>>>>>>>>>>>>>>>>>>>> : " + checkList[x].pairVal + ":" + checkList[j].pairVal + " result : " + result.pairVal);
					               //if(result.pairVal != 0) { 
					                 float m = Mathf.Max(checkList[x].pairVal,checkList[j].pairVal); if(m >= maxVal) { maxVal = m; }
					               //}
				                   break;
				          case 1:  result =  checkList.Find(item => checkList[x].H_Val_1 > checkList[j].H_Val_1); 
					               Debug.Log("--1-->>> >>>>>>>>>>>>>>>>>>>>>>>>>>>>>> : " + checkList[x].H_Val_1 + ":" + checkList[j].H_Val_1 + " result : " + result.H_Val_1);
					                //if(result.H_Val_1 != 0) {
					                float m1 = Mathf.Max(checkList[x].H_Val_1,checkList[j].H_Val_1); if(m1 >= maxVal) { maxVal = m1; }
					                //}
				                   break;
				          case 2:  result =  checkList.Find(item => checkList[x].H_Val_2 > checkList[j].H_Val_2); 
					               Debug.Log("--2-->>> >>>>>>>>>>>>>>>>>>>>>>>>>>>>>> : " + checkList[x].H_Val_2 + ":" + checkList[j].H_Val_2 + " result : " + result.H_Val_2);
					               //if(result.H_Val_2 != 0)
					                //{
					                 float m2 = Mathf.Max(checkList[x].H_Val_2,checkList[j].H_Val_2); if(m2 >= maxVal) { maxVal = m2; }
					                //}
				                   break;
				          case 3:  result =  checkList.Find(item => checkList[x].H_Val_3 > checkList[j].H_Val_3); 
					               Debug.Log("--3-->>> >>>>>>>>>>>>>>>>>>>>>>>>>>>>>> : " + checkList[x].H_Val_3 + ":" + checkList[j].H_Val_3 + " result : " + result.H_Val_3);
					               //if(result.H_Val_3 != 0) {
					               float m3 = Mathf.Max(checkList[x].H_Val_3,checkList[j].H_Val_3); if(m3 >= maxVal) { maxVal = m3; }
					               //}
				                   break;
                      }
            }
        }

		Debug.Log("FindBestSinglePair---->>> MaxVal : " + maxVal); 

		List<ListPairCheckVal> tmpRestVal = new List<ListPairCheckVal>();

		int counter = 0;
		foreach(ListPairCheckVal v in checkList) {
			switch(listPairCheckValCode) {
			case 0: if(v.pairVal == maxVal) {counter++;tmpRestVal.Add(v);}break;
			case 1: if(v.H_Val_1 == maxVal) {counter++;tmpRestVal.Add(v);}break;
			case 2: if(v.H_Val_2 == maxVal) {counter++;tmpRestVal.Add(v);}break;
			case 3: if(v.H_Val_3 == maxVal) {counter++;tmpRestVal.Add(v);}break;
			}

		}

		Debug.Log("FindBestSinglePair---->>> tmpRestVal : " + tmpRestVal.Count); 

	

		return tmpRestVal;
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

	public string getFormattedBestFive(int[] bFive) {
	 string[] tmpRes = new string[5];
		for(int x = 0;x < bFive.Length;x++) {
		   switch(bFive[x]) {
		     case 1: case 14: tmpRes[x] = "A";break;
			 case 11: tmpRes[x] = "J";break;
			 case 12: tmpRes[x] = "Q";break;
			 case 13: tmpRes[x] = "K";break;
			 default:
			  tmpRes[x] = bFive[x].ToString();
			 break;
		   }
		}
		string res = tmpRes[0] + " - " + tmpRes[1] + " - " + tmpRes[2] + " - " + tmpRes[3] + " - " + tmpRes[4];
        return res;
	}

}
}