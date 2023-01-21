using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BBUIButtonMessageForUUIMultiplayer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
	// Use this for initialization
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
	
	void recursiveController() {
	
		toSendGameObject = GameObject.FindGameObjectWithTag(toSendGameObjectTag);
		
		if(toSendGameObject) {
			CancelInvoke("recursiveController");
		}
	
	
	}
	
	void Start () {
	
	//  button = GetComponent<Button>();
	//  button.onClick.AddListener(clickCall);
	  
	    if(wantAutoFind) {
	       
	        if(recursiveFinding) {
				InvokeRepeating("recursiveController",1,1);
	        } else {
	          toSendGameObject = GameObject.FindGameObjectWithTag(toSendGameObjectTag);
            }
	   	}   
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void clickCall() {
	
	   
		  if(includeChildren)
			toSendGameObject.BroadcastMessage(functionToCall,gameObject,SendMessageOptions.DontRequireReceiver);  
		  else {
			 if(toSendGameObject) {
		       toSendGameObject.SendMessage(functionToCall,gameObject,SendMessageOptions.DontRequireReceiver);  
		     } else {
				InvokeRepeating("recursiveController",0.01f,0.1f);
		     }
	      }
	}
	
}



/*
for (int i = 0; i < 10; i++)
            {
                GameObject inst = (GameObject)Instantiate(textPrefab);
                inst.GetComponent<Text>().text = contentString;
 
                inst.transform.SetParent(layoutGroup.transform, false);

*/


