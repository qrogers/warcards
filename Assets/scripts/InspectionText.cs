using UnityEngine;
using System.Collections;

public class InspectionText : MonoBehaviour {
	
	private Card card;
	
	public void setCard(Card c) {
		card = c;
	}
	
	void Update() {
		if(card != null) {
            string text = "";
            if(card.isUnit()) {
                text += (card as Unit).getName();
                text += "\nMana:";
                text += (card as Unit).getManaCost();
                text += "\nHP:";
                text += (card as Unit).getCurrentHealth();
                text += "/";
                text += (card as Unit).getMaxHealth();
                text += "\nDmg:\n";
                text += (card as Unit).getAttack()[0] + "-" + (card as Unit).getAttack()[1];
                text += "\nSkills:\n";
                ArrayList skills = new ArrayList((card as Unit).getSkills());
                foreach(string skill in skills) {
                    text += skill + "\n";
                }
            } else if(card.isSpell()) {
                text += (card as Spell).getName();
                text += "\nMana:";
                text += (card as Spell).getManaCost();
                text += "\n";
                text += (card as Spell).getText();
            }
            gameObject.GetComponent<TextMesh>().text = text;
		} else {
			gameObject.GetComponent<TextMesh>().text = "";
		}
	}
	
}