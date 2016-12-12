using UnityEngine;
using System.Collections;

public class loader : MonoBehaviour {

    public GameObject gm;
    // Use this for initialization
    void Awake () {
        if (Gm.instance == null) {
            Instantiate(gm);
        }
    }
	

}
