using System;
using System.Collections.Generic;
using UnityEngine;

namespace BLabTexasHoldEmProject {

[Serializable]
public class BBCreateGameData : ScriptableObject {

  public Vector2[] playersCardsList = new Vector2[25];

  public bool isDataComplete = false;

	public void saveData() {

	  bool gotCompleteData = true;

	  GameObject[] slotContainerList = GameObject.FindGameObjectsWithTag("slotContainer");

		for(int x = 0; x < slotContainerList.Length;x++) {

		   BBDragHandler card = slotContainerList[x].GetComponentInChildren<BBDragHandler>();
			Debug.Log("  : " + slotContainerList[x].name);
		   if(card) {
				string[] slot = slotContainerList[x].name.Split(new char[] { '_' });
		        int idx = int.Parse(slot[1]);
				string[] cardV = card.gameObject.name.Split(new char[] { '_' });
				float val_1 = float.Parse(cardV[0]);
				float val_2 = float.Parse(cardV[1]);
						  playersCardsList[idx] = new Vector2(val_1,val_2);
//				          Debug.Log("gotCard : " + card.gameObject.name + " : " + val_1 + " : " + val_2);

		   } else {
//				Debug.Log("gotCard **** ERROR NO CARD ****");
				gotCompleteData = false;
		   }

      }

       if(gotCompleteData) {
        isDataComplete = true;
       } else {
         isDataComplete = false;
       }

	}

}
}