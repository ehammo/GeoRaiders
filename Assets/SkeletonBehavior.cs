using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class SkeletonBehavior : MonoBehaviour
{
    public Animator animator;
    public bool battle = false;
    public bool turno = false;
    bool morri = false;
    bool ataquei = false;
    bool useiSkill = false;
    public float hp;
    private int mana;
    public int damage = 10;

    Vector2 touchPos;
    public GraphicRaycaster GR;


    private int[] randomSkill = { 1, 1, 1, 2, 1, 1, 2, 1, 1 };
    public ThirdPersonUserControl PlayerScript;
    private GameObject healthBar;
    private GameObject victory;
    private GameObject buttonContinue;
    private GameObject imageItem;
    private Sprite[] sprite;
    // Use this for initialization
    public GameObject prefabItem;
    Item[] item;
    void Start()
    {
        if (battle)
        {
            sprite = new Sprite[5];
            sprite[0] = Resources.Load<Sprite>("W_Gun001");
            sprite[1] = Resources.Load<Sprite>("A_Armour02");
            sprite[2] = Resources.Load<Sprite>("Ac_Gloves07");
            sprite[3] = Resources.Load<Sprite>("A_Shoes05");
            sprite[4] = Resources.Load<Sprite>("C_Elm04");


            item = new Item[5];

            List<ItemAttribute> weapon = new List<ItemAttribute>();
            weapon.Add(new ItemAttribute("Damage", 10));
            List<ItemAttribute> chest = new List<ItemAttribute>();
            chest.Add(new ItemAttribute("Armour", 10));
            chest.Add(new ItemAttribute("Health", 10));
            List<ItemAttribute> hands = new List<ItemAttribute>();
            hands.Add(new ItemAttribute("Mana", 10));
            hands.Add(new ItemAttribute("Armour", 5));
            List<ItemAttribute> shoe = new List<ItemAttribute>();
            shoe.Add(new ItemAttribute("Armour", 5));
            List<ItemAttribute> head = new List<ItemAttribute>();
            head.Add(new ItemAttribute("Mana", 10));
            head.Add(new ItemAttribute("Armour", 10));


            item[0] = new Item("W_Gun001", 1, "", sprite[0],prefabItem, 1, ItemType.Weapon, "", weapon);
            item[1] = new Item("A_Armour02", 2, "", sprite[1], prefabItem, 1, ItemType.Chest, "", chest);
            item[2] = new Item("Ac_Gloves07", 3, "", sprite[2], prefabItem, 1, ItemType.Hands, "", hands);
            item[3] = new Item("A_Shoes05", 4, "", sprite[3], prefabItem, 1, ItemType.Shoe, "", shoe);
            item[4] = new Item("C_Elm04", 5, "", sprite[4], prefabItem, 1, ItemType.Head, "", head);


            victory = Camera.main.transform.FindChild("Canvas").FindChild("Text").gameObject;
            victory.GetComponent<Text>().text = "Victory!";
            victory.SetActive(false);
            buttonContinue = Camera.main.transform.FindChild("Canvas").FindChild("Button").gameObject;
            buttonContinue.SetActive(false);
            imageItem = Camera.main.transform.FindChild("Canvas").FindChild("Image").gameObject;
            imageItem.SetActive(false);
        }
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = player.GetComponent<ThirdPersonUserControl>();
        mana = 50;
        hp = 100f;
    }

    public void decreaseHp(float val)
    {
        this.hp -= val;
    }

    public void increaseHp(float val)
    {
        this.hp += val;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        
        if (battle&&!asi.IsName("Damage")&& !animator.IsInTransition(0))
        {
            
            if (healthBar == null) healthBar = (gameObject.transform.FindChild("EnemyHealth").FindChild("EnemyLife")).gameObject;

            if (healthBar.transform.localScale.x * 100 != hp)
            {
                Vector3 temp = healthBar.transform.localScale;
                temp.x = hp / 100f;
                if (hp / 100 < 0) temp.x = 0;
                healthBar.transform.localScale = temp;
            }


            if (hp <= 0 && !morri)
            {
                turno = false;
                animator.Play("Death");
                print("tocando morte");
                morri = true;
            }
            else if (hp <= 0 && morri && asi.normalizedTime >= 0.9f)
            {

                int rr = Random.Range(0, 5);
                victory.SetActive(true);
                buttonContinue.SetActive(true);

                if (rr != 5)
                {
                    imageItem.SetActive(true);
                    imageItem.GetComponent<Image>().sprite = sprite[rr];
                    GameObject inventory = GameObject.FindGameObjectWithTag("Canvas").transform.FindChild("Panel - Inventory(Clone)").gameObject;
                    Inventory mainInventory = inventory.GetComponent<Inventory>();
                    mainInventory.addItemToInventory(item[rr]);
                }

                battle = false;
                PlayerScript.battle = false;
            }

            if (battle && turno)
            {
                print("eh o turno dele e ele tem "+hp+" de vida");
                if (mana < 10)
                {
                    if (!ataquei)
                    {
                        animator.Play("Attack");
                        ataquei = true;
                    }else if(ataquei && !asi.IsName("Attack"))
                    {
                        PlayerScript.decreaseHp(damage);
                        turno = false;
                        PlayerScript.turno = true;
                        PlayerScript.battle = true;
                        ataquei = false;
                    }
               }
                else
                {
                    if (!ataquei && !useiSkill)
                    {
                        int skill = Random.Range(0, 8);
                        if (randomSkill[skill] == 1)
                        {
                            animator.Play("Attack");
                            ataquei = true;
                        }
                        else
                        {
                            animator.Play("Skill");
                            useiSkill = true;
                        }
                    }
                    else if (ataquei && !asi.IsName("Attack"))
                    {
                        PlayerScript.decreaseHp(damage);
                        PlayerScript.turno = true;
                        PlayerScript.battle = true;
                        turno = false;
                        ataquei = false;
                    }
                    else if (useiSkill && !asi.IsName("Skill")) {
                        PlayerScript.decreaseHp(damage*1.5f);
                        mana -= 25;
                        PlayerScript.turno = true;
                        PlayerScript.battle = true;
                        turno = false;
                        useiSkill = false;
                    }
                    
                }
                if (!turno) {
                    Text turnText = (Camera.main.transform.FindChild("Canvas").FindChild("Turn").gameObject).GetComponent<Text>();
                    turnText.text = "Your Turn";
                }
                
            }
        }
    }

    public void Morre()
    {
        DestroyObject(this);
        Application.LoadLevel("DynamicLoader");
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void OnMouseDown()
    {
        //GR = GameObject.FindGameObjectWithTag("Canvas").gameObject.GetComponent<Canvas>();
        if (!IsPointerOverUIObject() && !battle)
        {
            /* if (Input.touchCount > 0)
             {
                 if (Input.GetTouch(0).phase == TouchPhase.Began)
                 {
                     PointerEventData ped = new PointerEventData(null);
                     ped.position = Input.GetTouch(0).position;
                     List<RaycastResult> results = new List<RaycastResult>();
                     GR.Raycast(ped, results);
                     if (results.Count == 0)
                     {
                             PlayerScript.saveStats();
                             DestroyObject(this);
                             Application.LoadLevel("Demo_Scene");
                     }
                 }
             } */
            PlayerScript.saveStats();
            DestroyObject(this);
            Application.LoadLevel("Demo_Scene");
        }
    }
}