using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

        private int[] randomSkill = { 1, 1, 1, 2, 1, 1, 2, 1, 1 };
        public SkeletonBehavior EnemyScript;
        private GameObject healthBar;
        private GameObject ManaBar;

		Text hpText;
		Text manaText;
		Image hpImage;
		Image manaImage;

		float maxHealth = 100;
		float maxMana = 100;
		float maxDamage = 0;
		float maxArmor = 0;

		float currentHealth = 100;
        public float currentMana = 50;
		float currentDamage = 0;
		float currentArmor = 0;

		int normalSize = 3;

		public GameObject inventory;
		public GameObject characterSystem;
		private Inventory mainInventory;
		private Inventory characterSystemInventory;

		private InputManager inputManagerDatabase;

        private int[] battleSkill;

		public void OnEnable()
		{
			Inventory.ItemEquip += OnBackpack;
			Inventory.UnEquipItem += UnEquipBackpack;

			Inventory.ItemEquip += OnGearItem;
			Inventory.ItemConsumed += OnConsumeItem;
			Inventory.UnEquipItem += OnUnEquipItem;

			Inventory.ItemEquip += EquipWeapon;
			Inventory.UnEquipItem += UnEquipWeapon;
		}

		public void OnDisable()
		{
			Inventory.ItemEquip -= OnBackpack;
			Inventory.UnEquipItem -= UnEquipBackpack;

			Inventory.ItemEquip -= OnGearItem;
			Inventory.ItemConsumed -= OnConsumeItem;
			Inventory.UnEquipItem -= OnUnEquipItem;

			Inventory.UnEquipItem -= UnEquipWeapon;
			Inventory.ItemEquip -= EquipWeapon;
		}

		void EquipWeapon(Item item)
		{
			if (item.itemType == ItemType.Weapon)
			{
				//add the weapon if you equip the weapon
			}
		}

		void UnEquipWeapon(Item item)
		{
			if (item.itemType == ItemType.Weapon)
			{
				//delete the weapon if you unequip the weapon
			}
		}

		void OnBackpack(Item item)
		{
			if (item.itemType == ItemType.Backpack)
			{
				for (int i = 0; i < item.itemAttributes.Count; i++)
				{
					if (mainInventory == null)
						mainInventory = inventory.GetComponent<Inventory>();
					mainInventory.sortItems();
					if (item.itemAttributes[i].attributeName == "Slots")
						changeInventorySize(item.itemAttributes[i].attributeValue);
				}
			}
		}

		void UnEquipBackpack(Item item)
		{
			if (item.itemType == ItemType.Backpack)
				changeInventorySize(normalSize);
		}

		void changeInventorySize(int size)
		{
			dropTheRestItems(size);

			if (mainInventory == null)
				mainInventory = inventory.GetComponent<Inventory>();
			if (size == 3)
			{
				mainInventory.width = 3;
				mainInventory.height = 1;
				mainInventory.updateSlotAmount();
				mainInventory.adjustInventorySize();
			}
			if (size == 6)
			{
				mainInventory.width = 3;
				mainInventory.height = 2;
				mainInventory.updateSlotAmount();
				mainInventory.adjustInventorySize();
			}
			else if (size == 12)
			{
				mainInventory.width = 4;
				mainInventory.height = 3;
				mainInventory.updateSlotAmount();
				mainInventory.adjustInventorySize();
			}
			else if (size == 16)
			{
				mainInventory.width = 4;
				mainInventory.height = 4;
				mainInventory.updateSlotAmount();
				mainInventory.adjustInventorySize();
			}
			else if (size == 24)
			{
				mainInventory.width = 6;
				mainInventory.height = 4;
				mainInventory.updateSlotAmount();
				mainInventory.adjustInventorySize();
			}
		}

		void dropTheRestItems(int size)
		{
			if (size < mainInventory.ItemsInInventory.Count)
			{
				for (int i = size; i < mainInventory.ItemsInInventory.Count; i++)
				{
					GameObject dropItem = (GameObject)Instantiate(mainInventory.ItemsInInventory[i].itemModel);
					dropItem.AddComponent<PickUpItem>();
					dropItem.GetComponent<PickUpItem>().item = mainInventory.ItemsInInventory[i];
					dropItem.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.localPosition;
				}
			}
		}

		public void OnConsumeItem(Item item)
		{
			for (int i = 0; i < item.itemAttributes.Count; i++)
			{
				if (item.itemAttributes[i].attributeName == "Health")
				{
					if ((currentHealth + item.itemAttributes[i].attributeValue) > maxHealth)
						currentHealth = maxHealth;
					else
						currentHealth += item.itemAttributes[i].attributeValue;
				}
				if (item.itemAttributes[i].attributeName == "Mana")
				{
					if ((currentMana + item.itemAttributes[i].attributeValue) > maxMana)
						currentMana = maxMana;
					else
						currentMana += item.itemAttributes[i].attributeValue;
				}
				if (item.itemAttributes[i].attributeName == "Armor")
				{
					if ((currentArmor + item.itemAttributes[i].attributeValue) > maxArmor)
						currentArmor = maxArmor;
					else
						currentArmor += item.itemAttributes[i].attributeValue;
				}
				if (item.itemAttributes[i].attributeName == "Damage")
				{
					if ((currentDamage + item.itemAttributes[i].attributeValue) > maxDamage)
						currentDamage = maxDamage;
					else
						currentDamage += item.itemAttributes[i].attributeValue;
				}
			}
			//if (HPMANACanvas != null)
			//{
			//    UpdateManaBar();
			//    UpdateHPBar();
			//}
		}

		public void OnGearItem(Item item)
		{
            print("ghue");
			for (int i = 0; i < item.itemAttributes.Count; i++)
			{
				if (item.itemAttributes[i].attributeName == "Health")
					maxHealth += item.itemAttributes[i].attributeValue;
				if (item.itemAttributes[i].attributeName == "Mana")
					maxMana += item.itemAttributes[i].attributeValue;
				if (item.itemAttributes[i].attributeName == "Armor")
					maxArmor += item.itemAttributes[i].attributeValue;
				if (item.itemAttributes[i].attributeName == "Damage")
					maxDamage += item.itemAttributes[i].attributeValue;
			}
			//if (HPMANACanvas != null)
			//{
			//    UpdateManaBar();
			//    UpdateHPBar();
			//}
		}

		public void OnUnEquipItem(Item item)
		{
			for (int i = 0; i < item.itemAttributes.Count; i++)
			{
				if (item.itemAttributes[i].attributeName == "Health")
					maxHealth -= item.itemAttributes[i].attributeValue;
				if (item.itemAttributes[i].attributeName == "Mana")
					maxMana -= item.itemAttributes[i].attributeValue;
				if (item.itemAttributes[i].attributeName == "Armor")
					maxArmor -= item.itemAttributes[i].attributeValue;
				if (item.itemAttributes[i].attributeName == "Damage")
					maxDamage -= item.itemAttributes[i].attributeValue;
			}
			//if (HPMANACanvas != null)
			//{
			//    UpdateManaBar();
			//    UpdateHPBar();
			//}
		}


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

            if (inputManagerDatabase == null)
				inputManagerDatabase = (InputManager)Resources.Load("InputManager");

			if (inventory != null)
				mainInventory = inventory.GetComponent<Inventory>();
			if (characterSystem != null)
				characterSystemInventory = characterSystem.GetComponent<Inventory>();
		
		}



        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }
            if (battle)
            {
                setLimits();

                float hp = currentHealth;
                float mana = currentMana;

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
			if (Input.GetKeyDown(inputManagerDatabase.CharacterSystemKeyCode) && !battle)
			{
				if (!characterSystem.activeSelf)
				{
					characterSystemInventory.openInventory();
				}
				else
				{
					characterSystemInventory.closeInventory();
				}
			}

			if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode) && !battle)
			{
				if (!inventory.activeSelf)
				{
					mainInventory.openInventory();
				}
				else
				{
					mainInventory.closeInventory();
				}
			}

        }

        public void armourPiercing(float val) {
            this.currentHealth -= val;
        }

        public void decreaseHp(float val)
        {
            print(val);
            print(this.currentHealth);
            print(this.currentArmor);

            if (currentArmor > val)
            {
                print("if1");
                this.currentArmor -= val;
            }
            else if(currentArmor<=val)
            {
                print("if2");
                print("sub: " + (val - currentArmor));
                this.currentHealth -= (val - this.currentArmor);
                this.currentArmor -= val;
                print(currentHealth);


            }
        }

        public void increaseHp(float val)
        {
            this.currentHealth += val;
        }

        public void decreaseMana(float val)
        {
            this.currentMana -= val;
        }

        public void increaseMana(float val)
        {
            this.currentMana += val;
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

        void setLimits() {
            if (this.currentMana > this.maxMana)
            {
                this.currentMana = this.maxMana;
            }
            if (this.currentHealth > this.maxHealth)
            {
                this.currentHealth = this.maxHealth;
            }
            if (this.currentMana < 0)
            {
                this.currentMana = 0;
            }
            if (this.currentHealth < 0)
            {
                this.currentHealth = 0;
            }
        }
    }
}