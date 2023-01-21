using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BLabProjectMultiplayer.LoginController {

public class ProductionLoginController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	


	public void setLocalPlayerData (string sharedName,string hiddeneMail,Texture2D sharedPlayerImage) {
			MultiplayerCommonStaticData.avatarImageTexture = sharedPlayerImage;
			MultiplayerCommonStaticData.sharedPlayerName = sharedName;
			MultiplayerCommonStaticData.hiddenPlayerEmail = hiddeneMail;
			MultiplayerCommonStaticData.avatarImageByte = BLabTexasHoldEmProject.BBStaticVariable.getStringByteFromTexture(sharedPlayerImage);
#if USE_PHOTON
			PhotonNetwork.playerName = sharedName;
#endif

	}
}
}
