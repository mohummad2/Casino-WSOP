using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace BLabTexasHoldEmProject {

public class BBDrawContainerControl : MonoBehaviour {

  public Image imageHideContainer;
  public Sprite spriteHide;
  public Sprite spriteView;
  public GameObject containerToHide;

	public RectTransform rectToDrag;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void gotHideShowContailnerButton(GameObject _go) {

		if(imageHideContainer.sprite.name == "Windowed") {
			imageHideContainer.sprite = spriteView;
			containerToHide.SetActive(false);
	   } else {
			imageHideContainer.sprite = spriteHide;
			containerToHide.SetActive(true);
	   }
	
	   
	
	}

	public void OnEndDrag(UnityEngine.EventSystems.BaseEventData eventData)
	{
	}
	
	public void OnDrag(UnityEngine.EventSystems.BaseEventData eventData)
	{
		
		var pointerData = eventData as UnityEngine.EventSystems.PointerEventData;
		if (pointerData == null) { return; }
		
		
		var currentPosition = rectToDrag.position;
		currentPosition.x += pointerData.delta.x;
		currentPosition.y += pointerData.delta.y;
		rectToDrag.position = currentPosition;
		
	}



}
}