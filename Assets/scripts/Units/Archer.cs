using UnityEngine;
using System.Collections;

public class Archer : Unit {

    void Awake() {
        title = "Archer";
        color = new Color(0.0f, 0.224f, 0.537f);
        manaCost = 0;
        attack = new ArrayList();
        attack.Add(8);
        attack.Add(8);
        maxHealth = 4;
        currentHealth = maxHealth;
        skills = new ArrayList();
        skills.Add("Sniper");
    }

    override public void foeClicked(Unit card) {
        attackTarget(card);
    }

}
