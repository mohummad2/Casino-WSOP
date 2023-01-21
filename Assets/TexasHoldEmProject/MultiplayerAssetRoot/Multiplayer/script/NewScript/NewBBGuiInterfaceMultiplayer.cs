#if USE_PHOTON
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Photon;

namespace BLabTexasHoldEmProject {

public class NewBBGuiInterfaceMultiplayer : Photon.MonoBehaviour {

	public Transform GameActionButtonsRoot;
	public GameObject ImageCardsContainerOnButtons;

	[HideInInspector]
	public Text TextLocalPlayerGeneralMoney;
	[HideInInspector]
    public Text TextGamePhase;
	[HideInInspector]
    public Text TextGamePhaseInfo;
	[HideInInspector]
    public Text TextGamePOT;
	[HideInInspector]
    public Text TextGameLastBetvalue;
	[HideInInspector]
    public Text TextGameSTACK;
	[HideInInspector]
	public Text TextGameRAISECounter;
	[HideInInspector]
	public Text TextGameRAISEValue;

	NewGameControllerMultiplayer _GameController; 

	public Image ImageGameGeneralPhase;

	private Button buttonGameCALL;
	private Button buttonGameFOLD;
	private Button buttonGameRAISE;
	//private Button buttonGameCHECK;
	private Button buttonGameALLIN;

	void Start () {

	   _GameController = GetComponent<NewGameControllerMultiplayer>();

	   //ImageGameGeneralPhase = GameObject.Find("ImageGameGeneralPhase").GetComponent<Image>();

	   TextLocalPlayerGeneralMoney = GameObject.Find("TextLocalPlayerGeneralMoney").GetComponent<Text>();
	   //TextGameSTACK = GameObject.Find("TextGameSTACK").GetComponent<Text>();
	   //TextGameSTACK.text = BBStaticData.getMoneyValue( BBStaticVariable.gameLimitedStackValue );
	   //TextGamePhase = GameObject.Find("TextGamePhase").GetComponent<Text>();
	   //TextGamePhase.text = "Phase : " + "Wait...";
	   //TextGamePhaseInfo = GameObject.Find("TextGamePhaseInfo").GetComponent<Text>();
	   //TextGamePhaseInfo.text = BBStaticData.phaseMessageInfoWaitGameStarting;
	   //TextGameRAISECounter = GameObject.Find("TextGameRAISECounter").GetComponent<Text>();
	   //TextGameRAISECounter.text = "";

	   //TextGameRAISEValue = GameObject.Find("TextGameRAISEValue").GetComponent<Text>();
	   TextGamePOT = GameObject.Find("TextGamePOT").GetComponent<Text>();
	   //TextGameLastBetvalue = GameObject.Find("TextGameLastBetvalue").GetComponent<Text>();
	   //TextGameLastBetvalue.text = "";

		buttonGameCALL = GameObject.Find("buttonGameCALL").GetComponent<Button>();
		buttonGameCALL.interactable = false;
		buttonGameFOLD = GameObject.Find("buttonGameFOLD").GetComponent<Button>();
		buttonGameFOLD.interactable = false;
		buttonGameRAISE = GameObject.Find("buttonGameRAISE").GetComponent<Button>();
		buttonGameRAISE.interactable = false;
		//buttonGameCHECK = GameObject.Find("buttonGameCHECK").GetComponent<Button>();
		//buttonGameCHECK.interactable = false;
		buttonGameALLIN = GameObject.Find("buttonGameALLIN").GetComponent<Button>();
		buttonGameALLIN.interactable = false;


	   setActionButtonPosition(false);

		if(PhotonNetwork.player.IsLocal) {
			float tmpValue = PlayerPrefs.GetFloat("MPGeneralPlayerMoney");
			TextLocalPlayerGeneralMoney.text = BBStaticData.getMoneyValue(tmpValue);
		}
	
	}


	public void setActionButtonPosition(bool wantShow) {
		Image[] imList = GameActionButtonsRoot.GetComponentsInChildren<Image>(true); foreach(Image i in imList) i.enabled = wantShow;
		Text[] txList = GameActionButtonsRoot.GetComponentsInChildren<Text>(true); foreach(Text t in txList) t.enabled = wantShow;
	}

	public void activatePlayerToTalk(int playerID) {

		GetComponent<BBSoundController>().playClip(GetComponent<BBSoundController>().genBip);

			setActionButtonPosition(true);

		   switch(_GameController._BBGlobalDefinitions.gamePhaseDetail) {
			case BBGlobalDefinitions.GamePhaseDetail.FirstBettingRound: setGameButtons(true,true,true,false,false); break;
			case BBGlobalDefinitions.GamePhaseDetail.ClosingMoneyAllign: setGameButtons(true,true,false,false,false); break;
			case BBGlobalDefinitions.GamePhaseDetail.ClosingPreFlop: setGameButtons(true,true,false,false,false); break;
			case BBGlobalDefinitions.GamePhaseDetail.Flop: setGameButtons(true,true,true,false,false); break;
			case BBGlobalDefinitions.GamePhaseDetail.Turn: setGameButtons(true,true,true,false,false); break;
			case BBGlobalDefinitions.GamePhaseDetail.ClosingTurn: setGameButtons(true,true,false,false,false); break;
			case BBGlobalDefinitions.GamePhaseDetail.River: setGameButtons(true,true,true,false,false); break;
			case BBGlobalDefinitions.GamePhaseDetail.ClosingRiver: setGameButtons(true,true,false,false,false); break;

			case BBGlobalDefinitions.GamePhaseDetail.AllIn: setGameButtons(true,true,false,false,false,playerID); break;

		   }

	}

	public void setGameButtons(bool call,bool fold,bool raise,bool check, bool allin,int playerId) {

        check = false;

	    //buttonGameCHECK.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = check; buttonGameCHECK.interactable = check; buttonGameCHECK.GetComponent<Image>().enabled = check; buttonGameCHECK.GetComponentInChildren<Text>().enabled = check;
		buttonGameCALL.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = call; buttonGameCALL.interactable = call; buttonGameCALL.GetComponent<Image>().enabled = call; buttonGameCALL.GetComponentInChildren<Text>().enabled = call;
		buttonGameFOLD.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = fold; buttonGameFOLD.interactable = fold; buttonGameFOLD.GetComponent<Image>().enabled = fold; buttonGameFOLD.GetComponentInChildren<Text>().enabled = fold;
		buttonGameRAISE.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = raise; buttonGameRAISE.interactable = raise; buttonGameRAISE.GetComponent<Image>().enabled = raise; buttonGameRAISE.GetComponentInChildren<Text>().enabled = raise;

		if(_GameController._BBGlobalDefinitions.gameType == BBGlobalDefinitions.GameType.Limited) {
		    allin = false;
		} else {
		     PhotonPlayer pp = _GameController._NewMultiplayerHelper.getPhotonPlayer(_GameController.playerDataList,playerId);
		     string playerIsUnderAllIn = _GameController.getStringPlayerProperties(pp,NewGameControllerMultiplayer.CustomPropertiesKeyList.isPlayerUnderAllIn);
		  
			 if(_GameController._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.Flop ||
				_GameController._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.Turn ||
				_GameController._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.River
		     ) {
		        if(playerIsUnderAllIn == "Y") {
		           allin = true;
		        } else {
		          allin = false;
		        }
		     } else {
			   allin = false;
		     }

			buttonGameALLIN.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = allin; 
			buttonGameALLIN.interactable = allin; 
			buttonGameALLIN.GetComponent<Image>().enabled = allin; 
			buttonGameALLIN.GetComponentInChildren<Text>().enabled = allin;
		}

	}


   public void setGameButtons(bool call,bool fold,bool raise,bool check, bool allin) {

        check = false;

	    //buttonGameCHECK.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = check; buttonGameCHECK.interactable = check; buttonGameCHECK.GetComponent<Image>().enabled = check; buttonGameCHECK.GetComponentInChildren<Text>().enabled = check;
		buttonGameCALL.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = call; buttonGameCALL.interactable = call; buttonGameCALL.GetComponent<Image>().enabled = call; buttonGameCALL.GetComponentInChildren<Text>().enabled = call;
		buttonGameFOLD.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = fold; buttonGameFOLD.interactable = fold; buttonGameFOLD.GetComponent<Image>().enabled = fold; buttonGameFOLD.GetComponentInChildren<Text>().enabled = fold;
		buttonGameRAISE.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = raise; buttonGameRAISE.interactable = raise; buttonGameRAISE.GetComponent<Image>().enabled = raise; buttonGameRAISE.GetComponentInChildren<Text>().enabled = raise;

		if(_GameController._BBGlobalDefinitions.gameType == BBGlobalDefinitions.GameType.Limited) {
		    allin = false;
		} else {
			 if(_GameController._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.Flop ||
				_GameController._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.Turn ||
				_GameController._BBGlobalDefinitions.gamePhaseDetail == BBGlobalDefinitions.GamePhaseDetail.River
		     ) {
		       allin = true;
		     } else {
			   allin = false;
		     }

			buttonGameALLIN.gameObject.GetComponent<BBUIButtonMessageForUUI>().enabled = allin; 
			buttonGameALLIN.interactable = allin; 
			buttonGameALLIN.GetComponent<Image>().enabled = allin; 
			buttonGameALLIN.GetComponentInChildren<Text>().enabled = allin;
		}

			ImageCardsContainerOnButtons.SetActive(true);

			foreach(BBPlayerData pd in _GameController.playerDataList) {
			   if(pd.isLocalPlayer) {
			     Texture img_1 = pd.transform_card_1.gameObject.GetComponent<BBCard>().cardValueTexture;
				 Texture img_2 = pd.transform_card_2.gameObject.GetComponent<BBCard>().cardValueTexture;
				 GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_1").GetComponent<RawImage>().texture = img_1;
		         GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_2").GetComponent<RawImage>().texture = img_2;
		         break;
			   }
			}

			    if(_GameController.flopCardList[0] == Vector2.zero) {
				  GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_FLOP_1").GetComponent<RawImage>().enabled = false;
				} else {
				GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_FLOP_1").GetComponent<RawImage>().texture = BBStaticData.getCardImage(_GameController.flopCardList[0]);
					GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_FLOP_1").GetComponent<RawImage>().enabled = true;
				}
			    if(_GameController.flopCardList[1] == Vector2.zero) {
				  GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_FLOP_2").GetComponent<RawImage>().enabled = false;
				} else {
				GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_FLOP_2").GetComponent<RawImage>().texture = BBStaticData.getCardImage(_GameController.flopCardList[1]);
					GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_FLOP_2").GetComponent<RawImage>().enabled = true;
				}
			    if(_GameController.flopCardList[2] == Vector2.zero) {
				  GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_FLOP_3").GetComponent<RawImage>().enabled = false;
				} else {
				GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_FLOP_3").GetComponent<RawImage>().texture = BBStaticData.getCardImage(_GameController.flopCardList[2]);
					GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_FLOP_3").GetComponent<RawImage>().enabled = true;
				}
			    if(_GameController.turnCard == Vector2.zero) {
					GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_TURN").GetComponent<RawImage>().enabled = false;
				} else {
				GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_TURN").GetComponent<RawImage>().texture = BBStaticData.getCardImage(_GameController.turnCard);
					GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_TURN").GetComponent<RawImage>().enabled = true;
				}
			    if(_GameController.riverCard == Vector2.zero) {
					GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_RIVER").GetComponent<RawImage>().enabled = false;
				} else {
				GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_RIVER").GetComponent<RawImage>().texture = BBStaticData.getCardImage(_GameController.riverCard);
					GameActionButtonsRoot.Find("ImageCardsContainer/ImageCARD_RIVER").GetComponent<RawImage>().enabled = true;
				}


	}

}
}
#endif