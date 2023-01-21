
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
#if USE_FACEBOOK
using Facebook.Unity;
using Facebook.MiniJSON;
#endif

using BLabProjectMultiplayer.LoginController;

public class BBFacebookController : MonoBehaviour {

#if USE_FACEBOOK
	List<string> permissions = new List<string>() { "public_profile", "email", "user_friends" };
	public string ProfileName;
	public Text TProfileName;
	public Image ImageFBProfile;

	public Sprite playerAvatar;

	public Text TextLoginReresult;

	public ProductionLoginControllerAtStart productionLoginControllerAtStart;

   public void buttonsController(GameObject _go) {
     switch(_go.name) {
	  case "ButtonFBInit":
			FB.Init(this.OnInitComplete, this.OnHideUnity);
           
      break;
	  case "ButtonFBLogIn":
			FB.LogInWithReadPermissions(permissions, LoginFacebookCallBack);
	  break;
     }
   }

   public void executeFBLogin() {
		FB.LogInWithReadPermissions(permissions, LoginFacebookCallBack);

   }

	// Use this for initialization
	void Start () {
#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
		FB.Init(this.OnInitComplete, this.OnHideUnity);
#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnInitComplete()
    {
		Debug.Log("FB OnInitComplete init : " + FB.IsInitialized + " loggedIn : " + FB.IsLoggedIn);

    }

        private void OnHideUnity(bool isGameShown)
        {
        }

	private void LoginFacebookCallBack(ILoginResult loginResult)
    {
        if (loginResult == null)
        {
            Debug.Log("Could not log in to facebook.");
			TextLoginReresult.text = "Could not log in to facebook.";
			productionLoginControllerAtStart.gotFacebookResult(false);
            return;
        }

		if (string.IsNullOrEmpty(loginResult.Error) && !loginResult.Cancelled && !string.IsNullOrEmpty(loginResult.RawResult))
        {
            Debug.Log("Success while logging into Facebook.");
            GetProfile();


        } else {
			productionLoginControllerAtStart.gotFacebookResult(false);

			Debug.Log("Could not log in to facebook.");

        }


    }

	public void GetProfile() {
        FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);

    }

	void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
             ProfileName = "" + result.ResultDictionary["first_name"];
			 if(TProfileName) TProfileName.text = ProfileName;

			Debug.Log("FB ProfileName : " + ProfileName);

			FB.API("/me/picture?type=square&height=100&width=100", HttpMethod.GET, delegate (IGraphResult picResult) {

				if (picResult.Texture != null) {
					TextLoginReresult.text = "FB Login Success";
					playerAvatar = Sprite.Create(picResult.Texture, new Rect(0, 0, 64, 64), new Vector2());
					if(ImageFBProfile) ImageFBProfile.sprite = playerAvatar;
					productionLoginControllerAtStart.gotFacebookResult(true);

	            }
	            else
	            {
	                Debug.Log(picResult.RawResult);
	                TextLoginReresult.text = "FB Login Fail...";
					productionLoginControllerAtStart.gotFacebookResult(false);
	            }

			});

        }
        else
        {
            Debug.Log(result.Error);
			TextLoginReresult.text = "FB Login Fail...";
			productionLoginControllerAtStart.gotFacebookResult(false);
        }
    }


#endif
}
