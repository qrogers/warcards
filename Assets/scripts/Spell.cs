using UnityEngine;
using System.Collections;

public abstract class Spell : Card {

    protected ArrayList damage = new ArrayList();
    protected int level;
    protected int exp;
    protected string spellText;

    public ArrayList getDamage() {
        return damage;
    }

    public string getText() {
        return spellText;
    }

    virtual public void effect(Unit target) {}

}
