using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLabProjectMultiplayer.LoginController {

public class MultiplayerCommonStaticData : MonoBehaviour {

  public enum PlayerLoggedType { None,Guest,Facebook,Other}
  public static PlayerLoggedType playerLoggedType;

		public const string PUNPlayerPrefsBestPingKey = "PUNCloudBestRegion";

		public static float CoinsToGivePlayerAtGameInstall = 5000;
		public static float MinimumCoinsToStartPlayHand = 500;

		public static float startingBetValue = 10;

		public static Texture2D avatarImageTexture;
		public static string sharedPlayerName = "";
		public static string hiddenPlayerEmail = "";
		public static string avatarImageByte = "";

}
}