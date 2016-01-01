using UnityEngine;
using System.Collections;

public class Tornado : Spell {

    override protected void Awake() {
        color = new Color(0.0f, 0.765f, 0.251f);
        manaCost = 0;
        damage.Add(4);
        spellText = string.Format("Deal {0} damage to all units of the targeted faction", damage[0]);
    }

    override public void effect(Unit target) {
        ArrayList targets = deckController.getFactionZone(target.getOwner(), "field");
        int startCount = targets.Count;
        int i = 0;
        while(targets.Count > 0) {
            //Deal damage to the first target
            (targets[i] as Rigidbody).GetComponent<Unit>().takeDamage((int)damage[0]);
            //If the target didn't die move onto the next one
            //If it did die, a new target is now the first one, reset check of target dieing
            if(startCount == targets.Count) {
                i++;
            } else {
                startCount = targets.Count;
            }
            //Break when out of targets
            if(i > targets.Count - 1) {
                break;
            }
        }
    }  
}