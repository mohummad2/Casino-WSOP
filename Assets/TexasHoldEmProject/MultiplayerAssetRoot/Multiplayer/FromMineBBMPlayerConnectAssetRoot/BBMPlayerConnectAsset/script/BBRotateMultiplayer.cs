using UnityEngine;
using System.Collections;

public class BBRotateMultiplayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 0, Time.deltaTime * 30);
	}
}
