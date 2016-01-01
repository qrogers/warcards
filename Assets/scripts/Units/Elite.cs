using UnityEngine;
using System.Collections;

public class Elite : Unit {

    void Awake() {
        title = "Elite";
        color = new Color(0.141f, 0.506f, 0.0f);
        manaCost = 1;
        attack = new ArrayList();
        attack.Add(8);
        attack.Add(8);
        maxHealth = 20;
        currentHealth = maxHealth;
        skills = new ArrayList();
    }

    override public void foeClicked(Unit card) {
        attackTarget(card);
    }

}
