using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
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

    private int[] randomSkill = { 1, 1, 1, 2, 1, 1, 2, 1, 1 };
    public ThirdPersonUserControl PlayerScript;
    private GameObject healthBar;
    private GameObject victory;
    private GameObject buttonContinue;
    private GameObject imageItem;
    private Sprite[] sprite;
    // Use this for initialization
    void Start()
    {
        if (battle)
        {
            sprite = new Sprite[4];
            sprite[0] = Resources.Load<Sprite>("W_Gun001");
            sprite[1] = Resources.Load<Sprite>("A_Armour02");
            sprite[2] = Resources.Load<Sprite>("Ac_Gloves07");
            sprite[3] = Resources.Load<Sprite>("W_Bow14");

            victory = GameObject.FindGameObjectWithTag("endBattle");
            victory.GetComponent<Text>().text = "Victory!";
            victory.SetActive(false);
            buttonContinue = GameObject.FindGameObjectWithTag("buttonContinue");
            buttonContinue.SetActive(false);
            imageItem = GameObject.FindGameObjectWithTag("image").gameObject;
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
       
        if (battle)
        {
            if (healthBar == null) healthBar = (gameObject.transform.FindChild("EnemyHealth").FindChild("EnemyLife")).gameObject;

            if (healthBar.transform.localScale.x * 100 != hp)
            {
                Vector3 temp = healthBar.transform.localScale;
                temp.x = hp / 100f;
                if (hp / 100 < 0) temp.x = 0;
                healthBar.transform.localScale = temp;
            }

            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);

            if (hp <= 0 && !morri)
            {
                turno = false;
                animator.Play("Death");
                print("tocando morte");
                morri = true;
                int rr = Random.Range(0, 3);
                imageItem.GetComponent<Image>().sprite = sprite[rr];

            }
            else if (hp <= 0 && morri && asi.normalizedTime>=0.9f) {
                victory.SetActive(true);
                buttonContinue.SetActive(true);
                imageItem.SetActive(true);
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


    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            PlayerScript.saveStats();
            DestroyObject(this);
            Application.LoadLevel("Demo_Scene");
        }
           }
}