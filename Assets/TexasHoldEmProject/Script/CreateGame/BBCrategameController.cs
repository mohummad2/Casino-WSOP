using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace BLabTexasHoldEmProject {

public class BBCrategameController : MonoBehaviour {

	public BBCreateGameData _BBCreateGameData;

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
    gameObject.AddComponent<BBGetScreenShoot>();
#endif

	}

	void gotButtonClick(GameObject _go) {

	  switch(_go.name) {
		case "ButtonSaveData":
		 _BBCreateGameData.saveData();
	  break;
		case "ButtonReset":
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	  break;
		case "ButtonPopolateWithSaved":
			popolateTable();
	  break;
		case "ButtonMainMenu":
			SceneManager.LoadScene("MainMenu");
		break;
			
	  }

	}

	void popolateTable() {

	   Vector2[] savedCardsdata = _BBCreateGameData.playersCardsList;

	  List<GameObject> cardsPrefabList = new List<GameObject>();

	  GameObject[] cpl = GameObject.FindGameObjectsWithTag("cardOnSlot");

	  foreach(GameObject g in cpl) { cardsPrefabList.Add(g); }

	  for(int x = 0;x < savedCardsdata.Length;x++) {
			string name = savedCardsdata[x].x.ToString() + "_" + savedCardsdata[x].y;

			GameObject result = cardsPrefabList.Find(item => item.name == name);

			Debug.Log("result : " + result.name + " name " + name);

			string nameToPut = "TextCardPosition_" + x.ToString();

			GameObject toPutGO = GameObject.Find(nameToPut);

			result.transform.SetParent(toPutGO.transform);

	  }

		// TextCardPosition_0
	    //	result = valList.Find(item => item == toCheckStarting)

	}
			
}
}