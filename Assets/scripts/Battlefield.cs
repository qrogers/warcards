using UnityEngine;
using System.Collections;

public class Battlefield : MonoBehaviour {

	public ClickHandler clickHandler;

	void OnMouseDown() {
		clickHandler.battlefieldClick();
	}

}
