using System;
using System.Collections.Generic;
using UnityEngine;

namespace BLabTexasHoldEmProject {

[Serializable]
public class BBLastCardsHand : ScriptableObject {
	public Vector2[] playersCardsList = new Vector2[25];
}
}