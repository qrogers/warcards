using UnityEngine;
using System.Collections;

public class Elite : Unit {

    override protected void Awake() {
        base.Awake();
        color = new Color(0.141f, 0.506f, 0.0f);
        manaCost = 1;
        attack = new ArrayList();
        attack.Add(8);
        attack.Add(8);
        maxHealth = 20;
        currentHealth = maxHealth;
        skills = new ArrayList();
        skills.Add(Skills.Armored);
    }

    override public void foeClicked(Unit card) {
        attackTarget(card);
    }

}
