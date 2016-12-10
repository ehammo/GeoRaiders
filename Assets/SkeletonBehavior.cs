using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class SkeletonBehavior : MonoBehaviour
{
    public Animator animator;
    public bool battle = false;
    public bool turno = false;
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
            
            if (hp <= 0)
            {
                animator.Play("Death");
                battle = false;
                PlayerScript.battle = false;
                PlayerScript.turno = true;
                DestroyObject(this);
                Application.LoadLevel("DynamicLoader");
            }

            if (battle && turno)
            {
                if (mana < 10)
                {
                    PlayerScript.decreaseHp(10);
                    animator.Play("Attack");
                    turno = false;
                    PlayerScript.turno = true;
                    PlayerScript.battle = true;
                }
                else
                {
                    int skill = Random.Range(0, 8);
                    if (randomSkill[skill] == 1)
                    {
                        PlayerScript.decreaseHp(10);
                        animator.Play("Attack");
                        PlayerScript.turno = true;
                        PlayerScript.battle = true;
                        turno = false;
                    }
                    else
                    {
                        PlayerScript.decreaseHp(25);
                        animator.Play("Skill");
                        mana -= 25;
                        PlayerScript.turno = true;
                        PlayerScript.battle = true;
                        turno = false;
                    }
                }
                Text turnText = (Camera.main.transform.FindChild("Canvas").FindChild("Turn").gameObject).GetComponent<Text>();
                turnText.text = "Your Turn";
            }

            //animator.Play("Idle");

            /*
       Touch touch = Input.touches[0];
       Ray touchRay = Camera.main.ScreenPointToRay(touch.position);

       foreach (RaycastHit hit in Physics.RaycastAll(touchRay)) {
           print("Object: " + hit.transform.name);
       }
       */
        }
    }

    void OnMouseDown()
    {
        Application.LoadLevel("Demo_Scene");
        DestroyObject(this);
    }
}