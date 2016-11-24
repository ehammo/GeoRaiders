using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class SkeletonBehavior : MonoBehaviour
{
    Animator animator;
    public bool battle = false;
    private bool turno = false;
    private float hp;
    private int mana;
    private int[] randomSkill = { 1, 1, 1, 2, 1, 1, 2, 1, 1 };
    public ThirdPersonUserControl PlayerScript;
    private GameObject healthBar;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        mana = 50;
        hp = 100f;
        if(battle) healthBar = (gameObject.transform.FindChild("EnemyHealth").FindChild("EnemyLife")).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (battle)
        {
            if (healthBar.transform.localScale.x * 100 != hp)
            {
                Vector3 temp = healthBar.transform.localScale;
                temp.x = hp / 100f;
                if (hp / 100 < 0) temp.x = 0;
                healthBar.transform.localScale = temp;
            }

            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerScript = player.GetComponent<ThirdPersonUserControl>();

            if (hp <= 0)
            {
                animator.Play("Death");
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
                }
                else
                {
                    int skill = Random.Range(0, 8);
                    if (randomSkill[skill] == 1)
                    {
                        PlayerScript.decreaseHp(10);
                        animator.Play("Attack");
                        turno = false;
                    }
                    else
                    {
                        PlayerScript.decreaseHp(25);
                        animator.Play("Skill");
                        mana -= 25;
                        turno = false;
                    }
                }
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