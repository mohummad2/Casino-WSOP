using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace BLabTexasHoldEmProject {

public class BBDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

   public static GameObject itemBeingDragged;
   Vector3 startPosition;
   Transform startParent;

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		if(transform.parent == startParent) {
		  transform.position = startPosition;
//		  Debug.Log("not Parent..............");
		} else {
//			Debug.Log("........ Parent..............transform.parent : " + transform.parent.gameObject.GetInstanceID() + " : " + startParent.gameObject.GetInstanceID());
		}

	}

	#endregion



	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
      transform.position = Input.mousePosition;
	}

	#endregion




	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;

//		Debug.Log("........ OnBeginDrag.............. : " + startParent.gameObject.GetInstanceID());

		GetComponent<CanvasGroup>().blocksRaycasts = false;

	}
	#endregion

}
}