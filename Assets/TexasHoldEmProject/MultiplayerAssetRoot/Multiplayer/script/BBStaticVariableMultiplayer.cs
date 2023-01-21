using UnityEngine;
using System.Collections;

public class BBStaticVariableMultiplayer  {
#region multiplayerTmpData  	
#if USE_PHOTON
	public static CloudRegionCode selectedRegionCode = CloudRegionCode.eu;
    public static string currentMPPlayerName = "";
	public static string currentMPRoomName = "";
	public static int currentMPmaxPlayerNumber = 10;
	public static string photonConnectionVersion = "99.8.0";
	public static bool gotPlayerLoggedIn = false;
	public static bool UseDirectFindASeatThenGoIn = false;
#endif
#endregion	
}
