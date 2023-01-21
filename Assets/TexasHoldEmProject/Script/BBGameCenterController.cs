using UnityEngine;
using System.Collections;

#if UNITY_IOS && USE_GAME_CENTER	
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.SocialPlatforms;
#endif

namespace BLabTexasHoldEmProject {


public class BBGameCenterController : MonoBehaviour {

	// Use this for initialization
	public void initGC () {
#if USE_GAME_CENTER			
		Social.localUser.Authenticate (ProcessAuthentication);
#endif	

	}

#if USE_GAME_CENTER	

   public void showGCLeaderBoard() {
		Social.ShowLeaderboardUI();
   }

	void postScore() {
		//ReportScore(2000,"iCrapsLBBestScores");
	}
	
	void ProcessAuthentication (bool success) {
		if (success) {
			Debug.Log ("Authenticated, SUCCESS");
			//Social.ShowLeaderboardUI();
			//Invoke("postScore",30);
		}
		else
			Debug.Log ("Failed to authenticate");
	}
	

	public void ReportScore (long score, string leaderboardID) {
		Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore (score, leaderboardID, success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
	}
#endif

}
}