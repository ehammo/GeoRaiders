using UnityEngine;

public class Gm : MonoBehaviour
{
    public float maxHealth = 100;
    public float maxMana = 100;
    public int maxDamage = 25;
    public float maxArmor = 0;

    public float currentHealth = 100;
    public float currentMana = 50;
    public int currentDamage = 10;
    public float currentArmor = 0;


    void Awake()
    {

        // Do not destroy this game object:

        DontDestroyOnLoad(this);

    }
}