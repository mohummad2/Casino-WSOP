using UnityEngine;
using UnityEditor;
using System.Collections;

public class BBMenuEditor : EditorWindow {

	[MenuItem("Window/BBMenu/Delete PlayerPref")]
	public static void BBDeletePlayerpref() {

		if(EditorUtility.DisplayDialog("Delete All PlayerPref ?", "Are you sure you want delete all PlayerPref(Can't UNDO) : ", "YES", "NO")) { 
			PlayerPrefs.DeleteAll();
		}

	}
	

}
