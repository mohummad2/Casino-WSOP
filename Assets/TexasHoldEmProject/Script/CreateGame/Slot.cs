using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace BLabTexasHoldEmProject {

public class Slot : MonoBehaviour, IDropHandler {

    public GameObject item {
       get {
          if(transform.childCount>0) {
            return transform.GetChild(0).gameObject;
          }
          return null;
       }
    }


	#region IDropHandler implementation
	public void OnDrop (PointerEventData eventData)
	{
//	 Debug.Log("onDrop...............");

		if(!item) {
		    BBDragHandler.itemBeingDragged.transform.SetParent(transform);
			//Debug.Log("onDrop...............################## item : " + item.name);
		} 
	}
	#endregion
}
}