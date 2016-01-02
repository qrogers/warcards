using UnityEngine;
using System.Collections;

public class StatusText : MonoBehaviour {

    private DeckController deckController;

    void Update() {
            string text = "";
            text += "ENEMY\n";
            text += "HP: " + deckController.getEnemy().getHealth().ToString();
            text += "\nMana: " + deckController.getEnemyMana().ToString();
            text += "\n\n\n\n\n";
            text += "PLAYER\n";
            text += "HP: " + deckController.getPlayer().getHealth().ToString();
            text += "\nMana: " + deckController.getPlayerMana().ToString();
            gameObject.GetComponent<TextMesh>().text = text;
    }

    public void setDeckController(DeckController dc) {
        deckController = dc;
    }
}
