using UnityEngine;
using System.Collections;

using BLabProjectMultiplayer.LoginController;

namespace BLabTexasHoldEmProject {

public class BBSoundController : MonoBehaviour {

 public AudioClip pickCard;
 public AudioClip moveCard;
 public AudioClip chipHandle;
 public AudioClip cardsShuffle;
 public AudioClip genBip;
 public AudioClip rotateCard;
 [Header("Only Multiplayer")]
 public AudioClip cleanCards;
 [Header("Only Multiplayer")]
 public AudioClip cleanChips;

 public AudioClip backGroundMusic;

 public AudioSource audioSourceMusic;
 public AudioSource audioSourceSounds;
 public float musicVolume = 2.5f;

 BBGlobalDefinitions _BBGlobalDefinitions;

   void Awake() {
		_BBGlobalDefinitions = Resources.Load("BBGlobalDefinitions") as BBGlobalDefinitions;
   }

	// Use this for initialization
	void Start () {


	  if(_BBGlobalDefinitions.UseLoginSystem) {

		   //Debug.Log("********************* " + PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.MUSIC_ON));

		  if(PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.MUSIC_ON) == "ON") {
				audioSourceMusic.clip = backGroundMusic;
				audioSourceMusic.volume = musicVolume;
				audioSourceMusic.Play();
		  }
	  }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playClip(AudioClip ac) {

			if(_BBGlobalDefinitions.UseLoginSystem) {
				if(PlayerPrefs.GetString(ProductionMainMenuController.PlayerPreferKeyNames.SOUNDS_ON) == "ON") {
		         audioSourceSounds.PlayOneShot(ac);
	            }
			} else {
				audioSourceSounds.PlayOneShot(ac);
			}
	}

}
}