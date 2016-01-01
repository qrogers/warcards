using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public DeckController deckController;
    public PhaseHandler phaseHandler;

    private enum Result { CardPlayed, NoMana, NoCards, PlayFailed, FieldFull};

    public void enemyTurn() {
        bool donePlaying = false;
        while(!donePlaying && deckController.getEnemyMana() > -1) {
            switch (playRandomUnit()) {
                case Result.CardPlayed:
                    break;
                case Result.NoCards:
                    donePlaying = true;
                    break;
                default :
                    donePlaying = true;
                    break;
            }
        }
        if(deckController.getFactionZone(Card.Owner.Player, "field").Count >= 3) {
            playRandomSpellOfCost(0, randomFromList(deckController.getFactionZone(Card.Owner.Player, "field")).gameObject.GetComponent<Unit>());
        }
        if(deckController.getFactionZone(Card.Owner.Player, "field").Count > 0) {
            playRandomSpellOfCost(1, randomFromList(deckController.getFactionZone(Card.Owner.Player, "field")).gameObject.GetComponent<Unit>());
        }
        allAttackRandomUnit();
        phaseHandler.progressPhase();
    }

    private Result playRandomSpellOfCost(int cost, Unit target) {
        if(deckController.getEnemyMana() < cost) {
            return Result.NoMana;
        }
        ArrayList spellsOfCost = getSpellsOfCost(cost);
        if(spellsOfCost.Count == 0) {
            return Result.NoCards;
        }
        if(deckController.playSpell(randomFromList(spellsOfCost), target)) {
            return Result.CardPlayed;
        } else {
            return Result.PlayFailed;
        }
    }

    private ArrayList getSpellsOfCost(int cost) {
        ArrayList enemyHand = deckController.getEnemyHand();
        ArrayList spellsOfCost = new ArrayList();
        foreach(Rigidbody card in enemyHand) {
            if(card.gameObject.GetComponent<Card>().isSpell() && card.gameObject.GetComponent<Card>().getManaCost() == cost) {
                spellsOfCost.Add(card);
            }
        }
        return spellsOfCost;
    }

    private Result playRandomUnit() {
        ArrayList enemyOccupiedSlots = deckController.getEnemyOccupiedSlots();
        ArrayList enemyField = deckController.getEnemyField();
        ArrayList enemyHand = deckController.getEnemyHand();
        if(enemyField.Count < 5) {
            ArrayList openSlots = new ArrayList();
            int index = 0;
            foreach(bool slotOccupied in enemyOccupiedSlots) {
                if(!slotOccupied) {
                    openSlots.Add(index);
                }
                index++;
            }
            ArrayList unitCards = new ArrayList();
            foreach(Rigidbody unit in enemyHand) {
                if(unit.gameObject.GetComponent<Card>().isUnit()) {
                    unitCards.Add(unit);
                }
            }
            if(unitCards.Count > 0) {
                string slot = openSlots[Random.Range(0, openSlots.Count - 1)].ToString();
                if(deckController.playUnit(randomFromList(unitCards), slot)) {
                    return Result.CardPlayed;
                } else {
                    return Result.PlayFailed;
                }
            } else {
                return Result.NoCards;
            }
        }
        return Result.FieldFull;
    }

    /*private void fillFieldWithRandomCards() {
        ArrayList enemyOccupiedSlots = deckController.getEnemyOccupiedSlots();
        ArrayList enemyField = deckController.getEnemyField();
        ArrayList enemyHand = deckController.getEnemyHand();
        while(enemyField.Count < 5 && enemyHand.Count > 0) {
            ArrayList openSlots = new ArrayList();
            int index = 0;
            foreach(bool slotOccupied in enemyOccupiedSlots) {
                if(!slotOccupied) {
                    openSlots.Add(index);
                }
                index++;
            }
            string slot = openSlots[Random.Range(0, openSlots.Count - 1)].ToString();
            deckController.play(enemyHand[0] as Rigidbody, slot);
        }
    }*/

    private bool allAttackRandomUnit() {
        ArrayList enemyField = deckController.getEnemyField();
        ArrayList playerField = deckController.getPlayerField();
        ArrayList attackable = new ArrayList();
        foreach(Rigidbody attacker in enemyField) {
            playerField = deckController.getPlayerField();
            attackable = new ArrayList();
            if(playerField.Count > 0) {
                foreach(Rigidbody card in playerField) {
                    if(deckController.validateAttack(attacker.gameObject.GetComponent<Unit>(), card.gameObject.GetComponent<Unit>())) {
                        attackable.Add(card);
                    }
                }
                if(attackable.Count > 0) {
                    Rigidbody defender = randomFromList(attackable);
                    attacker.gameObject.GetComponent<Unit>().attackTarget(defender.gameObject.GetComponent<Unit>());
                    return true;
                }
            }
        }
        return false;
    }

    private void randomUnitAttackRandomUnit() {
        ArrayList enemyField = deckController.getEnemyField();
        ArrayList playerField = deckController.getPlayerField();
        ArrayList attackable = new ArrayList();
        if(playerField.Count > 0) {
            Rigidbody attacker = randomFromList(enemyField);
            foreach(Rigidbody card in playerField) {
                if(deckController.validateAttack(attacker.gameObject.GetComponent<Unit>(), card.gameObject.GetComponent<Unit>())) {
                    attackable.Add(card);
                }
            }
            Rigidbody defender = randomFromList(attackable);
            attacker.gameObject.GetComponent<Unit>().attackTarget(defender.gameObject.GetComponent<Unit>());
        }
    }

    public static Rigidbody randomFromList(ArrayList list) {
        if(list.Count > 0) {
            return list[Random.Range(0, list.Count - 1)] as Rigidbody;
        } else {
            return null;
        }
    }

    public void setDeckController(DeckController dc) {
        deckController = dc;
    }

    public void setPhaseHandler(PhaseHandler ch) {
        phaseHandler = ch;
    }
}
