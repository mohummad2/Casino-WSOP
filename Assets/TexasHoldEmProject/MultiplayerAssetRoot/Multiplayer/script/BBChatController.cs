using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
#if USE_PHOTON

namespace BLabHeliproject {

public class BBChatController : Photon.MonoBehaviour {

	public Text TextChatContainer;  
	private List<string> messagesList = new List<string>();
	public InputField chatInputField;
	public Scrollbar verticalScroll;

	public Image ImageMessageAlert;

	// Use this for initialization
	void Start () {
	
	}


  public void gotChatButtonSend() {
		photonView.RPC("SendChatMessage", PhotonTargets.All, chatInputField.text, PhotonNetwork.player.NickName);
  }

  void resetAlertCol() {
   ImageMessageAlert.color = Color.red;
  }

	[PunRPC]
    void SendChatMessage(string text, string sender){
		AddMessage(RemoveHashTag(text) + "#" + sender + "#\n");
        // text # sender #\n
    }

	void AddMessage(string value) {
	  Debug.Log("Chat message : " + value);
	  ImageMessageAlert.color = Color.green;
       messagesList.Add(value);

       TextChatContainer.text = "";

		foreach(string s in messagesList) {
		  string[] val = s.Split('#');
			TextChatContainer.text = TextChatContainer.text + " - " + val[1] + " -> : " + val[0] + "\n";  
		}

	   verticalScroll.value = 0;

    }

	string RemoveHashTag(string input)
    {
    return input.Replace("#", "");
    }
  																						
}
}
#endif
