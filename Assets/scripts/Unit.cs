using UnityEngine;
using System.Collections;

public abstract class Unit : Card {
	
	protected ArrayList attack;
	protected int currentHealth;
	protected int maxHealth;
	protected int level;
	protected int exp;
	protected ArrayList skills;
    protected string slot;

    public void attackTarget(Unit target) {
        if (active) {
            if (deckController.validateAttack(this, target)) {
                target.reciveAttack(Random.Range((int)attack[0], (int)attack[1] + 1));
                target.retaliateAttack(this);
                deactivate();
                mimicClick();
            } else {
                print("INVALID_SLOT_TO_ATTACK");
            }
        } else {
            print("UNIT_IS_INACTIVE");
        }
    }

    public void reciveAttack(int amount) {
        takeDamage(amount);
    }

    public void retaliateAttack(Unit attacker) {
        attacker.reciveAttack(Random.Range((int)attack[0], (int)attack[1] + 1));
    }

    public void takeDamage(int damage) {
        currentHealth -= damage;
        if(currentHealth <= 0) {
            die();
        }
    }

    protected void die() {
        deckController.kill(this, slot);
    }

	public int getCurrentHealth() {
		return currentHealth;
	}

    public int getMaxHealth() {
        return maxHealth;
    }

	public ArrayList getAttack() {
        return attack;
	}

	public ArrayList getSkills() {
		return skills;
	}

    public void setSlot(string newSlot) {
        slot = newSlot;
    }

    public string getSlot() {
        return slot;
    }

}
