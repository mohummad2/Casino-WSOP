using UnityEngine;
using System.Collections;

namespace BLabTexasHoldEmProject {

public class BBPlayerAIController : MonoBehaviour {

	// Use this for initialization
	void Awake () {
	    gameObject.AddComponent<BBRiverPhaseAIController>();
		gameObject.AddComponent<BBTurnPhaseAIController>();
		gameObject.AddComponent<BBFlopPhaseAIController>();
		gameObject.AddComponent<BBPreFlopPhaseAIController>();
	}


	public IEnumerator executeRiverResult() {
	  StartCoroutine( GetComponent<BBRiverPhaseAIController>().startExecuteRiverResult() );
	 yield break;
	}


	public IEnumerator executeTurnResult() {
		StartCoroutine( GetComponent<BBTurnPhaseAIController>().startExecuteTurnResult() );
		yield break;
	}


	public IEnumerator executeFlopResult() {
		StartCoroutine( GetComponent<BBFlopPhaseAIController>().startExecuteFlopResult() );
		yield break;
	}

	public IEnumerator executeClosingFlopResult() {
		StartCoroutine( GetComponent<BBFlopPhaseAIController>().startExecuteClosingFlopResult() );
		yield break;
	}

}
}