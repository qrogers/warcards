using UnityEngine;
using System.Collections;

public class Archer : Unit {

    override protected void Awake() {
        base.Awake();
        color = new Color(0.0f, 0.224f, 0.537f);
        manaCost = 0;
        attack = new ArrayList();
        attack.Add(8);
        attack.Add(8);
        maxHealth = 8;
        currentHealth = maxHealth;
        skills = new ArrayList();
    }

    override public void foeClicked(Unit card) {
        attackTarget(card);
    }

}
