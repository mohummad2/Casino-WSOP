using System;
using System.Collections.Generic;
using UnityEngine;

namespace BLab.GetCoinsSystem {

[Serializable]
	public class InAppConfigScriptableAsset : ScriptableObject {

  [System.Serializable]
   public class ConsumableProductsList {
     public string GPlayIOSProductID = "100.gold.coins";
	 public string MacStoreProductID = "100.gold.coins.mac";
	 public string TizenProductID = "000000596581";
	 public string MoolahAppStoreProductID = "com.ee";
	 public bool isEnabled = true;
	 public string smallDescription;
	 public float valueInSimulateDollars = 10000;
	 [Header("*** NOT CHANGE ***")]
	 public string playerPrefDefValue;
   }

	public ConsumableProductsList[] consumableProductsList;

	[System.Serializable]
   public class NOTConsumableProductsList {
     public string GPlayIOSProductID = "100.gold.coins";
	 public string MacStoreProductID = "100.gold.coins.mac";
	 public string TizenProductID = "000000596581";
	 public string MoolahAppStoreProductID = "com.ee";
	 public bool isEnabled = true;
	 public string smallDescription;
	 [Header("*** NOT CHANGE ***")]
	 public string playerPrefDefValue;
   }

	public NOTConsumableProductsList[] notConsumableProductsList;


	[System.Serializable]
	public class GameMoneySettings {
	  public float playerInitialMoney = 100000;
	  public float playerMINMoneyToRefund = 10000;	
	  public float playerMINMoneyToStartPlayGame = 1000;	

	}

	public GameMoneySettings gameMoneysettings;

	[HideInInspector]
	public bool useInAppPurchase = true;
	[HideInInspector]
	public bool useUnityAdvertising = true;
	[HideInInspector]
	public bool useUnityAdvertisingRewardToGetMoney = true;

	public float moneyToAddOnRewardedVideo = 1000;

	public string UnityServiceAdvertisingAndroidID = "1313223";
	public string UnityServiceAdvertisingIOSID = "1313224";



}
}