#if USE_PHOTON
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BLabTexasHoldEmProject {

public class BBTestResult : MonoBehaviour {

	public NewResultEngine _newResultEngine; 
	int progr = 0;
	int session = 0;
	public void getScreenShoot() {
		ScreenCapture.CaptureScreenshot("_Screenshot_" + session.ToString() + "_" + progr.ToString() + ".png");	
		progr++;	
	}

	void Start() {

	     session = UnityEngine.Random.Range(1,100000);

			GameObject.Find("Dropdown_player_0_1_seme").GetComponent<Dropdown>().value = 2; onDropDownValueChange(GameObject.Find("Dropdown_player_0_1_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_player_0_1_carta").GetComponent<Dropdown>().value = 2;
			GameObject.Find("Dropdown_player_0_2_seme").GetComponent<Dropdown>().value = 3; onDropDownValueChange(GameObject.Find("Dropdown_player_0_2_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_player_0_2_carta").GetComponent<Dropdown>().value = 3;

			GameObject.Find("Dropdown_player_1_1_seme").GetComponent<Dropdown>().value = 3; onDropDownValueChange(GameObject.Find("Dropdown_player_1_1_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_player_1_1_carta").GetComponent<Dropdown>().value = 3;
			GameObject.Find("Dropdown_player_1_2_seme").GetComponent<Dropdown>().value = 0; onDropDownValueChange(GameObject.Find("Dropdown_player_1_2_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_player_1_2_carta").GetComponent<Dropdown>().value = 4;

			GameObject.Find("Dropdown_player_2_1_seme").GetComponent<Dropdown>().value = 2; onDropDownValueChange(GameObject.Find("Dropdown_player_2_1_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_player_2_1_carta").GetComponent<Dropdown>().value = 1;
			GameObject.Find("Dropdown_player_2_2_seme").GetComponent<Dropdown>().value = 1; onDropDownValueChange(GameObject.Find("Dropdown_player_2_2_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_player_2_2_carta").GetComponent<Dropdown>().value = 8;

			GameObject.Find("Dropdown_fold_1_seme").GetComponent<Dropdown>().value = 0; onDropDownValueChange(GameObject.Find("Dropdown_fold_1_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_fold_1_carta").GetComponent<Dropdown>().value = 6;

			GameObject.Find("Dropdown_fold_2_seme").GetComponent<Dropdown>().value = 2; onDropDownValueChange(GameObject.Find("Dropdown_fold_2_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_fold_2_carta").GetComponent<Dropdown>().value = 4;

			GameObject.Find("Dropdown_fold_3_seme").GetComponent<Dropdown>().value = 0; onDropDownValueChange(GameObject.Find("Dropdown_fold_3_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_fold_3_carta").GetComponent<Dropdown>().value = 5;

			GameObject.Find("Dropdown_turn_seme").GetComponent<Dropdown>().value = 3; onDropDownValueChange(GameObject.Find("Dropdown_turn_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_turn_carta").GetComponent<Dropdown>().value = 7;

			GameObject.Find("Dropdown_river_seme").GetComponent<Dropdown>().value = 3; onDropDownValueChange(GameObject.Find("Dropdown_river_seme").GetComponent<Dropdown>());
			GameObject.Find("Dropdown_river_carta").GetComponent<Dropdown>().value = 0;

	}

	// Use this for initialization
	void executeResult () {



/* royal flush
		    data.flopCards[0] = new Vector2(3,10);
			data.flopCards[1] = new Vector2(3,11);
			data.flopCards[2] = new Vector2(3,12);
			data.turnCard = new Vector2(3,13);
			data.riverCard = new Vector2(2,3);
			data.playerCard_1 = new Vector2(4,3);
			data.playerCard_2 = new Vector2(3,1);
*/
/* straight Flush
			data.flopCards[0] = new Vector2(3,4);
			data.flopCards[1] = new Vector2(3,5);
			data.flopCards[2] = new Vector2(3,6);
			data.turnCard = new Vector2(3,7);
			data.riverCard = new Vector2(2,3);
			data.playerCard_1 = new Vector2(4,3);
			data.playerCard_2 = new Vector2(3,8);
*/
/* poker
			data.flopCards[0] = new Vector2(3,4);
			data.flopCards[1] = new Vector2(3,5);
			data.flopCards[2] = new Vector2(4,5);
			data.turnCard = new Vector2(2,5);
			data.riverCard = new Vector2(2,3);
			data.playerCard_1 = new Vector2(1,5);
			data.playerCard_2 = new Vector2(3,8);
*/

/*/ full
			data.flopCards[0] = new Vector2(3,13);
			data.flopCards[1] = new Vector2(4,13);
			data.flopCards[2] = new Vector2(4,1);
			data.turnCard = new Vector2(2,5);
			data.riverCard = new Vector2(2,3);
			data.playerCard_1 = new Vector2(1,1);
			data.playerCard_2 = new Vector2(2,13);
*/

/*/ flush
			data.flopCards[0] = new Vector2(3,3);
			data.flopCards[1] = new Vector2(4,7);
			data.flopCards[2] = new Vector2(4,1);
			data.turnCard = new Vector2(2,5);
			data.riverCard = new Vector2(4,9);
			data.playerCard_1 = new Vector2(4,1);
			data.playerCard_2 = new Vector2(4,13);
*/

/*/ straight
			data.flopCards[0] = new Vector2(4,5);
			data.flopCards[1] = new Vector2(4,6);
			data.flopCards[2] = new Vector2(4,1);
			data.turnCard = new Vector2(4,7);
			data.riverCard = new Vector2(4,8);
			data.playerCard_1 = new Vector2(1,1);
			data.playerCard_2 = new Vector2(4,9);
*/

/*/tris
			data.flopCards[0] = new Vector2(4,5);
			data.flopCards[1] = new Vector2(2,5);
			data.flopCards[2] = new Vector2(4,1);
			data.turnCard = new Vector2(4,7);
			data.riverCard = new Vector2(4,8);
			data.playerCard_1 = new Vector2(1,10);
			data.playerCard_2 = new Vector2(1,5);
*/

/*/ twoPair

			data.flopCards[0] = new Vector2(4,12);
			data.flopCards[1] = new Vector2(2,12);
			data.flopCards[2] = new Vector2(4,1);
			data.turnCard = new Vector2(0,0);
			data.riverCard = new Vector2(0,0);
			data.playerCard_1 = new Vector2(3,5);
			data.playerCard_2 = new Vector2(1,5);
*/
/*/pair
			data.flopCards[0] = new Vector2(4,12);
			data.flopCards[1] = new Vector2(2,12);
			data.flopCards[2] = new Vector2(4,1);
			data.turnCard = new Vector2(4,7);
			data.riverCard = new Vector2(1,6);
			data.playerCard_1 = new Vector2(3,5);
			data.playerCard_2 = new Vector2(1,4);
*/
/* /highCard
			data.flopCards[0] = new Vector2(4,12);
			data.flopCards[1] = new Vector2(2,10);
			data.flopCards[2] = new Vector2(4,1);
			data.turnCard = new Vector2(4,7);
			data.riverCard = new Vector2(1,6);
			data.playerCard_1 = new Vector2(3,5);
			data.playerCard_2 = new Vector2(1,4);
*/

	   Text TextResult = GameObject.Find("TextResult").GetComponent<Text>();
	   TextResult.text = "";
  
       BBPlayerData p_1 = new BBPlayerData();
       p_1.playerPosition = 0;
       //p_1.card_1_Value = new Vector2(3,5);
	   //p_1.card_2_Value = new Vector2(2,5);

	   p_1.card_1_Value = new Vector2((float)GameObject.Find("Dropdown_player_0_1_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_player_0_1_carta").GetComponent<Dropdown>().value+1);
	   p_1.card_2_Value = new Vector2((float)GameObject.Find("Dropdown_player_0_2_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_player_0_2_carta").GetComponent<Dropdown>().value+1);
	   
	   BBPlayerData p_2 = new BBPlayerData();
       p_2.playerPosition = 1;
	   p_2.card_1_Value = new Vector2((float)GameObject.Find("Dropdown_player_1_1_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_player_1_1_carta").GetComponent<Dropdown>().value+1);
	   p_2.card_2_Value = new Vector2((float)GameObject.Find("Dropdown_player_1_2_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_player_1_2_carta").GetComponent<Dropdown>().value+1);

	   BBPlayerData p_3 = new BBPlayerData();
       p_3.playerPosition = 2;
	   p_3.card_1_Value = new Vector2((float)GameObject.Find("Dropdown_player_2_1_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_player_2_1_carta").GetComponent<Dropdown>().value+1);
	   p_3.card_2_Value = new Vector2((float)GameObject.Find("Dropdown_player_2_2_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_player_2_2_carta").GetComponent<Dropdown>().value+1);

       List<BBPlayerData> pdList = new List<BBPlayerData>();
	   if(GameObject.Find("TogglePlayer_0").GetComponent<Toggle>().isOn) pdList.Add(p_1);
	   if(GameObject.Find("TogglePlayer_1").GetComponent<Toggle>().isOn) pdList.Add(p_2);
	   if(GameObject.Find("TogglePlayer_2").GetComponent<Toggle>().isOn) pdList.Add(p_3);


	   List<Vector2> flopCardsList = new List<Vector2>();
/*	        flopCardsList.Add(new Vector2(3,9));
			flopCardsList.Add(new Vector2(1,5));
			flopCardsList.Add(new Vector2(2,11));
			Vector2 TurnCard = new Vector2(1,11);
			Vector2 RiverCard = new Vector2(4,3);*/

			flopCardsList.Add(new Vector2((float)GameObject.Find("Dropdown_fold_1_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_fold_1_carta").GetComponent<Dropdown>().value+1));
			flopCardsList.Add(new Vector2((float)GameObject.Find("Dropdown_fold_2_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_fold_2_carta").GetComponent<Dropdown>().value+1));
			flopCardsList.Add(new Vector2((float)GameObject.Find("Dropdown_fold_3_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_fold_3_carta").GetComponent<Dropdown>().value+1));
			Vector2 TurnCard = new Vector2((float)GameObject.Find("Dropdown_turn_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_turn_carta").GetComponent<Dropdown>().value+1);
			Vector2 RiverCard = new Vector2((float)GameObject.Find("Dropdown_river_seme").GetComponent<Dropdown>().value+1,(float)GameObject.Find("Dropdown_river_carta").GetComponent<Dropdown>().value+1);


	   for(int x = 0;x < pdList.Count;x++) {
			NewResultEngine.CompleteResultStruct data = new NewResultEngine.CompleteResultStruct();
			NewResultEngine.CompleteResultStruct dataOut = new NewResultEngine.CompleteResultStruct();
			data.playerCard_1 = pdList[x].card_1_Value;
			data.playerCard_2 = pdList[x].card_2_Value;
			data.flopCards[0] = flopCardsList[0];
			data.flopCards[1] = flopCardsList[1];
			data.flopCards[2] = flopCardsList[2];

			if(GameObject.Find("ToggleTurnCard").GetComponent<Toggle>().isOn) data.turnCard = TurnCard;
			 else data.turnCard = Vector2.zero;

			if(GameObject.Find("ToggleRiverCard").GetComponent<Toggle>().isOn) data.riverCard = RiverCard;
			 else data.riverCard = Vector2.zero;

            data.playerIdx = pdList[x].playerPosition;

			Debug.Log("playerCard_1 : " + data.playerCard_1 + "  playerCard_2 : " + data.playerCard_2);
			Debug.Log("data.flopCards[0] : " + data.flopCards[0] + "  data.flopCards[1] : " + data.flopCards[1] + "  data.flopCards[2] : " + data.flopCards[2]);
			Debug.Log("data.turnCard : " + data.turnCard + "  data.riverCard : " + data.riverCard);


			_newResultEngine.getPlayerCardsResult(pdList[x].playerPosition,data,out dataOut);
			pdList[x].completeResultStruct = dataOut;
	   }

	   int bestResult = 10;
	   for(int x = 0;x < pdList.Count;x++) {
		 Debug.Log("-----------------------------------------------");
	     Debug.Log("++++++++++ Result idx : " + pdList[x].playerPosition);
		 Debug.Log("++++++++++ Result result : " + pdList[x].completeResultStruct.cardsResultValues);
		 TextResult.text = TextResult.text + "[" + pdList[x].playerPosition + "] cardsResultValues : " + pdList[x].completeResultStruct.cardsResultValues + "\n"; 
		 string bestFive = "";
		  foreach(int i in pdList[x].completeResultStruct.bestFive) {
		   bestFive = bestFive + i + " : ";
		  }
		 Debug.Log(bestFive);
		 TextResult.text = TextResult.text + "[" + pdList[x].playerPosition + "] bestFive : " + bestFive + " kiker :" + pdList[x].completeResultStruct.kikers.ToString() + "\n"; 
		 Debug.Log("-----------------------------------------------");
		 if((int)pdList[x].completeResultStruct.cardsResultValues < bestResult) bestResult = (int)pdList[x].completeResultStruct.cardsResultValues;
	   }

			Debug.Log("bestResult : " + (NewResultEngine.CardsResultValues)bestResult);

			List<BBPlayerData> pdWinnerList = new List<BBPlayerData>();

			for(int x = 0;x < pdList.Count;x++) {
				if(pdList[x].completeResultStruct.cardsResultValues == (NewResultEngine.CardsResultValues)bestResult) {
				  pdWinnerList.Add(pdList[x]);
				}
			}

			Debug.Log("pdWinnerList Count : " + pdWinnerList.Count);
			for(int x = 0;x < pdWinnerList.Count;x++) {
				TextResult.text = TextResult.text + "Winner : " + pdWinnerList[x].playerPosition + " result : " + pdWinnerList[x].completeResultStruct.cardsResultValues + "\n";
			}


			if(pdWinnerList.Count > 1) {
				List<BBPlayerData> _res = new List<BBPlayerData>();
				switch((NewResultEngine.CardsResultValues)bestResult) {
				 case NewResultEngine.CardsResultValues.HighCard:
				    _res = _newResultEngine.getWinnerCheck_HighCard(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 TextResult.text = TextResult.text + "FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"; 
				   }
				 break;
				 case NewResultEngine.CardsResultValues.Pair:
				   _res = _newResultEngine.getWinnerCheck_Pair(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 TextResult.text = TextResult.text + "FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"; 
				   }
				 break;
				 case NewResultEngine.CardsResultValues.TwoPair:
				   _res = _newResultEngine.getWinnerCheck_TwoPair(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 TextResult.text = TextResult.text + "FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"; 
				   }
				 break;
				 case NewResultEngine.CardsResultValues.ThreeOfAkind:
				   _res = _newResultEngine.getWinnerCheck_Tris(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 TextResult.text = TextResult.text + "FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"; 
				   }
				 break;
				 case NewResultEngine.CardsResultValues.Straight:
				   _res = _newResultEngine.getWinnerCheck_Straight(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 TextResult.text = TextResult.text + "FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"; 
				   }
				 break;
				 case NewResultEngine.CardsResultValues.Flush:
				   _res = _newResultEngine.getWinnerCheck_Flush(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 TextResult.text = TextResult.text + "FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"; 
				   }
				 break;
				 case NewResultEngine.CardsResultValues.FullHouse:
					_res = _newResultEngine.getWinnerCheck_FullHouse(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 TextResult.text = TextResult.text + "FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"; 
				   }
				 break;
				 case NewResultEngine.CardsResultValues.Poker:
					_res = _newResultEngine.getWinnerCheck_Poker(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 TextResult.text = TextResult.text + "FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"; 
				   }
				 break;
				 case NewResultEngine.CardsResultValues.StraightFlush:
					_res = _newResultEngine.getWinner_StraightFlush(pdWinnerList);
				   foreach(BBPlayerData pd in _res) {
					 TextResult.text = TextResult.text + "FINAL WIN NUMBER : " + _res.Count + " WINNER : " + pd.playerPosition + "\n"; 
				   }
				 break;
				}
			}

	}


  public void getResult() {

			executeResult();

  }

  public void onDropDownValueChange(Dropdown dd) {

     switch(dd.value) {
            case 0: dd.gameObject.GetComponent<Image>().color = Color.red;break;
			case 1: dd.gameObject.GetComponent<Image>().color = Color.green;break;
			case 2: dd.gameObject.GetComponent<Image>().color = Color.yellow;break;
			case 3: dd.gameObject.GetComponent<Image>().color = Color.magenta;break;
     }

  }

}
}
#endif