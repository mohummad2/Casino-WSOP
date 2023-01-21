using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.IO;
using UnityEngine.UI;
using System.Text;

namespace BLabProjectMultiplayer.LoginController {

public class BBMacBundleController: MonoBehaviour {

    public static RawImage testImage;


	[DllImport ("BBOpenDialogFile")]
	extern static private void _getFilePath();

	[DllImport ("BBOpenDialogFile")]
	extern static private IntPtr _getSelectedFile();

	[DllImport ("BBOpenDialogFile")]
	extern static private bool _saveImageAtPath(string message);

	[DllImport ("BBOpenDialogFile")]
	extern static private bool _saveImageAtPathAndResize(string message,float newSizeW, float newSizeH);



	public static IEnumerator openFileDialogToSaveAndResize(float newSizeW, float newSizeH,System.Action<Texture2D > retValue) {

			string newPath = Application.persistentDataPath +  BLabTexasHoldEmProject.BBStaticVariable.tmpMacImageSaved + ".png";

		bool result = (_saveImageAtPathAndResize(string.Format ("{0}", newPath),newSizeW,newSizeH));

		Debug.Log("openFileDialog ===================>>> : " + result + " newPath : " + newPath);

		  if(result) {
					WWW www = new WWW(@"file://" + newPath);

				    yield return www;

					if (!string.IsNullOrEmpty(www.error)) {

					} else {
						 Texture2D tex = new Texture2D(256,256);
					     www.LoadImageIntoTexture(tex); 
						 if(result)
						   retValue(tex);
						 else retValue(null);
					}
		  } else {
			retValue(null);
		  }

		  
		//testImage = GameObject.Find("RawImageForTest").GetComponent<RawImage>();

		//testImage.texture = tex;

		//Debug.Log("openFileDialog ========SIZE===========>>> : " + tex.width + " : " + tex.height);


	}


	public static IEnumerator openFileDialog()
	{

	   string newPath = Application.persistentDataPath + "/bbTempImage.png";

		// Call plugin only when running on real device
		//if (Application.platform == RuntimePlatform.OSXPlayer)
		//_getFilePath();
		//string result = Marshal.PtrToStringAuto( _saveImageAtPath(string.Format ("{0}", newPath)));

		bool result = (_saveImageAtPath(string.Format ("{0}", newPath)));

		Debug.Log("openFileDialog ===================>>> : " + result + " newPath : " + newPath);

		WWW www = new WWW(@"file://" + newPath);

	    yield return www;

		Texture2D tex = new Texture2D(256,256);

		www.LoadImageIntoTexture(tex); 

		//testImage = GameObject.Find("RawImageCurrentAvatar").GetComponent<RawImage>();


		//testImage.texture = tex;

		GameObject.Find("PanelMainRegister").SendMessage("gotImageFromMac",tex,SendMessageOptions.DontRequireReceiver);




		//Debug.Log( Marshal.PtrToStringAuto (_getSelectedFile()));

		//byte []bytes = System.Text.Encoding.UTF8.GetBytes(result);
		//byte[] bytes = Encoding.ASCII.GetBytes(result);

		// byte[] bytes = result.ToCharArray(); //System.Text.Encoding.UTF8.GetBytes(result);

		 //foreach(byte b in bytes) Debug.Log(b);

		//Debug.Log("openFileDialog ===============path ====>>> : " + path);

/*
		testImage = GameObject.Find("RawImageForTest").GetComponent<RawImage>();
		SphereTest = GameObject.Find("SphereTest");
		MeshRenderer mr = SphereTest.GetComponent<MeshRenderer>();


		Texture2D tex = new Texture2D(256,256);

		(new WWW(@"file//" + newPath)).LoadImageIntoTexture(tex);

        testImage.texture = tex;

        mr.sharedMaterial.mainTexture = tex;
*/

/* 
	string path = Application.temporaryCachePath + "/bbtestImage.jpg";

     if (File.Exists(path))     {
         fileData = File.ReadAllBytes(path);
         tex = new Texture2D(2, 2);
         tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
			Debug.Log("openFileDialog ====FOUND===========path ====>>> : " + path);

     } else {
			Debug.Log("openFileDialog =========NOT FOUND======path ====>>> : " + path);

     }

		//testImage.texture = LoadPNG(path);

*/
	}



   public static Texture2D LoadPNG(string filePath) {
 
     Texture2D tex = null;
     byte[] fileData;
 
     if (File.Exists(filePath))     {
         fileData = File.ReadAllBytes(filePath);
         tex = new Texture2D(2, 2);
         tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
     }
     return tex;
 }

}
}