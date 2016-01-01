using UnityEngine;
using System.Collections;

public class ClickHandler : MonoBehaviour {

    public GameObject deckHandler;
    public InspectionText inspectionText;

    public GameObject enemy;

    private ArrayList listeners = new ArrayList();
    private DeckController deckController;
    private GameObject selected;

    void Start() {
        deckController = deckHandler.GetComponent<DeckController>();
        enemy.GetComponent<Enemy>().EnemyOnClicked += enemyClicked;
    }

    public void addClickEventListener(GameObject go) {
        go.GetComponent<Card>().CardOnClicked += locationResolver;
        listeners.Add(go);
    }

    void OnDisable() {
        foreach(GameObject item in listeners) {
            if(item != null) {
                item.GetComponent<Card>().CardOnClicked -= locationResolver;
            }
            //listeners.Remove(item);
            //Has errors?
        }
    }

    private void enemyClicked(GameObject card, string cardType) {
        card.GetComponent<Unit>().attackEnemy(enemy);
    }

    private void locationResolver(GameObject card, string cardType) {
        if(card.Equals(selected)) {
            unselect();
            return;
        }
        if(card.GetComponent<Card>().getLocation() == Card.Zone.PlayerField || card.GetComponent<Card>().getLocation() == Card.Zone.EnemyField) {
            cardInFieldClick(card, cardType);
        } else if(card.GetComponent<Card>().getLocation() == Card.Zone.PlayerHand) {
            switchSelection(card, cardType);
        } else {
            print("CARD_CLICKED_NOT_SELECTABLE");
        }
    }

    private void cardInFieldClick(GameObject card, string cardType) {
        if(selected == null) {
            setSelected(card);
            selected.GetComponent<Card>().select();
        //Selected card is on either field and is owned by player
        } else if((selected.GetComponent<Card>().getLocation() == Card.Zone.PlayerField || selected.GetComponent<Card>().getLocation() == Card.Zone.EnemyField) && selected.GetComponent<Card>().getOwner() == Card.Owner.Player) {
            //Clicked card is owned by player
            if(card.GetComponent<Card>().getOwner() == Card.Owner.Player) {
                selected.GetComponent<Card>().deselect();
                setSelected(card);
                selected.GetComponent<Card>().select();
            //Clicked card is owned by enemy
            } else if(card.GetComponent<Card>().getOwner() == Card.Owner.Enemy) {
                selected.GetComponent<Card>().foeClicked(card.GetComponent<Card>() as Unit);
            } else {
                print("FACTION_NOT_FOUND_ERROR");
            }
        //Selected card is in player hand
        } else if(selected.GetComponent<Card>().getLocation() == Card.Zone.PlayerHand) {
            //Clicked card is owned by player
            if(card.GetComponent<Card>().getOwner() == Card.Owner.Player) {
                //Selected card is a spell
                if(selected.GetComponent<Card>().isSpell()) {
                    deckController.playSpell(selected.GetComponent<Rigidbody>(), card.GetComponent<Card>() as Unit);
                } else {
                    selected.GetComponent<Card>().deselect();
                    setSelected(card);
                    selected.GetComponent<Card>().select();
                }
            } else if(card.GetComponent<Card>().getOwner() == Card.Owner.Enemy) {
                deckController.playSpell(selected.GetComponent<Rigidbody>(), card.GetComponent<Card>() as Unit);
            } else {
                print("FACTION_NOT_FOUND_ERROR");
            }
        } else {
            switchSelection(card, cardType);
        }
    }

    private void switchSelection(GameObject card, string cardType) {
        if(selected != null) {
            selected.GetComponent<Card>().deselect();
        }
        setSelected(card);
        selected.GetComponent<Card>().select();
    }

	public void unselect() {
		if(selected != null) {
			selected.GetComponent<Card>().deselect();
		}
		setSelected(null);
	}

	public void battlefieldClick() {
        if(selected != null) {
            selected.GetComponent<Card>().deselect();
        }
        setSelected(null);
    }

    public void slotClick(string slot) {
        if(selected != null) {
            if(selected.GetComponent<Card>().isUnit()) {
                switch(selected.GetComponent<Card>().getLocation()) {
                    case Card.Zone.PlayerHand:
                        playUnit(slot);
                        break;
                    case Card.Zone.PlayerField:
                        selected.GetComponent<Card>().deselect();
                        setSelected(null);
                        break;
                }
            } else {
                unselect();
            }
        }
    }

	private void setSelected(GameObject go) {
		selected = go;
		if(selected != null) {
			inspectionText.setCard(selected.GetComponent<Card>());
		}
	}

	private void playUnit(string slot) {
        deckController.playUnit(selected.GetComponent<Rigidbody>(), slot);
	}
	
}