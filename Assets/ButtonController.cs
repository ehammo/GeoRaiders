﻿using UnityEngine;
using UnityEngine.UI; // <-- you need this to access UI (button in this case) functionalities
using UnityStandardAssets.Characters.ThirdPerson;

public class ButtonController : MonoBehaviour
{
    Button myButton;
    SkeletonBehavior EnemyScript;
    ThirdPersonUserControl PlayerScript;
    bool damAnim=false;
    AnimatorStateInfo asi;

    void Awake()
    {
        myButton = GetComponent<Button>(); // <-- you get access to the button component here

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = player.GetComponent<ThirdPersonUserControl>();
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        EnemyScript = enemy.GetComponent<SkeletonBehavior>();
        Animator animator = enemy.GetComponent<Animator>();
        asi = animator.GetCurrentAnimatorStateInfo(0);

        myButton.onClick.AddListener(() => { myFunctionForOnClickEvent(); });  // <-- you assign a method to the button OnClick event here
   
    }

    void myFunctionForOnClickEvent()
    {
        bool turn = PlayerScript.turno;
        print("Turn: "+turn);

        if (turn&&myButton.name == "Skill1Btn" && !damAnim)
        {
            int damage = Random.Range(PlayerScript.currentDamage, PlayerScript.maxDamage);
            EnemyScript.decreaseHp(damage);
            EnemyScript.animator.Play("Damage");
            damAnim = true;
        }
        else if (turn&&myButton.name == "Skill2Btn"&&PlayerScript.currentMana>=25 && !damAnim) {
            int damage = Random.Range(PlayerScript.maxDamage-10, PlayerScript.maxDamage);
            EnemyScript.decreaseHp(damage);
            PlayerScript.decreaseMana(25);
            EnemyScript.animator.Play("Damage");
            damAnim = true;
        }


    }

    void Update()
    {
        asi = EnemyScript.animator.GetCurrentAnimatorStateInfo(0);

        if (!asi.IsName("Damage") && damAnim) {
            EnemyScript.turno = true;
            PlayerScript.turno = false;
            Text turnText = (Camera.main.transform.FindChild("Canvas").FindChild("Turn").gameObject).GetComponent<Text>();
            turnText.text = "His Turn";
            damAnim = false;
           }

    }

}