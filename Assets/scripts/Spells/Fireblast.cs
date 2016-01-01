using UnityEngine;

public class Fireblast : Spell {

    void Awake() {
        title = "Fireblast";
        color = new Color(1.0f, 0.224f, 0.537f);
        manaCost = 1;
        damage.Add(10);
        spellText = string.Format("Deal {0} damage to target unit", damage[0]);
    }

    override public void effect(Unit target) {
        target.takeDamage((int)damage[0]);
    }

}