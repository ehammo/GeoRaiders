using UnityEngine;
using UnityEngine.UI; // <-- you need this to access UI (button in this case) functionalities
using UnityStandardAssets.Characters.ThirdPerson;

public class ButtonController : MonoBehaviour
{
    Button myButton;
    SkeletonBehavior EnemyScript;
    ThirdPersonUserControl PlayerScript;
    void Awake()
    {
        myButton = GetComponent<Button>(); // <-- you get access to the button component here

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = player.GetComponent<ThirdPersonUserControl>();
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        EnemyScript = enemy.GetComponent<SkeletonBehavior>();


        myButton.onClick.AddListener(() => { myFunctionForOnClickEvent(); });  // <-- you assign a method to the button OnClick event here
   
    }

    void myFunctionForOnClickEvent()
    {
        bool turn = PlayerScript.turno;
        print("Turn: "+turn);

        if (turn&&myButton.name == "Skill1Btn")
        {
            EnemyScript.decreaseHp(10);
            EnemyScript.animator.Play("Damage");
            EnemyScript.turno = true;
            PlayerScript.turno = false;
            print("Turn: " + turn);
        }
        else if (turn&&myButton.name == "Skill2Btn"&&PlayerScript.currentMana>=25) {
            EnemyScript.decreaseHp(25);
            PlayerScript.decreaseMana(25);
            EnemyScript.turno = true;
            PlayerScript.turno = false;
        }
    }

}