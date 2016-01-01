using UnityEngine;
using System.Collections;

public class Deselect : MonoBehaviour {

	public ClickHandler clickHandler;

	void OnMouseDown() {
		clickHandler.unselect();
	}
}
