using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BLab.GetCoinsSystem {

public class BBUIButtonMessageForUUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler{
	public enum _EventType {click, buttUP, ButtDown}
	public _EventType useEvent;
	
	Button button;
	
	public GameObject toSendGameObject;
	public string toSendGameObjectTag = "GameController";
	public bool includeChildren = false;
	public string functionToCall = "myFunction";
	public bool wantAutoFind = false;
	public bool recursiveFinding = false;
	
	public void setTargetUGUI(GameObject _target) {
	
	    toSendGameObject = _target;
	  
	}
	
	public void OnPointerDown(PointerEventData eventData)
	{
		 if(useEvent == _EventType.ButtDown) clickCall();		
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		if(useEvent == _EventType.buttUP) clickCall();		
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if(useEvent == _EventType.click) clickCall();		
	}

	public void BBOnPointerEnter(GameObject _go)
	{
	}

	
	void recursiveController() {
		toSendGameObject = GameObject.FindGameObjectWithTag(toSendGameObjectTag);
		if(toSendGameObject) {
			CancelInvoke("recursiveController");
		}
	}
	
	void Start () {
	    if(wantAutoFind) {
	        if(recursiveFinding) {
				InvokeRepeating("recursiveController",1,1);
	        } else {
	          toSendGameObject = GameObject.FindGameObjectWithTag(toSendGameObjectTag);
            }
	   	}   
	}
	
	public void clickCall() {

		GameObject tap = Resources.Load("tapPrefab") as GameObject;
		if(tap != null) {
		  GameObject _tap = Instantiate(tap);
		  Destroy(_tap,1);
		}

	   if(toSendGameObject) {
		  if(includeChildren)
			toSendGameObject.BroadcastMessage(functionToCall,gameObject,SendMessageOptions.DontRequireReceiver);  
		  else
		    toSendGameObject.SendMessage(functionToCall,gameObject,SendMessageOptions.DontRequireReceiver);  
	   }
	}
	
}
}
