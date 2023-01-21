using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

namespace BLabTexasHoldEmProject {

public class BBMenuController : EditorWindow {

   public static List<string> saveOnFileLog;

	[MenuItem("Window/BBMenu/Activate Save Log On File")]
	public static void ActivateSaveLogOnFile() {
	  saveOnFileLog = new List<string>();
	}

	[MenuItem("Window/BBMenu/Save Last Hand Cards")]
	public static void SaveLastHandCards() {

			BBLastCardsHand lastCards = (BBLastCardsHand)AssetDatabase.LoadAssetAtPath("Assets/TexasHoldEmProject/ScriptableAssets/BBLastCardsHand.asset",typeof(BBLastCardsHand));
			List<string> cards = new List<string>();

			foreach(Vector2 v in lastCards.playersCardsList) {
			   cards.Add( v.x + "=" + v.y );
			}

			saveStringList(cards,"Lastcards");

			GameObject go = GameObject.FindGameObjectWithTag("GameController");
			List<string> tmpData = new List<string>();

			foreach(BBTestSimulationController.TestSimulateAiCommand sm in go.GetComponent<BBTestSimulationController>().simulateAiCommandPlayers_ClosingFlop) {
			   tmpData.Add(sm.ToString());
			}
			saveStringList(tmpData,"simulateAiCommandPlayers_ClosingFlop");

			tmpData.Clear();
				
			foreach(BBTestSimulationController.TestSimulateAiCommand sm in go.GetComponent<BBTestSimulationController>().simulateAiCommandPlayers_ClosingPreFlop) {
			   tmpData.Add(sm.ToString());
			}
			saveStringList(tmpData,"simulateAiCommandPlayers_ClosingPreFlop");

			tmpData.Clear();
				
			foreach(BBTestSimulationController.TestSimulateAiCommand sm in go.GetComponent<BBTestSimulationController>().simulateAiCommandPlayers_FirstBettingRound) {
			   tmpData.Add(sm.ToString());
			}
			saveStringList(tmpData,"simulateAiCommandPlayers_FirstBettingRound");

			tmpData.Clear();
				
			foreach(BBTestSimulationController.TestSimulateAiCommand sm in go.GetComponent<BBTestSimulationController>().simulateAiCommandPlayers_Flop) {
			   tmpData.Add(sm.ToString());
			}
			saveStringList(tmpData,"simulateAiCommandPlayers_Flop");


			tmpData.Clear();
				
			foreach(BBTestSimulationController.TestSimulateAiCommand sm in go.GetComponent<BBTestSimulationController>().simulateAiCommandPlayers_Turn) {
			   tmpData.Add(sm.ToString());
			}
			saveStringList(tmpData,"simulateAiCommandPlayers_Turn");

	}

  static void saveStringList(List<string> dataList, string fileName) {
			if(File.Exists("Assets/" + fileName)) {
				File.Delete("Assets/" + fileName);
		}
			System.IO.File.WriteAllLines("Assets/" + fileName, dataList.ToArray());
  } 

  static List<string> getStringList(string fileName) {
		List<string> tempRet = new List<string>();

		using (StreamReader r = new StreamReader(fileName)) {
	        string line;
	        while ((line = r.ReadLine()) != null)
	        {
		    tempRet.Add(line);
	        }
	    }
	    return tempRet;
  }   

  public static void saveDataOnLogFile(string data) {
			saveOnFileLog.Add(data);
			saveStringList(saveOnFileLog,"handLogFile");
  }

}
}