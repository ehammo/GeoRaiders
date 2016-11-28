using System;
using UnityEngine;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump = false;                      // the world-relative desired move direction, calculated from the camForward and user input.
        public bool turno = true;
        public bool battle = false;
        private float hp;
        public float mana;
        private int[] randomSkill = { 1, 1, 1, 2, 1, 1, 2, 1, 1 };
        public SkeletonBehavior EnemyScript;
        private GameObject healthBar;
        private GameObject ManaBar;

        private int[] battleSkill;

        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();

            mana = 50f;
            hp = 100f;
            if (battle) healthBar = (gameObject.transform.FindChild("EnemyHealth").FindChild("EnemyLife")).gameObject;
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }
            if (battle)
            {
                if (healthBar == null||ManaBar==null)
                {
                    healthBar = (Camera.main.transform.FindChild("Canvas").FindChild("PlayerHealth").FindChild("PlayerLife")).gameObject;
                    ManaBar = (Camera.main.transform.FindChild("Canvas").FindChild("PlayerMana").FindChild("PlayerMana")).gameObject;
                }
                if (healthBar.transform.localScale.x * 100 != hp)
                {
                    Vector3 temp = healthBar.transform.localScale;
                    temp.x = hp / 100f;
                    if (hp / 100 < 0) temp.x = 0;
                    healthBar.transform.localScale = temp;
                }
                if (ManaBar.transform.localScale.x * 50 != mana) {
                    Vector3 temp = ManaBar.transform.localScale;
                    temp.x = mana / 100f;
                    if (mana / 100 < 0) temp.x = 0;
                    ManaBar.transform.localScale = temp;
                }

                GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
                EnemyScript = enemy.GetComponent<SkeletonBehavior>();

                if (hp <= 0)
                {
                    //Aparecer voce perdeu de alguma forma
                    battle = false;
                    EnemyScript.battle = false;
                    DestroyObject(this);
                    Application.LoadLevel("DynamicLoader");
                }
                
            }

        }



        public void decreaseHp(float val)
        {
            this.hp -= val;
        }

        public void increaseHp(float val)
        {
            this.hp += val;
        }

        public void decreaseMana(float val)
        {
            this.mana -= val;
        }

        public void increaseMana(float val)
        {
            this.mana += val;
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            //          Vector3 offset = new Vector3(m_Character.transform.position.x, m_Character.transform.position.y + 60.0f, m_Character.transform.position.z - 60.0f);
            //          offset = Quaternion.AngleAxis(-h*4.0f,Vector3.forward)*offset;
            //          m_Cam.position = m_Character.transform.localPosition + offset;
            //          m_Cam.LookAt(m_Character.transform.position);
            m_Jump = false;
        }
    }
}