using UnityEngine;
using System.Collections;

public class Warrior : Unit {

    override protected void Awake() {
        base.Awake();
        color = new Color(0.733f, 0.0f, 0.133f);
        manaCost = 0;
        attack = new ArrayList();
        attack.Add(4);
        attack.Add(4);
		maxHealth = 12;
		currentHealth = maxHealth;
		skills = new ArrayList();
        skills.Add(Skills.Regeneration);
    }

    override public void foeClicked(Unit card) {
        attackTarget(card);
    }

}
