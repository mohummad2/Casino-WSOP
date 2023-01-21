using UnityEngine;
using System.Collections;

namespace BLabTexasHoldEmProject {

public class BBTestSimulationController : MonoBehaviour {

 public enum TestSimulateAiCommand {None,Call,Fold,Raise,Check}

	public BBCreateGameData _BBCreateGameData;


	[System.Serializable]
	public class SimulateCardValuerForPlayerID {
		public Vector2[] simulateAllCardsValue = {new Vector2(1,2),new Vector2(2,4)};
	}

	public SimulateCardValuerForPlayerID[] simulateCardValuerForPlayerID = new SimulateCardValuerForPlayerID[10];

	public Vector2[] flopCards;
	public Vector2   turnCard;
	public Vector2   riverCard;



    [HideInInspector]   
	public BBGlobalDefinitions.GamePhaseDetail[] simulateAiPhasePlayers_FirstBettingRound = new BBGlobalDefinitions.GamePhaseDetail[10];
	public TestSimulateAiCommand[] simulateAiCommandPlayers_FirstBettingRound = new TestSimulateAiCommand[10];
	[HideInInspector]   
	public BBGlobalDefinitions.GamePhaseDetail[] simulateAiPhasePlayers_ClosingPreFlop = new BBGlobalDefinitions.GamePhaseDetail[10];
	public TestSimulateAiCommand[] simulateAiCommandPlayers_ClosingPreFlop = new TestSimulateAiCommand[10];
	[HideInInspector]   
	public BBGlobalDefinitions.GamePhaseDetail[] simulateAiPhasePlayers_Flop = new BBGlobalDefinitions.GamePhaseDetail[10];
	public TestSimulateAiCommand[] simulateAiCommandPlayers_Flop = new TestSimulateAiCommand[10];
	[HideInInspector]   
	public BBGlobalDefinitions.GamePhaseDetail[] simulateAiPhasePlayers_ClosingFlop = new BBGlobalDefinitions.GamePhaseDetail[10];
	public TestSimulateAiCommand[] simulateAiCommandPlayers_ClosingFlop = new TestSimulateAiCommand[10];
	[HideInInspector]   
	public BBGlobalDefinitions.GamePhaseDetail[] simulateAiPhasePlayers_Turn = new BBGlobalDefinitions.GamePhaseDetail[10];
	public TestSimulateAiCommand[] simulateAiCommandPlayers_Turn = new TestSimulateAiCommand[10];

	public bool useSimulate = false;

	// Use this for initialization
	void Start () {
	    for(int x = 0; x < simulateAiPhasePlayers_FirstBettingRound.Length;x++) simulateAiPhasePlayers_FirstBettingRound[x] = BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound;
		for(int x = 0; x < simulateAiPhasePlayers_ClosingPreFlop.Length;x++) simulateAiPhasePlayers_ClosingPreFlop[x] = BBGlobalDefinitions.GamePhaseDetail.ClosingPreFlop;
		for(int x = 0; x < simulateAiPhasePlayers_Flop.Length;x++) simulateAiPhasePlayers_Flop[x] = BBGlobalDefinitions.GamePhaseDetail.Flop;
		for(int x = 0; x < simulateAiPhasePlayers_ClosingFlop.Length;x++) simulateAiPhasePlayers_ClosingFlop[x] = BBGlobalDefinitions.GamePhaseDetail.ClosingFlop;
		for(int x = 0; x < simulateAiPhasePlayers_Turn.Length;x++) simulateAiPhasePlayers_Turn[x] = BBGlobalDefinitions.GamePhaseDetail.Turn;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void popolateGlobalCardsDeckWhitSavedData() {


		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[2]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[3]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[4]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[5]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[6]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[7]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[8]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[9]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[10]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[11]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[12]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[13]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[14]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[15]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[16]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[17]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[18]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[19]);
		// players cards end
		// to burn
		GetComponent<BBGameController>().GlobalDeck.Add(new Vector2(1,1));
        // flop
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[20]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[21]);
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[22]);
		// to burn
		GetComponent<BBGameController>().GlobalDeck.Add(new Vector2(1,2));
		// turn
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[23]);
		// to burn
		GetComponent<BBGameController>().GlobalDeck.Add(new Vector2(1,3));
		// river
		GetComponent<BBGameController>().GlobalDeck.Add(_BBCreateGameData.playersCardsList[24]);



	}


	public void popolateGlobalCardsDeck() {

		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[1].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[1].simulateAllCardsValue[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[2].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[2].simulateAllCardsValue[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[3].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[3].simulateAllCardsValue[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[4].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[4].simulateAllCardsValue[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[5].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[5].simulateAllCardsValue[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[6].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[6].simulateAllCardsValue[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[7].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[7].simulateAllCardsValue[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[8].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[8].simulateAllCardsValue[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[9].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[9].simulateAllCardsValue[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[0].simulateAllCardsValue[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(simulateCardValuerForPlayerID[0].simulateAllCardsValue[1]);
		// players cards end
		// to burn
		GetComponent<BBGameController>().GlobalDeck.Add(new Vector2(1,1));
        // flop
		GetComponent<BBGameController>().GlobalDeck.Add(flopCards[0]);
		GetComponent<BBGameController>().GlobalDeck.Add(flopCards[1]);
		GetComponent<BBGameController>().GlobalDeck.Add(flopCards[2]);
		// to burn
		GetComponent<BBGameController>().GlobalDeck.Add(new Vector2(1,2));
		// turn
		GetComponent<BBGameController>().GlobalDeck.Add(turnCard);
		// to burn
		GetComponent<BBGameController>().GlobalDeck.Add(new Vector2(1,3));
		// river
		GetComponent<BBGameController>().GlobalDeck.Add(riverCard);



	}
}
}