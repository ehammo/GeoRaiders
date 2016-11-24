using UnityEngine;
using System.Collections;

public class SkeletonBehavior : MonoBehaviour {
    Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
        animator.Play("Idle");
        /*
        Touch touch = Input.touches[0];
        Ray touchRay = Camera.main.ScreenPointToRay(touch.position);

        foreach (RaycastHit hit in Physics.RaycastAll(touchRay)) {
            print("Object: " + hit.transform.name);
        }
        */

    }

    void OnMouseDown() {

    }
}
