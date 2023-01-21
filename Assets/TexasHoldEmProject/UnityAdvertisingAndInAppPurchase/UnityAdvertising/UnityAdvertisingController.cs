using UnityEngine;
using UnityEngine.UI;

#if USE_UNITY_ADV && (UNITY_ANDROID || UNITY_IOS)
using UnityEngine.Advertisements;
#endif

namespace BLab.GetCoinsSystem {

public class UnityAdvertisingController : MonoBehaviour {


	public const string zoneIdVideo = "video";
	public const string zoneIdrewardedVideo = "rewardedVideo";

	public static float moneyOnRewardedVideo = 1000;

	static InAppConfigScriptableAsset iap;



#if (USE_UNITY_ADV || USE_UNITY_ADV_REWARDED) && (UNITY_ANDROID || UNITY_IOS)

	static string lastAdvRequest = "video";

	// Use this for initialization
	void Start () {
		Debug.Log("[UnityAdvertisingController][Start] 1 ");

		iap = Resources.Load("InAppConfigScriptableAsset") as InAppConfigScriptableAsset;
			Debug.Log("[UnityAdvertisingController][Start] 2 ");

		moneyOnRewardedVideo = iap.moneyToAddOnRewardedVideo;

		Debug.Log("[UnityAdvertisingController][Start] moneyOnRewardedVideo : " + moneyOnRewardedVideo);


	   if(Advertisement.isInitialized) {

	   } else {
#if UNITY_ANDROID
	        Advertisement.Initialize(iap.UnityServiceAdvertisingAndroidID,true);
#endif
#if UNITY_IOS
			Advertisement.Initialize(iap.UnityServiceAdvertisingIOSID,true);
#endif
	   }

	}
	
	public static void ShowAdPlacement (string zoneId) {

		Debug.Log("Advertisement.isInitialized : " + Advertisement.isInitialized);
		Debug.Log("Advertisement.Ready : " + Advertisement.IsReady());
		Debug.Log("Advertisement.zoneId : " + zoneId);
			Debug.Log("iap == null : " + (iap == null));

		lastAdvRequest = zoneId;

		if(iap == null) return;

		if(zoneId.Equals(zoneIdVideo)) {
		  //if(PlayerPrefs.HasKey(iap.notConsumableProductsList[0].playerPrefDefValue)) {
		  //  return;
		  //}
		}
              var options = new ShowOptions();
              options.resultCallback = HandleShowResult;

              if(Advertisement.IsReady()) {
			    Debug.Log("Advertisement.Show");
                Advertisement.Show(zoneId, options);
              }

     }

	static void HandleShowResult (ShowResult result) {

		Debug.Log ("Video HandleShowResult. result : " + result.ToString());

                    switch(result)
                    {
                     case ShowResult.Finished:
			             if(lastAdvRequest.Equals(zoneIdVideo)) {

			             } else {
					      float totMoney = PlayerPrefs.GetFloat("MPGeneralPlayerMoney");
			              totMoney += moneyOnRewardedVideo;
					      PlayerPrefs.SetFloat("MPGeneralPlayerMoney",totMoney);
				          Debug.Log ("Video completed. Offer a reward to the player. totMoney : " + totMoney);
			             }
                        break;
                     case ShowResult.Skipped:
                        Debug.LogWarning ("Video was skipped.");
                        break;
                     case ShowResult.Failed:
                        Debug.LogError ("Video failed to show.");
                        break;
                    }

     }

#endif

}
}