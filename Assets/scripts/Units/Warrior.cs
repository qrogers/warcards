using UnityEngine;
using System.Collections;

public class Warrior : Unit {

	void Awake() {
		title = "Warrior";
        color = new Color(0.733f, 0.0f, 0.133f);
        manaCost = 0;
        attack = new ArrayList();
        attack.Add(4);
        attack.Add(4);
		maxHealth = 12;
		currentHealth = maxHealth;
		skills = new ArrayList();
		skills.Add("Evasion");
		skills.Add("Power Hit");
        skills.Add("Swift");
	}

    override public void foeClicked(Unit card) {
        attackTarget(card);
    }

}
