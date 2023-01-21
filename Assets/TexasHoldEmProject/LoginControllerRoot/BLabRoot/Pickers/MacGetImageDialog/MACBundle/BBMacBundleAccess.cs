using UnityEngine;
using System.Collections;

namespace BLabProjectMultiplayer.LoginController {

public class BBMacBundleAccess : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void requestOpenFileDialogAndSave() {
	   StartCoroutine( BBMacBundleController.openFileDialog() );
	}

	public void requestOpenFileDialogAndSaveAndResize(float newSizeW, float newSizeH) {
		//StartCoroutine( BBMacBundleController.openFileDialogToSaveAndResize(newSizeW,newSizeH) );
	}

}
}