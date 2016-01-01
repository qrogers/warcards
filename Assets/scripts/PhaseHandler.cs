using UnityEngine;
using System.Collections;

public class PhaseHandler : MonoBehaviour {

    public enum Phase { Player, Enemy };

    public GameObject enemyAIGameoObject;
    private EnemyAI enemyAI;

    private DeckController deckController;

    private Phase phase = Phase.Player;

    void Start() {
        enemyAI = enemyAIGameoObject.GetComponent<EnemyAI>();
        enemyAI.setPhaseHandler(this);
    }

    void OnMouseDown() {
        gameObject.transform.Translate(0.0f, gameObject.transform.localScale.y / -2, 0.0f);
    }

    void OnMouseUp() {
        gameObject.transform.Translate(0.0f, gameObject.transform.localScale.y / 2, 0.0f);
        progressPhase();
        if (phase == Phase.Enemy) {
            enemyAI.enemyTurn();
        }
    }

    public Phase getPhase() {
		return phase;
	}

    public void progressPhase() {
        if(phase == Phase.Player) {
            phase = Phase.Enemy;
            deckController.startEnemyTurn();
        } else if(phase == Phase.Enemy) {
            phase = Phase.Player;
            deckController.startPlayerTurn();
        } else {
            print("PHASE_STATE_WRONG");
        }
    }

    public void setPhase(Phase newPhase) {
        phase = newPhase;
    }

    public void setDeckController(DeckController dc) {
        deckController = dc;
    }


}