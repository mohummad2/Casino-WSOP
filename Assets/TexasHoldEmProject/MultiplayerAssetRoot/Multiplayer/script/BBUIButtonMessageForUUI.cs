using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace BLabHeliproject {

public class BBUIButtonMessageForUUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler{
	// Use this for initialization
	public enum _EventType {click, buttUP, ButtDown}
	public _EventType useEvent;
	
	public GameObject toSendGameObject;
	public string toSendGameObjectTag = "GameController";
	public bool includeChildren = false;
	public string functionToCall = "myFunction";
	public bool wantAutoFind = false;
	public bool recursiveFinding = false;
	
	//private GameObject ContainerShowTracks;
	
	public void reserLinkConnection() {
		if(wantAutoFind) {
			
			if(recursiveFinding) {
				InvokeRepeating("recursiveController",1,1);
			} else {
				toSendGameObject = GameObject.FindGameObjectWithTag(toSendGameObjectTag);
			}
		}   

	}
	
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
	
	public void BBOnPointerExit(GameObject _go)
	{
	//	ContainerShowTracks.SetActive(false);
	}
	
	public void BBOnPointerEnter(GameObject _go)
	{
		Debug.Log("OnPointerEnter : " + _go.name );
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
	

	void clickCall() {
		
		if(toSendGameObject) {
			
			if(includeChildren)
				toSendGameObject.BroadcastMessage(functionToCall,gameObject,SendMessageOptions.DontRequireReceiver);  
			else
				toSendGameObject.SendMessage(functionToCall,gameObject,SendMessageOptions.DontRequireReceiver);  
			
		}
	}
	
}
}

