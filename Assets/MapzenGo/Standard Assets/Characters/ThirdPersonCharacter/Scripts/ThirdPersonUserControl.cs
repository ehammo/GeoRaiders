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
       

        public TestLocationService gps;
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
		public int maxDamage = 25;
		float maxArmor = 0;

		float currentHealth = 100;
        public float currentMana = 50;
        public int currentDamage = 10;
		float currentArmor = 0;
        float testing = 0;

        int normalSize = 3;

		public GameObject inventory;
		public GameObject characterSystem;
		private Inventory mainInventory;
		private Inventory characterSystemInventory;

		private InputManager inputManagerDatabase;

        private int[] battleSkill;

        public Gm gm;
        private GameObject defeat;
        private GameObject buttonContinue;
        private GameObject imageItem;
        private Sprite[] sprite;

        public void saveStats() {
            print("vou dar save");
            gm.maxArmor = this.maxArmor;
            gm.maxDamage = this.maxDamage;
            gm.maxHealth = this.maxHealth;
            gm.maxMana = this.maxMana;
            gm.currentArmor = this.currentArmor;
            gm.currentDamage = this.currentDamage;
            gm.currentHealth = this.currentHealth;
            gm.currentMana = this.currentMana;
        }

        public void setStats()
        {
            print("vou dar set");
            this.maxArmor= gm.maxArmor;
            this.maxDamage= gm.maxDamage;
            this.maxHealth= gm.maxHealth;
            this.maxMana= gm.maxMana;
            this.currentArmor= gm.currentArmor;
            this.currentDamage= gm.currentDamage;
            this.currentHealth= gm.currentHealth;
            this.currentMana= gm.currentMana;
            this.inventory = GameObject.FindGameObjectWithTag("Canvas").transform.FindChild("Panel - Inventory(Clone)").gameObject;
            this. characterSystem = GameObject.FindGameObjectWithTag("Canvas").transform.FindChild("Panel - EquipmentSystem(Clone)").gameObject;

        }

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
            int t = maxDamage;
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
            currentArmor = maxArmor;
            currentMana = maxMana;
            currentHealth = maxHealth;
            currentDamage += Math.Abs(t-maxDamage);
            //if (HPMANACanvas != null)
            //{
            //    UpdateManaBar();
            //    UpdateHPBar();
            //}
        }

        public void OnUnEquipItem(Item item)
		{
            int t = maxDamage;
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

            currentArmor = maxArmor;
            currentMana = maxMana;
            currentHealth = maxHealth;
            currentDamage -= Math.Abs(t - maxDamage);
            //if (HPMANACanvas != null)
            //{
            //    UpdateManaBar();
            //    UpdateHPBar();
            //}
        }

        private void Start()
        {
            print("hey");
            gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gm>();
            setStats();
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

            maxDamage = 30;
            currentDamage = 10;

            if (battle) {
                defeat = GameObject.FindGameObjectWithTag("endBattle");
                defeat.GetComponent<Text>().text = "Defeat!";
                defeat.SetActive(false);
                buttonContinue = GameObject.FindGameObjectWithTag("buttonContinue");
                buttonContinue.SetActive(false);
                imageItem = GameObject.FindGameObjectWithTag("image").gameObject;
                imageItem.SetActive(false);
            }

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
                mainInventory.closeInventory();
                characterSystemInventory.closeInventory();
                float hp = currentHealth;
                float mana = currentMana;
                if (healthBar == null || ManaBar == null)
                {
                    healthBar = (Camera.main.transform.FindChild("Canvas").FindChild("PlayerHealth").FindChild("PlayerLife")).gameObject;
                    ManaBar = (Camera.main.transform.FindChild("Canvas").FindChild("PlayerMana").FindChild("PlayerMana")).gameObject;
                }

                if (healthBar.transform.localScale.x * 100 != hp)
                {
                    print("entrei no id da vida");
                    Vector3 temp = healthBar.transform.localScale;
                    temp.x = hp / 100f;
                    if (hp / 100 < 0) temp.x = 0;
                    healthBar.transform.localScale = temp;
                }
                if (ManaBar.transform.localScale.x * 50 != mana)
                {
                    Vector3 temp = ManaBar.transform.localScale;
                    temp.x = mana / 100f;
                    if (mana / 100 < 0) temp.x = 0;
                    ManaBar.transform.localScale = temp;
                }

                GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
                EnemyScript = enemy.GetComponent<SkeletonBehavior>();

                if (hp <= 0)
                {
                    defeat.SetActive(true);
                    buttonContinue.SetActive(true);
                    battle = false;
                    EnemyScript.battle = false;
                }

            }
            else {
                if (Input.GetKeyDown(inputManagerDatabase.CharacterSystemKeyCode))
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

                if (Input.GetKeyDown(inputManagerDatabase.InventoryKeyCode))
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
			

        }

        public void armourPiercing(float val) {
            this.currentHealth -= val;
        }

        public void decreaseHp(float val)
        {
            if (currentArmor > val)
            {
                this.currentArmor -= val;
            }
            else if(currentArmor<=val)
            {
                this.currentHealth -= (val - this.currentArmor);
                this.currentArmor -= val;
                if (this.currentArmor < 0) this.currentArmor = 0;
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
            if (m_Character!=null)
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
                    //for mobile
                   // m_Move = new Vector3(gps.getLonDesloc() * 150000.706f,0, gps.getLatDesloc()*150000.077f);
                }
                else
                {
                    // we use world-relative directions in the case of no main camera
                    m_Move = v * Vector3.forward + h * Vector3.right;
                    //for mobile
                   // m_Move = new Vector3(gps.getLonDesloc() * 150000.706f, 0, gps.getLatDesloc() * 150000.077f);
                }
#if !MOBILE_INPUT
                // walk speed multiplier
                if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

                // pass all parameters to the character control script
                m_Character.Move(m_Move, crouch, m_Jump);
                //m_Character.transform.position += m_Move;
                //          Vector3 offset = new Vector3(m_Character.transform.position.x, m_Character.transform.position.y + 60.0f, m_Character.transform.position.z - 60.0f);
                //          offset = Quaternion.AngleAxis(-h*4.0f,Vector3.forward)*offset;
                //          m_Cam.position = m_Character.transform.localPosition + offset;
                //          m_Cam.LookAt(m_Character.transform.position);
                m_Jump = false;
            }
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

        void stats() {
            String s = "Current Damage: " + currentDamage + "\nMaxDamage: " + maxDamage + "\nCurrentArmor: " + currentArmor;
            print(s);
            print("CurrentArmor: " + currentArmor);
            print("CurrentHealth: " + currentHealth);
        }


        public void BeginEffect(GameObject fireball)
        {
            Vector3 pos;
            DigitalRuby.PyroParticles.FireBaseScript currentPrefabScript;
            float yRot = transform.rotation.eulerAngles.y;
            Vector3 forwardY = Quaternion.Euler(0.0f, yRot, 0.0f) * Vector3.forward;
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            Vector3 up = transform.up;
            Quaternion rotation = Quaternion.identity;
            GameObject currentPrefabObject = GameObject.Instantiate(fireball);
            currentPrefabScript = currentPrefabObject.GetComponent<DigitalRuby.PyroParticles.FireConstantBaseScript>();
            if (currentPrefabScript == null)
            {
                // temporary effect, like a fireball
                currentPrefabScript = currentPrefabObject.GetComponent<DigitalRuby.PyroParticles.FireBaseScript>();
                if (currentPrefabScript.IsProjectile)
                {
                    // set the start point near the player
                    rotation = transform.rotation;
                    pos = transform.position + forward;
                    pos.x = pos.x - 1;
                    pos.y = pos.y + 1;
                    pos.z = 47;
                    //pos.z = 47;
                    //pos.y = -5;
                    //  pos = transform.position + forward + right + up;
                }
                else
                {
                    // set the start point in front of the player a ways
                    pos = transform.position + (forwardY * 10.0f);
                }
            }
            else
            {
                // set the start point in front of the player a ways, rotated the same way as the player
                pos = transform.position + (forwardY * 5.0f);
                rotation = transform.rotation;
                pos.y = 0.0f;
            }


            DigitalRuby.PyroParticles.FireProjectileScript projectileScript = currentPrefabObject.GetComponentInChildren<DigitalRuby.PyroParticles.FireProjectileScript>();
            if (projectileScript != null)
            {
                // make sure we don't collide with other friendly layers
                projectileScript.ProjectileCollisionLayers &= (~UnityEngine.LayerMask.NameToLayer("FriendlyLayer"));
            }


            currentPrefabObject.transform.position = pos;
            currentPrefabObject.transform.rotation = rotation;
        }

        void OnMouseDown()
        {
            stats();
            openInventory();
        }

        public void openInventory()
        {
            if (!battle)
            {
                if (!inventory.activeSelf && !characterSystem.activeSelf)
                {
                    mainInventory.openInventory();
                    characterSystemInventory.openInventory();
                }
                else
                {
                    mainInventory.closeInventory();
                    characterSystemInventory.closeInventory();
                }
            }
        }
    }
}