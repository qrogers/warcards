using UnityEngine;
using System.Collections;

public class DeckController : MonoBehaviour {

    private const int MAX_HAND_SIZE = 7;
    private const int MAX_PLAYER_HEALTH = 45;
    private const int MAX_ENEMY_HEALTH = 45;

    private ArrayList playerDeck = new ArrayList();
	private ArrayList fullPlayerDeck = new ArrayList();
	private ArrayList playerHand = new ArrayList();
	private ArrayList playerField = new ArrayList();
	private ArrayList playerGraveyard = new ArrayList();

    private int playerHealth = MAX_PLAYER_HEALTH;
    private int enemyHealth = MAX_ENEMY_HEALTH;
    private int playerMana = 0;
    private int enemyMana = 0;

    private ArrayList enemyDeck = new ArrayList();
    private ArrayList fullEnemyDeck = new ArrayList();
    private ArrayList enemyHand = new ArrayList();
    private ArrayList enemyField = new ArrayList();
    private ArrayList enemyGraveyard = new ArrayList();

    private Vector3 offScreenVector = new Vector3(30.0f, 5.5f, 5.0f);

	private Vector3 inHandVector = new Vector3(0.0f, 0.75f, -4.7f);
	private Vector3 onPlayerFieldVector = new Vector3(0.0f, 0.5f, -1.0f);
	private Vector3 onPlayerFieldScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 playerFieldTextPosition = new Vector3(0.5f, 0.25f, 0.0f);

    private Vector3 inEnemyHandVector = new Vector3(0.0f, 0.75f, 7.0f);
    private Vector3 onEnemyFieldVector = new Vector3(0.0f, 0.5f, 1.0f);
    private Vector3 onEnemyFieldScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 enemyFieldTextPosition = new Vector3(0.5f, 0.761f, 0.0f);

    private ArrayList playerOccupiedSlots = new ArrayList{false, false, false, false, false};
    private ArrayList enemyOccupiedSlots = new ArrayList{false, false, false, false, false};
    private float[] fieldSlotsValues = {-6.0f, -3.0f, 0.0f, 3.0f, 6.0f};

    private ArrayList playerFieldTexts = new ArrayList();
	private GUIText playerFieldText0;
	private GUIText playerFieldText1;
	private GUIText playerFieldText2;
	private GUIText playerFieldText3;
	private GUIText playerFieldText4;

    private ArrayList enemyFieldTexts = new ArrayList();
    private GUIText enemyFieldText0;
    private GUIText enemyFieldText1;
    private GUIText enemyFieldText2;
    private GUIText enemyFieldText3;
    private GUIText enemyFieldText4;

    public GUIText deckText;
	public GUIText textPrefab;
    public TextMesh statusText;
	public Rigidbody warriorPrefab;
    public Rigidbody archerPrefab;
    public Rigidbody fireblastPrefab;
    public GameObject cardDictionary;
    public GameObject clickHandler;
    public GameObject phaseHandler;
    public GameObject enemyAI;
	public Camera MainCamera;

	void Start() {
        enemyAI.GetComponent<EnemyAI>().setDeckController(this);
        phaseHandler.GetComponent<PhaseHandler>().setDeckController(this);
        statusText.gameObject.GetComponent<StatusText>().setDeckController(this);

		playerFieldText0 = (GUIText)Instantiate(textPrefab, playerFieldTextPosition - new Vector3(0.26f, 0.0f, 0.0f), new Quaternion());
		playerFieldText1 = (GUIText)Instantiate(textPrefab, playerFieldTextPosition - new Vector3(0.13f, 0.0f, 0.0f), new Quaternion());
		playerFieldText2 = (GUIText)Instantiate(textPrefab, playerFieldTextPosition + new Vector3(0.00f, 0.0f, 0.0f), new Quaternion());
		playerFieldText3 = (GUIText)Instantiate(textPrefab, playerFieldTextPosition + new Vector3(0.13f, 0.0f, 0.0f), new Quaternion());
		playerFieldText4 = (GUIText)Instantiate(textPrefab, playerFieldTextPosition + new Vector3(0.26f, 0.0f, 0.0f), new Quaternion());
        enemyFieldText0  = (GUIText)Instantiate(textPrefab, enemyFieldTextPosition  - new Vector3(0.26f, 0.0f, 0.0f), new Quaternion());
        enemyFieldText1  = (GUIText)Instantiate(textPrefab, enemyFieldTextPosition  - new Vector3(0.13f, 0.0f, 0.0f), new Quaternion());
        enemyFieldText2  = (GUIText)Instantiate(textPrefab, enemyFieldTextPosition  + new Vector3(0.00f, 0.0f, 0.0f), new Quaternion());
        enemyFieldText3  = (GUIText)Instantiate(textPrefab, enemyFieldTextPosition  + new Vector3(0.13f, 0.0f, 0.0f), new Quaternion());
        enemyFieldText4  = (GUIText)Instantiate(textPrefab, enemyFieldTextPosition  + new Vector3(0.26f, 0.0f, 0.0f), new Quaternion());
        playerFieldTexts.Add(playerFieldText0);
		playerFieldTexts.Add(playerFieldText1);
		playerFieldTexts.Add(playerFieldText2);
		playerFieldTexts.Add(playerFieldText3);
		playerFieldTexts.Add(playerFieldText4);
        enemyFieldTexts.Add(enemyFieldText0);
        enemyFieldTexts.Add(enemyFieldText1);
        enemyFieldTexts.Add(enemyFieldText2);
        enemyFieldTexts.Add(enemyFieldText3);
        enemyFieldTexts.Add(enemyFieldText4);
        foreach (GUIText text in playerFieldTexts) {
            text.fontStyle = FontStyle.Bold;
            text.fontSize = 18;
        }
        foreach(GUIText text in enemyFieldTexts) {
            text.fontStyle = FontStyle.Bold;
            text.fontSize = 18;
        }

        spawnCard("warrior", Card.Owner.Player, 6);
        spawnCard("warrior", Card.Owner.Enemy, 6);  
        spawnCard("archer", Card.Owner.Player, 4);
        spawnCard("archer", Card.Owner.Enemy, 4);
        spawnCard("fireblast", Card.Owner.Player, 4);
        spawnCard("fireblast", Card.Owner.Enemy, 4);
        spawnCard("tornado", Card.Owner.Player, 2);
        spawnCard("tornado", Card.Owner.Enemy, 2);
        spawnCard("elite", Card.Owner.Player);
        spawnCard("elite", Card.Owner.Enemy);

        updateDeckText("DEFAULT_TEXT");
		playerDeck = fullPlayerDeck;
        enemyDeck = fullEnemyDeck;
		shuffle(playerDeck, "player");
        shuffle(enemyDeck, "enemy");
		playerDraw();
        playerDraw();
        playerDraw();
        playerDraw();
        playerDraw();
        enemyDraw();
        enemyDraw();
        enemyDraw();
        enemyDraw();
        enemyDraw();
    }

    private void initCard(Card card) {
        //card.gameObject.GetComponent<Card>().setColor(ColorUtils.getRandomColor());
        card.gameObject.GetComponent<Card>().initColor();
        card.gameObject.GetComponent<Card>().setDeckController(this);
    }

    private void spawnCard(string card,Card.Owner owner, int count=1) {
        for(int i = 1; i <= count; i++) {
            Rigidbody newCard = (Rigidbody)Instantiate(cardDictionary.GetComponent<CardDictionary>().getCard(card), offScreenVector, new Quaternion());
            initCard(newCard.gameObject.GetComponent<Card>());
            newCard.gameObject.GetComponent<Card>().setOwner(owner);
            getFactionZone(owner, "fulldeck").Add(newCard);
            clickHandler.GetComponent<ClickHandler>().addClickEventListener(newCard.gameObject);
        }
    }

    private void shuffle(ArrayList deck, string owner) {
		ArrayList newDeck = new ArrayList();
		string deckString = "";
		int deckSize = deck.Count;
		for(int i = 0; i < deckSize; i++){
			//The 2nd arg of Random is not inclusive
			int index = Random.Range(0, deck.Count);
			newDeck.Add(deck[index]);
			deck.RemoveAt(index);
		}
        if(owner == "player") {
            playerDeck = newDeck;
            foreach(Object item in deck) {
                deckString += " " + item.ToString();
            }
            updateDeckText(deck.Count.ToString());
        } else if(owner == "enemy") {
            enemyDeck = newDeck;
        }
		
	}

    public void startPlayerTurn() {
        playerMana++;
        activateUnits();
        if(playerHand.Count < MAX_HAND_SIZE) {
            playerDraw();
        }
    }

    public void startEnemyTurn() {
        enemyMana++;
        activateUnits();
        if(enemyHand.Count < MAX_HAND_SIZE) {
            enemyDraw();
        }
    }

    private void activateUnits() {
        foreach(Rigidbody card in enemyField) {
            card.gameObject.GetComponent<Card>().activate();
        }
        foreach(Rigidbody card in playerField) {
            card.gameObject.GetComponent<Card>().activate();
        }
    }

	public void playerDraw() {
		if(playerDeck.Count > 0){
			string deckString = "";
			foreach(Object item in playerDeck){
				deckString += " " + item.ToString();
			}
            updateHand(playerDeck[0] as Rigidbody, "add");
			(playerDeck[0] as Rigidbody).GetComponent<Card>().moveToHand();
			playerDeck.RemoveAt(0);
		} else {
			print("PLAYER_DECK_EMPTY");
		}
        updateDeckText(playerDeck.Count.ToString());
    }

    public void enemyDraw() {
        if(enemyDeck.Count > 0) {
            updateHand(enemyDeck[0] as Rigidbody, "add");
            (enemyDeck[0] as Rigidbody).GetComponent<Card>().moveToHand();
            enemyDeck.RemoveAt(0);
        } else {
            print("ENEMY_DECK_EMPTY");
        }
    }

    public bool playSpell(Rigidbody cardBody, Unit target) {
        Spell card = cardBody.GetComponent<Spell>();
        if(spendMana(card.getManaCost(), card.getOwner())) {
            card.effect(target);
            updateHand(cardBody, "remove");
            card.moveToGraveyard();
            updateGraveyard(cardBody, "add");
            return true;
        }
        return false;
    }

    public bool playUnit(Rigidbody cardBody, string slot) {
        Unit card = cardBody.GetComponent<Unit>();
        if(spendMana(card.getManaCost(), card.getOwner())) {
            if(card.getOwner() == Card.Owner.Player) {
                if(!(bool)playerOccupiedSlots[int.Parse(slot)]) {
                    updateHand(cardBody, "remove");
                    card.moveToField();
                    updateField(cardBody, "add", slot);
                    card.deselect();
                    card.deactivate();
                    clickHandler.GetComponent<ClickHandler>().unselect();
                    card.setSlot(slot);
                    return true;
                } else {
                    print("SLOT_TAKEN");
                    return false;
                }
            } else if(card.getOwner() == Card.Owner.Enemy) {
                if(!(bool)enemyOccupiedSlots[int.Parse(slot)]) {
                    updateHand(cardBody, "remove");
                    card.moveToField();
                    updateField(cardBody, "add", slot);
                    card.deselect();
                    card.deactivate();
                    card.setSlot(slot);
                    return true;
                } else {
                    print("SLOT_TAKEN");
                    return false;
                }
            } else {
                print("PLAY_OWNER_NOT_FOUND");
                return false;
            }
        } else {
            print("NOT_ENOUGH_MANA");
            return false;
        }
	}

	public void kill(Card card, string slot) {
		switch (card.getLocation()) {
			case Card.Zone.PlayerField :
				updateField(card.GetComponent<Rigidbody>(), "remove", slot);
				updateGraveyard(card.GetComponent<Rigidbody>(), "add");
                card.GetComponent<Card>().moveToGraveyard();
                break;
            case Card.Zone.EnemyField:
                updateField(card.GetComponent<Rigidbody>(), "remove", slot);
                updateGraveyard(card.GetComponent<Rigidbody>(), "add");
                card.GetComponent<Card>().moveToGraveyard();
                break;
            case Card.Zone.PlayerHand :
				break;
			case Card.Zone.PlayerGraveyard :
				break;
		}

	}

	private void updateHand(Rigidbody card, string mode) {
        ArrayList hand = getFactionZone(card.GetComponent<Card>().getOwner(), "hand");
        Vector3 handPosition;
        if(card.GetComponent<Card>().getOwner() == Card.Owner.Player) {
            handPosition = inHandVector;
        } else if(card.GetComponent<Card>().getOwner() == Card.Owner.Enemy) {
            handPosition = inEnemyHandVector;
        } else {
            print("FACTION_ERROR");
            handPosition = new Vector3();
        }
        if(mode == "add") {
            hand.Add(card);
            } else if(mode == "remove") {
            hand.Remove(card);
        }
		foreach(Rigidbody rb in hand) {
			Vector3	cardTransform = handPosition;
			cardTransform = new Vector3(handPosition.x + ((hand.IndexOf(rb) * (Card.IN_HAND_SCALE * 2)) - (hand.Count - 1)), cardTransform.y, cardTransform.z);
            float cardScale = Card.IN_HAND_SCALE;
            rb.transform.localScale = new Vector3(cardScale, cardScale, cardScale);
            changeCardLocation(rb, cardTransform);
		}
	}

	private void updateField(Rigidbody card, string mode, string slot) {
        ArrayList field = getFactionZone(card.GetComponent<Card>().getOwner(), "field");
        ArrayList fieldSlots = getFactionZone(card.GetComponent<Card>().getOwner(), "fieldslots");
        ArrayList fieldTexts = getFactionZone(card.GetComponent<Card>().getOwner(), "fieldtexts");
        int slotNum = int.Parse(slot);
        if(mode == "add") {
			field.Add(card);
			Vector3	newCardPosition = onPlayerFieldVector;
            fieldSlots[slotNum] = true;
			(fieldTexts[slotNum] as GUIText).GetComponent<FieldText>().setCard(card.GetComponent<Card>());
			newCardPosition = new Vector3(newCardPosition.x + (fieldSlotsValues[slotNum]), newCardPosition.y, newCardPosition.z);
            //Adjust enemy cards up, swtich to getting faction vector
            if(card.GetComponent<Card>().getOwner() == Card.Owner.Enemy) {
                newCardPosition = newCardPosition + new Vector3(0.0f, 0.0f, 4.0f);
            }
			changeCardScale(card, onPlayerFieldScale);
            changeCardLocation(card, newCardPosition);
		} else if(mode == "remove") {
			field.Remove(card);
            fieldSlots[slotNum] = false;
        }
	}

	private void updateGraveyard(Rigidbody card, string mode) {
        ArrayList graveyard = getFactionZone(card.GetComponent<Card>().getOwner(), "graveyard");
        if(mode == "add") {
            graveyard.Add(card);
		} else if(mode == "remove") {
            graveyard.Remove(card);
		}
		changeCardLocation(card, offScreenVector);
	}

    public ArrayList getFactionZone(Card.Owner owner, string zone) {
        switch(zone) {
            case "hand":
                if(owner == Card.Owner.Player) {
                    return playerHand;
                } else if(owner == Card.Owner.Enemy) {
                    return enemyHand;
                }
                break;
            case "field":
                if(owner == Card.Owner.Player) {
                    return playerField;
                } else if(owner == Card.Owner.Enemy) {
                    return enemyField;
                }
                break;
            case "fieldslots":
                if(owner == Card.Owner.Player) {
                    return playerOccupiedSlots;
                } else if(owner == Card.Owner.Enemy) {
                    return enemyOccupiedSlots;
                }
                break;
            case "fieldtexts":
                if(owner == Card.Owner.Player) {
                    return playerFieldTexts;
                } else if(owner == Card.Owner.Enemy) {
                    return enemyFieldTexts;
                }
                break;
            case "deck":
                if(owner == Card.Owner.Player) {
                    return playerDeck;
                } else if(owner == Card.Owner.Enemy) {
                    return enemyDeck;
                }
                break;
            case "fulldeck":
                if(owner == Card.Owner.Player) {
                    return fullPlayerDeck;
                } else if(owner == Card.Owner.Enemy) {
                    return fullEnemyDeck;
                }
                break;
            case "graveyard":
                if(owner == Card.Owner.Player) {
                    return playerGraveyard;
                } else if(owner == Card.Owner.Enemy) {
                    return enemyGraveyard;
                }
                break;
            default:
                print("ZONE NOT FOUND");
                return new ArrayList();
        }
        print("ERROR IN SWITCH");
        return new ArrayList();
    }

    public bool validateAttack(Unit card1, Unit card2) {
        int slot1 = int.Parse(card1.getSlot());
        int slot2 = int.Parse(card2.getSlot());
        if(slot1 == slot2 || slot1 - 1 == slot2 || slot1 + 1 == slot2) {
            return true;
        }
        return false;
    }

    public ArrayList getPlayerField() {
        return playerField;
    }

    public ArrayList getEnemyField() {
        return enemyField;
    }

    public ArrayList getEnemyHand() {
        return enemyHand;
    }

    public ArrayList getEnemyOccupiedSlots() {
        return enemyOccupiedSlots;
    }

    public int getPlayerHealth() {
        return playerHealth;
    }

    public int getEnemyHealth() {
        return enemyHealth;
    }

    public int getPlayerMana() {
        return playerMana;
    }

    public int getEnemyMana() {
        return enemyMana;
    }

    public bool spendMana(int amount, Card.Owner owner) {
        if(owner == Card.Owner.Player) {
            return spendPlayerMana(amount);
        } else if(owner == Card.Owner.Enemy) {
            return spendEnemyMana(amount);
        } else {
            print("OWNER_NOT_FOUND_SPEND_MANA");
            return false;
        }
    }

    private bool spendPlayerMana(int amount) {
        if(playerMana - amount >= 0) {
            playerMana -= amount;
            return true;
        }
        return false;
    }

    private bool spendEnemyMana(int amount) {
        if(enemyMana - amount >= 0) {
            enemyMana -= amount;
            return true;
        }
        return false;
    }

    private void changeCardLocation(Rigidbody card, Vector3 newPosition) {
        card.transform.position = newPosition;
    }

    private void changeCardScale(Rigidbody card, Vector3 newScale) {
        card.transform.localScale = newScale;
    }

    private void changeCardRotation(Rigidbody card, Quaternion newRotation) {
        card.transform.rotation = newRotation;
    }

    private void updateDeckText(string text) {
        deckText.text = text;
    }

}
