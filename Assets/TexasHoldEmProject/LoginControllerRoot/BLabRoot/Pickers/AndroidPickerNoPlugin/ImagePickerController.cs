using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using com.blab.imagePicker;
using BLabTexasHoldEmProject;

namespace BLabProjectMultiplayer.LoginController {

public class ImagePickerController : MonoBehaviour
{


	public enum LoginAccessResult {None,RegisterSuccess,RegisterFail,LoginSuccess,LoginFail}
	LoginAccessResult loginAccessResult;

	public string URLPlayersDataPath = "http://www.blab-dev.com/_newapps/playerLoginCheckDemoGeniTeam/players/";
	public string URLUploadPlayersDataPath = "http://www.blab-dev.com/_newapps/playerLoginCheckDemoGeniTeam/addPlayer.php";

    public Text TextMessage;

	public RawImage ImageAlbum;
	public RawImage RawImageCurrentAvatarCam;

	public Image ImageRounded;

#if UNITY_ANDROID
	[SerializeField]
	private BLabImagePicker imagePicker;
#endif

	[HideInInspector]
	public List<string> galleryImagesAndroid;
	public GameObject[] AndroidObjects;
	public GameObject[] MacObjects;
	public GameObject[] IOSObjects;
	public GameObject[] WindowsObjects;



	int currentSelectedImgIdx = 0;
	public InputField InputFieldRegisterEmail;
	public InputField InputFieldRegisterPlayerName;

	string avatarStringToSave = "";

	public GameObject[] getImageWithCamObjects;
	public GameObject[] getImageWithAlbumObjects;

	public WebCamTexture webcamTexture;
	//int frontCamIdx = 0;
	string frontCamName = "";
	public Toggle ToggleGetCamImage;
	public Button _buttbuttonGetImageCamAndroid;


    private IEnumerator LoadImage(string path) {

            var url = "file://" + path;
            var www = new WWW(url);
            yield return www;

            var texture = www.texture;
            if (texture == null)
            {
                Debug.LogError("Failed to load texture url:" + url);
            }

			ImageAlbum.texture = texture;
			setRoundedPicture();
    }

	void Awake() {

#if UNITY_ANDROID

            if(imagePicker == null) {
				imagePicker = GameObject.Find("BLabImagePicker").GetComponent<BLabImagePicker>();
            }

            imagePicker.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path));
            };
#endif


     }

	IEnumerator Start() {

		

		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
			Debug.Log("HasUserAuthorization : YES");
        } else {
			Debug.Log("HasUserAuthorization : NO");
        }


#if UNITY_WEBGL

#else				
		WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++) {
            Debug.Log("name : " + devices[i].name + " if front : " + devices[i].isFrontFacing);
			if(devices[i].isFrontFacing) {
			 //frontCamIdx = i;
			 frontCamName =  devices[i].name;
			 break;
			}
        }

		webcamTexture = new WebCamTexture(frontCamName);
		RawImageCurrentAvatarCam.texture = webcamTexture;
		RawImageCurrentAvatarCam.material.mainTexture = webcamTexture;

#endif

		Debug.Log("Application.platform : " + Application.platform);


	   switch(Application.platform) {
	    case RuntimePlatform.Android:
			 foreach(GameObject g in MacObjects) g.SetActive(false);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(false);
			 foreach(GameObject g in WindowsObjects) g.SetActive(false);

			//setAlbumImagesPathList();
			//	foreach(string s in galleryImagesAndroid) Debug.Log(s);
			//ImageAlbum.texture = getAlbumImageAndroid(0);
			//setRoundedPicture();
			 
	    break;
		case RuntimePlatform.OSXPlayer: case RuntimePlatform.OSXEditor:
			 foreach(GameObject g in MacObjects) g.SetActive(true);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(false);
			 foreach(GameObject g in WindowsObjects) g.SetActive(false);
			 _buttbuttonGetImageCamAndroid.gameObject.SetActive(false);
		break;
		case RuntimePlatform.WindowsPlayer:
			 foreach(GameObject g in MacObjects) g.SetActive(false);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(false);
			 foreach(GameObject g in WindowsObjects) g.SetActive(true);
			 ToggleGetCamImage.interactable = false;
			 activateCam();
			 _buttbuttonGetImageCamAndroid.gameObject.SetActive(false);
		break;
		case RuntimePlatform.IPhonePlayer:
			 foreach(GameObject g in MacObjects) g.SetActive(false);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(true);
			 foreach(GameObject g in WindowsObjects) g.SetActive(false);
			 _buttbuttonGetImageCamAndroid.gameObject.SetActive(false);

		break;
			case RuntimePlatform.WebGLPlayer:
			 
		break;
	   }

	   yield break;

/*
#if UNITY_ANDROID// && !UNITY_EDITOR
		List<string> galleryImages = GetAllGalleryImagePaths();
		foreach(string s in galleryImages) {
		  Debug.Log(s);
		}


		for(int x = 0; x < galleryImages.Count;x++) {
		   Texture2D t = new Texture2D(2, 2);
		   (new WWW(galleryImages[x])).LoadImageIntoTexture(t);

		   blab.utility.scripting.BBStaticData.ScaleTexture(t,64,64);

		   ImageAlbum.texture = t;
		   yield return new WaitForSeconds(0.5f);

		   Debug.Log(x + ")t ---->> : " + t.dimension);
		}
#endif
*/

	}

	public void buttonGetImageCamAndroid() {

			if(ToggleGetCamImage.isOn) {
		       Texture2D snap = new Texture2D(webcamTexture.width, webcamTexture.height);
			   snap.SetPixels(webcamTexture.GetPixels());
			   snap.Apply();
			   Texture2D newTex = BBStaticVariable.ScaleTexture(snap,64,64);
				Texture2D rTex = BBStaticVariable.CalculateTexture(64,64,32,32,32,newTex);
		       Sprite tmpSprite = Sprite.Create (rTex, new Rect(0,0,rTex.width,rTex.height), new Vector2(.5f,.5f));
		       ImageRounded.sprite = tmpSprite;
		    } else {
#if UNITY_ANDROID
				imagePicker.Show("Select Image", "AppBlabImagePicker", 1024);
#endif
		    }
	}

	public void buttonGetImageIOS() {
		if(ToggleGetCamImage.isOn) {
			   Texture2D snap = new Texture2D(webcamTexture.width, webcamTexture.height);
			   snap.SetPixels(webcamTexture.GetPixels());
			   snap.Apply();
				Texture2D newTex = BBStaticVariable.ScaleTexture(snap,64,64);
				Texture2D rTex = BBStaticVariable.CalculateTexture(64,64,32,32,32,newTex);
		       Sprite tmpSprite = Sprite.Create (rTex, new Rect(0,0,rTex.width,rTex.height), new Vector2(.5f,.5f));
		       ImageRounded.sprite = tmpSprite;
		} else {
#if UNITY_IPHONE
		  LoadTextureFromImagePicker.ShowPhotoLibrary(gameObject.name, "OnFinishedImagePickerIOS");
#endif
		}
	}

	private void OnFinishedImagePickerIOS (string message) {
		#if UNITY_IPHONE
		Texture2D texture = LoadTextureFromImagePicker.GetLoadedTexture(message, 512, 512);
		if (texture) {
			ImageAlbum.texture = texture;
			setRoundedPicture();
		}
		#endif
	}

	public void buttonGetImageWindows() {
            Texture2D snap = new Texture2D(webcamTexture.width, webcamTexture.height);
            snap.SetPixels(webcamTexture.GetPixels());
			   snap.Apply();
			Texture2D newTex = BBStaticVariable.ScaleTexture(snap,64,64);
			Texture2D rTex = BBStaticVariable.CalculateTexture(64,64,32,32,32,newTex);
		       Sprite tmpSprite = Sprite.Create (rTex, new Rect(0,0,rTex.width,rTex.height), new Vector2(.5f,.5f));
		       ImageRounded.sprite = tmpSprite;
	}

	public void buttonGetImageMac() {
	   if(ToggleGetCamImage.isOn) {

			   Texture2D snap = new Texture2D(webcamTexture.width, webcamTexture.height);
			   snap.SetPixels(webcamTexture.GetPixels());
			   snap.Apply();
				Texture2D newTex = BBStaticVariable.ScaleTexture(snap,64,64);
				Texture2D rTex = BBStaticVariable.CalculateTexture(64,64,32,32,32,newTex);
		       Sprite tmpSprite = Sprite.Create (rTex, new Rect(0,0,rTex.width,rTex.height), new Vector2(.5f,.5f));
		       ImageRounded.sprite = tmpSprite;


	   } else {
		  GetComponent<BBMacBundleAccess>().requestOpenFileDialogAndSave();
	   }
	}

	void gotImageFromMac(Texture2D tex) {
		ImageAlbum.texture = tex;
		setRoundedPicture();
	}

	IEnumerator executeSaveRegisterData(GameObject _goButt) {

		yield return StartCoroutine(checkPlayerIfGood(InputFieldRegisterEmail.text));

		yield return new WaitForEndOfFrame();

		Debug.Log("executeSaveRegisterData : " + loginAccessResult);

		switch(loginAccessResult) {
		 case LoginAccessResult.RegisterFail:
			TextMessage.text = "User Already Exsists...";
			_goButt.GetComponent<Button>().interactable = true;
			yield break;
		 //break;
		 case LoginAccessResult.RegisterSuccess:
			TextMessage.text = "Register Success...";
		 break;
		}

		loginAccessResult = LoginAccessResult.None;

		yield return new WaitForSeconds(2);

		TextMessage.text = "Saving...";

		yield return StartCoroutine(uploadPlayerData(InputFieldRegisterEmail.text,InputFieldRegisterPlayerName.text));

		yield return new WaitForEndOfFrame();

		Debug.Log("uploadPlayerData : " + loginAccessResult);

		switch(loginAccessResult) {
		 case LoginAccessResult.RegisterFail: case LoginAccessResult.None:
			TextMessage.text = "Fail To Save data...";
		 break;
		 case LoginAccessResult.RegisterSuccess:
			TextMessage.text = "Save Success...";
			PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER_SHARE_PICTURE, avatarStringToSave);
			PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.PLAYER__SHARE_NAME, InputFieldRegisterPlayerName.text);
			PlayerPrefs.SetString(ProductionMainMenuController.PlayerPreferKeyNames.GOT_PLAYER_INFO_DATA,"YES");
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		 break;
		}

	}

	public void closeAndSaveRegisterdata(GameObject _go) {

		if(webcamTexture.isPlaying) {
		 webcamTexture.Stop();
		}

		TextMessage.text = "";

		_go.GetComponent<Button>().interactable = false;

		   if(InputFieldRegisterEmail.text.Length > 5 && InputFieldRegisterPlayerName.text.Length > 4) {
				TextMessage.text = "Please Wait...";
			    StartCoroutine(executeSaveRegisterData(_go));
		    } else {
				TextMessage.text = "Input Not Valid, Try Again(Data Lower 4 Characters)";
				_go.GetComponent<Button>().interactable = true;
		    }
	}

	public void getNextImageButton() {
		//int nextImg = 

		if(currentSelectedImgIdx < galleryImagesAndroid.Count) {
			currentSelectedImgIdx++;
			ImageAlbum.texture = getAlbumImageAndroid(currentSelectedImgIdx);
			setRoundedPicture();
		}

		Debug.Log(galleryImagesAndroid.Count + ")getNextImageButton>>> : " + currentSelectedImgIdx);

	}

	public void getPreviousImageButton() {
		if(currentSelectedImgIdx > 0) {
			currentSelectedImgIdx--;
			ImageAlbum.texture = getAlbumImageAndroid(currentSelectedImgIdx);
			setRoundedPicture();
		}
	}

	public Texture2D getAlbumImageAndroid(int idx) {
		Texture2D toRet = new Texture2D(2, 2);
		(new WWW(galleryImagesAndroid[idx])).LoadImageIntoTexture(toRet);
			BBStaticVariable.ScaleTexture(toRet,64,64);
		return toRet;
	}

	public void setAlbumImagesPathList() {
		galleryImagesAndroid = GetAllGalleryImagePaths();
	}

	private List<string> GetAllGalleryImagePaths() {

         List<string> results = new List<string>();
         HashSet<string> allowedExtesions = new HashSet<string>() { ".png", ".jpg",  ".jpeg"  };
 
         try
         {
             AndroidJavaClass mediaClass = new AndroidJavaClass("android.provider.MediaStore$Images$Media");
 
             // Set the tags for the data we want about each image.  This should really be done by calling; 
             //string dataTag = mediaClass.GetStatic<string>("DATA");
             // but I couldn't get that to work...
             
             const string dataTag = "_data";
 
             string[] projection = new string[] { dataTag };
             AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
             AndroidJavaObject currentActivity = player.GetStatic<AndroidJavaObject>("currentActivity");
 
             string[] urisToSearch = new string[] { "EXTERNAL_CONTENT_URI", "INTERNAL_CONTENT_URI" };
             foreach (string uriToSearch in urisToSearch)
             {
                 AndroidJavaObject externalUri = mediaClass.GetStatic<AndroidJavaObject>(uriToSearch);
                 AndroidJavaObject finder = currentActivity.Call<AndroidJavaObject>("managedQuery", externalUri, projection, null, null, null);
                 bool foundOne = finder.Call<bool>("moveToFirst");
                 while (foundOne)
                 {
                     int dataIndex = finder.Call<int>("getColumnIndex", dataTag);
                     string data = finder.Call<string>("getString", dataIndex);
                     if (allowedExtesions.Contains(Path.GetExtension(data).ToLower()))
                     {
                         string path = @"file:///" + data;
                         results.Add(path);
							Debug.Log("path : " + path);
                     }
 
                     foundOne = finder.Call<bool>("moveToNext");
                 }
             }
         }
         catch (System.Exception e)
         {
				Debug.Log("Exception : " + e.Message);
             // do something with error...
         }
 
         return results;
     }


	void setRoundedPicture() {
			Texture2D newTex = BBStaticVariable.ScaleTexture((Texture2D)ImageAlbum.texture,64,64);
			Texture2D rTex = BBStaticVariable.CalculateTexture(64,64,32,32,32,newTex);
		        Sprite tmpSprite = Sprite.Create (rTex, new Rect(0,0,rTex.width,rTex.height), new Vector2(.5f,.5f));
		        ImageRounded.sprite = tmpSprite;
	}

	IEnumerator checkPlayerIfGood(string eMail) {
		 WWW w = new WWW(URLPlayersDataPath + eMail);

         yield return w;

			if(!string.IsNullOrEmpty(w.error)) {
			    Debug.Log("error checkPlayerIfGood : " + w.error);
			    loginAccessResult = LoginAccessResult.RegisterSuccess;
             } else {
			    Debug.Log("text checkPlayerIfGood : " + w.text);
				loginAccessResult = LoginAccessResult.LoginFail;
				yield break;
             }

	}

	IEnumerator uploadPlayerData(string eMail,string nickName) {


		     Texture2D avatar = ImageRounded.sprite.texture;
			string avatarValue = BBStaticVariable.getStringByteFromTexture(avatar);
			// string hexVal = blab.utility.scripting.BBStaticData.ConvertStringToHex(avatarValue);

			 avatarStringToSave = avatarValue;

		     string playerData = eMail + "#" + nickName; // + "#" + hexVal;

		     WWWForm form = new WWWForm();
			 form.AddField("action", playerData);
			 form.AddField("fname", eMail);

			 WWW dataPost = new WWW(URLUploadPlayersDataPath, form);

			yield return dataPost;

		    if(!string.IsNullOrEmpty(dataPost.error)) {
				Debug.Log("uploadPlayerData FAIL : " + dataPost.error);
				loginAccessResult = LoginAccessResult.RegisterFail;
			} else {
				Debug.Log("uploadPlayerData SUCCESS : " + dataPost.text);
				loginAccessResult = LoginAccessResult.RegisterSuccess;
			}

	}

	public void onToggleGetCamImage(Toggle t) {
		Debug.Log("onToggleGetCamImage  : " + t.isOn);

		if(t.isOn) {
			activateCam();
		} else {
			disactivateCam();
		}
	}

	void activateCam() {

		switch(Application.platform) {
	     case RuntimePlatform.Android:
			 foreach(GameObject g in MacObjects) g.SetActive(false);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(false);
			 foreach(GameObject g in WindowsObjects) g.SetActive(false);
			 _buttbuttonGetImageCamAndroid.interactable = true;
			AndroidObjects[0].GetComponent<Button>().interactable = false;
			AndroidObjects[1].GetComponent<Button>().interactable = false;

	     break;
	     case RuntimePlatform.IPhonePlayer:
			 foreach(GameObject g in MacObjects) g.SetActive(false);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(true);
			 foreach(GameObject g in WindowsObjects) g.SetActive(false);
	     break;
		 case RuntimePlatform.OSXEditor:  case RuntimePlatform.OSXPlayer: 
			 foreach(GameObject g in MacObjects) g.SetActive(true);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(false);
			 foreach(GameObject g in WindowsObjects) g.SetActive(false);
		 break;
		 case RuntimePlatform.WindowsPlayer:  case RuntimePlatform.WindowsEditor:
			 foreach(GameObject g in MacObjects) g.SetActive(false);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(false);
			 foreach(GameObject g in WindowsObjects) g.SetActive(true);
		 break;
	    }

	  foreach(GameObject g in getImageWithAlbumObjects) {
	   g.SetActive(false);
	  }
	  foreach(GameObject g in getImageWithCamObjects) {
	   g.SetActive(true);
	  }

            webcamTexture.Play();


        }

	void disactivateCam() {

	   webcamTexture.Stop();

		switch(Application.platform) {
	     case RuntimePlatform.Android:
			 foreach(GameObject g in MacObjects) g.SetActive(false);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(false);
			 foreach(GameObject g in WindowsObjects) g.SetActive(false);

			_buttbuttonGetImageCamAndroid.interactable = true;

			AndroidObjects[0].GetComponent<Button>().interactable = true;
			AndroidObjects[1].GetComponent<Button>().interactable = true;

	     break;
	     case RuntimePlatform.IPhonePlayer:
			 foreach(GameObject g in MacObjects) g.SetActive(false);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(true);
			 foreach(GameObject g in WindowsObjects) g.SetActive(false);
	     break;
		 case RuntimePlatform.OSXEditor:  case RuntimePlatform.OSXPlayer:
			 foreach(GameObject g in MacObjects) g.SetActive(true);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(false);
			 foreach(GameObject g in WindowsObjects) g.SetActive(false);
		 break;
		 case RuntimePlatform.WindowsPlayer:  case RuntimePlatform.WindowsEditor:
			 foreach(GameObject g in MacObjects) g.SetActive(false);
		     foreach(GameObject g in AndroidObjects) g.SetActive(false);
			 foreach(GameObject g in IOSObjects) g.SetActive(false);
			 foreach(GameObject g in WindowsObjects) g.SetActive(true);
		 break;
	    }

	  foreach(GameObject g in getImageWithAlbumObjects) {
	   g.SetActive(true);
	  }
	  foreach(GameObject g in getImageWithCamObjects) {
	   g.SetActive(false);
	  }

	}

}
}