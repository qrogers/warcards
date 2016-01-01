using UnityEngine;
using System.Collections;

public class FieldText : MonoBehaviour {

	private Card card;

	public void setCard(Card c) {
		card = c;
	}

	void Update() {
		if(card != null && (card.getLocation() == Card.Zone.PlayerField || card.getLocation() == Card.Zone.EnemyField)) {
			string text = "";
			text += (card as Unit).getName();
			text += "\nHP: ";
			text += (card as Unit).getCurrentHealth();
            text += "/";
            text += (card as Unit).getMaxHealth();
            text += " Dmg: ";
			text += (card as Unit).getAttack()[0] + "-" + (card as Unit).getAttack()[1];
			text += "\nSkills: ";
			ArrayList skills = new ArrayList((card as Unit).getSkills());
			foreach(Card.Skills skill in skills) {
				text += skill.ToString() + ",\n";
			}
            //Remove final comma and not the last weird char
            text = text.Remove(text.Length - 2, 1);
			gameObject.GetComponent<GUIText>().text = text;
		} else {
			gameObject.GetComponent<GUIText>().text = "";
		}
	}

}