using UnityEngine;
using System.Collections;

namespace BLabHeliproject {

public class BBUIDragAndHide : MonoBehaviour {

    public enum MoveMode {RightToLeft,LeftToRight,TopToDown,BottonToUp};
    public MoveMode moveMode;

    public Transform menuBlockerOpen;
	public Transform menuBlockerClose;
	public Transform buttonActivate;
	public float moveSpeed = 10;
	public bool close = false;
	public bool open = false;

	public bool isOpen = false;
	public bool isMoveing = false;

	Vector3 currentVectorDirectionOnOpen = Vector3.zero;
	Vector3 currentVectorDirectionOnClose = Vector3.zero;

	public AudioClip tapClip;
	public AudioClip dragClip;

	float WValue;
	float HValue;

	public Camera cameraToUse;

	void Awake() {
	    gameObject.AddComponent<AudioSource>();
		if(!tapClip) tapClip = Resources.Load("tap") as AudioClip;
		if(!dragClip) dragClip = Resources.Load("drag") as AudioClip;
		if(!cameraToUse) cameraToUse = Camera.main;
	}

	// Use this for initialization
	void Start () {
	    WValue = GetComponent<RectTransform>().sizeDelta.x;
		HValue = GetComponent<RectTransform>().sizeDelta.y;

	    switch(moveMode) {
		case MoveMode.BottonToUp: 
		        currentVectorDirectionOnOpen = Vector3.up; 
		        currentVectorDirectionOnClose = Vector3.down; 
		        break;
		case MoveMode.TopToDown: currentVectorDirectionOnOpen = Vector3.down; currentVectorDirectionOnClose = Vector3.up; break;
		case MoveMode.RightToLeft: currentVectorDirectionOnOpen = Vector3.left; currentVectorDirectionOnClose = Vector3.right; break;
		case MoveMode.LeftToRight: currentVectorDirectionOnOpen = Vector3.right; currentVectorDirectionOnClose = Vector3.left; break;
	    }

	    setPositionsController();

	}

	public void gotButtonController() {

	  if(isMoveing) return;

	  gameObject.BroadcastMessage("resetAlertCol",SendMessageOptions.DontRequireReceiver);


	  GetComponent<AudioSource>().PlayOneShot(tapClip);

	  if(!isOpen) {
	    open = true;
	  } else {
	    close = true;
	  }

	  StartCoroutine(playDrag());
	}

	IEnumerator playDrag() {
	   yield return new WaitForSeconds(1);
		GetComponent<AudioSource>().PlayOneShot(dragClip);
	}
	void FixedUpdate () {

	    if(open) {
           switch(moveMode) {
            case MoveMode.LeftToRight:
				if(transform.localPosition.x + (WValue/2) < menuBlockerOpen.transform.localPosition.x) {
					transform.Translate( currentVectorDirectionOnOpen * (Time.deltaTime * moveSpeed),cameraToUse.transform);
				    isMoveing = true;
				} else {
					open = false;
					isOpen = true;
					isMoveing = false;
				}
            break;
			case MoveMode.RightToLeft:
				if(transform.localPosition.x + (WValue/2) > menuBlockerOpen.transform.localPosition.x) {
					transform.Translate( currentVectorDirectionOnOpen * (Time.deltaTime * moveSpeed),cameraToUse.transform);
				    isMoveing = true;
				} else {
					open = false;
					isOpen = true;
					isMoveing = false;
				}
            break;
			case MoveMode.TopToDown:
				if(transform.localPosition.y + (HValue/2) > menuBlockerOpen.transform.localPosition.y) {
					transform.Translate( currentVectorDirectionOnOpen * (Time.deltaTime * moveSpeed),cameraToUse.transform);
				    isMoveing = true;
				} else {
					open = false;
					isOpen = true;
					isMoveing = false;
				}
            break;
			case MoveMode.BottonToUp:
				if(transform.localPosition.y + (HValue/2) < menuBlockerOpen.transform.localPosition.y) {
					transform.Translate( currentVectorDirectionOnOpen * (Time.deltaTime * moveSpeed),cameraToUse.transform);
				    isMoveing = true;
				} else {
					open = false;
					isOpen = true;
					isMoveing = false;
				}
            break;
           }
	    }

		if(close) {
		  switch(moveMode) {
		   case MoveMode.LeftToRight:
				if(transform.localPosition.x - (WValue/2) > menuBlockerClose.transform.localPosition.x) {
					transform.Translate(currentVectorDirectionOnClose * (Time.deltaTime * moveSpeed),cameraToUse.transform);
			    isMoveing = true;
			} else {
				close = false;
				isOpen = false;
				isMoveing = false;
			}
		   break;
			case MoveMode.RightToLeft:
				if(transform.localPosition.x - (WValue/2) < menuBlockerClose.transform.localPosition.x) {
					transform.Translate(currentVectorDirectionOnClose * (Time.deltaTime * moveSpeed),cameraToUse.transform);
			    isMoveing = true;
			} else {
				close = false;
				isOpen = false;
				isMoveing = false;
			}
		   break;
			case MoveMode.TopToDown:
				if(transform.localPosition.y - (HValue/2) < menuBlockerClose.transform.localPosition.y) {
					transform.Translate(currentVectorDirectionOnClose * (Time.deltaTime * moveSpeed),cameraToUse.transform);
			    isMoveing = true;
			} else {
				close = false;
				isOpen = false;
				isMoveing = false;
			}
		   break;
			case MoveMode.BottonToUp:
				if(transform.localPosition.y - (HValue/2) > menuBlockerClose.transform.localPosition.y) {
					transform.Translate(currentVectorDirectionOnClose * (Time.deltaTime * moveSpeed),cameraToUse.transform);
			    isMoveing = true;
			} else {
				close = false;
				isOpen = false;
				isMoveing = false;
			}
		   break;
		  }
	    }
	
	}

	void setPositionsController() {

		if(moveMode == MoveMode.BottonToUp) {
						float pos_y = GetComponent<RectTransform>().localPosition.y;
						float H = HValue;
						float moveRange = H + Mathf.Abs(pos_y);
						moveRange = moveRange - buttonActivate.GetComponent<RectTransform>().sizeDelta.y;
						float RealMove = moveRange - Mathf.Abs(pos_y);

					    menuBlockerOpen.GetComponent<RectTransform>().localPosition = new Vector3(
					    menuBlockerOpen.GetComponent<RectTransform>().localPosition.x,
						menuBlockerOpen.GetComponent<RectTransform>().localPosition.y + (RealMove + buttonActivate.GetComponent<RectTransform>().sizeDelta.y),
						menuBlockerOpen.GetComponent<RectTransform>().localPosition.z);

					    menuBlockerClose.GetComponent<RectTransform>().localPosition = new Vector3(
						menuBlockerClose.GetComponent<RectTransform>().localPosition.x,
						menuBlockerClose.GetComponent<RectTransform>().localPosition.y - (RealMove + (buttonActivate.GetComponent<RectTransform>().sizeDelta.y)), // + buttonActivate.GetComponent<RectTransform>().sizeDelta.y),
						menuBlockerClose.GetComponent<RectTransform>().localPosition.z);
				}
		  else if(moveMode == MoveMode.TopToDown) {

					    menuBlockerOpen.GetComponent<RectTransform>().localPosition = new Vector3(
					    menuBlockerOpen.GetComponent<RectTransform>().localPosition.x,
						menuBlockerOpen.GetComponent<RectTransform>().localPosition.y,
						menuBlockerOpen.GetComponent<RectTransform>().localPosition.z);

					    menuBlockerClose.GetComponent<RectTransform>().localPosition = new Vector3(
						menuBlockerClose.GetComponent<RectTransform>().localPosition.x,
						menuBlockerClose.GetComponent<RectTransform>().localPosition.y,
						menuBlockerClose.GetComponent<RectTransform>().localPosition.z);
				} 
		  else if(moveMode == MoveMode.LeftToRight) {
						float pos_x = GetComponent<RectTransform>().localPosition.x;
						float W = WValue;
						float moveRange = W + Mathf.Abs(pos_x);
						moveRange = moveRange - buttonActivate.GetComponent<RectTransform>().sizeDelta.x;
						float RealMove = moveRange - Mathf.Abs(pos_x);

					    menuBlockerOpen.GetComponent<RectTransform>().localPosition = new Vector3(
				        menuBlockerOpen.GetComponent<RectTransform>().localPosition.x + (RealMove + buttonActivate.GetComponent<RectTransform>().sizeDelta.x),
						menuBlockerOpen.GetComponent<RectTransform>().localPosition.y,
						menuBlockerOpen.GetComponent<RectTransform>().localPosition.z);

					    menuBlockerClose.GetComponent<RectTransform>().localPosition = new Vector3(
				        menuBlockerClose.GetComponent<RectTransform>().localPosition.x - (RealMove + buttonActivate.GetComponent<RectTransform>().sizeDelta.x),
						menuBlockerClose.GetComponent<RectTransform>().localPosition.y,
						menuBlockerClose.GetComponent<RectTransform>().localPosition.z);
				} 
		 else if(moveMode == MoveMode.RightToLeft) {

					    menuBlockerOpen.GetComponent<RectTransform>().localPosition = new Vector3(
				        menuBlockerOpen.GetComponent<RectTransform>().localPosition.x,
						menuBlockerOpen.GetComponent<RectTransform>().localPosition.y,
						menuBlockerOpen.GetComponent<RectTransform>().localPosition.z);

					    menuBlockerClose.GetComponent<RectTransform>().localPosition = new Vector3(
				        menuBlockerClose.GetComponent<RectTransform>().localPosition.x,
						menuBlockerClose.GetComponent<RectTransform>().localPosition.y,
						menuBlockerClose.GetComponent<RectTransform>().localPosition.z);
				} 

	}
}
}