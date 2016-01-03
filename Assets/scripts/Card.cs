using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

    public delegate void ClickAction(GameObject c, string s);
    public event ClickAction CardOnClicked;
    public Material outline;
    public Material baseMaterial;
    public Color color;

    public static float MOUSE_OVER_SCALE = 1.05f;
    public static float IN_HAND_SCALE = 0.9f;

    private int pulseDirection = 1;

    private Vector3 mouseOverSize = new Vector3(MOUSE_OVER_SCALE, MOUSE_OVER_SCALE, MOUSE_OVER_SCALE);
    private Vector3 mouseExitSize = new Vector3(IN_HAND_SCALE, IN_HAND_SCALE, IN_HAND_SCALE);
    private bool firstOver = true;
    private bool selected = false;
    private float xRotate;
    private float yRotate;
    private float zRotate;
    private Material material;

    protected string title;
    protected bool active = false;
    protected int manaCost;
    protected ArrayList skills;

    public enum Owner { Player, Enemy };
    public enum Zone { PlayerHand, PlayerDeck, PlayerField, PlayerGraveyard, EnemyHand, EnemyDeck, EnemyField, EnemyGraveyard };
    public enum Skills { Armored, Regeneration, Guarding};

    private Owner owner = Owner.Player;
    protected Zone location = Zone.PlayerHand;

    protected DeckController deckController;

    virtual protected void Awake() {
        title = GetType().ToString();
    }

    public void initColor() {
        renderNewColor(color);
    }

    public void dullColor() {
        //renderNewColor(ColorUtils.fadeColor(ColorUtils.dullColor(color)));
        renderNewColor(ColorUtils.dullColor(color));
    }

    public void greyOut() {
        renderNewColor(ColorUtils.GREY);
    }

    private void renderNewColor(Color newColor) {
        material = new Material(baseMaterial);
        print(material.shaderKeywords.IsReadOnly);
        material.color = newColor;
        Material[] outlineMaterial = gameObject.GetComponent<MeshRenderer>().materials;
        outlineMaterial[1] = material;
        outlineMaterial[0] = material;
        gameObject.GetComponent<MeshRenderer>().materials = outlineMaterial;
    }

    private void renderOutline() {
        Material[] outlineMaterial = gameObject.GetComponent<MeshRenderer>().materials;
        outlineMaterial[1] = outline;
        gameObject.GetComponent<MeshRenderer>().materials = outlineMaterial;
        gameObject.GetComponent<MeshRenderer>().materials[1].SetColor("_OutlineColor", ColorUtils.invertColor(color));
    }

    private void unrenderOutline() {
        Material[] outlineMaterial = gameObject.GetComponent<MeshRenderer>().materials;
        outlineMaterial[1] = material;
        gameObject.GetComponent<MeshRenderer>().materials = outlineMaterial;
    }

    void OnMouseDown() {
        if(CardOnClicked != null) {
            CardOnClicked(this.gameObject, this.title);
        }
    }

    void OnMouseOver() {
        if(location == Zone.PlayerHand) {
            transform.localScale = mouseOverSize;
        }
    }

    void Update() {
        if(selected && location == Zone.PlayerHand) {
            rotating();
        } else if(location == Zone.EnemyHand) {
            rotating();
        } else if(active) {
            if(transform.localScale.x < 1.0f) {
                pulseDirection = 1;
            } else if(transform.localScale.x > 1.1f) {
                pulseDirection = -1;
            }
            float pulseRate = 0.0001f;
            Vector3 newSize = new Vector3(transform.localScale.x + pulseRate * pulseDirection, transform.localScale.y + pulseRate * pulseDirection, transform.localScale.z + pulseRate * pulseDirection);
            transform.localScale = newSize;
        }
    }

    void OnMouseExit() {
        if(location == Zone.PlayerHand) {
            transform.localScale = mouseExitSize;
        }
    }

    private void rotating() {
        if(firstOver) {
            firstOver = false;

            ArrayList randoms = new ArrayList();
            randoms.Add(1);
            randoms.Add(2);
            randoms.Add(3);
            ArrayList order = new ArrayList();
            int[] sign = { -1, 1 };

            for(int i = 0; i <= 2; i++) {
                int index = Random.Range(0, randoms.Count);
                order.Add(randoms[index]);
                randoms.RemoveAt(index);
            }

            xRotate = float.Parse(order[0].ToString()) * 0.15f * sign[Random.Range(0, 2)];
            yRotate = float.Parse(order[1].ToString()) * 0.15f * sign[Random.Range(0, 2)];
            zRotate = float.Parse(order[2].ToString()) * 0.15f * sign[Random.Range(0, 2)];
        }

        transform.Rotate(new Vector3(xRotate, yRotate, zRotate));
    }

    public void select() {
        renderOutline();
        selected = true;
    }

    public void deselect() {
        unrenderOutline();
        firstOver = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.0f);
        transform.rotation = new Quaternion();
        selected = false;
    }

    protected void mimicClick() {
        CardOnClicked(this.gameObject, this.title);
    }

    public void activate() {
        active = true;
        initColor();
    }

    public void deactivate() {
        active = false;
        dullColor();
        transform.rotation = new Quaternion();
    }

    public void setName(string newName) {
        title = newName;
    }

    public string getName() {
        return title;
    }

    public void moveToField() {
        if(owner == Owner.Player) {
            location = Zone.PlayerField;
        } else if(owner == Owner.Enemy) {
            location = Zone.EnemyField;
            initColor();
        }
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        activate();
    }

    public void moveToHand() {
        if(owner == Owner.Player) {
            location = Zone.PlayerHand;
            activate();
        } else if(owner == Owner.Enemy) {
            location = Zone.EnemyHand;
            greyOut();
        }
        gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    public void moveToGraveyard() {
        if(owner == Owner.Player) {
            location = Zone.PlayerGraveyard;
        } else if(owner == Owner.Enemy) {
            location = Zone.EnemyGraveyard;
        }
        activate();
    }

    public void moveToDeck() {
        if(owner == Owner.Player) {
            location = Zone.PlayerDeck;
        } else if(owner == Owner.Enemy) {
            location = Zone.EnemyDeck;
        }
        activate();
    }

    virtual public void foeClicked(Unit card) {}

    public void setDeckController(DeckController dc) {
        deckController = dc;
    }

    public Zone getLocation() {
        return location;
    }

    public void setColor(Color c) {
        color = c;
    }

    public ClickAction getEvent() {
        return CardOnClicked;
    }

    public Owner getOwner() {
        return owner;
    }

    public int getManaCost() {
        return manaCost;
    }

    public void setOwner(Owner newOwner) {
        owner = newOwner;
    }

    public bool isUnit() {
        return GetType().IsSubclassOf(typeof(Unit));
    }

    public bool isSpell() {
        return GetType().IsSubclassOf(typeof(Spell));
    }

    public bool hasSkill(Skills skill) {
        return skills.Contains(skill);
    }

    //public static Owner invertOwner(Owner owner) {
    //    if(owner == Owner.Player) {
    //        return Owner.Enemy;
    //    } else if(owner == Owner.Enemy) {
    //        return Owner.Player;
    //    } else {
    //        print("ERROR_INVERTING_OWNER");
    //        return Owner.Error;
    //    }
    //}
}