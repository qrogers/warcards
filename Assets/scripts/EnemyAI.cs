using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public DeckController deckController;
    public PhaseHandler phaseHandler;

    private enum Result { CardPlayed, NoMana, NoCards, PlayFailed, FieldFull, AttackSucessful, AttackFailed};

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
        if(deckController.getPlayerField().Count == 0) {
            allAttackPlayer();
        }
        phaseHandler.progressPhase();
    }

    private Result allAttackPlayer() {
        ArrayList enemyField = deckController.getEnemyField();
        foreach(Rigidbody attacker in enemyField) {
            attacker.GetComponent<Unit>().directAttack(deckController.getPlayer());
        }
        return Result.AttackSucessful;
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

    private Result allAttackRandomUnit() {
        ArrayList enemyField = deckController.getEnemyField();
        ArrayList playerField = deckController.getPlayerField();
        ArrayList attackable = new ArrayList();
        //FIX USING FOREACH ON DYNAMIC LIST, UNITS DIE FROM COUTNERATTACK

        int startCount = enemyField.Count;
        int i = 0;
        //Account for attackers dieing and chaning array
        while(enemyField.Count > 0) {
            Rigidbody attacker = enemyField[i] as Rigidbody;
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
                }
            }
            if(startCount == enemyField.Count) {
                i++;
            } else {
                startCount = enemyField.Count;
            }
            //Break when out of targets
            if(i > enemyField.Count - 1) {
                break;
            }
        }

        //foreach(Rigidbody attacker in enemyField) {
        //    playerField = deckController.getPlayerField();
        //    attackable = new ArrayList();
        //    if(playerField.Count > 0) {
        //        foreach(Rigidbody card in playerField) {
        //            if(deckController.validateAttack(attacker.gameObject.GetComponent<Unit>(), card.gameObject.GetComponent<Unit>())) {
        //                attackable.Add(card);
        //            }
        //        }
        //        if(attackable.Count > 0) {
        //            Rigidbody defender = randomFromList(attackable);
        //            attacker.gameObject.GetComponent<Unit>().attackTarget(defender.gameObject.GetComponent<Unit>());
        //        }
        //    }
        //}
        return Result.AttackSucessful;
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
