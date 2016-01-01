using UnityEngine;
using System.Collections;

public abstract class Unit : Card {
	
	protected ArrayList attack;
	protected int currentHealth;
	protected int maxHealth;
	protected int level;
	protected int exp;
    protected string slot;

    public void attackTarget(Unit target) {
        if (active) {
            if (deckController.validateAttack(this, target)) {
                target.reciveAttack(Random.Range((int)attack[0], (int)attack[1] + 1));
                target.retaliateAttack(this);
                deactivate();
                mimicClick();
            }
        } else {
            print("UNIT_IS_INACTIVE");
        }
    }

    public void attackEnemy(Enemy enemy) {
        if(active) {
            deckController.attackEnemy(Random.Range((int)attack[0], (int)attack[1] + 1));
        } else {
            print("UNIT_IS_INACTIVE");
        }
    }

    public void reciveAttack(int amount) {
        if(skills.Contains(Skills.Armored)) {
            amount -= 1;
        }
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

    public void startTurn() {
        activate();
        if(skills.Contains(Skills.Regeneration)) {
            heal(1);
        }
    }

    protected void heal(int amount) {
        if(currentHealth < maxHealth) {
            currentHealth += amount;
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
