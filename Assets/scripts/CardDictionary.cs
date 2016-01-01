using UnityEngine;
using System.Collections;

public class CardDictionary : MonoBehaviour {

    public Rigidbody warriorPrefab;
    public Rigidbody archerPrefab;
    public Rigidbody fireblastPrefab;
    public Rigidbody elitePrefab;
    public Rigidbody tornadoPrefab;

    private ArrayList cards = new ArrayList();

    void Awake() {
        cards.Add(warriorPrefab);
        cards.Add(archerPrefab);
        cards.Add(fireblastPrefab);
        cards.Add(elitePrefab);
        cards.Add(tornadoPrefab);
    }

    public Rigidbody getCard(string name) {
        foreach(Rigidbody card in cards) {
            if(card.name.ToLower().Contains(name)) {
                return card;
            }
        }
        print("CARD_LOOK_UP_NOT_FOUND");
        return null;
    }

}
