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
    public int damage = 0;
    private int[] randomSkill = { 1, 1, 1, 2, 1, 1, 2, 1, 1 };
    public ThirdPersonUserControl PlayerScript;
    private GameObject healthBar;
    private GameObject fimBatalha;

    // Use this for initialization
    void Start()
    {
        if (battle)
        {
            fimBatalha = GameObject.FindGameObjectWithTag("Finish");
            fimBatalha.SetActive(false);
        }
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        print("hey olha esse bool: " + player == null);
        PlayerScript = player.GetComponent<ThirdPersonUserControl>();
        print("hey2: " + PlayerScript == null);
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
            }
            else if (hp <= 0 && morri && asi.normalizedTime>=0.9f) {
                print("morri");
                battle = false;
                PlayerScript.battle = false;
                DestroyObject(this);
                Application.LoadLevel("DynamicLoader");
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

            //animator.Play("Idle");

            /*
       Touch touch = Input.touches[0];
       Ray touchRay = Camera.main.ScreenPointToRay(touch.position);

       foreach (RaycastHit hit in Physics.RaycastAll(touchRay)) {
           print("Object: " + hit.transform.name);
       }
       

            if (!anim.IsPlaying("Death") && !anim.IsPlaying("Attack") && !anim.IsPlaying("Skill"))
            {
                debugAnim = false;
            }*/

        }
    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            print("hey3: " + PlayerScript == null);
            PlayerScript.saveStats();
            Application.LoadLevel("Demo_Scene");
            DestroyObject(this);
        }
           }
}