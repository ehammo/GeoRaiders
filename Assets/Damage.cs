using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {
    private int damage;
    // Use this for initialization
    public int getDamage() {
        return damage;
    }

    public void setDamage(int d)
    {
        damage = d;
    }

}
