using System;
using System.Collections.Generic;
using UnityEngine;

namespace BLabTexasHoldEmProject {

public class BBGlobalDefinitions : ScriptableObject {

    public bool UseLoginSystem = true;


	public bool useSimulateMoneyValueForLocalPlayer = false;
	public float localPlayerMoneyValueSimulation = 4000;

    public bool useSimulatedSavedData = false;
	public bool useLastCardsHandSavedData = false;

    public enum GameType {Limited,NoLimit};
	public enum GamePhaseGeneral {Closed,Open};
	public enum GamePhaseDetail {None,SmallBlind,BigBlind,GiveingCards,FirstBettingRound,ClosingPreFlop,Flop,ClosingFlop,Turn,
		ClosingTurn,River,ClosingRiver,ShowDown,ShowDownAllInOnFlop,ShowDownAllInOnTurn,ShowDownAllInOnRiver,ClosingMoneyAllign,AllIn};

	public GameType gameType;
	public GamePhaseGeneral gamePhaseGeneral;
	public GamePhaseDetail gamePhaseDetail;
	public GamePhaseDetail lastPhaseBeforeAllignMoney;
	public GamePhaseDetail allInRequestAtPhase;

	public bool useDeepLog = false;
	public int playersOnTable = 0;
	public int currentActivePlayer = 0;
	public int currentActivedealer = 0;

	public float limitedLow = 25;
	public float limitedHight = 50;

	public int localPlayer = 1;

	public int playerToTalk = 0;

	public int roundRaiseCounter = 0;

	public float smallBlindValue = 25;

	public float bigBlindValue = 50;

	public float moneyOnTable = 0;

	public int smallBlindPlayerId = 0;

	public int bigBlindPlayerId = 0;

	public float lastBet = 0;

	public float firstBettingRoundValueToPlay = 0;

	public Vector2 firstLastPlayerToTalkOnTable = new Vector2(0,9);

	//public bool useLimitedRaiseTimes = true;
	public string[] playersName = new string[10];

	public float[] playersCashDuringOpenGame = new float[10];
	public bool[] playersStateIsOutDuringOpenGame = new bool[10];

	public float stackMoneyAtStart = 2000;


	public bool isAnOpenGame = false;


	public void resetInitialPlayerIsOutState() {
		for(int x = 0;x < playersStateIsOutDuringOpenGame.Length;x++) {
			playersStateIsOutDuringOpenGame[x] = false;
		}
	}

	public void savePlayersCashDuringGame(float[] playersCashValue) {
		for(int x = 0;x < playersCashDuringOpenGame.Length;x++) {
	     playersCashDuringOpenGame[x] = playersCashValue[x];
	   }
	}

	public void loadInitialPlayersCashDuringGame() {
	   for(int x = 0;x < playersCashDuringOpenGame.Length;x++) {
	         playersCashDuringOpenGame[x] = BBStaticData.gameLimitedStackValue;
	   }
	}


	public void setInitialData() {

       for(int f = 0; f < playersCashDuringOpenGame.Length;f++) {
			playersCashDuringOpenGame[f] = stackMoneyAtStart;
       }

		playersOnTable = 0;

	    limitedLow = 25;

	    limitedHight = 50;

	    playerToTalk = 0;

	    roundRaiseCounter = 0;

	   // smallBlindValue = 0;

	   // bigBlindValue = 0;

	    moneyOnTable = 0;


		firstBettingRoundValueToPlay = 0;

		firstLastPlayerToTalkOnTable = Vector2.zero;

		currentActivePlayer = BBStaticData.currentActivePlayer;

		currentActivedealer = BBStaticData.currentActivedealer;

		localPlayer = BBStaticData.localPlayer;

		smallBlindPlayerId = BBStaticData.smallBlindPlayerId;

		bigBlindPlayerId = BBStaticData.bigBlindPlayerId;

        //useLimitedRaiseTimes = true;
		lastBet = limitedHight;

	}

}
}