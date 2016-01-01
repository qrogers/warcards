using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {

    public ClickHandler clickHandler;

    void OnMouseDown() {
        clickHandler.slotClick(gameObject.name[gameObject.name.Length - 1].ToString());
    }

}
