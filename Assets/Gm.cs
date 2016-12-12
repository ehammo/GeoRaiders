using UnityEngine;

public class Gm : MonoBehaviour
{

    public static Gm instance = null;
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
        if (instance == null) instance = this;
        else DestroyObject(gameObject);
        // Do not destroy this game object:

        DontDestroyOnLoad(gameObject);

    }
}