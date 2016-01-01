using UnityEngine;

public class Fireblast : Spell {

    override protected void Awake() {
        base.Awake();
        color = new Color(1.0f, 0.31f, 0.0f);
        manaCost = 1;
        damage.Add(10);
        spellText = string.Format("Deal {0} damage to target unit", damage[0]);
    }

    override public void effect(Unit target) {
        target.takeDamage((int)damage[0]);
    }

}