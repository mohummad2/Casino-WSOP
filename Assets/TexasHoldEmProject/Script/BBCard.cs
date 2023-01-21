using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BLabTexasHoldEmProject {

public class BBCard : MonoBehaviour {

	public int bettingPosID = 0;
    public int cardValue = 0;
    public Texture cardValueTexture;
    public Vector2 v2_value;

	// Use this for initialization
	void Start () {
	   if(SceneManager.GetActiveScene().name.Contains("Multiplayer")) {
         transform.Find("UP").GetComponent<MeshRenderer>().material.mainTexture = cardValueTexture;
	   }

		string[] numbers = gameObject.name.Split(new char[] { '_' });
		
		int res = int.Parse(numbers[1]);
		
		if(res == 11 || res == 12 || res == 13) res = 10;
		
		cardValue = res;
	
	}

	public void setCardImage(Vector2 cVal) {
	  Texture2D cardImg = BBStaticData.getCardImage(cVal); 
	  cardValueTexture = cardImg;		   
	  transform.Find("UP").GetComponent<MeshRenderer>().material.mainTexture = cardImg;

	}

}
}