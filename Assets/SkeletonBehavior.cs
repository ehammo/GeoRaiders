using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class SkeletonBehavior : MonoBehaviour
{
    public Animator animator;
    public bool battle = false;
    public bool turno = false;
    bool morri = false;
    bool ataquei = false;
    bool useiSkill = false;
    private float hp;
    private int mana;
    private int[] randomSkill = { 1, 1, 1, 2, 1, 1, 2, 1, 1 };
    public ThirdPersonUserControl PlayerScript;
    private GameObject healthBar;
    // Use this for initialization
    void Start()
    {
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
            if(morri) print(asi.normalizedTime);
            if (hp <= 0 && !morri)
            {
                animator.Play("Death");
                battle = false;
                PlayerScript.battle = false;
                PlayerScript.turno = true;
                morri = true;
            }
            else if (hp <= 0 && morri && asi.normalizedTime>=0.9) {
                DestroyObject(this);
                Application.LoadLevel("DynamicLoader");
            }

            if (battle && turno)
            {
                if (mana < 10)
                {
                    if (!ataquei)
                    {
                        animator.Play("Attack");
                        print("Sobre a animacao: " + asi.IsName("Attack"));
                        print("ataquei");
                        ataquei = true;
                    }else if(ataquei && !asi.IsName("Attack"))
                    {
                        print("Sobre a animacao: "+ asi.IsName("Attack"));
                        PlayerScript.decreaseHp(10);
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
                            print("Sobre a animacao: " + asi.IsName("Attack"));
                            print("ataquei");
                            ataquei = true;
                        }
                        else
                        {
                            animator.Play("Skill");
                            print("Sobre a animacao: " + asi.IsName("Skill"));
                            print("usei skill");
                            useiSkill = true;
                        }
                    }
                    else if (ataquei && !asi.IsName("Attack"))
                    {
                        PlayerScript.decreaseHp(10);
                        PlayerScript.turno = true;
                        PlayerScript.battle = true;
                        turno = false;
                        ataquei = false;
                    }
                    else if (useiSkill && !asi.IsName("Skill")) {
                        PlayerScript.decreaseHp(15);
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
        PlayerScript.saveStats();
        Application.LoadLevel("Demo_Scene");
        DestroyObject(this);
    }
}